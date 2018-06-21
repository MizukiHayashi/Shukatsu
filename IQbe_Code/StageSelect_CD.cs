using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelect_CD : MonoBehaviour
{
    [SerializeField]
    private Button countDowaButton; //カウントダウンモードボタン
    [SerializeField]
    private Button selectButton_1;  //1ページ目の初期ボタン
    [SerializeField]
    private Button selectButton_11; //2ページ目の初期ボタン
    [SerializeField]
    private Button selectButton_21; //3ページ目の初期ボタン
    [SerializeField]
    private Button selectButton_31; //4ページ目の初期ボタン
    [SerializeField]
    private Button selectButton_41; //5ページ目の初期ボタン
    [SerializeField]
    private GameObject panels;      //パネルオブジェクト

    //スクロール時の各位置
    [SerializeField]
    private Vector3 panelPos_1;     
    [SerializeField]
    private Vector3 panelPos_2;
    [SerializeField]
    private Vector3 panelPos_3;
    [SerializeField]
    private Vector3 panelPos_4;
    [SerializeField]
    private Vector3 panelPos_5;

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
            if ((selectStage.tag == "Stage_CD"))
            {
                HighScoreText();    //ハイスコア表示
                if (prevStage != selectStage)
                {
                    Sound.PlaySE(0);
                }
                //スクロールしていなければ
                if(isMoving==false)
                {
                    if(Input.GetButtonDown("R")||Input.GetButtonDown("L"))
                    {
                        ScrollPanel();  //パネルスクロール
                    }
                }
                //キャンセルボタンを押したら
                if (Input.GetButtonDown("Cancel"))
                {
                    //パネルを初期位置へ
                    LeanTween.moveX(panels, panelPos_1.x, 0.6f).setOnComplete(IsMoveEnd);
                    scrollCount = 0;
                    Sound.PlaySE(1);
                    //カウントダウンモードボタンをアクティブボタンに
                    countDowaButton.Select();
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
            if (scrollCount > 4)
            {
                scrollCount = 4;
            }
        }
        else if (Input.GetButtonDown("L"))
        {
            scrollCount -= 1;
            if (scrollCount < 0)
            {
                scrollCount = 0;
            }
        }
        //現在のスクロール番号と前回のスクロール番号が一緒なら無視
        if (scrollCount == prevScrollCount) return;
        //スクロール番号に応じてパネルを移動
        switch (scrollCount)
        {
            case 0:
                isMoving = true;
                selectButton_1.Select();
                LeanTween.moveX(panels, panelPos_1.x, 0.6f).setOnComplete(IsMoveEnd);
                break;
            case 1:
                isMoving = true;
                selectButton_11.Select();
                LeanTween.moveX(panels, panelPos_2.x, 0.6f).setOnComplete(IsMoveEnd);
                break;
            case 2:
                isMoving = true;
                selectButton_21.Select();
                LeanTween.moveX(panels, panelPos_3.x, 0.6f).setOnComplete(IsMoveEnd);
                break;
            case 3:
                isMoving = true;
                selectButton_31.Select();
                LeanTween.moveX(panels, panelPos_4.x, 0.6f).setOnComplete(IsMoveEnd);
                break;
            case 4:
                isMoving = true;
                selectButton_41.Select();
                LeanTween.moveX(panels, panelPos_5.x, 0.6f).setOnComplete(IsMoveEnd);
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
                highScoreTime = PlayerPrefs.GetFloat("1_time");
                highScoreMove = PlayerPrefs.GetInt("1_move");
                break;
            case "Stage (2)":
                highScoreTime = PlayerPrefs.GetFloat("2_time");
                highScoreMove = PlayerPrefs.GetInt("2_move");
                break;
            case "Stage (3)":
                highScoreTime = PlayerPrefs.GetFloat("3_time");
                highScoreMove = PlayerPrefs.GetInt("3_move");
                break;
            case "Stage (4)":
                highScoreTime = PlayerPrefs.GetFloat("4_time");
                highScoreMove = PlayerPrefs.GetInt("4_move");
                break;
            case "Stage (5)":
                highScoreTime = PlayerPrefs.GetFloat("5_time");
                highScoreMove = PlayerPrefs.GetInt("5_move");
                break;
            case "Stage (6)":
                highScoreTime = PlayerPrefs.GetFloat("6_time");
                highScoreMove = PlayerPrefs.GetInt("6_move");
                break;
            case "Stage (7)":
                highScoreTime = PlayerPrefs.GetFloat("7_time");
                highScoreMove = PlayerPrefs.GetInt("7_move");
                break;
            case "Stage (8)":
                highScoreTime = PlayerPrefs.GetFloat("8_time");
                highScoreMove = PlayerPrefs.GetInt("8_move");
                break;
            case "Stage (9)":
                highScoreTime = PlayerPrefs.GetFloat("9_time");
                highScoreMove = PlayerPrefs.GetInt("9_move");
                break;
            case "Stage (10)":
                highScoreTime = PlayerPrefs.GetFloat("10_time");
                highScoreMove = PlayerPrefs.GetInt("10_move");
                break;
            case "Stage (11)":
                highScoreTime = PlayerPrefs.GetFloat("11_time");
                highScoreMove = PlayerPrefs.GetInt("11_move");
                break;
            case "Stage (12)":
                highScoreTime = PlayerPrefs.GetFloat("12_time");
                highScoreMove = PlayerPrefs.GetInt("12_move");
                break;
            case "Stage (13)":
                highScoreTime = PlayerPrefs.GetFloat("13_time");
                highScoreMove = PlayerPrefs.GetInt("13_move");
                break;
            case "Stage (14)":
                highScoreTime = PlayerPrefs.GetFloat("14_time");
                highScoreMove = PlayerPrefs.GetInt("14_move");
                break;
            case "Stage (15)":
                highScoreTime = PlayerPrefs.GetFloat("15_time");
                highScoreMove = PlayerPrefs.GetInt("15_move");
                break;
            case "Stage (16)":
                highScoreTime = PlayerPrefs.GetFloat("16_time");
                highScoreMove = PlayerPrefs.GetInt("16_move");
                break;
            case "Stage (17)":
                highScoreTime = PlayerPrefs.GetFloat("17_time");
                highScoreMove = PlayerPrefs.GetInt("17_move");
                break;
            case "Stage (18)":
                highScoreTime = PlayerPrefs.GetFloat("18_time");
                highScoreMove = PlayerPrefs.GetInt("18_move");
                break;
            case "Stage (19)":
                highScoreTime = PlayerPrefs.GetFloat("19_time");
                highScoreMove = PlayerPrefs.GetInt("19_move");
                break;
            case "Stage (20)":
                highScoreTime = PlayerPrefs.GetFloat("20_time");
                highScoreMove = PlayerPrefs.GetInt("20_move");
                break;
            case "Stage (21)":
                highScoreTime = PlayerPrefs.GetFloat("21_time");
                highScoreMove = PlayerPrefs.GetInt("21_move");
                break;
            case "Stage (22)":
                highScoreTime = PlayerPrefs.GetFloat("22_time");
                highScoreMove = PlayerPrefs.GetInt("22_move");
                break;
            case "Stage (23)":
                highScoreTime = PlayerPrefs.GetFloat("23_time");
                highScoreMove = PlayerPrefs.GetInt("23_move");
                break;
            case "Stage (24)":
                highScoreTime = PlayerPrefs.GetFloat("24_time");
                highScoreMove = PlayerPrefs.GetInt("24_move");
                break;
            case "Stage (25)":
                highScoreTime = PlayerPrefs.GetFloat("25_time");
                highScoreMove = PlayerPrefs.GetInt("25_move");
                break;
            case "Stage (26)":
                highScoreTime = PlayerPrefs.GetFloat("26_time");
                highScoreMove = PlayerPrefs.GetInt("26_move");
                break;
            case "Stage (27)":
                highScoreTime = PlayerPrefs.GetFloat("27_time");
                highScoreMove = PlayerPrefs.GetInt("27_move");
                break;
            case "Stage (28)":
                highScoreTime = PlayerPrefs.GetFloat("28_time");
                highScoreMove = PlayerPrefs.GetInt("28_move");
                break;
            case "Stage (29)":
                highScoreTime = PlayerPrefs.GetFloat("29_time");
                highScoreMove = PlayerPrefs.GetInt("29_move");
                break;
            case "Stage (30)":
                highScoreTime = PlayerPrefs.GetFloat("30_time");
                highScoreMove = PlayerPrefs.GetInt("30_move");
                break;
            case "Stage (31)":
                highScoreTime = PlayerPrefs.GetFloat("31_time");
                highScoreMove = PlayerPrefs.GetInt("31_move");
                break;
            case "Stage (32)":
                highScoreTime = PlayerPrefs.GetFloat("32_time");
                highScoreMove = PlayerPrefs.GetInt("32_move");
                break;
            case "Stage (33)":
                highScoreTime = PlayerPrefs.GetFloat("33_time");
                highScoreMove = PlayerPrefs.GetInt("33_move");
                break;
            case "Stage (34)":
                highScoreTime = PlayerPrefs.GetFloat("34_time");
                highScoreMove = PlayerPrefs.GetInt("34_move");
                break;
            case "Stage (35)":
                highScoreTime = PlayerPrefs.GetFloat("35_time");
                highScoreMove = PlayerPrefs.GetInt("35_move");
                break;
            case "Stage (36)":
                highScoreTime = PlayerPrefs.GetFloat("36_time");
                highScoreMove = PlayerPrefs.GetInt("36_move");
                break;
            case "Stage (37)":
                highScoreTime = PlayerPrefs.GetFloat("37_time");
                highScoreMove = PlayerPrefs.GetInt("37_move");
                break;
            case "Stage (38)":
                highScoreTime = PlayerPrefs.GetFloat("38_time");
                highScoreMove = PlayerPrefs.GetInt("38_move");
                break;
            case "Stage (39)":
                highScoreTime = PlayerPrefs.GetFloat("39_time");
                highScoreMove = PlayerPrefs.GetInt("39_move");
                break;
            case "Stage (40)":
                highScoreTime = PlayerPrefs.GetFloat("40_time");
                highScoreMove = PlayerPrefs.GetInt("40_move");
                break;
            case "Stage (41)":
                highScoreTime = PlayerPrefs.GetFloat("41_time");
                highScoreMove = PlayerPrefs.GetInt("41_move");
                break;
            case "Stage (42)":
                highScoreTime = PlayerPrefs.GetFloat("42_time");
                highScoreMove = PlayerPrefs.GetInt("42_move");
                break;
            case "Stage (43)":
                highScoreTime = PlayerPrefs.GetFloat("43_time");
                highScoreMove = PlayerPrefs.GetInt("43_move");
                break;
            case "Stage (44)":
                highScoreTime = PlayerPrefs.GetFloat("44_time");
                highScoreMove = PlayerPrefs.GetInt("44_move");
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
