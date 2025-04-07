using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using UnityEditor;
using Unity.Plastic.Newtonsoft.Json;
using NUnit.Framework;
using PlasticPipe.PlasticProtocol.Client;

public class ExcelReader : Editor
{
    private enum Food_ExcelTitleEnum
    {
        ID,//ID
        Name,//名字
        Description,//描述
        Cost,//价格
        Buff,// buff
        ResPath,//资源路径
    }
    private enum NcpData_ExcelTitleEnum
    {
        ID,//ID
        Name,//名字
        Hobby,//爱好
        Personality,//个性
        Description,//描述
        Birthday,//生日
        Favorability,// 好感度
        FavorvateThing,//喜欢的事务
        MoveSpeed,//移动速度
        WorkSpeed,//工作速度
        Work_Time,//工作时间/偷懒结算倒数时间
        Eat_Time,//吃饭时间
        Hungry_Time,//饥饿倒数时间
        Res,//NPC图片资源路径
    }
    private enum Gift_ExcelTitleEnum
    {
        ID,//ID
        Name,//名字
        Description,//描述
        Cost,//价格
        favorability_Plus_Num,//好感度加值
        Sprite_ResPath,//资源路径
    }
    [MenuItem("Tools/CreatAsset_From_Excel")]
    static void CreateAssets_Form_Excel()
    {
        //excel的文件路径
        string path = Path.Combine(Application.dataPath, "Editor/Data_Table.xlsx");
        //获取文件信息
        FileInfo fileInfo = new FileInfo(path);

        using (ExcelPackage excel = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet workSheet_NPCData = excel.Workbook.Worksheets[1];
            ExcelWorksheet workSheet_Food = excel.Workbook.Worksheets[2];
            ExcelWorksheet workSheet_Gift= excel.Workbook.Worksheets[3];

            NPC_DataJson_Create(workSheet_NPCData);
            FoodDataJson_Create(workSheet_Food);
            Gift_DataJson_Create(workSheet_Gift);

        }
    }
    /// <summary>
    /// 创建食物数据FoodItem_Data.json文件
    /// </summary>
    /// <param name="worksheet"></param>
    private static void FoodDataJson_Create(ExcelWorksheet worksheet)
    {
        int StartRow = 4,StartCol=1;//起始行列

        List<FoodItem_Data> list = new List<FoodItem_Data>();
        //将读取到的Excel数据填充到相应的可序列化字段中
        for(int i=StartRow;i<worksheet.Dimension.Rows-3;i++)
        {
            FoodItem_Data foodItem_Data = new FoodItem_Data();
            foodItem_Data.ID = int.Parse(worksheet.Cells[i, StartCol].Text);
            foodItem_Data.Name = worksheet.Cells[i, StartCol + (int)Food_ExcelTitleEnum.Name].Text;
            foodItem_Data.Description = worksheet.Cells[i, StartCol + (int)Food_ExcelTitleEnum.Description].Text;
            foodItem_Data.Cost = int.Parse(worksheet.Cells[i, StartCol + (int)Food_ExcelTitleEnum.Cost].Text);
            foodItem_Data.Buff = float.Parse(worksheet.Cells[i, StartCol + (int)Food_ExcelTitleEnum.Buff].Text);
            foodItem_Data.ResPath = worksheet.Cells[i, StartCol + (int)Food_ExcelTitleEnum.ResPath].Text;

            list.Add(foodItem_Data);
        }
        //将可序列化数据转为Json数据并保存到目标路径
        string savePath = Path.Combine(Application.dataPath, "Resources/JsonDataAsset/FoodItem_Data.json");
        //检测路径是否存在
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }
        //
        File.WriteAllText(savePath, JsonConvert.SerializeObject(list,Formatting.Indented));

        Debug.Log("生成JsonData于："+savePath);
    }
    /// <summary>
    /// 创建NPC数据NPCData.json文件
    /// </summary>
    /// <param name="worksheet"></param>
    private static void NPC_DataJson_Create(ExcelWorksheet worksheet)
    {
        int StartRow = 4, StartCol = 1;//起始行列

        List<NPCData> list = new List<NPCData>();
        //将读取到的Excel数据填充到相应的可序列化字段中
        for (int i = StartRow; i < worksheet.Dimension.Rows+1; i++)
        {
            NPCData npc_data = new NPCData();

            npc_data.ID = int.Parse(worksheet.Cells[i, StartCol].Text);
            npc_data.Name = worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Name].Text;
            npc_data.Hobby = worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Hobby].Text;
            npc_data.Personality = worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Personality].Text;
            npc_data.Description = worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Description].Text;
            npc_data.BirthDay = worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Birthday].Text;
            npc_data.Favorability = int.Parse(worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Favorability].Text);
            npc_data.FavorvateThing = worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.FavorvateThing].Text;
            npc_data.MoveSpeed = float.Parse(worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.MoveSpeed].Text);
            npc_data.Work_Speed = float.Parse(worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.WorkSpeed].Text);
            npc_data.Work_Time = float.Parse(worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Work_Time].Text);
            npc_data.Eat_Time = float.Parse(worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Eat_Time].Text);
            npc_data.Hungry_Time = float.Parse(worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Hungry_Time].Text);
            npc_data.Sprite_Res = worksheet.Cells[i, StartCol + (int)NcpData_ExcelTitleEnum.Res].Text;

            list.Add(npc_data);
        }
        //将可序列化数据转为Json数据并保存到目标路径
        string savePath = Path.Combine(Application.dataPath, "Resources/JsonDataAsset/NPC_Data.json");
        //检测路径是否存在
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }
        //
        File.WriteAllText(savePath, JsonConvert.SerializeObject(list, Formatting.Indented));

        Debug.Log("生成JsonData于：" + savePath);
    }

    private static void Gift_DataJson_Create(ExcelWorksheet worksheet)
    {
        int StartRow = 4, StartCol = 1;//起始行列

        List<GiftData> list = new List<GiftData>();
        //将读取到的Excel数据填充到相应的可序列化字段中
        for (int i = StartRow; i < worksheet.Dimension.Rows-3; i++)
        {
            GiftData giftData = new GiftData();

            Debug.Log(worksheet.Cells[i, StartCol].Text);
            giftData.ID = int.Parse(worksheet.Cells[i, StartCol].Text);
            giftData.Name = worksheet.Cells[i, StartCol + (int)Gift_ExcelTitleEnum.Name].Text;
            giftData.Description = worksheet.Cells[i, StartCol + (int)Gift_ExcelTitleEnum.Description].Text;
            giftData.Cost = int.Parse(worksheet.Cells[i, StartCol + (int)Gift_ExcelTitleEnum.Cost].Text);
            giftData.favorability_Plus_Num = int.Parse(worksheet.Cells[i, StartCol + (int)Gift_ExcelTitleEnum.favorability_Plus_Num].Text);
            giftData.Sprite_ResPath = worksheet.Cells[i, StartCol + (int)Gift_ExcelTitleEnum.Sprite_ResPath].Text;

            list.Add(giftData);
        }
        //将可序列化数据转为Json数据并保存到目标路径
        string savePath = Path.Combine(Application.dataPath, "Resources/JsonDataAsset/Gift_Data.json");
        //检测路径是否存在
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }
        File.WriteAllText(savePath, JsonConvert.SerializeObject(list, Formatting.Indented));

        Debug.Log("生成JsonData于：" + savePath);
    }
}
