using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

//TCP网络通信封装：连接、发送、接收消息
public class ClientSocket : MonoBehaviour
{
    private byte[] m_recvBuff;//接收缓冲区（大小 16KB）
    private AsyncCallback m_recvCb;//异步接收回调函数
    private Queue<string> m_msgQueue = new Queue<string>();//存放接收到的消息队列（线程安全队列）
    private Socket m_socket;//当前是否已连接标志
    /// <summary>
    /// 初始化 socket 和缓冲区
    /// </summary>
    /// <returns></returns>
    private Socket init()
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个新的 TCP socket。

        m_recvBuff = new byte[0x4000];// 设置接收的消息数据包大小限制为 0x4000 byte, 即16KB
        m_recvCb = new AsyncCallback(RecvCallBack);//设置异步接收回调函数为 RecvCallBack。
        return clientSocket;
    }

    /// <summary>
    /// 初始化 socket 连接服务器
    /// </summary>
    /// <param name="host">ip地址</param>
    /// <param name="port">端口号</param>
    public void Connect(string host, int port)
    {
        if (m_socket == null)//如果 socket 未初始化，则调用 init()进行初始化。
            m_socket = init();
        try
        {
            Debug.Log("connect: " + host + ":" + port);
            m_socket.SendTimeout = 3;//设置发送超时时间为 3 秒。
            m_socket.Connect(host, port);//调用 Connect() 连接服务器。
            connected = true;//成功则设置 connected = true，否则输出错误日志。
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    /// <summary>
    /// 通过 NetworkStream 发送数据到服务端
    /// </summary>
    public void SendData(byte[] bytes)
    {
        NetworkStream netstream = new NetworkStream(m_socket);//创建 NetworkStream 流对象。
        netstream.Write(bytes, 0, bytes.Length);//写入字节数据到流中，发送给服务端。
    }

    /// <summary>
    /// 尝试接收消息（每帧调用）
    /// </summary>
    public void BeginReceive()
    {
        //启动异步接收操作，使用 m_recvBuff 缓冲区。
        //接收完成后会触发 RecvCallBack 回调。
        m_socket.BeginReceive(m_recvBuff, 0, m_recvBuff.Length, SocketFlags.None, m_recvCb, this);
    }
    /// <summary>
    /// 当收到服务器的消息时会回调这个函数，数据到达后回调，放入 m_msgQueue
    /// </summary>
    private void RecvCallBack(IAsyncResult ar)
    {
        // 获取实际接收的数据长度。
        var len = m_socket.EndReceive(ar);
        byte[] msg = new byte[len];
        // 复制有效数据到新数组。
        Array.Copy(m_recvBuff, msg, len);
        // 转换为 UTF8 字符串后加入消息队列。
        var msgStr = System.Text.Encoding.UTF8.GetString(msg);
        // 将消息塞入队列中
        m_msgQueue.Enqueue(msgStr);
        // 清空缓冲区以便下次接收。
        for (int i = 0; i < m_recvBuff.Length; ++i)
        {
            m_recvBuff[i] = 0;
        }
    }

    /// <summary>
    /// 从消息队列中取出消息供UI层读取 从队列中取出一条消息，供主线程读取。
    /// </summary>
    /// <returns></returns>
    public string GetMsgFromQueue()
    {
        if (m_msgQueue.Count > 0)
            return m_msgQueue.Dequeue();
        return null;
    }

    /// <summary>
    /// 关闭Socket
    /// </summary>
    public void CloseSocket()
    {
        Debug.Log("close socket");
        try
        {
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
        }
        catch (Exception e)
        {
            //Debug.LogError(e);
        }
        finally
        {
            m_socket = null;
            connected = false;
        }
    }
    public bool connected = false;
}
