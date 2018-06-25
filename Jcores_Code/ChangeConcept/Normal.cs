using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//色と文字の課題(その２)
namespace Jcores
{
    namespace ExecutiveFunction
    {
        namespace ChangeConcept
        {
            public class Normal : MonoBehaviour
            {
                enum GameState
                {
                    None,
                    QuestionSet_Practice,
                    QuestionSet_Production,
                    QuestionAnswer,
                    Correct,
                    Miss,
                    ProductionStart,
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
                private GameObject band;           //バンド(次の課題です、終了を表示)
                [SerializeField]
                private Sprite[] bandSprite;       //バンドスプライト(0:正解 1:おしい 2:次の課題です 3:終了)
                [SerializeField]
                private GameObject resultImage;    //正解・不正解表示用オブジェクト
                [SerializeField]
                private Sprite[] resultSprite;     //正解・不正解のスプライト
                [SerializeField]
                private GameObject productionText;  //「さあ本番です」の表示用オブジェクト

                [SerializeField]
                private GameObject colorButtons;    //親ボタンオブジェクト
                [SerializeField]
                private Image[] buttons;            //ボタンオブジェクト(0:あか 1:あお 2:きいろ 3:みどり 4:くろ)
                [SerializeField]
                private Sprite[] colorButtonSprite; //色のボタンのスプライト
                [SerializeField]
                private Sprite[] wordButtonSprite;  //文字のボタンのスプライト

                [SerializeField]
                private GameObject wordObjectsPar;     //文字イメージの親オブジェクト
                [SerializeField]
                private GameObject[] wordObjects;      //文字イメージオブジェクト
                [SerializeField]
                private Image[] words;                 //文字イメージ(0～4:左上から右上の一列 5～9:左下から右下の一列)
                [SerializeField]
                private GameObject selectWordBack;     //現在の問題の文字の背景

                [SerializeField]
                private Sprite[] practiceWord;       //練習用単語(文字と色があっている)
                [SerializeField]
                private Sprite[] WordSprite_Red;     //単語(あか)
                [SerializeField]
                private Sprite[] WordSprite_Blue;    //単語(あお)
                [SerializeField]
                private Sprite[] WordSprite_Yellow;  //単語(きいろ)
                [SerializeField]
                private Sprite[] WordSprite_Green;   //単語(みどり)
                [SerializeField]
                private Sprite[] WordSprite_Black;   //単語(くろ)
                [SerializeField]
                private Sprite[] ColorSprite_Red;    //色(あか)
                [SerializeField]
                private Sprite[] ColorSprite_Blue;   //色(あお)
                [SerializeField]
                private Sprite[] ColorSprite_Yellow; //色(きいろ)
                [SerializeField]
                private Sprite[] ColorSprite_Green;  //色(みどり)
                [SerializeField]
                private Sprite[] ColorSprite_Black;  //色(くろ)

                //問題
                private string[] questionWord = {
                    "文字の色を表すボタンを押してね",
                    "文字の意味を表すボタンを押してね"
                };
                private int question_rand;    //問題を選ぶための変数

                private int color;  //色を選ぶための変数

                private int buttontype; //ボタンタイプを選ぶための変数

                private string[] corrects = new string[10];  //正解
                private string pushButtonsName; //押されたボタンの名前

                private int wordNum;    //現在の選ばれているの文字の番号

                private bool isPush;    //押されたか

                private float questionAllCount;   //すべての問題数
                private int nowQuestionCount;   //現在の問題数
                private int one_QuestionCount;  //１設問分の問題数

                private float correctCount;       //正解数
                private float correctAvg;       //正解率
                private float answerTimer;      //解答時間
                private float answerAvgTime;    //平均解答時間

                private float resultDrawTime;       //正解・不正解の表示時間
                private float nextBandDrawTime;     //次の設問ですの表示時間
                private float productionDrawTime;   //さぁ本番ですの表示時間
                private float endDrawTime;          //終了の表示時間

