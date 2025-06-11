using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;//用于字符串编码转换（如 UTF8 编码）。
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatRoomUI : NewUIBase//UI 控件管理、用户输入处理、消息发送与显示	使用 ClientSocket 进行网络通信
{
    private const string IP = "192.168.80.119";//服务器地址
    private const int PORT = 8080;//端口号

    // 用户名输入
    public TMP_InputField unameInput;// 用户名输入框
    // 消息输入
    public TMP_InputField msgInput;//消息 输入框

    public Button loginBtn;// 登录/断开按钮

    public Button sendBtn;

    public TextMeshProUGUI stateTxt;// 显示当前连接的状态文本

    public TextMeshProUGUI connectBtnText;// 显示“连接”或者“断开”文本

    public TextMeshProUGUI chatMsgTxt;// 聊天室聊天文本/聊天记录

    private ClientSocket clientSocket = new ClientSocket();// 封装的ClientSocket对象

    void Start()
    {
        chatMsgTxt.text = "";

        loginBtn.onClick.AddListener(() =>
        {
            if (clientSocket.connected)//如果已连接，则关闭连接，更新状态为“已断开”，并允许用户名输入。
            {

                clientSocket.CloseSocket();//说明之前没断干净，断开重新登录
                stateTxt.text = "已断开";//修改状态文本
                connectBtnText.text = "连接";// 修改按钮文本
                unameInput.enabled = true;
            }
            else//如果未连接，则尝试连接服务器，并根据是否成功更新状态，禁止用户名输入。
            {
                clientSocket.Connect(IP, PORT);
                stateTxt.text = clientSocket.connected ? "已连接" : "未连接";
                connectBtnText.text = clientSocket.connected ? "断开" : "连接";
                if (clientSocket.connected)
                    unameInput.enabled = false;
                
                Send("login");//发送 "login" 协议消息给服务端。
            }
        });

        sendBtn.onClick.AddListener(() =>
        {
            Send("chat", msgInput.text);//给发送按钮添加点击事件监听器，调用 Send("chat", msg) 发送聊天消息。
            msgInput.text = "";
        });
    }

    private void Update()//每帧执行，接收消息并更新 UI
    {
        if (clientSocket.connected)//如果已连接
        {
            clientSocket.BeginReceive();//调用 BeginReceive() 开始异步接收数据。
        }
        var msg = clientSocket.GetMsgFromQueue();//从消息队列中取出消息供UI层读取
        if (!string.IsNullOrEmpty(msg))//如果取到了非空的消息：
        {
            chatMsgTxt.SetAllDirty(); //通知 Unity 的 UI 文本组件（如 Text 或 TMP_Text）内容发生了变化，需要重新渲染。
            chatMsgTxt.text += msg + "\n";//将新消息追加到聊天框文本中，并换行。

            Debug.Log("RecvCallBack: " + msg);
        }
    }
    /// <summary>
    /// 构造Json并发送消息
    /// </summary>
    /// <param name="protocol"></param>
    /// <param name="msg"></param>
    private void Send(string protocol, string msg = "")
    {
        //JSONObject jsonObj = new JSONObject();
        //jsonObj["protocol"] = protocol;
        //jsonObj["uname"] = unameInput.text;
        //jsonObj["msg"] = msg;
        //// JSONObject转string
        //string jsonStr = JsonConvert.SerializeObject(jsonObj);
        //// string转byte[]
        //byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        //// 发送消息给服务端
        //clientSocket.SendData(data);

        // 构建要发送的数据对象，包含协议、用户名、消息等字段，形式为字典
        Dictionary<string, string> sendData = new Dictionary<string, string>
        {
            { "protocol", protocol },
            { "uname", unameInput.text },
            { "msg", msg }
        };

        // 序列化为 JSON 字符串
        string jsonStr = JsonConvert.SerializeObject(sendData);

        // 转换为字节数组并发送到服务器端
        byte[] data = Encoding.UTF8.GetBytes(jsonStr);
        clientSocket.SendData(data);
    }
    /// <summary>
    /// 退出时清理资源
    /// </summary>
    private void OnApplicationQuit()
    {
        if (clientSocket.connected)
        {
            clientSocket.CloseSocket();//关闭Socket连接，防止资源泄露
        }
    }
}
