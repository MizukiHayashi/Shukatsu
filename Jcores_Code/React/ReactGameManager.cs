using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jcores
{
    namespace Attention
    {
        namespace React
        {
            public class ReactGameManager : MonoBehaviour
            {
                //ゲームの状態
                enum GameState
                {
                    None,
                    Learn,      //文字を覚える
                    NotAnswer,  //間違いの文字が表示
                    Answer,     //合っている文字が表示
                    Maru,       //正解
                    Batu,       //不正解
                    Miss,       //見逃し
                    Result      //結果
                }

                private int difficalty; //難易度

                private GameState state = GameState.None; //ステート

                [SerializeField]
                private GameObject startTimer;      //開始タイマー
                [SerializeField]
                private GameObject countTimer;      //ゲームタイマー
                [SerializeField]
                private GameObject teacherMessage;  //先生のメッセージテキスト
                [SerializeField]
                private GameObject endImage;        //終了のイメージ

                [SerializeField]
                private GameObject resultImage;     //解答(○か×)の表示オブジェクト
                private Image answer;               //解答の表示イメージ
                [SerializeField]
                private Sprite maru;                //○画像
                [SerializeField]
                private Sprite batu;                //×画像

                [SerializeField]
                private GameObject onePanel;    //１文字の問題のオブジェクト
                private Text oneText;           //１文字の問題のテキスト
                private string correct_one;     //１文字の問題の答え
                private string nowText_one;     //１文字の問題の現在表示されている文字

                [SerializeField]
                private GameObject twoPanel;                   //２文字の問題のオブジェクト
                private Text twoText_L;                        //２文字の問題のテキスト(左)
                private Text twoText_R;                        //２文字の問題のテキスト(右)
                private string[] correct_two = new string[2];  //２文字の問題の答え
                private string[] nowText_two = new string[2];  //２文字の問題の現在表示されている文字

                [SerializeField]
                private GameObject threePanel;                   //３文字の問題のオブジェクト
                private Image threeImage_L;                      //３文字の問題のイメージ(左)
                private Image threeImage_C;                      //３文字の問題のイメージ(中心)
                private Image threeImage_R;                      //３文字の問題のイメージ(右)
                private Sprite[] correct_three = new Sprite[3];  //３文字の問題の答え
                private Sprite[] nowImage_three = new Sprite[3]; //３文字の問題の現在表示されている絵

                private bool isDrawPanel;   //パネルが表示されているか

                [SerializeField]
                private GameObject submit;  //「決定ボタン」

                //記号
                private string[] mark = { "－", "＋", "±", "×", "÷", "∀", "Σ", "≡", "⊥", "∞" };
                //数字
                private string[] number = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                //ひらがな
                private string[] word = { "あ", "い", "う", "え", "お", "か", "が", "せ", "ぱ", "め" };
                //絵
                [SerializeField]
                private Sprite[] texture;

                private int decisionAnswer;     //合っている文字を出すまでの回数
                [SerializeField]
                private int decisionAnswerMax;  //合っている文字を出すまでの回数の最大値
                private int notAnswerCount;     //間違った文字がでた回数

                private float speedInSecond;    //解答可能時間
                private float drawTime;         //次の文字の表示までの時間
                private float drawChangeTime;   //切り替える時の速さ
                private float resultDrawTime;   //解答の表示時間
                private float endDrawTime;      //終了の表示時間

                private int allQuestionNum;    //問題の表示回数
                private int nowQuestionCount;  //現在の回答数
                private int batuCount;         //不正解の回数
                private int missCount;         //見逃しの回数

                private float answerTimer;          //決定ボタンを押すまでの時間カウント
                private float answerTime_first;     //決定ボタンを押すまでの時間の総計(前半)
                private float answerTime_latter;    //決定ボタンを押すまでの時間の総計(後半)
                private float answerTime_all;       //決定ボタンを押すまでの時間の総計(全体)
                private float answerAvgTime_first;  //平均反応時間(1～５回まで)
                private float answerAvgTime_latter; //平均反応時間(6～10回まで)
                private float answerAvgTime_all;    //平均反応時間(1～10回まで)

                [SerializeField]
                private FadeTransit fadeTransit;

                // Use this for initialization
                void Start()
                {
                    //難易度の取得
                    Settings.Instance.SetSettings();
                    difficalty = Settings.Instance.DifficultyInt;

                    //１文字の問題のテキストの取得
                    oneText = onePanel.transform.Find("Text").GetComponent<Text>();
                    //２文字の問題のテキストの取得
                    twoText_L = twoPanel.transform.Find("LeftText").GetComponent<Text>();
                    twoText_R = twoPanel.transform.Find("RightText").GetComponent<Text>();
                    //３文字の問題のテキストの取得
                    threeImage_L = threePanel.transform.Find("LeftImg").GetComponent<Image>();
                    threeImage_C = threePanel.transform.Find("CenterImg").GetComponent<Image>();
                    threeImage_R = threePanel.transform.Find("RightImg").GetComponent<Image>();

                    //解答の表示イメージの取得
                    answer = resultImage.GetComponent<Image>();

                    isDrawPanel = false;

                    decisionAnswer = Random.Range(1, decisionAnswerMax + 1);
                    notAnswerCount = 0;

                    //解答可能時間の取得
                    speedInSecond = Settings.Instance.SpeedInSecond;
                    drawTime = speedInSecond;
                    //各時間の初期化
                    drawChangeTime = 0.5f;
                    resultDrawTime = 1.0f;
                    endDrawTime = 1.0f;

                    answerTimer = 0.0f;
                    Debug.Log(speedInSecond);
                    //問題の表示回数の取得
                    allQuestionNum = Settings.Instance.CurrentSettings.QuestionNum;
                    //各カウンターの初期化
                    nowQuestionCount = 0;
                    batuCount = 0;
                    missCount = 0;

                }

                // Update is called once per frame
                void Update()
                {
                    //ゲームの状態が初期で開始タイマーが０なら
                    if (state == GameState.None && startTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                    {
                        teacherMessage.GetComponent<Text>().text = "表示されたものを覚えてください。";
                        //難易度によって動作を変える
                        switch (difficalty)
                        {
                            case 0:
                                OneQuestion_Start("mark");
                                break;
                            case 1:
                                OneQuestion_Start("word");
                                break;
                            case 2:
                                OneQuestion_Start("number");
                                break;
                            case 3:
                                TwoMarkQuestion_Start();
                                break;
                            case 4:
                                ThreeTextureQuestion_Start();
                                break;
                        }
                        state = GameState.Learn;    //ゲーム状態を文字を覚えるに変更
                    }

                    //文字を覚える
                    if (state == GameState.Learn)   
                    {
                        //カウントタイマーが０なら
                        if (countTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                        {
                            countTimer.SetActive(false);    //カウントタイマーの非表示
                            state = GameState.NotAnswer;    //ゲーム状態を間違った文字を表示に
                        }
                    }

                    //間違いの文字が表示
                    if (state == GameState.NotAnswer)
                    {
                        teacherMessage.GetComponent<Text>().text = "覚えたものが表示されたら\n下のボタンをクリックしてください";
                        submit.SetActive(true);
                        //難易度によって動作を変える
                        switch (difficalty)
                        {
                            case 0:
                                OneQuestion_NotAnswer("mark");
                                break;
                            case 1:
                                OneQuestion_NotAnswer("word");
                                break;
                            case 2:
                                OneQuestion_NotAnswer("number");
                                break;
                            case 3:
                                TwoMarkQuestion_NotAnswer();
                                break;
                            case 4:
                                ThreeTextureQuestion_NotAnswer();
                                break;
                        }
                    }

                    switch (state)
                    {
                        case GameState.Answer:  //正しい文字が表示
                            Answer();
                            break;
                        case GameState.Maru:    //正解
                            Maru();
                            break;
                        case GameState.Batu:    //不正解
                            Batu();
                            break;
                        case GameState.Miss:    //見逃し
                            Miss();
                            break;
                        case GameState.Result:  //結果
                            Result();
                            break;
                    }
                }

                /// <summary>
                /// 問題の初期動作(１文字)
                /// </summary>
                /// <param name="quesType">問題の種類("mark","word","number")</param>
                private void OneQuestion_Start(string quesType)
                {
                    //１文字の問題のオブジェクトをアクティブに
                    onePanel.SetActive(true);

                    //覚える文字を設定
                    if (quesType == "mark")
                        correct_one = mark[Random.Range(0, mark.Length)];
                    else if (quesType == "word")
                        correct_one = word[Random.Range(0, word.Length)];
                    else if (quesType == "number")
                        correct_one = number[Random.Range(0, number.Length)];
                    //覚える文字を表示
                    oneText.text = correct_one;
                }

                /// <summary>
                /// 間違った文字を表示中(１文字)
                /// </summary>
                /// <param name="quesType">問題の種類("mark","word","number")</param>
                private void OneQuestion_NotAnswer(string quesType)
                {
                    if (drawChangeTime > 0.0f)
                    {
                        //文字の表示回数と現在の回答数が同じなら結果へ
                        if (nowQuestionCount == allQuestionNum)
                            state = GameState.Result;

                        //１文字の問題のオブジェクトを非アクティブに
                        onePanel.SetActive(false);
                        isDrawPanel = false;
                        drawChangeTime -= Time.deltaTime;
                    }

                    if (drawChangeTime <= 0.0f && drawTime == speedInSecond)
                    {
                        //文字を設定
                        if (quesType == "mark")
                            nowText_one = mark[Random.Range(0, mark.Length)];
                        else if (quesType == "word")
                            nowText_one = word[Random.Range(0, word.Length)];
                        else if (quesType == "number")
                            nowText_one = number[Random.Range(0, number.Length)];

                        //間違った文字が出た回数が合っている文字を出す回数になったら
                        if (notAnswerCount == decisionAnswer)
                        {
                            //文字を合っている文字にする
                            nowText_one = correct_one;
                        }
                        
                        //文字の表示
                        oneText.text = nowText_one;
                        onePanel.SetActive(true);
                        isDrawPanel = true;

                        notAnswerCount += 1;

                        //文字が合っていたら
                        if (nowText_one == correct_one)
                        {
                            if (quesType == "mark")
                                nowText_one = mark[Random.Range(0, mark.Length)];
                            else if (quesType == "word")
                                nowText_one = word[Random.Range(0, word.Length)];
                            else if (quesType == "number")
                                nowText_one = number[Random.Range(0, number.Length)];

                            decisionAnswer = Random.Range(1, decisionAnswerMax + 1);
                            notAnswerCount = 0;
                            state = GameState.Answer;
                        }
                    }

                    if (drawChangeTime <= 0.0f)
                    {
                        drawTime -= Time.deltaTime;
                        if (drawTime <= 0.0f)
                        {
                            drawChangeTime = 0.5f;
                            drawTime = speedInSecond;
                        }
                    }
                }

                // 問題の初期動作(２文字)
                private void TwoMarkQuestion_Start()
                {
                    if (nowQuestionCount == allQuestionNum)
                        state = GameState.Result;

                    twoPanel.SetActive(true);
                    correct_two[0] = mark[Random.Range(0, mark.Length)];
                    correct_two[1] = mark[Random.Range(0, mark.Length)];

                    twoText_L.text = correct_two[0];
                    twoText_R.text = correct_two[1];
                }

                // 間違った文字を表示中(２文字)
                private void TwoMarkQuestion_NotAnswer()
                {
                    if (drawChangeTime > 0.0f)
                    {
                        twoPanel.SetActive(false);
                        isDrawPanel = false;
                        drawChangeTime -= Time.deltaTime;
                    }

                    if (drawChangeTime <= 0.0f && drawTime == speedInSecond)
                    {
                        nowText_two[0] = mark[Random.Range(0, mark.Length)];
                        nowText_two[1] = mark[Random.Range(0, mark.Length)];

                        if (notAnswerCount == decisionAnswer)
                        {
                            nowText_two[0] = correct_two[0];
                            nowText_two[1] = correct_two[1];
                        }

                        twoText_L.text = nowText_two[0];
                        twoText_R.text = nowText_two[1];
                        twoPanel.SetActive(true);
                        isDrawPanel = true;

                        notAnswerCount += 1;

                        if (nowText_two[0] == correct_two[0] && nowText_two[1] == correct_two[1])
                        {
                            nowText_two[0] = mark[Random.Range(0, mark.Length)];
                            nowText_two[1] = mark[Random.Range(0, mark.Length)];

                            decisionAnswer = Random.Range(1, decisionAnswerMax + 1);
                            notAnswerCount = 0;
                            state = GameState.Answer;
                        }
                    }

                    if (drawChangeTime <= 0.0f)
                    {
                        drawTime -= Time.deltaTime;
                        if (drawTime <= 0.0f)
                        {
                            drawChangeTime = 0.5f;
                            drawTime = speedInSecond;
                        }
                    }
                }

                // 問題の初期動作(３文字)
                private void ThreeTextureQuestion_Start()
                {
                    threePanel.SetActive(true);
                    correct_three[0] = texture[Random.Range(0, texture.Length)];
                    correct_three[1] = texture[Random.Range(0, texture.Length)];
                    correct_three[2] = texture[Random.Range(0, texture.Length)];
                    threeImage_L.sprite = correct_three[0];
                    threeImage_C.sprite = correct_three[1];
                    threeImage_R.sprite = correct_three[2];
                }

                // 間違った文字を表示中(３文字)
                private void ThreeTextureQuestion_NotAnswer()
                {
                    if (drawChangeTime > 0.0f)
                    {
                        if (nowQuestionCount == allQuestionNum)
                            state = GameState.Result;

                        threePanel.SetActive(false);
                        isDrawPanel = false;
                        drawChangeTime -= Time.deltaTime;
                    }

                    if (drawChangeTime <= 0.0f && drawTime == speedInSecond)
                    {
                        nowImage_three[0] = texture[Random.Range(0, texture.Length)];
                        nowImage_three[1] = texture[Random.Range(0, texture.Length)];
                        nowImage_three[2] = texture[Random.Range(0, texture.Length)];

                        if (notAnswerCount == decisionAnswer)
                        {
                            nowImage_three[0] = correct_three[0];
                            nowImage_three[1] = correct_three[1];
                            nowImage_three[2] = correct_three[2];
                        }

                        threeImage_L.sprite = nowImage_three[0];
                        threeImage_C.sprite = nowImage_three[1];
                        threeImage_R.sprite = nowImage_three[2];
                        threePanel.SetActive(true);
                        isDrawPanel = true;

                        notAnswerCount += 1;

                        if (nowImage_three[0] == correct_three[0] && nowImage_three[1] == correct_three[1] && nowImage_three[2] == correct_three[2])
                        {
                            nowImage_three[0] = texture[Random.Range(0, texture.Length)];
                            nowImage_three[1] = texture[Random.Range(0, texture.Length)];
                            nowImage_three[2] = texture[Random.Range(0, texture.Length)];

                            decisionAnswer = Random.Range(1, decisionAnswerMax + 1);
                            notAnswerCount = 0;
                            state = GameState.Answer;
                        }
                    }

                    if (drawChangeTime <= 0.0f)
                    {
                        drawTime -= Time.deltaTime;
                        if (drawTime <= 0.0f)
                        {
                            drawChangeTime = 0.5f;
                            drawTime = speedInSecond;
                        }
                    }
                }

                // 合っている文字が表示中
                private void Answer()
                {
                    drawTime -= Time.deltaTime;
                    answerTimer += Time.deltaTime;
                    if (drawTime <= 0.0f)
                        state = GameState.Miss;
                }

                //正解処理
                private void Maru()
                {
                    submit.GetComponent<Button>().interactable = false;
                    resultDrawTime -= Time.deltaTime;
                    resultImage.SetActive(true);
                    answer.sprite = maru;
                    if (resultDrawTime < 0.0f)
                    {
                        if (nowQuestionCount < allQuestionNum / 2)
                        {
                            answerTime_first += answerTimer;
                            answerTime_all += answerTimer;
                            answerTimer = 0.0f;
                        }
                        else if (nowQuestionCount < allQuestionNum)
                        {
                            answerTime_latter += answerTimer;
                            answerTime_all += answerTimer;
                            answerTimer = 0.0f;
                        }

                        resultImage.SetActive(false);
                        nowQuestionCount += 1;
                        resultDrawTime = 1.0f;
                        drawChangeTime = 0.5f;
                        submit.GetComponent<Button>().interactable = true;
                        state = GameState.NotAnswer;
                    }
                }

                //不正解処理
                private void Batu()
                {
                    submit.GetComponent<Button>().interactable = false;
                    resultDrawTime -= Time.deltaTime;
                    resultImage.SetActive(true);
                    answer.sprite = batu;
                    if (resultDrawTime < 0.0f)
                    {
                        resultImage.SetActive(false);
                        batuCount += 1;
                        resultDrawTime = 1.0f;
                        drawChangeTime = 0.5f;
                        submit.GetComponent<Button>().interactable = true;
                        state = GameState.NotAnswer;
                    }
                }

                //見逃し処理
                private void Miss()
                {
                    submit.GetComponent<Button>().interactable = false;
                    resultDrawTime -= Time.deltaTime;
                    resultImage.SetActive(true);
                    answer.sprite = batu;
                    if (resultDrawTime < 0.0f)
                    {
                        resultImage.SetActive(false);
                        missCount += 1;
                        resultDrawTime = 1.0f;
                        drawChangeTime = 0.5f;
                        answerTimer = 0.0f;
                        submit.GetComponent<Button>().interactable = true;
                        state = GameState.NotAnswer;

                    }
                }

                //決定ボタンを押した処理
                public void OnClickSubmit()
                {
                    if (state == GameState.NotAnswer && isDrawPanel == true)
                        state = GameState.Batu;
                    else if (state == GameState.Answer && isDrawPanel == true)
                        state = GameState.Maru;
                }

                //結果処理
                private void Result()
                {
                    endDrawTime -= Time.deltaTime;
                    endImage.SetActive(true);
                    if (endDrawTime <= 0.0f)
                    {
                        answerAvgTime_first = answerTime_first / (allQuestionNum / 2);
                        answerAvgTime_latter = answerTime_latter / (allQuestionNum / 2);
                        answerAvgTime_all = answerTime_all / allQuestionNum;

                        PlayerPrefs.SetInt("batuCount", batuCount);
                        PlayerPrefs.SetInt("missCount", missCount);
                        PlayerPrefs.SetFloat("avgTime_first", answerAvgTime_first);
                        PlayerPrefs.SetFloat("avgTime_latter", answerAvgTime_latter);
                        PlayerPrefs.SetFloat("avgTime_all", answerAvgTime_all);

                        endDrawTime = 1.0f;

                        fadeTransit.Transit(SceneData.GetSceneName(SceneData.ID.React, SceneData.Suffix.Result));
                    }
                }
            }
        }
    }
}
