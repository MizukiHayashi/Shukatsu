using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawClearTime : MonoBehaviour
{
    //テキスト
    private Text m_Text;

    //クリアタイム
    private float clearTime;
    //コロン
    private string coron;

    // Use this for initialization
    void Start()
    {
        m_Text = GetComponent<Text>();
        try
        {
            clearTime = GameObject.Find("TimeTex").GetComponent<TimeLimit>().GetClearTime();
        }
        catch { }
        coron = " : ";
        m_Text.text = ConvertTime(clearTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //秒を分と秒に分けて表示するために変換
    private string ConvertTime(float timeLimit)
    {
        //分
        int min = (int)clearTime / 60;
        //秒
        float sec = clearTime - (min * 60);

        return min + coron + string.Format("{0:00}", (int)sec);
    }
}
