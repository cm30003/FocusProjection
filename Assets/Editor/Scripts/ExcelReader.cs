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
    private enum Cloth_ExcelTitleEnum
    {
        ID,//ID
        Name,//名字
        Description,//描述
        Cost,//价格
        Sprite_ResPath,//资源路径
    }
    private enum Plant_ExcelTitleEnum
    {
        ID,
        Name,
        Description,
        Cost,
        Sell_Price,//出售价格
        Germinate_Time,//发芽时间
        Grown_Time,//成长时间
        Mature_Time,//成熟时间
        Plant_Time,//种植时间
        Water_Time,//浇水时间
        fertilize_Time,//施肥时间
        BugControl_Time,//虫子控制时间
        Harvest_Time,//收获时间
        Harvest_Num,//数量
        Germainate_SpriteResPath,//发芽图片资源路径
        Grown_SpriteResPath,//生长图片资源路径
        Mature_SpriteResPath,//资源路径
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
            ExcelWorksheet workSheet_Cloth = excel.Workbook.Worksheets[4];
            ExcelWorksheet workSheet_Plant = excel.Workbook.Worksheets[5];
            //Debug.Log(excel.Workbook.Worksheets[5].Cells[4, 1].Text);

            NPC_DataJson_Create(workSheet_NPCData);
            FoodDataJson_Create(workSheet_Food);
            Gift_DataJson_Create(workSheet_Gift);
            Cloth_DataJson_Create(workSheet_Cloth);
            Plant_DataJson_Create(workSheet_Plant);
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
    /// <summary>
    /// 创建商店礼物数据Gift_Data.json文件
    /// </summary>
    /// <param name="worksheet"></param>
    private static void Gift_DataJson_Create(ExcelWorksheet worksheet)
    {
        int StartRow = 4, StartCol = 1;//起始行列

        List<GiftData> list = new List<GiftData>();
        //将读取到的Excel数据填充到相应的可序列化字段中
        for (int i = StartRow; i < worksheet.Dimension.Rows-3; i++)
        {
            GiftData giftData = new GiftData();

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
    /// <summary>
    /// 创建商城服装数据Cloth_Data.json文件
    /// </summary>
    /// <param name="worksheet"></param>
    private static void Cloth_DataJson_Create(ExcelWorksheet worksheet)
    {
        int StartRow = 4, StartCol = 1;//起始行列

        List<ClothItem_Data> list = new List<ClothItem_Data>();

        //将读取到的Excel数据填充到相应的可序列化字段中
        for (int i = StartRow; i < worksheet.Dimension.Rows; i++)
        {
            //Debug.Log(worksheet.Cells[i, StartCol + (int)Cloth_ExcelTitleEnum.Cost].Text);

            ClothItem_Data ClothData = new ClothItem_Data();

            ClothData.ID = int.Parse(worksheet.Cells[i, StartCol].Text);
            ClothData.Name = worksheet.Cells[i, StartCol + (int)Cloth_ExcelTitleEnum.Name].Text;
            ClothData.Description = worksheet.Cells[i, StartCol + (int)Cloth_ExcelTitleEnum.Description].Text;
            ClothData.Cost = int.Parse(worksheet.Cells[i, StartCol + (int)Cloth_ExcelTitleEnum.Cost].Text);
            
            ClothData.ResPath = worksheet.Cells[i, StartCol + (int)Cloth_ExcelTitleEnum.Sprite_ResPath].Text;

            list.Add(ClothData);

            //Debug.Log(i);
        }
        //将可序列化数据转为Json数据并保存到目标路径
        string savePath = Path.Combine(Application.dataPath, "Resources/JsonDataAsset/Cloth_Data.json");
        //检测路径是否存在
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }
        File.WriteAllText(savePath, JsonConvert.SerializeObject(list, Formatting.Indented));

        Debug.Log("生成JsonData于：" + savePath);
    }
    /// <summary>
    /// 创建植物数据Plant_Data.json文件
    /// </summary>
    /// <param name="worksheet"></param>
    private static void Plant_DataJson_Create(ExcelWorksheet worksheet)
    {
        int StartRow = 4, StartCol = 1;//起始行列

        List<PlantItem_Data> list = new List<PlantItem_Data>();

        //将读取到的Excel数据填充到相应的可序列化字段中
        for (int i = StartRow; i < worksheet.Dimension.Rows-9; i++)
        {
            PlantItem_Data plantData = new PlantItem_Data();
            
            Debug.Log(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Name].Text);
            plantData.ID = int.Parse(worksheet.Cells[i, StartCol].Text);
            plantData.Name = worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Name].Text;
            plantData.Description = worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Description].Text;
            plantData.Cost = int.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Cost].Text);
            plantData.Sell_Price = int.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Sell_Price].Text);
            plantData.Germinate_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Germinate_Time].Text);
            plantData.Grown_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Grown_Time].Text);
            plantData.Mature_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Mature_Time].Text);
            plantData.Plant_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Plant_Time].Text);
            plantData.Water_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Water_Time].Text);
            plantData.fertilize_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.fertilize_Time].Text);
            plantData.BugControl_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.BugControl_Time].Text);
            plantData.Harvest_Time = float.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Harvest_Time].Text);
            plantData.Harvest_Num = int.Parse(worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Harvest_Num].Text);
            plantData.Germinate_SpriteResPath = worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Germainate_SpriteResPath].Text;
            plantData.Grown_SpriteResPath = worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Grown_SpriteResPath].Text;
            plantData.Mature_SpriteResPath = worksheet.Cells[i, StartCol + (int)Plant_ExcelTitleEnum.Mature_SpriteResPath].Text;

            list.Add(plantData);

            Debug.Log(i);
        }
        //将可序列化数据转为Json数据并保存到目标路径
        string savePath = Path.Combine(Application.dataPath, "Resources/JsonDataAsset/Plant_Data.json");
        //检测路径是否存在
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }
        File.WriteAllText(savePath, JsonConvert.SerializeObject(list, Formatting.Indented));

        Debug.Log("生成JsonData于：" + savePath);
    }
}
