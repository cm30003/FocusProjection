using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Phone : NewUIBase
{
    protected override void OnClick(string btnName,Button button)
    {
        switch (btnName)
        {
            case "Calendar_Button":
                UIManager.GetInstance().ShowNewUI<Phone_Calendar>("Phone_Calendar");
                break;
            case "Shop_Button":
                UIManager.GetInstance().ShowNewUI<Phone_Shop>("Phone_Shop");
                break;
            case "Dinning_Button":
                UIManager.GetInstance().ShowNewUI<Phone_DinningHall>("Phone_DiningHall");
                break;
            case "WareHouse_Button":
                UIManager.GetInstance().ShowNewUI<Phone_WareHouse>("Phone_WareHouse");
                break;
            case "PlantManager_Button":
                UIManager.GetInstance().ShowNewUI<Phone_PlantManager>("Phone_PlantManager");
                break;
            case "NPCManager_Button":
                UIManager.GetInstance().ShowNewUI<Phone_NPCManager>("Phone_NPCManager");
                break;

            case "PlayerCard_Button":
                UIManager.GetInstance().ShowOldUI<IDCardUI>("！！Player IDCard！！");
                break;
            case "Settings_Button":
                UIManager.GetInstance().ShowOldUI<SystemMenueUI>("！！System Menue UI CanvasGroup！！");
                break;
            case "Mail_Button":
                UIManager.GetInstance().ShowOldUI<MailUI>("！！Mail！！");
                break;
        }

    }
}
