using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeController : MonoBehaviour
{
    //選択タイプ
    public enum SelectType
    {
        Stage,         //ステージ
        Tutorial_Y,    //チュートリアルプレイ
        Tutorial_N     //チュートリアルプレイしない
    }

    public SelectType type;
    public bool isHitRay;           //レイが当たってるか
    public Scene_State nextScene;   //次のシーンステート
    [SerializeField]
    private Image gauge;            //ゲージイメージ
    private float timer;            //ゲージチャージまでの時間

    private bool isChange;          //シーン遷移開始
    private bool isSelect_No;        //チュートリアルプレイしないを選択したか

    private Scene_Manager_ scene_manager;
    private SoundsManager sounds_manager;
    private TVModelController tv_cont;
    [SerializeField]
    private GameObject iconLorR;

    // Use this for initialization
    void Start()
    {
        //各変数の初期化
        isHitRay = false;
        isChange = false;
        isSelect_No = false;
        timer = 1.5f;

        scene_manager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        sounds_manager = GameObject.Find("ScriptManager").GetComponent<SoundsManager>();
        tv_cont = GameObject.Find("TVModelContObj").GetComponent<TVModelController>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKey(KeyCode.C)) isHitRay = true;
        //選択タイプがステージだったら
        if (type == SelectType.Stage)
        {
            //レイが当たっていたら
            if (isHitRay == true)
            {
                //ゲージを進める
                gauge.fillAmount += 1 - Mathf.Clamp01((1.5f - Time.deltaTime) / 1.5f);
                timer -= Time.deltaTime;
                //時間が０になるかつ遷移が始まっていなければ
                if (timer < 0.0f && isChange == false)
                {
                    sounds_manager.PlaySE("Beam", 1);
                    scene_manager.SelectStage(nextScene);
                    isChange = true;
                    tv_cont.OnCompleteGauge_Stage();
                }
            }
            else
            {
                //初期化
                gauge.fillAmount = 0;
                timer = 1.5f;
            }
        }
        //選択タイプがチュートリアルをプレイだったら
        if (type == SelectType.Tutorial_Y)
        {
            //レイが当たっていたら
            if (isHitRay == true)
            {
                //ゲージを進める
                gauge.fillAmount += 1 - Mathf.Clamp01((1.5f - Time.deltaTime) / 1.5f);
                timer -= Time.deltaTime;
                //時間が０になるかつ遷移が始まっていなければ
                if (timer < 0.0f && isChange == false)
                {
                    sounds_manager.PlaySE("Beam", 1);
                    scene_manager.SelectStage(nextScene);
                    iconLorR.SetActive(false);
                    isChange = true;
                    tv_cont.OnCompleteGauge_TutoY();
                }
            }
            else
            {
                gauge.fillAmount = 0;
                timer = 1.5f;
            }
        }
        //選択タイプがチュートリアルをプレイしないだったら
        if (type == SelectType.Tutorial_N)
        {
            //レイが当たっていたら
            if (isHitRay == true)
            {
                //ゲージを進める
                gauge.fillAmount += 1 - Mathf.Clamp01((1.5f - Time.deltaTime) / 1.5f);
                timer -= Time.deltaTime;
                if (timer < 0.0f && isSelect_No == false)
                {
                    sounds_manager.PlaySE("Beam", 1);
                    iconLorR.SetActive(false);
                    tv_cont.OnCompleteGauge_TutoN();
                    isSelect_No = true;
                }
            }
            else
            {
                gauge.fillAmount = 0;
                timer = 1.5f;
            }
        }
        isHitRay = false;
    }
}
