using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopTool : MonoBehaviour
{
    public List<PlantData> plantDatas;

    public GameObject[] CharaController;

    public TextMeshProUGUI Text;
    private void Start()
    {
        plantDatas = ObjectKeeper_Singleton.Instance.gamerData.Items;

        CharaController=ObjectKeeper_Singleton.Instance.NPCs;
    }
    private void Update()
    {
        
    }
}
