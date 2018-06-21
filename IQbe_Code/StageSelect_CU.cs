using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelect_CU : MonoBehaviour
{
    [SerializeField]
    private Button countUpButton;   //カウントアップモードボタン
    [SerializeField]
    private Button selectButton_1;  //1ページ目の初期ボタン
    [SerializeField]
    private Button selectButton_11; //2ページ目の初期ボタン
    [SerializeField]
    private GameObject panels;      //パネルオブジェクト

    //スクロール時の各位置
    [SerializeField]
    private Vector3 panel1Pos;
    [SerializeField]
    private Vector3 panel2Pos;

    [SerializeField]
    private Text time;  //タイムスコア表示テキスト
    [SerializeField]
    private Text move;  //移動回数スコア表示テキスト

    private float highScoreTime;    //タイムハイスコア
    private float highScoreMove;    //移動回数ハイスコア

    private GameObject selectStage; //選択中ステージボタンオブジェクト
    private GameObject prevStage;   //前回の選択ステージボタンオブジェクト

    private int prevScrollCount;    //前回のスクロール番号
    private int scrollCount;        //現在のスクロール番号
    private bool isMoving;          //スクロールしているか

    // Use this for initialization
    void Start()
    {
        //変数の初期化
        scrollCount = 0;
        prevScrollCount = scrollCount;
        isMoving = false;
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
        if (selectStage != null)
        {
            if ((selectStage.tag == "Stage_CU"))
            {
                HighScoreText();    //ハイスコア表示
                if (prevStage != selectStage)
                {
                    Sound.PlaySE(0);
                }
                //スクロールしていなければ
                if (isMoving == false)
                {
                    if (Input.GetButtonDown("R") || Input.GetButtonDown("L"))
                    {
                        ScrollPanel();  //パネルスクロール
                    }
                }
                //キャンセルボタンを押したら
                if (Input.GetButtonDown("Cancel"))
                {
                    //パネルを初期位置へ
                    LeanTween.moveX(panels, panel1Pos.x, 0.6f).setOnComplete(IsMoveEnd);
                    scrollCount = 0;
                    Sound.PlaySE(1);
                    //カウントダウンモードボタンをアクティブボタンに
                    countUpButton.Select();
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
    //移動終了時処理
    public void IsMoveEnd()
    {
        isMoving = false;
    }
    //パネルスクロール処理
    public void ScrollPanel()
    {
        //RボタンLボタンでページスクロール
        if (Input.GetButtonDown("R"))
        {
            scrollCount += 1;
            if (scrollCount >= 1)
            {
                scrollCount = 1;
            }
        }
        else if (Input.GetButtonDown("L"))
        {
            scrollCount -= 1;
            if (scrollCount <= 0)
            {
                scrollCount = 0;
            }
        }
        //現在のスクロール番号と前回のスクロール番号が一緒なら無視
        if (prevScrollCount == scrollCount) return;
        //スクロール番号に応じてパネルを移動
        switch (scrollCount)
        {
            case 0:
                isMoving = true;
                selectButton_1.Select();
                LeanTween.moveX(panels, panel1Pos.x, 0.6f).setOnComplete(IsMoveEnd);
                break;
            case 1:
                isMoving = true;
                selectButton_11.Select();
                LeanTween.moveX(panels, panel2Pos.x, 0.6f).setOnComplete(IsMoveEnd);
                break;
        }
        //前回のスクロール番号を取得
        prevScrollCount = scrollCount;
    }
    //ハイスコア表示処理
    public void HighScoreText()
    {
        //選択中のステージ名に応じてスコアを取得
        switch (selectStage.name)
        {
            case "Stage (1)":
                highScoreTime = PlayerPrefs.GetFloat("CU1_time");
                highScoreMove = PlayerPrefs.GetInt("CU1_move");
                break;
            case "Stage (2)":
                highScoreTime = PlayerPrefs.GetFloat("CU2_time");
                highScoreMove = PlayerPrefs.GetInt("CU2_move");
                break;
            case "Stage (3)":
                highScoreTime = PlayerPrefs.GetFloat("CU3_time");
                highScoreMove = PlayerPrefs.GetInt("CU3_move");
                break;
            case "Stage (4)":
                highScoreTime = PlayerPrefs.GetFloat("CU4_time");
                highScoreMove = PlayerPrefs.GetInt("CU4_move");
                break;
            case "Stage (5)":
                highScoreTime = PlayerPrefs.GetFloat("CU5_time");
                highScoreMove = PlayerPrefs.GetInt("CU5_move");
                break;
            case "Stage (6)":
                highScoreTime = PlayerPrefs.GetFloat("CU6_time");
                highScoreMove = PlayerPrefs.GetInt("CU6_move");
                break;
            case "Stage (7)":
                highScoreTime = PlayerPrefs.GetFloat("CU7_time");
                highScoreMove = PlayerPrefs.GetInt("CU7_move");
                break;
            case "Stage (8)":
                highScoreTime = PlayerPrefs.GetFloat("CU8_time");
                highScoreMove = PlayerPrefs.GetInt("CU8_move");
                break;
            case "Stage (9)":
                highScoreTime = PlayerPrefs.GetFloat("CU9_time");
                highScoreMove = PlayerPrefs.GetInt("CU9_move");
                break;
            case "Stage (10)":
                highScoreTime = PlayerPrefs.GetFloat("CU10_time");
                highScoreMove = PlayerPrefs.GetInt("CU10_move");
                break;
            case "Stage (11)":
                highScoreTime = PlayerPrefs.GetFloat("CU11_time");
                highScoreMove = PlayerPrefs.GetInt("CU11_move");
                break;
            case "Stage (12)":
                highScoreTime = PlayerPrefs.GetFloat("CU12_time");
                highScoreMove = PlayerPrefs.GetInt("CU12_move");
                break;
            case "Stage (13)":
                highScoreTime = PlayerPrefs.GetFloat("CU13_time");
                highScoreMove = PlayerPrefs.GetInt("CU13_move");
                break;
            case "Stage (14)":
                highScoreTime = PlayerPrefs.GetFloat("CU14_time");
                highScoreMove = PlayerPrefs.GetInt("CU14_move");
                break;
            case "Stage (15)":
                highScoreTime = PlayerPrefs.GetFloat("CU15_time");
                highScoreMove = PlayerPrefs.GetInt("CU15_move");
                break;
            case "Stage (16)":
                highScoreTime = PlayerPrefs.GetFloat("CU16_time");
                highScoreMove = PlayerPrefs.GetInt("CU16_move");
                break;
            case "Stage (17)":
                highScoreTime = PlayerPrefs.GetFloat("CU17_time");
                highScoreMove = PlayerPrefs.GetInt("CU17_move");
                break;
            case "Stage (18)":
                highScoreTime = PlayerPrefs.GetFloat("CU18_time");
                highScoreMove = PlayerPrefs.GetInt("CU18_move");
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