                // Use this for initialization
                void Start()
                {
                    state = GameState.None;  //ステートの初期化
                    wordNum = 0;

                    Settings.Instance.SetSettings();
                    //全設問数を取得
                    questionAllCount = Settings.Instance.CurrentSettings.QuestionNum;
                    //各変数を初期化
                    nowQuestionCount = 0;
                    one_QuestionCount = 15;

                    correctCount = 0;
                    correctAvg = 0.0f;
                    answerTimer = 0.0f;
                    answerAvgTime = 0.0f;

                    resultDrawTime = 0.2f;
                    productionDrawTime = 1.0f;
                    nextBandDrawTime = 1.0f;
                    endDrawTime = 1.0f;

                    QuestionSet_Practice();

                }

                // Update is called once per frame
                void Update()
                {
                    //時間が０になったら
                    if (state == GameState.None && startTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                    {
                        state = GameState.QuestionAnswer;
                    }

                    switch (state)
                    {
                        case GameState.QuestionSet_Practice:
                            QuestionSet_Practice();
                            break;
                        case GameState.QuestionSet_Production:
                            QuestionSet_Production();
                            break;
                        case GameState.QuestionAnswer:
                            QuestionAnswer();
                            break;
                        case GameState.Correct:
                            Correct();
                            break;
                        case GameState.Miss:
                            Miss();
                            break;
                        case GameState.ProductionStart:
                            ProductionStart();
                            break;
                        case GameState.Next:
                            NextQuestion();
                            break;
                        case GameState.Result:
                            Result();
                            break;
                    }

                }

                //問題をセット(練習)
                private void QuestionSet_Practice()
                {
                    //ボタンと文字をアクティブに
                    colorButtons.SetActive(true);
                    wordObjectsPar.SetActive(true);
                    //5～9番までの文字を非アクティブに(練習は0～4番まで)
                    for (int n = 5; n < wordObjects.Length; n++)
                    {
                        wordObjects[n].SetActive(false);
                    }
                    //文字の背景の枠をアクティブに
                    selectWordBack.GetComponent<RectTransform>().position = wordObjects[wordNum].GetComponent<RectTransform>().position;
                    selectWordBack.SetActive(true);
                    //ボタンの種類を設定
                    buttontype = Random.Range(0, 2);
                    switch (buttontype)
                    {
                        case 0:
                            for (int i = 0; i < buttons.Length; i++)
                            {
                                buttons[i].sprite = colorButtonSprite[i];
                            }
                            break;
                        case 1:
                            for (int i = 0; i < buttons.Length; i++)
                            {
                                buttons[i].sprite = wordButtonSprite[i];
                            }
                            break;
                    }
                    //問題を設定
                    question_rand = Random.Range(0, questionWord.Length);
                    teacherText.GetComponent<Text>().text = questionWord[question_rand];
                    //文字の色を表すボタンを押してね
                    if (question_rand == 0)
                    {
                        SetPracticeWord();
                    }
                    //文字の意味を表すボタンを押してね
                    else if (question_rand == 1)
                    {
                        SetPracticeWord();
                    }

                    if (startTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                    {
                        state = GameState.QuestionAnswer;
                    }
                }
                //練習での文字の設定
                private void SetPracticeWord()
                {
                    for (int i = 0; i < words.Length / 2; i++)
                    {
                        color = Random.Range(0, 5);
                        switch (color)
                        {
                            case 0: //あか
                                words[i].sprite = practiceWord[0];
                                corrects[i] = "red";
                                break;
                            case 1: //あお
                                words[i].sprite = practiceWord[1];
                                corrects[i] = "blue";
                                break;
                            case 2: //きいろ
                                words[i].sprite = practiceWord[2];
                                corrects[i] = "yellow";
                                break;
                            case 3: //みどり
                                words[i].sprite = practiceWord[3];
                                corrects[i] = "green";
                                break;
                            case 4: //くろ
                                words[i].sprite = practiceWord[4];
                                corrects[i] = "black";
                                break;
                        }
                    }
                }

                //問題をセット(本番)
                private void QuestionSet_Production()
                {
                    //ボタンと文字をアクティブに
                    colorButtons.SetActive(true);
                    wordObjectsPar.SetActive(true);
                    //5～9番までの文字をアクティブに
                    for (int n = 5; n < wordObjects.Length; n++)
                    {
                        wordObjects[n].SetActive(true);
                    }
                    //文字の背景の枠をアクティブに
                    selectWordBack.SetActive(true);
                    selectWordBack.GetComponent<RectTransform>().position = wordObjects[wordNum].GetComponent<RectTransform>().position;
                    //文字の色を表すボタンを押してね
                    if (question_rand == 0)
                    {
                        for (int i = 0; i < words.Length; i++)
                        {
                            color = Random.Range(0, 5);
                            switch (color)
                            {
                                case 0: //あか
                                    words[i].sprite = ColorSprite_Red[Random.Range(0, ColorSprite_Red.Length)];
                                    corrects[i] = "red";
                                    break;
                                case 1: //あお
                                    words[i].sprite = ColorSprite_Blue[Random.Range(0, ColorSprite_Blue.Length)];
                                    corrects[i] = "blue";
                                    break;
                                case 2: //きいろ
                                    words[i].sprite = ColorSprite_Yellow[Random.Range(0, ColorSprite_Yellow.Length)];
                                    corrects[i] = "yellow";
                                    break;
                                case 3: //みどり
                                    words[i].sprite = ColorSprite_Green[Random.Range(0, ColorSprite_Green.Length)];
                                    corrects[i] = "green";
                                    break;
                                case 4: //くろ
                                    words[i].sprite = ColorSprite_Black[Random.Range(0, ColorSprite_Black.Length)];
                                    corrects[i] = "black";
                                    break;
                            }
                        }
                    }
                    //文字の意味を表すボタンを押してね
                    else if (question_rand == 1)
                    {
                        for (int i = 0; i < words.Length; i++)
                        {
                            color = Random.Range(0, 5);
                            switch (color)
                            {
                                case 0: //あか
                                    words[i].sprite = WordSprite_Red[Random.Range(0, WordSprite_Red.Length)];
                                    corrects[i] = "red";
                                    break;
                                case 1: //あお
                                    words[i].sprite = WordSprite_Blue[Random.Range(0, WordSprite_Blue.Length)];
                                    corrects[i] = "blue";
                                    break;
                                case 2: //きいろ
                                    words[i].sprite = WordSprite_Yellow[Random.Range(0, WordSprite_Yellow.Length)];
                                    corrects[i] = "yellow";
                                    break;
                                case 3: //みどり
                                    words[i].sprite = WordSprite_Green[Random.Range(0, WordSprite_Green.Length)];
                                    corrects[i] = "green";
                                    break;
                                case 4: //くろ
                                    words[i].sprite = WordSprite_Black[Random.Range(0, WordSprite_Black.Length)];
                                    corrects[i] = "black";
                                    break;
                            }
                        }
                    }
                    state = GameState.QuestionAnswer;
                }
                //問題の解答
                private void QuestionAnswer()
                {
                    //解答までの時間を加算
                    answerTimer += Time.deltaTime;
                    //文字の背景を今の問題の文字の位置へ
                    selectWordBack.GetComponent<RectTransform>().position = wordObjects[wordNum].GetComponent<RectTransform>().position;
                    //ボタンが押されたら
                    if (isPush == true)
                    {
                        //押されたボタンと正解のボタンが同じなら
                        if (pushButtonsName == corrects[wordNum]) state = GameState.Correct; //正解
                        else state = GameState.Miss; //不正解
                    }
                }
                //正解
                private void Correct()
                {
                    //「正解」を表示
                    resultImage.GetComponent<Image>().sprite = resultSprite[0];
                    resultImage.SetActive(true);
                    //リザルトの表示を終了
                    ResultDrawEnd(true);
                }
                //不正解
                private void Miss()
                {
                    //「不正解」を表示
                    resultImage.GetComponent<Image>().sprite = resultSprite[1];
                    resultImage.SetActive(true);
                    ResultDrawEnd(false);
                }
                //リザルトの表示を終了
                private void ResultDrawEnd(bool isCorrect)
                {
                    //表示時間が０になったら
                    resultDrawTime -= Time.deltaTime;
                    if (resultDrawTime <= 0.0f)
                    {
                        wordNum += 1;   //文字番号を加算
                        resultImage.SetActive(false);　//「正解・不正解」を非アクティブに
                        resultDrawTime = 0.2f;  //表示時間を初期化
                        nowQuestionCount += 1;  //現在の設問数を加算
                        one_QuestionCount -= 1; //設問数を減らしていく
                        isPush = false; //押されてたを押されてないにする
                        //正解だったら
                        if (isCorrect == true)
                        {
                            correctCount += 1;  //正解の数を加算
                            //正解の数で背景を変更
                            if (correctCount == 5) back.sprite = backGroundSprite[0];
                            else if (correctCount == 15) back.sprite = backGroundSprite[1];
                            else if (correctCount == 30) back.sprite = backGroundSprite[2];
                            else if (correctCount == 45) back.sprite = backGroundSprite[3];
                            else if (correctCount == 60) back.sprite = backGroundSprite[4];
                            else if (correctCount == 75) back.sprite = backGroundSprite[5];
                        }
                        //一つの設問の解答数でステートを変更
                        if (nowQuestionCount == questionAllCount) state = GameState.Result;
                        else if (one_QuestionCount > 10) state = GameState.QuestionAnswer;
                        else if (one_QuestionCount == 10) state = GameState.ProductionStart;
                        else if (one_QuestionCount > 0) state = GameState.QuestionAnswer;
                        else if (one_QuestionCount == 0) state = GameState.Next;
                    }
                }
                //さぁ本番です
                private void ProductionStart()
                {
                    //ボタンと文字と文字の背景を非アクティブに
                    colorButtons.SetActive(false);
                    wordObjectsPar.SetActive(false);
                    selectWordBack.SetActive(false);
                    //「さぁ本番です」を表示
                    productionText.SetActive(true);
                    productionDrawTime -= Time.deltaTime;
                    //表示時間が０になったら
                    if (productionDrawTime <= 0.0f)
                    {
                        wordNum = 0;　//文字番号を初期化
                        //「さぁ本番です」を非アクティブに
                        productionText.SetActive(false);
                        productionDrawTime = 1.0f; //表示時間を初期化
                        state = GameState.QuestionSet_Production;
                    }
                }
                //次の課題です
                private void NextQuestion()
                {
                    //ボタンと文字と文字の背景を非アクティブに
                    colorButtons.SetActive(false);
                    wordObjectsPar.SetActive(false);
                    selectWordBack.SetActive(false);
                    //「次の課題です」を表示
                    band.GetComponent<Image>().sprite = bandSprite[0];
                    band.SetActive(true);
                    //表示時間が０になったら
                    nextBandDrawTime -= Time.deltaTime;
                    if (nextBandDrawTime <= 0.0f)
                    {
                        wordNum = 0;  //文字番号を初期化
                        one_QuestionCount = 15;  //１設問分の問題数を初期化
                        band.SetActive(false);  //帯を非アクティブに
                        nextBandDrawTime = 1.0f;  //表示時間を初期化
                        state = GameState.QuestionSet_Practice;
                    }
                }
                //終了-リザルトへ
                private void Result()
                {
                    //ボタンと文字と文字の背景を非アクティブに
                    colorButtons.SetActive(false);
                    wordObjectsPar.SetActive(false);
                    selectWordBack.SetActive(false);
                    //「終了」を表示
                    band.GetComponent<Image>().sprite = bandSprite[1];
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
                public void GetPushButton(string name)
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

