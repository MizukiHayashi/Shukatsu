using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageTimer : MonoBehaviour {


    private Text m_Text;

    //コロン
    private string coron;
    //カウントストップフラグ
    private bool isCountStop;

    //経過時間
    private float timeCount;

    private Scene_Manager_ scene_Manager;
    private Scene_Manager_Fade scene_Fade;

    private BossPlusHP plusHP;

    private float timeleft;

    void Start()
    {
        if (gameObject.GetComponent<Text>() == null) gameObject.AddComponent<Text>();
        isCountStop = false;
        m_Text = GetComponent<Text>();
        timeCount = 0;
        coron = " : ";

        plusHP = GameObject.Find("BossPlusHP").GetComponent<BossPlusHP>();
        scene_Manager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        scene_Fade = Camera.main.GetComponent<Scene_Manager_Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCountStop == false)
        {
            timeCount += Time.deltaTime;
            timeleft -= Time.deltaTime;
        }

        //経過時間表示
        m_Text.text = ConvertTime(timeCount);


    }

    //秒を分と秒に分けて表示するために変換
    private string ConvertTime(float timeLimit)
    {
        //分
        int min = (int)timeCount / 60;
        //秒
        float sec = timeCount - (min * 60);

        
        if (timeleft <= 0.0f)
        {
            timeleft = 1.0f;

            if (min < 2)
            {
                plusHP.PlusHPUp(1.0f);
            }
            else
            {
                plusHP.PlusHPUp(min);
            }
        }
        return "[Time]" + min + coron + string.Format("{0:00}", (int)sec);
    }

    //カウントを止める
    public void CountStop(bool isStop)
    {
        isCountStop = isStop;
    }

    //クリア時間を渡す
    public float GetClearTime()
    {
        return timeCount;
    }
}
