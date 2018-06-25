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
            public class VaryHard : MonoBehaviour
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
                private GameObject startTimer;      //開始タイマー
                [SerializeField]
                private GameObject band;           //バンド(正解、おしいなどを表示)
                [SerializeField]
                private Sprite[] bandSprite;       //バンドスプライト(0:正解 1:おしい 2:次の課題です 3:終了)

                [SerializeField]
                private GameObject characterObject; //キャラクターのオブジェクト
                [SerializeField]
                private Sprite[] characterSprite;     //キャラクターのスプライト(0:普通顔 1:正解顔)

                [SerializeField]
                private GameObject questionText;  //問題表示オブジェクト

                [SerializeField]
                private GameObject buttonParent;    //ボタンの親オブジェクト
                [SerializeField]
                private GameObject[] buttons;         //各ボタン(0:左上 1:中央上 2:右上 3:中央左 4:中央 5:中央右 6:左下 7:中央下 8:右下 )    
                [SerializeField]
                private Text[] buttonTexts;         //各ボタンに表示するテキスト(0:左上 1:中央上 2:右上 3:中央左 4:中央 5:中央右 6:左下 7:中央下 8:右下 ) 
                private string[] wordPatterns =
                {
                    "*",
                    "*        *",
                    "*   *   *",
                    "*        *\n\n*        *",
                    "*        *\n*\n*        *",
                    "*        *\n*        *\n*        *",
                    "*   *   *\n*\n*   *   *",
                    "*   *   *\n*        *\n*   *   *",
                    "*   *   *\n*   *   *\n*   *   *"
                };
                private int[] selectNumber = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };   //ボタンに表示する数字

                //問題の最初の文字部分
                private string[] questionStartWord = { "数字が", "個数が" };
                //問題の数字の部分
                private string[] questionNumberWord = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                //問題の最後の部分
                private string questionEndWord = "なのはどれ?";

                private int selectWordRand;     //文字部分を選ぶための変数
                private int selectNumberRand;   //数字を選ぶための変数
                private int selectCountRand;    //個数を選ぶための個数
                private int correctButtonRand;  //正解のボタンを選ぶための変数
                private int patternRand;        //ボタンの配置パターンを選ぶための変数
                private int[] buttonsId;
                private int[] pattarnSortOrigin;

                private int correctNumber;
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
                    state = GameState.None;

                    teacherText.GetComponent<Text>().text = "指示にあったパネルを選択してください\n数字と個数に気をつけよう";

                    Settings.Instance.SetSettings();
                    questionAllCount = Settings.Instance.CurrentSettings.QuestionNum;
                    nowQuestionCount = 0;
                    correctCount = 0;
                    correctAvg = 0.0f;
                    answerTimer = 0.0f;
                    answerAvgTime = 0.0f;

                    resultDrawTime = 1.0f;
                    nextBandDrawTime = 1.0f;
                    endDrawTime = 1.0f;
                }

                // Update is called once per frame
                void Update()
                {
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

                private void QuestionSet()
                {
                    questionNumberWord = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

                    selectWordRand = UnityEngine.Random.Range(0, questionStartWord.Length);
                    if (selectWordRand == 0)
                    {
                        selectNumberRand = UnityEngine.Random.Range(0, questionNumberWord.Length);
                    }
                    else if (selectWordRand == 1)
                    {
                        selectNumberRand = UnityEngine.Random.Range(1, 10);
                    }
                    patternRand = UnityEngine.Random.Range(0, 8);

                    questionText.GetComponent<Text>().text = questionStartWord[selectWordRand] + questionNumberWord[selectNumberRand] + questionEndWord;
                    questionText.SetActive(true);

                    if (selectWordRand == 0) //数字が
                    {
                        switch (selectNumberRand)
                        {
                            case 0: //0
                                correctNumber = 0;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 1: //1
                                correctNumber = 1;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 2: //2
                                correctNumber = 2;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 3: //3
                                correctNumber = 3;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 4: //4
                                correctNumber = 4;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 5: //5
                                correctNumber = 5;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 6: //6
                                correctNumber = 6;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 7: //7
                                correctNumber = 7;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 8: //8
                                correctNumber = 8;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;
                            case 9: //9
                                correctNumber = 9;
                                SetButtonType_Number(patternRand, correctNumber);
                                break;

                        }
                    }
                    else if (selectWordRand == 1) //個数が
                    {
                        switch (selectNumberRand)
                        {
                            case 1: //1個
                                correctNumber = 0;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 2: //2個
                                correctNumber = 1;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 3: //3個
                                correctNumber = 2;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 4: //4個
                                correctNumber = 3;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 5: //5個
                                correctNumber = 4;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 6: //6個
                                correctNumber = 5;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 7: //7個
                                correctNumber = 6;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 8: //8個
                                correctNumber = 7;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;
                            case 9: //9個
                                correctNumber = 8;
                                SetButtonType_Count(patternRand, correctNumber);
                                break;

                        }
                    }
                    state = GameState.QuestionAnswer;
                }

                private void SetButtonType_Number(int pattern, int correctNumber)
                {
                    switch (pattern)
                    {
                        case 0: //ボード2枚
                            correctButtonRand = UnityEngine.Random.Range(0, 2);
                            buttonsId = new int[2] { 3, 5 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[1] { 5 };
                                    SetButton_Number(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[1] { 3 };
                                    SetButton_Number(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                            }
                            break;
                        case 1: //ボード3枚
                            correctButtonRand = UnityEngine.Random.Range(0, 3);
                            buttonsId = new int[3] { 3, 4, 5 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[2] { 4, 5 };
                                    SetButton_Number(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[2] { 3, 5 };
                                    SetButton_Number(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 2:
                                    buttonsId = new int[2] { 3, 4 };
                                    SetButton_Number(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                            }
                            break;
                        case 2: //ボード4枚
                            correctButtonRand = UnityEngine.Random.Range(0, 4);
                            buttonsId = new int[4] { 0, 2, 6, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[3] { 2, 6, 8 };
                                    SetButton_Number(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[3] { 0, 6, 8 };
                                    SetButton_Number(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 2:
                                    buttonsId = new int[3] { 0, 3, 8 };
                                    SetButton_Number(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 3:
                                    buttonsId = new int[3] { 0, 3, 6 };
                                    SetButton_Number(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 3: //ボード5枚
                            correctButtonRand = UnityEngine.Random.Range(0, 5);
                            buttonsId = new int[5] { 0, 2, 4, 6, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[4] { 2, 4, 6, 8 };
                                    SetButton_Number(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[4] { 0, 4, 6, 8 };
                                    SetButton_Number(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 2:
                                    buttonsId = new int[4] { 0, 2, 6, 8 };
                                    SetButton_Number(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 3:
                                    buttonsId = new int[4] { 0, 2, 4, 8 };
                                    SetButton_Number(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 4:
                                    buttonsId = new int[4] { 0, 2, 4, 6 };
                                    SetButton_Number(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 4: //ボード6枚
                            correctButtonRand = UnityEngine.Random.Range(0, 6);
                            buttonsId = new int[6] { 0, 2, 3, 5, 6, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[5] { 2, 3, 5, 6, 8 };
                                    SetButton_Number(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[5] { 0, 3, 5, 6, 8 };
                                    SetButton_Number(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 2:
                                    buttonsId = new int[5] { 0, 2, 5, 6, 8 };
                                    SetButton_Number(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 3:
                                    buttonsId = new int[5] { 0, 2, 3, 6, 8 };
                                    SetButton_Number(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                                case 4:
                                    buttonsId = new int[5] { 0, 2, 3, 5, 8 };
                                    SetButton_Number(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 5:
                                    buttonsId = new int[5] { 0, 2, 3, 5, 6 };
                                    SetButton_Number(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 5: //ボード7枚
                            correctButtonRand = UnityEngine.Random.Range(0, 7);
                            buttonsId = new int[7] { 0, 1, 2, 4, 6, 7, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[6] { 1, 2, 4, 6, 7, 8 };
                                    SetButton_Number(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[6] { 0, 2, 4, 6, 7, 8 };
                                    SetButton_Number(1, buttonsId, correctNumber);
                                    correct = "UpperCenter";
                                    break;
                                case 2:
                                    buttonsId = new int[6] { 0, 1, 4, 6, 7, 8 };
                                    SetButton_Number(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 3:
                                    buttonsId = new int[6] { 0, 1, 2, 6, 7, 8 };
                                    SetButton_Number(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 4:
                                    buttonsId = new int[6] { 0, 1, 2, 4, 7, 8 };
                                    SetButton_Number(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 5:
                                    buttonsId = new int[6] { 0, 1, 2, 4, 6, 8 };
                                    SetButton_Number(7, buttonsId, correctNumber);
                                    correct = "LowerCenter";
                                    break;
                                case 6:
                                    buttonsId = new int[6] { 0, 1, 2, 4, 6, 7 };
                                    SetButton_Number(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 6: //ボード8枚
                            correctButtonRand = UnityEngine.Random.Range(0, 8);
                            buttonsId = new int[8] { 0, 1, 2, 3, 5, 6, 7, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[7] { 1, 2, 3, 5, 6, 7, 8 };
                                    SetButton_Number(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[7] { 0, 2, 3, 5, 6, 7, 8 };
                                    SetButton_Number(1, buttonsId, correctNumber);
                                    correct = "UpperCenter";
                                    break;
                                case 2:
                                    buttonsId = new int[7] { 0, 1, 3, 5, 6, 7, 8 };
                                    SetButton_Number(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 3:
                                    buttonsId = new int[7] { 0, 1, 2, 5, 6, 7, 8 };
                                    SetButton_Number(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 4:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 6, 7, 8 };
                                    SetButton_Number(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                                case 5:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 5, 7, 8 };
                                    SetButton_Number(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 6:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 5, 6, 8 };
                                    SetButton_Number(7, buttonsId, correctNumber);
                                    correct = "LowerCenter";
                                    break;
                                case 7:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 5, 6, 7 };
                                    SetButton_Number(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 7: //ボード9枚
                            correctButtonRand = UnityEngine.Random.Range(0, 9);
                            buttonsId = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
                                    SetButton_Number(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[8] { 0, 2, 3, 4, 5, 6, 7, 8 };
                                    SetButton_Number(1, buttonsId, correctNumber);
                                    correct = "UpperCenter";
                                    break;
                                case 2:
                                    buttonsId = new int[8] { 0, 1, 3, 4, 5, 6, 7, 8 };
                                    SetButton_Number(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 3:
                                    buttonsId = new int[8] { 0, 1, 2, 4, 5, 6, 7, 8 };
                                    SetButton_Number(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 4:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 5, 6, 7, 8 };
                                    SetButton_Number(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 5:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 6, 7, 8 };
                                    SetButton_Number(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                                case 6:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 5, 7, 8 };
                                    SetButton_Number(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 7:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 5, 6, 8 };
                                    SetButton_Number(7, buttonsId, correctNumber);
                                    correct = "LowerCenter";
                                    break;
                                case 8:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
                                    SetButton_Number(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                    }
                }

                private void SetButtonType_Count(int pattern, int correctNumber)
                {
                    switch (pattern)
                    {
                        case 0: //ボード2枚
                            correctButtonRand = UnityEngine.Random.Range(0, 2);
                            buttonsId = new int[2] { 3, 5 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[1] { 5 };
                                    SetButton_Count(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[1] { 3 };
                                    SetButton_Count(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                            }
                            break;
                        case 1: //ボード3枚
                            correctButtonRand = UnityEngine.Random.Range(0, 3);
                            buttonsId = new int[3] { 3, 4, 5 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[2] { 4, 5 };
                                    SetButton_Count(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[2] { 3, 5 };
                                    SetButton_Count(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 2:
                                    buttonsId = new int[2] { 3, 4 };
                                    SetButton_Count(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                            }
                            break;
                        case 2: //ボード4枚
                            correctButtonRand = UnityEngine.Random.Range(0, 4);
                            buttonsId = new int[4] { 0, 2, 6, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[3] { 2, 6, 8 };
                                    SetButton_Count(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[3] { 0, 6, 8 };
                                    SetButton_Count(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 2:
                                    buttonsId = new int[3] { 0, 3, 8 };
                                    SetButton_Count(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 3:
                                    buttonsId = new int[3] { 0, 3, 6 };
                                    SetButton_Count(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 3: //ボード5枚
                            correctButtonRand = UnityEngine.Random.Range(0, 5);
                            buttonsId = new int[5] { 0, 2, 4, 6, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[4] { 2, 4, 6, 8 };
                                    SetButton_Count(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[4] { 0, 4, 6, 8 };
                                    SetButton_Count(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 2:
                                    buttonsId = new int[4] { 0, 2, 6, 8 };
                                    SetButton_Count(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 3:
                                    buttonsId = new int[4] { 0, 2, 4, 8 };
                                    SetButton_Count(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 4:
                                    buttonsId = new int[4] { 0, 2, 4, 6 };
                                    SetButton_Count(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 4: //ボード6枚
                            correctButtonRand = UnityEngine.Random.Range(0, 6);
                            buttonsId = new int[6] { 0, 2, 3, 5, 6, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[5] { 2, 3, 5, 6, 8 };
                                    SetButton_Count(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[5] { 0, 3, 5, 6, 8 };
                                    SetButton_Count(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 2:
                                    buttonsId = new int[5] { 0, 2, 5, 6, 8 };
                                    SetButton_Count(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 3:
                                    buttonsId = new int[5] { 0, 2, 3, 6, 8 };
                                    SetButton_Count(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                                case 4:
                                    buttonsId = new int[5] { 0, 2, 3, 5, 8 };
                                    SetButton_Count(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 5:
                                    buttonsId = new int[5] { 0, 2, 3, 5, 6 };
                                    SetButton_Count(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 5: //ボード7枚
                            correctButtonRand = UnityEngine.Random.Range(0, 7);
                            buttonsId = new int[7] { 0, 1, 2, 4, 6, 7, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[6] { 1, 2, 4, 6, 7, 8 };
                                    SetButton_Count(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[6] { 0, 2, 4, 6, 7, 8 };
                                    SetButton_Count(1, buttonsId, correctNumber);
                                    correct = "UpperCenter";
                                    break;
                                case 2:
                                    buttonsId = new int[6] { 0, 1, 4, 6, 7, 8 };
                                    SetButton_Count(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 3:
                                    buttonsId = new int[6] { 0, 1, 2, 6, 7, 8 };
                                    SetButton_Count(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 4:
                                    buttonsId = new int[6] { 0, 1, 2, 4, 7, 8 };
                                    SetButton_Count(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 5:
                                    buttonsId = new int[6] { 0, 1, 2, 4, 6, 8 };
                                    SetButton_Count(7, buttonsId, correctNumber);
                                    correct = "LowerCenter";
                                    break;
                                case 6:
                                    buttonsId = new int[6] { 0, 1, 2, 4, 6, 7 };
                                    SetButton_Count(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 6: //ボード8枚
                            correctButtonRand = UnityEngine.Random.Range(0, 8);
                            buttonsId = new int[8] { 0, 1, 2, 3, 5, 6, 7, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[7] { 1, 2, 3, 5, 6, 7, 8 };
                                    SetButton_Count(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[7] { 0, 2, 3, 5, 6, 7, 8 };
                                    SetButton_Count(1, buttonsId, correctNumber);
                                    correct = "UpperCenter";
                                    break;
                                case 2:
                                    buttonsId = new int[7] { 0, 1, 3, 5, 6, 7, 8 };
                                    SetButton_Count(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 3:
                                    buttonsId = new int[7] { 0, 1, 2, 5, 6, 7, 8 };
                                    SetButton_Count(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 4:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 6, 7, 8 };
                                    SetButton_Count(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                                case 5:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 5, 7, 8 };
                                    SetButton_Count(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 6:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 5, 6, 8 };
                                    SetButton_Count(7, buttonsId, correctNumber);
                                    correct = "LowerCenter";
                                    break;
                                case 7:
                                    buttonsId = new int[7] { 0, 1, 2, 3, 5, 6, 7 };
                                    SetButton_Count(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                        case 7: //ボード9枚
                            correctButtonRand = UnityEngine.Random.Range(0, 9);
                            buttonsId = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                            for (int i = 0; i < buttonsId.Length; i++)
                            {
                                buttons[buttonsId[i]].SetActive(true);
                            }
                            switch (correctButtonRand)
                            {
                                case 0:
                                    buttonsId = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
                                    SetButton_Count(0, buttonsId, correctNumber);
                                    correct = "UpperLeft";
                                    break;
                                case 1:
                                    buttonsId = new int[8] { 0, 2, 3, 4, 5, 6, 7, 8 };
                                    SetButton_Count(1, buttonsId, correctNumber);
                                    correct = "UpperCenter";
                                    break;
                                case 2:
                                    buttonsId = new int[8] { 0, 1, 3, 4, 5, 6, 7, 8 };
                                    SetButton_Count(2, buttonsId, correctNumber);
                                    correct = "UpperRight";
                                    break;
                                case 3:
                                    buttonsId = new int[8] { 0, 1, 2, 4, 5, 6, 7, 8 };
                                    SetButton_Count(3, buttonsId, correctNumber);
                                    correct = "CenterLeft";
                                    break;
                                case 4:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 5, 6, 7, 8 };
                                    SetButton_Count(4, buttonsId, correctNumber);
                                    correct = "Center";
                                    break;
                                case 5:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 6, 7, 8 };
                                    SetButton_Count(5, buttonsId, correctNumber);
                                    correct = "CenterRight";
                                    break;
                                case 6:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 5, 7, 8 };
                                    SetButton_Count(6, buttonsId, correctNumber);
                                    correct = "LowerLeft";
                                    break;
                                case 7:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 5, 6, 8 };
                                    SetButton_Count(7, buttonsId, correctNumber);
                                    correct = "LowerCenter";
                                    break;
                                case 8:
                                    buttonsId = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
                                    SetButton_Count(8, buttonsId, correctNumber);
                                    correct = "LowerRight";
                                    break;
                            }
                            break;
                    }
                }

                private void SetButton_Number(int correctButtonId, int[] buttonsId, int correctNum)
                {
                    switch (correctNum)
                    {
                        case 0:
                            selectNumber = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                            pattarnSortOrigin = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                            break;
                        case 1:
                            selectNumber = new int[9] { 0, 2, 3, 4, 5, 6, 7, 8, 9 };
                            pattarnSortOrigin = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 }; //１文字以外(id=0)
                            break;
                        case 2:
                            selectNumber = new int[9] { 0, 1, 3, 4, 5, 6, 7, 8, 9 };
                            pattarnSortOrigin = new int[8] { 0, 2, 3, 4, 5, 6, 7, 8 }; //2文字以外(id=1)
                            break;
                        case 3:
                            selectNumber = new int[9] { 0, 1, 2, 4, 5, 6, 7, 8, 9 };
                            pattarnSortOrigin = new int[8] { 0, 1, 3, 4, 5, 6, 7, 8 }; //3文字以外(id=2)
                            break;
                        case 4:
                            selectNumber = new int[9] { 0, 1, 2, 3, 5, 6, 7, 8, 9 };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 4, 5, 6, 7, 8 }; //4文字以外(id=3)
                            break;
                        case 5:
                            selectNumber = new int[9] { 0, 1, 2, 3, 4, 6, 7, 8, 9 };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 5, 6, 7, 8 }; //5文字以外(id=4)
                            break;
                        case 6:
                            selectNumber = new int[9] { 0, 1, 2, 3, 4, 5, 7, 8, 9 };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 6, 7, 8 }; //6文字以外(id=5)
                            break;
                        case 7:
                            selectNumber = new int[9] { 0, 1, 2, 3, 4, 5, 6, 8, 9 };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 5, 7, 8 }; //7文字以外(id=6)
                            break;
                        case 8:
                            selectNumber = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 9 };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 5, 6, 8 }; //8文字以外(id=7)
                            break;
                        case 9:
                            selectNumber = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 }; //9文字以外(id=8)
                            break;
                    }
                    var patternSort = pattarnSortOrigin.OrderBy(n => Guid.NewGuid()).ToArray();
                    var numberSort = selectNumber.OrderBy(n => Guid.NewGuid()).ToArray();
                    buttonTexts[correctButtonId].text = wordPatterns[patternSort[0]].Replace("*", correctNum.ToString());
                    for (int i = 0; i < buttonsId.Length; i++)
                    {
                        if (i == 8) buttonTexts[buttonsId[i]].text = wordPatterns[patternSort[i - 1]].Replace("*", numberSort[i].ToString());
                        else if (i == 7) buttonTexts[buttonsId[i]].text = wordPatterns[patternSort[i]].Replace("*", numberSort[i].ToString());
                        else buttonTexts[buttonsId[i]].text = wordPatterns[patternSort[i + 1]].Replace("*", numberSort[i].ToString());
                    }
                }

                private void SetButton_Count(int correctButtonId, int[] buttonId, int correctNum)
                {

                    switch (correctNum)
                    {
                        case 0:
                            questionNumberWord = new string[] { "0", "2", "3", "4", "5", "6", "7", "8", "9" };
                            pattarnSortOrigin = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
                            break;
                        case 1:
                            questionNumberWord = new string[] { "0", "1", "3", "4", "5", "6", "7", "8", "9" };
                            pattarnSortOrigin = new int[8] { 0, 2, 3, 4, 5, 6, 7, 8 };
                            break;
                        case 2:
                            questionNumberWord = new string[] { "0", "1", "2", "4", "5", "6", "7", "8", "9" };
                            pattarnSortOrigin = new int[8] { 0, 1, 3, 4, 5, 6, 7, 8 };
                            break;
                        case 3:
                            questionNumberWord = new string[] { "0", "1", "2", "3", "5", "6", "7", "8", "9" };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 4, 5, 6, 7, 8 };
                            break;
                        case 4:
                            questionNumberWord = new string[] { "0", "1", "2", "3", "4", "6", "7", "8", "9" };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 5, 6, 7, 8 };
                            break;
                        case 5:
                            questionNumberWord = new string[] { "0", "1", "2", "3", "4", "5", "7", "8", "9" };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 6, 7, 8 };
                            break;
                        case 6:
                            questionNumberWord = new string[] { "0", "1", "2", "3", "4", "5", "6", "8", "9" };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 5, 7, 8 };
                            break;
                        case 7:
                            questionNumberWord = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "9" };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 5, 6, 8 };
                            break;
                        case 8:
                            questionNumberWord = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
                            pattarnSortOrigin = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
                            break;
                    }
                    var patternSort = pattarnSortOrigin.OrderBy(n => Guid.NewGuid()).ToArray();
                    var numberSort = questionNumberWord.OrderBy(n => Guid.NewGuid()).ToArray();
                    buttonTexts[correctButtonId].text = wordPatterns[correctNum].Replace("*", numberSort[0]);
                    for (int i = 0; i < buttonsId.Length; i++)
                    {
                        buttonTexts[buttonId[i]].text = wordPatterns[patternSort[i]].Replace("*", numberSort[i + 1]);
                    }
                }
                //問題の解答
                private void QuestionAnswer()
                {
                    //解答までの時間を加算
                    answerTimer += Time.deltaTime;
                    //ボタンが押されたら
                    if (isPush == true)
                    {
                        //押されたボタンと正解のボタンが同じなら
                        if (pushButtonsName == correct)state = GameState.Correct; //正解
                        else state = GameState.Close; //不正解
                    }
                }
                //正解
                private void Correct()
                {
                    //キャラの描画を前面に
                    characterObject.transform.SetAsLastSibling();
                    characterObject.GetComponent<Image>().sprite = characterSprite[1];
                    //「正解」を表示
                    band.GetComponent<Image>().sprite = bandSprite[0];
                    band.SetActive(true);

                    ResultDrawEnd(true);
                }

                private void Close()
                {
                    //キャラの描画を前面に
                    characterObject.transform.SetAsLastSibling();
                    characterObject.GetComponent<Image>().sprite = characterSprite[0];
                    //「おしい」を表示
                    band.GetComponent<Image>().sprite = bandSprite[1];
                    band.SetActive(true);
                    ResultDrawEnd(false);
                }

                //リザルトの表示を終了
                private void ResultDrawEnd(bool isCorrect)
                {
                    //表示時間が０になったら
                    resultDrawTime -= Time.deltaTime;
                    if (resultDrawTime <= 0.0f)
                    {
                        //帯を非アクティブに
                        band.SetActive(false);
                        resultDrawTime = 1.0f; //表示時間を初期化
                        nowQuestionCount += 1; //現在の設問数を加算
                        if (isCorrect == true) correctCount += 1;
                        //現在の設問数が全設問数に到達したらリザルトへ(違ったら次の設問へ)
                        if (nowQuestionCount == questionAllCount) state = GameState.Result;
                        else state = GameState.Next;
                    }
                }
                //次の課題です
                private void Next()
                {
                    //キャラクターの描画を背面へ
                    characterObject.transform.SetSiblingIndex(2);
                    characterObject.GetComponent<Image>().sprite = characterSprite[0];
                    //問題を非アクティブに
                    questionText.SetActive(false);
                    //「次の課題です」を表示
                    band.GetComponent<Image>().sprite = bandSprite[2];
                    band.SetActive(true);
                    //全ボタンを非アクティブに
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].SetActive(false);
                    }
                    //表示時間が０になったら
                    nextBandDrawTime -= Time.deltaTime;
                    if (nextBandDrawTime <= 0.0f)
                    {
                        band.SetActive(false);
                        nextBandDrawTime = 1.0f;
                        isPush = false;
                        state = GameState.QuestionSet;
                    }
                }
                //終了-リザルトへ
                private void Result()
                {
                    //問題とボタンを非アクティブに
                    questionText.SetActive(false);
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].SetActive(false);
                    }
                    //「終了」を表示
                    band.GetComponent<Image>().sprite = bandSprite[3];
                    band.SetActive(true);         
                    characterObject.GetComponent<Image>().sprite = characterSprite[0];
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
