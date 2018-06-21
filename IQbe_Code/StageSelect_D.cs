using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class StageSelect_D : MonoBehaviour
{
    [SerializeField]
    private Button dungeonButton;   //ダンジョンモードボタン
    [SerializeField]
    private Button selectButton_1;  //1ページ目の初期ボタン

    [SerializeField]
    private Text time;   //タイムスコア表示テキスト
    [SerializeField]
    private Text move;  //移動回数スコア表示テキスト

    private float highScoreTime;    //タイムハイスコア
    private float highScoreMove;    //移動回数ハイスコア

    private GameObject selectStage; //選択中ステージボタンオブジェクト
    private GameObject prevStage;   //前回の選択ステージボタンオブジェクト

    // Use this for initialization
    void Start()
    {
        //変数の初期化
        selectStage = EventSystem.current.currentSelectedGameObject;
        prevStage = selectStage;
    }

    // Update is called once per frame
    void Update()
    {
        //前回の選択ステージボタンを更新
        prevStage = selectStage;
        //現在の選択ステージボタンを更新
        selectStage = EventSystem.current.currentSelectedGameObject;
        //ステージが選択されていたら
        if (selectStage!=null)
        {
            if((selectStage.tag == "Stage_D"))
            {
                HighScoreText();    //ハイスコア表示
                if (prevStage!=selectStage)
                {
                    Sound.PlaySE(0);
                }
                //キャンセルボタンを押したら
                if (Input.GetButtonDown("Cancel"))
                {
                    Sound.PlaySE(1);
                    //カウントダウンモードボタンをアクティブボタンに
                    dungeonButton.Select();
                }
            }
        }
    }
    //ステージボタン選択時の遷移処理
    public void StageChange(string sceneName)
    {
        Sound.PlaySE(3);
        SceneManager.LoadScene(sceneName);
    }
    //ハイスコア表示処理
    public void HighScoreText()
    {
        //選択中のステージ名に応じてスコアを取得
        switch (selectStage.name)
        {
            case "Stage (1)":
                highScoreTime = PlayerPrefs.GetFloat("D1_time");
                highScoreMove = PlayerPrefs.GetInt("D1_move");
                break;
            case "Stage (2)":
                highScoreTime = PlayerPrefs.GetFloat("D2_time");
                highScoreMove = PlayerPrefs.GetInt("D2_move");
                break;
            case "Stage (3)":
                highScoreTime = PlayerPrefs.GetFloat("D3_time");
                highScoreMove = PlayerPrefs.GetInt("D3_move");
                break;
            case "Stage (4)":
                highScoreTime = PlayerPrefs.GetFloat("D4_time");
                highScoreMove = PlayerPrefs.GetInt("D4_move");
                break;
            case "Stage (5)":
                highScoreTime = PlayerPrefs.GetFloat("D5_time");
                highScoreMove = PlayerPrefs.GetInt("D5_move");
                break;
            case "Stage (6)":
                highScoreTime = PlayerPrefs.GetFloat("D6_time");
                highScoreMove = PlayerPrefs.GetInt("D6_move");
                break;
            case "Stage (7)":
                highScoreTime = PlayerPrefs.GetFloat("D7_time");
                highScoreMove = PlayerPrefs.GetInt("D7_move");
                break;
            case "Stage (8)":
                highScoreTime = PlayerPrefs.GetFloat("D8_time");
                highScoreMove = PlayerPrefs.GetInt("D8_move");
                break;
            case "Stage (9)":
                highScoreTime = PlayerPrefs.GetFloat("D9_time");
                highScoreMove = PlayerPrefs.GetInt("D9_move");
                break;
            case "Stage (10)":
                highScoreTime = PlayerPrefs.GetFloat("D10_time");
                highScoreMove = PlayerPrefs.GetInt("D10_move");
                break;
        };
        //スコアに応じて表示内容切り替え
        if (highScoreTime == 0)
            time.text = "--";
        else if (highScoreTime < 10)
            time.text = "00" + highScoreTime;
        else if (highScoreTime < 100)
            time.text = "0" + highScoreTime;
        else
            time.text = "" + highScoreTime;

        if (highScoreMove == 0)
            move.text = "--";
        else if (highScoreMove < 10)
            move.text = "00" + highScoreMove;
        else if (highScoreMove < 100)
            move.text = "0" + highScoreMove;
        else
            move.text = "" + highScoreMove;
    }
}
