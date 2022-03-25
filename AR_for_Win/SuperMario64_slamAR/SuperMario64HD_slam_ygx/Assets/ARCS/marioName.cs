using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marioName : MonoBehaviour
{
    private GUIStyle fontStyle = new GUIStyle();
    private Transform namePoint;
    public Camera playerCamera;

    string ygx = "";
    // Start is called before the first frame update
    void Start()
    {
        // 字体设置
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.fontSize = 18;       //字体大小
        fontStyle.fontStyle = FontStyle.Bold;
        fontStyle.wordWrap = true;
        namePoint = gameObject.transform.Find("namePoint");

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            ygx = "";
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            ygx = "大家好呀";
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            ygx = "我是super马里奥";
        }
    }

    // 字体描边
    private void fontStroke(Vector2 uiposition, string txtString, Color txtColor, Color outlineColor, int outlineWidth)
    {
        Vector2 nameSize = fontStyle.CalcSize(new GUIContent(ygx));
        Rect position = new Rect(uiposition.x - (nameSize.x / 2), uiposition.y - nameSize.y - 5.0f, nameSize.x, nameSize.y);
        fontStyle.normal.textColor = outlineColor;

        position.y -= outlineWidth;
        GUI.color = outlineColor;
        GUI.Label(position, txtString, fontStyle);
        position.y += outlineWidth * 2;
        GUI.Label(position, txtString, fontStyle);
        position.y -= outlineWidth;
        position.x -= outlineWidth;
        GUI.Label(position, txtString, fontStyle);
        position.x += outlineWidth * 2;
        GUI.Label(position, txtString, fontStyle);
        position.x -= outlineWidth;
        fontStyle.normal.textColor = txtColor;
        GUI.Label(position, txtString, fontStyle);
    }

    public void OnGUI()
    {
       Vector3 worldPosition = new Vector3(namePoint.position.x, namePoint.position.y + 0.0f, namePoint.position.z);


       //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
       Vector2 uiposition = playerCamera.WorldToScreenPoint(worldPosition);

       //得到真实NPC头顶的2D坐标
       uiposition = new Vector2(uiposition.x, Screen.height - uiposition.y);

       //entity_name = "打火机登记卡是环境萨等不及氨基酸可等不及";
       //计算NPC名称的宽高
       Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(ygx));

       //设置显示颜色为黄色
       GUI.color = Color.blue;

       //绘制NPC名称
       fontStroke(uiposition, ygx, Color.black, Color.white, 2);
    }

}