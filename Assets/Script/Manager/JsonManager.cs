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
/// Json���ݹ����࣬json���л��ͷ����л� �洢��Ӳ��
/// </summary>
public class JsonManager 
{
    private static JsonManager instance=new JsonManager();

    public static JsonManager Instance => instance;

    private JsonManager() { }
    //�洢Json���� ���л�
    public void SaveData(object data,string fileName,JsonType type=JsonType.LitJson)
    {
        //ȷ���洢·��
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        Debug.Log(path);
        //���л� �õ�Json�ַ���
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
        //�����л���Json�ַ��� �洢���ƶ�·�����ļ���
        File.WriteAllText(path, jsonStr);
    }
    //��ȡָ���ļ��е�Json���� �����л�
    public T LoadData<T>(string fileName,JsonType type=JsonType.LitJson) where T:new()
    {
        //�ж� Ĭ�������ļ������Ƿ�������ݣ��������ȡ
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //�ж�Ĭ���ļ������Ƿ���ڸ��ļ�
        if (!File.Exists(path))
        {
            //Ĭ���ļ����в����ڸ��ļ�����Ӷ�д�ļ�����Ѱ��
            path = Application.persistentDataPath + "/" + fileName + ".json";
        }
        //������д�ļ�����Ҳ�����ڸ��ļ����򷵻�Ĭ�϶���
        if(!File.Exists(path))
        {
            return new T();
        }
        //�������ϼ���Ϳ��Խ��з����л�
        string JsonStr = File.ReadAllText(path);
        //���ݶ���
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
