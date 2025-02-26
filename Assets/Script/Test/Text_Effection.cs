using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public enum EffectType
{
    typeWriter=0,//打字机
    Floating=1,//浮动
    Italic=2,//斜体
}


//RequireComponent：保证对象拥有该组件
//DisallowMultipleComponent：保证对象不存在多个该组件
[RequireComponent(typeof(TextMeshProUGUI)), DisallowMultipleComponent]
public class Text_Effection : MonoBehaviour
{
    public TMP_Text m_text;
    [Range(0, 1)] public float speed = 1;//打字机显示速度
    [SerializeField]
    private EffectType effectType = EffectType.typeWriter;
    private int characterCount = 0;//Text总字符数


    [Header("浮动")]
    [SerializeField, Range(1, 5)]
    private float frequence = 1.0f;//浮动频率
    [SerializeField, Range(1, 5), Tooltip("浮动范围")]
    private float floatRange = 1.0f;//浮动范围

    [Header("斜体效果的相关设置"), Space]
    [SerializeField, Range(0, 60), Tooltip("斜体角度X")]
    private float slopXAngle = 0f;
    [SerializeField,Range(0,45),Tooltip("斜体角度Y")]
    private float slopYAngle = 0f;
    [SerializeField,Tooltip("勾选该选项，使字体向另一个方向偏转")]
    private bool reverseDirection = false;
    private Vector3[] originalVertices;//Text的原始顶点信息
    private void Awake()
    {
        gameObject.TryGetComponent<TMP_Text>(out m_text);//获取组件并为m_text赋值
    }
    private void Start()
    {
        if(m_text==null)//如果m_text为空，则创建一个TMP_Text组件，并为m_text赋值
        {
            gameObject.AddComponent<TMP_Text>();
            gameObject.TryGetComponent<TMP_Text>(out m_text);
        }
        //强制更新网格数据
        m_text.ForceMeshUpdate();
        //获取Text总字符数量
        characterCount=m_text.textInfo.characterCount;
        //拷贝原始顶点信息
        originalVertices=m_text.textInfo.meshInfo[0].vertices;

        StartCoroutine(MainBody());
    }
    private IEnumerator MainBody()
    {
        while(true) 
        {
            //强制更新网格数据
            m_text.ForceMeshUpdate();
            //存储要显示的文字及其网格信息
            TMP_TextInfo textInfo = m_text.textInfo;
            //拷贝网格的顶点数据
            TMP_MeshInfo[] textInfoCopy = textInfo.CopyMeshInfoVertexData();
            //字符总数量
            characterCount = textInfo.characterCount;
            for (int i = 0; i < characterCount; i++)//获取每个文字的顶点信息，并做相应修改
            {
                //存储每个文字的详细信息
                TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];
                //获取当前角色使用的材质索引
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                //获取此文本呢元素使用的第一个顶点的索引
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                //获取文本元素使用的网格的顶点颜色
                Color32[] vertexColor = textInfo.meshInfo[characterInfo.materialReferenceIndex].colors32;
                //vertices: 数组，包含文本中所有字符的顶点位置信息。
                //每个字符通常有四个顶点（左下、左上、右上、右下），这些顶点用于定义字符的几何形状和位置。
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
                Vector3[] Copyvertices = textInfoCopy[materialIndex].vertices;
                //跳过不可见的字符，当字符不可见时，不执行任何操作
                if(!characterInfo.isVisible)
                {
                    continue;
                }
                else
                {
                    switch (effectType)
                    {
                        case EffectType.typeWriter:
                            yield return StartCoroutine(TypeWriter());
                            break;
                        case EffectType.Floating:
                            Floating(vertexIndex,ref vertices);
                            break;
                        case EffectType.Italic:
                            Italic(vertexIndex,vertices);
                            break;
                        default:
                            break;
                    }
                }
            }
            for(int i=0;i<textInfo.meshInfo.Length;i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                //更新集合网格数据
                m_text.UpdateGeometry(meshInfo.mesh, i);
            }
            //更新顶点数据
            m_text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return null;
        }
    }
    private IEnumerator TypeWriter()//打字机文字显示效果
    {
        m_text.ForceMeshUpdate();//更新文字Mesh
        TMP_TextInfo textInfo = m_text.textInfo;//获取文字信息
        int TotalNum=textInfo.characterCount;//获取文本对象中的文字数量
        bool compelete = false;//是否打印完成
        int currentNum = 0;//当前已显示的文字数量
        //以下为打字机逻辑
        while (!compelete)//当文本未打印完成时
        {
            if(currentNum>TotalNum)//当已显示的文字数量大于总文字数量时
            {
                currentNum=TotalNum;
                yield return new WaitForSeconds(1);
                compelete = true;//完成打印，跳出循环
            }
            //maxVisibleCharacters:控制文本中可见字符的最大数量
            m_text.maxVisibleCharacters = currentNum;
            currentNum += 1;
            yield return new WaitForSeconds(speed);
        }
        yield return null;
    }
    /// <summary>
    /// 浮动文字
    /// </summary>
    /// <param name="VertexIndex">顶点索引值</param>
    /// <param name="vertices">存储顶点的数组</param>
    private void Floating(int VertexIndex,ref Vector3[] vertices)
    {
        for(int i=0;i<4;i++)//每个字体网格只有四个顶点
        {
            Vector3 originalValue= vertices[VertexIndex+i];//存储文字当前的顶点信息
            //使用三角函数实现浮动
            //Mathf.PI 是 Unity 中 Mathf 类的一个静态常量，表示圆周率 π 的值。
            float VerticesYPosition = Mathf.Sin(Time.time*frequence*Mathf.PI+originalValue.x)*floatRange;
            vertices[VertexIndex+i]=originalValue+new Vector3(0,VerticesYPosition,0);
        }
    }
    /// <summary>
    /// 斜体文字
    /// </summary>
    /// <param name="VertexIndex">顶点索引值</param>
    /// <param name="vertices">存储顶点的数组</param>
    public void Italic(int VertexIndex,Vector3[] vertices)
    {
        for(int i=0;i<4;i++)
        {
            //角度转弧度
            float XAngle = slopXAngle * (Mathf.PI/180);
            float YAngle = slopYAngle * (Mathf.PI/180);


            vertices[VertexIndex + i] = originalVertices[VertexIndex+i]+(reverseDirection?(-1):1)
                *new Vector3(originalVertices[VertexIndex + i].y * Mathf.Tan(XAngle), 
                originalVertices[VertexIndex+i].x*Mathf.Tan(YAngle),0);
        }
    }
}
