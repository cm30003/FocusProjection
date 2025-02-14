using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    protected Button Current_Button;
    protected void CloseUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    protected void OpenUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// ��ѡ�еİ�ťת��Ϊѡ��״̬
    /// </summary>
    /// <param name="ButtonGroup">��ť�����İ�ť��</param>
    /// <param name="Clicked_Image">��ť��������ͼƬ</param>
    /// <param name="Start_Image">��ť�ĳ�ʼͼƬ</param>
    protected void Button_Image_Change(GameObject ButtonGroup, Sprite Clicked_Image, Sprite Start_Image)
    {
        for (int i = 0; i < ButtonGroup.transform.childCount; i++)
        {
            Button button = ButtonGroup.transform.GetChild(i).GetComponent<Button>();
            if (button == Current_Button)
            {
                button.image.sprite = Clicked_Image;
            }
            else if(button.image.sprite != Start_Image)
            {
                button.image.sprite = Start_Image;
            }
        }
    }
}
