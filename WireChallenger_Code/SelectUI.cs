using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UIの選択処理
public class SelectUI : MonoBehaviour
{
    //UIの種類
    public enum SelectUIType
    {
        Resume,     //再開
        TryAgain,   //最初から
        BackToMenu, //メニューに戻る
        NextStage,  //次のステージへ
    }

    public SelectUIType selectType = SelectUIType.Resume;

    public bool isHitRay;   //レイが当たっているか
    [SerializeField]
    private Image gauge;    //ゲージ

    private float timer;    //ゲージチャージタイマー

    private bool isChange;  //シーン遷移開始

    private Scene_Manager_Fade scene_Fade;
    private Scene_Manager_ scene_Manager;
    private SoundsManager sounds_Manager;

    private PlayerAbility ability;
    private BossPlusHP plusHP;

    // Use this for initialization
    void Start()
    {
        //各変数の初期化
        isHitRay = false;
        timer = 1.5f;
        scene_Fade = Camera.main.GetComponent<Scene_Manager_Fade>();
        scene_Manager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        sounds_Manager = GameObject.Find("ScriptManager").GetComponent<SoundsManager>();

        ability = GameObject.Find("PlayerAbility").GetComponent<PlayerAbility>();
        plusHP = GameObject.Find("BossPlusHP").GetComponent<BossPlusHP>();
    }

    // Update is called once per frame
    void Update()
    {
        //UIの種類ごとに処理
        switch (selectType)
        {
            case SelectUIType.Resume:
                SelectResume();
                break;
            case SelectUIType.TryAgain:
                SelectTryAgain();
                break;
            case SelectUIType.BackToMenu:
                SelectBackToMenu();
                break;
            case SelectUIType.NextStage:
                SelectNextStage();
                break;
        }

        isHitRay = false;
    }

    //Resume選択時
    private void SelectResume()
    {
        //レイが当たっていれば
        if (isHitRay == true)
        {
            //ゲージを溜める
            gauge.fillAmount += 1 - Mathf.Clamp01((1.5f - Time.deltaTime) / 1.5f);
            timer -= Time.deltaTime;
            //ゲージが溜まり切りかつシーン遷移が開始されていなければ
            if (timer < 0.0f && isChange == false)
            {
                sounds_Manager.PlaySE("Beam", 1);
                //ポーズ画面を閉じる
                scene_Manager.SelectPause();
                isChange = true;
                GameObject.FindGameObjectWithTag(TagName.PLAYER).GetComponent<VR_PlayerWireAction>().MenuModeChange(false);
            }
        }
        else
        {
            gauge.fillAmount = 0;
            timer = 1.5f;
        }
    }

    //TryAgain選択時
    private void SelectTryAgain()
    {
        //レイが当たっていれば
        if (isHitRay == true)
        {
            //ゲージを溜める
            gauge.fillAmount += 1 - Mathf.Clamp01((1.5f - Time.deltaTime) / 1.5f);
            timer -= Time.deltaTime;
            //ゲージが溜まり切りかつシーン遷移が開始されていなければ
            if (timer < 0.0f && isChange == false)
            {
                sounds_Manager.PlaySE("Beam", 1);
                //シーン再読み込み
                scene_Fade.LoadSceenWithFade(() => { scene_Manager.SelectRestart(); });
                isChange = true;
                GameObject.FindGameObjectWithTag(TagName.PLAYER).GetComponent<VR_PlayerWireAction>().MenuModeChange(false);
            }

        }
        else
        {
            gauge.fillAmount = 0;
            timer = 1.5f;
        }
    }

    //BackToMenu選択時
    private void SelectBackToMenu()
    {
        //レイが当たっていれば
        if (isHitRay == true)
        {
            //ゲージを溜める
            gauge.fillAmount += 1 - Mathf.Clamp01((1.5f - Time.deltaTime) / 1.5f);
            timer -= Time.deltaTime;
            //ゲージが溜まり切りかつシーン遷移が開始されていなければ
            if (timer < 0.0f && isChange == false)
            {
                sounds_Manager.PlaySE("Beam", 1);
                //メニューシーンに戻る
                scene_Fade.LoadSceenWithFade(() => { scene_Manager.SelectBackMenu(); }, true);
                //プレイヤーとボスの追加ステータスの初期化
                ability.ResetAll();
                plusHP.ResetPlusHP();
                isChange = true;
                GameObject.FindGameObjectWithTag(TagName.PLAYER).GetComponent<VR_PlayerWireAction>().MenuModeChange(false);
            }
        }
        else
        {
            gauge.fillAmount = 0;
            timer = 1.5f;
        }
    }

    //NextStage選択時
    private void SelectNextStage()
    {
        //レイが当たっていれば
        if (isHitRay == true)
        {
            //ゲージを溜める
            gauge.fillAmount += 1 - Mathf.Clamp01((1.5f - Time.deltaTime) / 1.5f);
            timer -= Time.deltaTime;
            //ゲージが溜まり切りかつシーン遷移が開始されていなければ
            if (timer < 0.0f && isChange == false)
            {
                sounds_Manager.PlaySE("Beam", 1);
                //次のシーンへ
                scene_Fade.LoadSceenWithFade(() => { scene_Manager.NextStage(); }, true);
                //プレイヤーとボスの追加ステータスの初期化
                ability.ResetAll();
                plusHP.ResetPlusHP();
                isChange = true;
                GameObject.FindGameObjectWithTag(TagName.PLAYER).GetComponent<VR_PlayerWireAction>().MenuModeChange(false);
            }
        }
        else
        {
            gauge.fillAmount = 0;
            timer = 1.5f;
        }
    }
}
