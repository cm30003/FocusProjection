using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum JsonType
{
    JsonUtlity,
    LitJson,
}
/// <summary>
/// Json数据管理类，json序列化和反序列化 存储到硬盘
/// </summary>
public class JsonManager 
{
    private static JsonManager instance=new JsonManager();

    public static JsonManager Instance => instance;

    private JsonManager() { }
    //存储Json数据 序列化
    public void SaveData(object data,string fileName,JsonType type=JsonType.LitJson)
    {
        //确定存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        Debug.Log(path);
        //序列化 得到Json字符串
        string jsonStr = "";
        switch(type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data,true);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        //把序列化的Json字符串 存储到制定路径的文件中
        File.WriteAllText(path, jsonStr);
    }
    //读取指定文件中的Json数据 反序列化
    public T LoadData<T>(string fileName,JsonType type=JsonType.LitJson) where T:new()
    {
        //判断 默认数据文件夹中是否存在数据，存在则获取
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //判断默认文件夹中是否存在该文件
        if (!File.Exists(path))
        {
            //默认文件夹中不存在该文件，则从读写文件夹中寻找
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        //倘若读写文件夹中也不存在该文件，则返回默认对象
        if(!File.Exists(path))
        {
            return new T();
        }
        //经过以上检查后就可以进行反序列化
        string JsonStr = File.ReadAllText(path);
        //数据对象
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(JsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(JsonStr);
                break;
        }
        return data;
    }

}
