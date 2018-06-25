using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Jcores
{
    namespace ExecutiveFunction
    {
        namespace ChangeConcept
        {
            public class Hard : MonoBehaviour
            {
                enum GameState
                {
                    None,
                    QuestionSet,
                    QuestionAnswer,
                    Correct,
                    Close,
                    Next,
                    Result
                }

                private GameState state;

                [SerializeField]
                private FadeTransit fadeTransit;

                [SerializeField]
                private GameObject teacherText; //先生のテキスト

                [SerializeField]
                private Image back;                 //背景
                [SerializeField]
                private Sprite[] backGroundSprite;  //背景のスプライト

                [SerializeField]
                private GameObject startTimer;      //開始タイマー
                [SerializeField]
                private GameObject band;           //バンド(正解、おしいなどを表示)
                [SerializeField]
                private Sprite[] bandSprite;       //バンドスプライト(0:正解 1:おしい 2:次の課題です 3:終了)

                [SerializeField]
                private GameObject questionObject;  //問題表示オブジェクト

                [SerializeField]
                private GameObject buttons;             //各位置にあるボタンの親オブジェクト
                [SerializeField]
                private Image[] posButtons;             //各位置にあるボタン(0:上 1:下 2:左 3:右)
                [SerializeField]
                private Image[] posWordImages;          //ボタンに表示する文字イメージ(0:上 1:下 2:左 3:右)
                [SerializeField]
                private Sprite[] posWordSprites;        //ボタンに表示する文字イメージのスプライト(0:上 1:下 2:左 3:右)
                private int[] posWordSpriteID;          //ボタンに表示する文字イメージのスプライトID(0:上 1:下 2:左 3:右)
                [SerializeField]
                private Sprite[] posWordCorrectSprite;  //ボタンに表示する文字イメージの正解時スプライト(0:上 1:下 2:左 3:右)

                private string[] posWord = { "『上』", "『下』", "『左』", "『右』" };      //位置
                private string[] questionWord = { "の文字を押してね", "の位置を押してね" }; //問題


                private int rand_position;        //位置文字のランダム用変数
                private int rand_question;        //問題のランダム用変数
                private int rand_posWordPos;      //文字の位置のランダム用変数(0:上 1:下 2:左 3:右)
                private int correctNum;           //正解の位置のイメージの番号
                private int[] othrePosImageID;    //正解以外のボタンのイメージのID
                private int[] posWordSpriteID_correct;

                private string correct;  //正解のボタン
                private string pushButtonsName; //押されたボタンの名前

                private bool isPush;    //押されたか

                private float questionAllCount;   //すべての問題数
                private int nowQuestionCount;   //現在の問題数

                private float correctCount;       //正解数
                private float correctAvg;       //正解率
                private float answerTimer;      //解答時間
                private float answerAvgTime;    //平均解答時間

                private float resultDrawTime;       //正解・不正解の表示時間
                private float nextBandDrawTime;     //次の設問ですの表示時間
                private float endDrawTime;          //終了の表示時間

                // Use this for initialization
                void Start()
                {
                    state = GameState.None;  //ステートの初期化
                    //先生のテキストを設定
                    teacherText.GetComponent<Text>().text = "表示文にあてはまる\nボタンから選んでね";

                    Settings.Instance.SetSettings();
                    //全設問数を取得
                    questionAllCount = Settings.Instance.CurrentSettings.QuestionNum;
                    //各変数を初期化
                    nowQuestionCount = 0;
                    correctCount = 0;
                    correctAvg = 0.0f;
                    answerTimer = 0.0f;
                    answerAvgTime = 0.0f;

                    resultDrawTime = 0.5f;
                    nextBandDrawTime = 0.5f;
                    endDrawTime = 1.0f;

                }

                // Update is called once per frame
                void Update()
                {
                    //時間が０になったら
                    if (state == GameState.None && startTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                    {
                        state = GameState.QuestionSet;
                    }

                    switch (state)
                    {
                        case GameState.QuestionSet:
                            QuestionSet();
                            break;
                        case GameState.QuestionAnswer:
                            QuestionAnswer();
                            break;
                        case GameState.Correct:
                            Correct();
                            break;
                        case GameState.Close:
                            Close();
                            break;
                        case GameState.Next:
                            Next();
                            break;
                        case GameState.Result:
                            Result();
                            break;
                    }
                }

                //問題をセット
                private void QuestionSet()
                {
                    //位置文字と問題をランダムで設定
                    rand_position = UnityEngine.Random.Range(0, posWord.Length);
                    rand_question = UnityEngine.Random.Range(0, questionWord.Length);
                    //問題を設定
                    questionObject.GetComponent<Text>().text = posWord[rand_position] + questionWord[rand_question];
                    //問題とボタンをアクティブに
                    questionObject.SetActive(true);
                    buttons.SetActive(true);

                    if (rand_question == 0)　//○○の文字を押してね
                    {
                        //正解の位置をランダムで指定
                        rand_posWordPos = UnityEngine.Random.Range(0, 3);
                        switch (rand_position)  //文字
                        {
                            case 0: //上                             
                                posWordSpriteID = new int[3] { 1, 2, 3 };
                                switch (rand_posWordPos) //文字を出す位置
                                {
                                    case 0: //下位置
                                        othrePosImageID = new int[3] { 0, 2, 3 };
                                        correctNum = 1;
                                        SetWord_Word(rand_posWordPos, 0, posWordSpriteID);
                                        correct = "Under";
                                        break;
                                    case 1: //左位置
                                        othrePosImageID = new int[3] { 0, 1, 3 };
                                        correctNum = 2;
                                        SetWord_Word(rand_posWordPos, 0, posWordSpriteID);
                                        correct = "Left";
                                        break;
                                    case 2: //右位置
                                        othrePosImageID = new int[3] { 0, 1, 2 };
                                        correctNum = 3;
                                        SetWord_Word(rand_posWordPos, 0, posWordSpriteID);
                                        correct = "Right";
                                        break;
                                }
                                break;
                            case 1: //下
                                posWordSpriteID = new int[3] { 0, 2, 3 };
                                switch (rand_posWordPos)
                                {
                                    case 0: //上位置
                                        othrePosImageID = new int[3] { 1, 2, 3 };
                                        correctNum = 0;
                                        SetWord_Word(rand_posWordPos, 1, posWordSpriteID);
                                        correct = "Up";
                                        break;
                                    case 1: //左位置
                                        othrePosImageID = new int[3] { 0, 1, 3 };
                                        correctNum = 2;
                                        SetWord_Word(rand_posWordPos, 1, posWordSpriteID);
                                        correct = "Left";
                                        break;
                                    case 2: //右位置
                                        othrePosImageID = new int[3] { 0, 1, 2 };
                                        correctNum = 3;
                                        SetWord_Word(rand_posWordPos, 1, posWordSpriteID);
                                        correct = "Right";
                                        break;
                                }
                                break;
                            case 2: //左
                                posWordSpriteID = new int[3] { 0, 1, 3 };
                                switch (rand_posWordPos)
                                {
                                    case 0: //上位置
                                        othrePosImageID = new int[3] { 1, 2, 3 };
                                        correctNum = 0;
                                        SetWord_Word(rand_posWordPos, 2, posWordSpriteID);
                                        correct = "Up";
                                        break;
                                    case 1: //下位置
                                        othrePosImageID = new int[3] { 0, 2, 3 };
                                        correctNum = 1;
                                        SetWord_Word(rand_posWordPos, 2, posWordSpriteID);
                                        correct = "Under";
                                        break;
                                    case 2: //右位置
                                        othrePosImageID = new int[3] { 0, 1, 2 };
                                        correctNum = 3;
                                        SetWord_Word(rand_posWordPos, 2, posWordSpriteID);
                                        correct = "Right";
                                        break;
                                }
                                break;
                            case 3: //右
                                posWordSpriteID = new int[3] { 0, 1, 2 };
                                switch (rand_posWordPos)
                                {
                                    case 0: //上位置
                                        othrePosImageID = new int[3] { 1, 2, 3 };
                                        correctNum = 0;
                                        SetWord_Word(rand_posWordPos, 3, posWordSpriteID);
                                        correct = "Up";
                                        break;
                                    case 1: //下位置
                                        othrePosImageID = new int[3] { 0, 2, 3 };
                                        correctNum = 1;
                                        SetWord_Word(rand_posWordPos, 3, posWordSpriteID);
                                        correct = "Under";
                                        break;
                                    case 2: //左位置
                                        othrePosImageID = new int[3] { 0, 1, 3 };
                                        correctNum = 2;
                                        SetWord_Word(rand_posWordPos, 3, posWordSpriteID);
                                        correct = "Left";
                                        break;
                                }
                                break;
                        }
                    }
                    else if (rand_question == 1) //○○の位置を押してね
                    {
                        switch (rand_position)
                        {
                            case 0: //上位置
                                posWordSpriteID_correct = new int[3] { 1, 2, 3 };
                                othrePosImageID = new int[3] { 1, 2, 3 };
                                correctNum = 0;
                                SetWord_Position(posWordSpriteID_correct);
                       
                                correct = "Up";
                                break;
                            case 1: //下位置
                                posWordSpriteID_correct = new int[3] { 0, 2, 3 };
                                othrePosImageID = new int[3] { 0, 2, 3 };
                                correctNum = 1;
                                SetWord_Position(posWordSpriteID_correct);                        
                                correct = "Under";
                                break;
                            case 2: //左位置
                                posWordSpriteID_correct = new int[3] { 0, 1, 3 };
                                othrePosImageID = new int[3] { 0, 1, 3 };
                                correctNum = 2;
                                SetWord_Position(posWordSpriteID_correct);                          
                                correct = "Left";
                                break;
                            case 3: //右位置
                                posWordSpriteID_correct = new int[3] { 0, 1, 2 };
                                othrePosImageID = new int[3] { 0, 1, 2 };
                                correctNum = 3;
                                SetWord_Position(posWordSpriteID_correct);
                                correct = "Right";
                                break;
                        }
                    }
                    state = GameState.QuestionAnswer;
                }
                //○の文字を押してねの時の文字の設定
                private void SetWord_Word(int rand_posWordPos, int correctWordID, int[] posWordSpriteID)
                {
                    var sort = posWordSpriteID.OrderBy(n => Guid.NewGuid()).ToArray();
                    posWordImages[correctNum].sprite = posWordSprites[correctWordID];
                    for (int i = 0; i < othrePosImageID.Length; i++)
                    {
                        //正解の位置以外のボードに文字を設定
                        posWordImages[othrePosImageID[i]].sprite = posWordSprites[sort[i]];
                    }
                }

                //○の位置を押してねの時の文字の設定
                private void SetWord_Position(int[] posWordSpriteID_correct)
                {
                    //文字スプライトを設定するためのシャッフルソート
                    var sort_correct = posWordSpriteID_correct.OrderBy(n => Guid.NewGuid()).ToArray();
                    posWordImages[correctNum].sprite = posWordSprites[sort_correct[0]];
                    int[] posWordSpriteID_othre=new int[3];
                    switch (sort_correct[0])
                    {
                        case 0:
                            posWordSpriteID_othre = new int[3] { 1, 2, 3 };
                            break;
                        case 1:
                            posWordSpriteID_othre = new int[3] { 0, 2, 3 };
                            break;
                        case 2:
                            posWordSpriteID_othre = new int[3] { 0, 1, 3 };
                            break;
                        case 3:
                            posWordSpriteID_othre = new int[3] { 0, 1, 2 };
                            break;
                    }
                    var sort_othre = posWordSpriteID_othre.OrderBy(n => Guid.NewGuid()).ToArray();
                    for (int i = 0; i < posWordImages.Length-1; i++)
                    {
                        posWordImages[othrePosImageID[i]].sprite = posWordSprites[sort_othre[i]];
                    }
                }

                private void QuestionAnswer()
                {
                    answerTimer += Time.deltaTime;
                    if (isPush == true)
                    {
                        if (pushButtonsName == correct)
                        {
                            state = GameState.Correct;
                        }
                        else
                        {
                            state = GameState.Close;
                        }
                    }
                }
                //正解
                private void Correct()
                {
                    //「正解」を表示
                    band.GetComponent<Image>().sprite = bandSprite[0];
                    band.SetActive(true);
                   
                    //答えの文字を正解用の文字に変更する
                    if (posWordImages[correctNum].sprite == posWordSprites[0]) posWordImages[correctNum].sprite = posWordCorrectSprite[0];
                    else if (posWordImages[correctNum].sprite == posWordSprites[1]) posWordImages[correctNum].sprite = posWordCorrectSprite[1];
                    else if (posWordImages[correctNum].sprite == posWordSprites[2]) posWordImages[correctNum].sprite = posWordCorrectSprite[2];
                    else if (posWordImages[correctNum].sprite == posWordSprites[3]) posWordImages[correctNum].sprite = posWordCorrectSprite[3];
                    ResultDrawEnd(true);
                }
                //おしい
                private void Close()
                {
                    band.GetComponent<Image>().sprite = bandSprite[1];
                    band.SetActive(true);
                    ResultDrawEnd(false);
                }

                private void ResultDrawEnd(bool isCorrect)
                {
                    //表示時間が０になったら
                    resultDrawTime -= Time.deltaTime;
                    if (resultDrawTime <= 0.0f)
                    {
                        band.SetActive(false); //「正解・不正解」を非アクティブに
                        resultDrawTime = 0.5f; //表示時間を初期化
                        nowQuestionCount += 1; //現在の設問数を加算

                        if (isCorrect == true)
                        {
                            correctCount += 1;     //正解の数を加算
                            //正解の数で背景を変更
                            if (correctCount == 1) back.sprite = backGroundSprite[0];
                            else if (correctCount == 4) back.sprite = backGroundSprite[1];
                            else if (correctCount == 7) back.sprite = backGroundSprite[2];
                            else if (correctCount == 10) back.sprite = backGroundSprite[3];
                            else if (correctCount == 15) back.sprite = backGroundSprite[4];
                        }
                        //現在の設問数が全設問数に到達したらリザルトへ(違ったら次の設問へ)
                        if (nowQuestionCount == questionAllCount) state = GameState.Result;
                        else state = GameState.Next;
                    }
                }
                //次の課題です
                private void Next()
                {
                    //問題とボタンを非アクティブに
                    questionObject.SetActive(false);
                    buttons.SetActive(false);
                    //「次の課題です」を表示
                    band.GetComponent<Image>().sprite = bandSprite[2];
                    band.SetActive(true);
                    //表示時間が０になったら
                    nextBandDrawTime -= Time.deltaTime;
                    if (nextBandDrawTime <= 0.0f)
                    {
                        band.SetActive(false);
                        nextBandDrawTime = 0.5f;
                        isPush = false;
                        state = GameState.QuestionSet;
                    }
                }
                //終了-リザルトへ
                private void Result()
                {
                    //問題とボタンを非アクティブに
                    questionObject.SetActive(false);
                    buttons.SetActive(false);
                    //「終了」を表示
                    band.GetComponent<Image>().sprite = bandSprite[3];
                    band.SetActive(true);
                    //表示時間が０になったら
                    resultDrawTime -= Time.deltaTime;
                    if (resultDrawTime <= 0.0f)
                    {
                        //正解率を計算
                        correctAvg = correctCount / questionAllCount * 100;
                        //平均解答時間を計算
                        answerAvgTime = answerTimer / questionAllCount;
                        //リザルトをセット
                        Settings.Instance.SetResult(correctAvg, answerAvgTime, (int)correctCount);
                        //リザルトへ移行
                        fadeTransit.Transit(SceneData.GetSceneName(SceneData.ID.ChangeConcept, SceneData.Suffix.Result));
                    }
                }
                //ボタンが押されたか
                public void OnPushButton(string name)
                {
                    //押されていないかつ解答モードなら
                    if (isPush == false && state == GameState.QuestionAnswer)
                    {
                        pushButtonsName = name;
                        isPush = true;
                    }
                }
            }
        }
    }
}