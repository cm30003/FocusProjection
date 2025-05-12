using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DayAndNight : MonoBehaviour
{
    //2D灯光
    public Light2D Globallight;
    public List<Light2D> light2Ds;
    //后处理
    public VolumeProfile volume;
    public Bloom bloom;
    //背景音乐
    public string BKMusic_Name;
    public string Current_BKMusic_Name;
    private void Start()
    {
        volume.TryGet(out bloom);
        EventCenter.GetInstance().AddEventListener("Day_Night",Day_Night);
        AudioManager.GetInstance().PlayBKMusic(BKMusic_Name);
    }
    private void Update()
    {
        Day_Night();
    }
    public void Day_Night()
    {
        int hour=DateTime.Now.Hour;
        if (hour >= 6 && hour < 18)
        {
            Globallight.intensity = 1f;
            bloom.intensity.value = 0.25f;
            BKMusic_Name = "德国傍晚的休闲广场";

            for (int i=0;i<light2Ds.Count;i++)
            {
                light2Ds[i].enabled = false;
            }
        }
        else
        {
            Globallight.intensity = 0.65f;
            bloom.intensity.value = 2f;
            BKMusic_Name = "德国傍晚的休闲广场";

            for (int i = 0; i < light2Ds.Count; i++)
            {
                light2Ds[i].enabled = true;
            }
        }
    }
    //public void Change_BKMusic()
    //{
    //    if (BKMusic_Name!=Current_BKMusic_Name)
    //    {
    //        Current_BKMusic_Name = BKMusic_Name;
    //        AudioManager.GetInstance().PlayBKMusic(BKMusic_Name);
    //    }
        
    //}
}
