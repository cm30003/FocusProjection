using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 新的UI基类
/// 帮助开发者通过在代码中快速获取控件脚本
/// 方便在子类中处理逻辑
/// 节约找到控件的时间
/// </summary>
public class NewUIBase : MonoBehaviour
{
    //通过里式交换原则，用字典存储所有控件
    //里式交换原则：可替换性：子类对象应该能够替换父类对象而不会影响程序的正确性。
    //行为一致性：子类应该遵循父类的契约，即子类应该实现父类定义的行为，而不能改变父类的行为。
    private Dictionary<string,List<UIBehaviour>>ControlDic=new Dictionary<string, List<UIBehaviour>>();

    protected Button Current_Button;
    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<InputField>();
    }
    /// <summary>
    /// 显示控件
    /// </summary>
    public virtual void ShowMe()
    {

    }
    /// <summary>
    /// 隐藏控件
    /// </summary>
    public virtual void HideMe()
    {

    }
    protected virtual void OnClick(string ButtonName,Button button)
    {
        
    }
    /// <summary>
    /// 得到对应名字的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Controlname">名字</param>
    /// <returns></returns>
    protected T GetControl<T>(string Controlname) where T : UIBehaviour
    {
        if(ControlDic.ContainsKey(Controlname))
        {
            for(int i = 0; i < ControlDic[Controlname].Count;i++)
            {
                if (ControlDic[Controlname][i]is T)
                {
                    return ControlDic[Controlname][i] as T;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 得到对应名字的对应控件（在这里是Button/Image等）
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        //得到所有子控件
        T[] Controls=this.GetComponentsInChildren<T>();
        //遍历所有子控件
        for(int i=0;i<Controls.Length;i++)
        {
            string ObjName =Controls[i].gameObject.name;
            if(ControlDic.ContainsKey(ObjName))//如果字典中包含这个控件的名字
            {
                //
                ControlDic[ObjName].Add(Controls[i]);
            }
            else
            {
                ControlDic.Add(ObjName,new List<UIBehaviour>() { Controls[i] });
            }
            //如果获取的控件是Button，那么就添加点击事件
            if(Controls[i] is Button)
            {
                Button button = Controls[i] as Button;
                (Controls[i] as Button).onClick.AddListener(()=>
                {
                    OnClick(ObjName, button);
                });
            }
        }
    }

    /// <summary>
    /// 被选中的按钮转换为选中状态
    /// </summary>
    /// <param name="ButtonGroup">按钮所属的按钮组</param>
    /// <param name="Clicked_Image">按钮被点击后的图片</param>
    /// <param name="Start_Image">按钮的初始图片</param>
    protected void Button_Image_Change(GameObject ButtonGroup, Sprite Clicked_Image, Sprite Start_Image)
    {
        for (int i = 0; i < ButtonGroup.transform.childCount; i++)
        {
            Button button = ButtonGroup.transform.GetChild(i).GetComponent<Button>();
            if (button == Current_Button)
            {
                button.image.sprite = Clicked_Image;
            }
            else if (button.image.sprite != Start_Image)
            {
                button.image.sprite = Start_Image;
            }
        }
    }
}
