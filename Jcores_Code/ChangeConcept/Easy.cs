using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jcores
{
    namespace ExecutiveFunction
    {
        namespace ChangeConcept
        {
            public class Easy : MonoBehaviour
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
                private GameObject startTimer;      //開始タイマー
                [SerializeField]
                private GameObject band;           //バンド(正解、おしいなどを表示)
                [SerializeField]
                private Sprite[] bandSprite;       //バンドスプライト(0:正解 1:おしい 2:次の課題です 3:終了)

                [SerializeField]
                private GameObject QuestionTextObject;  //問題表示オブジェクト
                private Text questionText;              //問題表示テキスト

                [SerializeField]
                private GameObject characterObject; //キャラクターのオブジェクト
                [SerializeField]
                private Sprite[] characterSprite;     //キャラクターのスプライト(0:普通顔 1:正解顔)

                [SerializeField]
                private GameObject RightButton;     //右の看板ボタン
                private Image RightWordImage;       //右の単語イメージ
                [SerializeField]
                private GameObject LeftButton;      //左の看板ボタン           
                private Image LeftWordImage;        //左の単語イメージ


                [SerializeField]
                private Sprite[] WordSprite_Red; //単語(あか)
                [SerializeField]
                private Sprite[] WordSprite_Blue; //単語(あお)
                [SerializeField]
                private Sprite[] WordSprite_Yellow; //単語(きいろ)
                [SerializeField]
                private Sprite[] WordSprite_Green; //単語(みどり)
                [SerializeField]
                private Sprite[] ColorSprite_Red; //色(あか)
                [SerializeField]
                private Sprite[] ColorSprite_Blue; //色(あお)
                [SerializeField]
                private Sprite[] ColorSprite_Yellow; //色(きいろ)
                [SerializeField]
                private Sprite[] ColorSprite_Green; //色(みどり)
                //色選択配列
                private string[] colorWord = { "あか", "あお", "きいろ", "みどり" };
                //問題選択配列
                private string[] questionWord = { "の文字なのはどっち？", "の色なのはどっち？" };
                private int color;  //色を選ぶランダム変数
                private int word;   //文字を選ぶランダム変数
                private string correctButton = ""; //正解のボタンを指定する変数

                private bool isPush;         //押されたか？
                private string selectButton; //押されたボタンの名前

                private float questionAllCount;   //全設問数
                private int nowQuestionCount;   //現在の設問数
                private float correctCount;       //正解の数
                private float correctAvg;       //平均正解数
                private float answerTimer;      //解答時間
                private float answerAvgTime;    //平均解答時間

                private float bandDrawTime;     //正解・不正解の表示時間
                private float nextImgDrawTime;  //次の課題ですの表示時間
                private float endDrawTime;      //終了の表示時間


                // Use this for initialization
                void Start()
                {
                    state = GameState.None; //ステートの初期化

                    //問題を表示するテキストを取得
                    questionText = QuestionTextObject.GetComponent<Text>();

                    //右と左の文字を表示するイメージを取得
                    RightWordImage = RightButton.transform.Find("Image").GetComponent<Image>();
                    LeftWordImage = LeftButton.transform.Find("Image").GetComponent<Image>();

                    Settings.Instance.SetSettings();
                    //全設問数を取得
                    questionAllCount = Settings.Instance.CurrentSettings.QuestionNum;
                    //各変数を初期化
                    nowQuestionCount = 0;
                    correctCount = 0;
                    correctAvg = 0.0f;
                    answerTimer = 0.0f;
                    answerAvgTime = 0.0f;

                    bandDrawTime = 1.0f;
                    nextImgDrawTime = 1.0f;
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
                    //色・問題をランダムで設定
                    color = Random.Range(0, colorWord.Length);
                    word = Random.Range(0, questionWord.Length);
                    //問題を表示
                    questionText.text = colorWord[color] + questionWord[word];
                    QuestionTextObject.SetActive(true);

                    //ボタンのアクティブに
                    RightButton.SetActive(true);
                    LeftButton.SetActive(true);

                    //正解のボタンをランダムで設定(0:右 1:左)
                    int randCorrentButton = Random.Range(0, 2);  //正解の位置(0:右　1:左)

                    //○○の文字なのはどっち？
                    if (word == 0)
                    {
                        switch (color)
                        {
                            case 0: //あか
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = WordSprite_Red[Random.Range(0, WordSprite_Red.Length)];
                                    LeftWordImage.sprite = ColorSprite_Red[Random.Range(0, ColorSprite_Red.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {
                                    RightWordImage.sprite = ColorSprite_Red[Random.Range(0, ColorSprite_Red.Length)];
                                    LeftWordImage.sprite = WordSprite_Red[Random.Range(0, WordSprite_Red.Length)];
                                    correctButton = "Left";
                                }
                                break;
                            case 1: //あお
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = WordSprite_Blue[Random.Range(0, WordSprite_Blue.Length)];
                                    LeftWordImage.sprite = ColorSprite_Blue[Random.Range(0, ColorSprite_Blue.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {
                                    RightWordImage.sprite = ColorSprite_Blue[Random.Range(0, ColorSprite_Blue.Length)];
                                    LeftWordImage.sprite = WordSprite_Blue[Random.Range(0, WordSprite_Blue.Length)];
                                    correctButton = "Left";
                                }
                                break;
                            case 2: //きいろ
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = WordSprite_Yellow[Random.Range(0, WordSprite_Yellow.Length)];
                                    LeftWordImage.sprite = ColorSprite_Yellow[Random.Range(0, ColorSprite_Yellow.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {
                                    RightWordImage.sprite = ColorSprite_Yellow[Random.Range(0, ColorSprite_Yellow.Length)];
                                    LeftWordImage.sprite = WordSprite_Yellow[Random.Range(0, WordSprite_Yellow.Length)];
                                    correctButton = "Left";
                                }
                                break;
                            case 3: //みどり
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = WordSprite_Green[Random.Range(0, WordSprite_Green.Length)];
                                    LeftWordImage.sprite = ColorSprite_Green[Random.Range(0, ColorSprite_Green.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {
                                    RightWordImage.sprite = ColorSprite_Green[Random.Range(0, ColorSprite_Green.Length)];
                                    LeftWordImage.sprite = WordSprite_Green[Random.Range(0, WordSprite_Green.Length)];
                                    correctButton = "Left";
                                }
                                break;

                        }
                    }
                    //○○の色なのはどっち？
                    else if (word == 1)
                    {
                        switch (color)
                        {
                            case 0: //あか
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = ColorSprite_Red[Random.Range(0, ColorSprite_Red.Length)];
                                    LeftWordImage.sprite = WordSprite_Red[Random.Range(0, WordSprite_Red.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {
                                    RightWordImage.sprite = WordSprite_Red[Random.Range(0, WordSprite_Red.Length)];
                                    LeftWordImage.sprite = ColorSprite_Red[Random.Range(0, ColorSprite_Red.Length)];
                                    correctButton = "Left";
                                }
                                break;
                            case 1: //あお
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = ColorSprite_Blue[Random.Range(0, ColorSprite_Blue.Length)];
                                    LeftWordImage.sprite = WordSprite_Blue[Random.Range(0, WordSprite_Blue.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {
                                    RightWordImage.sprite = WordSprite_Blue[Random.Range(0, WordSprite_Blue.Length)];
                                    LeftWordImage.sprite = ColorSprite_Blue[Random.Range(0, ColorSprite_Blue.Length)];
                                    correctButton = "Left";
                                }
                                break;
                            case 2: //きいろ
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = ColorSprite_Yellow[Random.Range(0, ColorSprite_Yellow.Length)];
                                    LeftWordImage.sprite = WordSprite_Yellow[Random.Range(0, WordSprite_Yellow.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {

                                    RightWordImage.sprite = WordSprite_Yellow[Random.Range(0, WordSprite_Yellow.Length)];
                                    LeftWordImage.sprite = ColorSprite_Yellow[Random.Range(0, ColorSprite_Yellow.Length)];
                                    correctButton = "Left";
                                }
                                break;
                            case 3: //みどり
                                if (randCorrentButton == 0)
                                {
                                    RightWordImage.sprite = ColorSprite_Green[Random.Range(0, ColorSprite_Green.Length)];
                                    LeftWordImage.sprite = WordSprite_Green[Random.Range(0, WordSprite_Green.Length)];
                                    correctButton = "Right";
                                }
                                else if (randCorrentButton == 1)
                                {

                                    RightWordImage.sprite = WordSprite_Green[Random.Range(0, WordSprite_Green.Length)];
                                    LeftWordImage.sprite = ColorSprite_Green[Random.Range(0, ColorSprite_Green.Length)];
                                    correctButton = "Left";
                                }
                                break;
                        }
                    }
                    state = GameState.QuestionAnswer;
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
                        if (selectButton == correctButton)
                        {
                            state = GameState.Correct; //正解
                        }
                        else
                        {
                            state = GameState.Close;   //おしい
                        }
                    }
                }
                //正解
                private void Correct()
                {
                    //「正解」を表示
                    band.GetComponent<Image>().sprite = bandSprite[0];
                    band.SetActive(true);
                    //キャラの画像を変更
                    characterObject.GetComponent<Image>().sprite = characterSprite[1];
                    //帯を表示
                    BandDraw(true);
                }
                //おしい
                private void Close()
                {
                    //「おしい」を表示
                    band.GetComponent<Image>().sprite = bandSprite[1];
                    band.SetActive(true);
                    //キャラの画像を変更
                    characterObject.GetComponent<Image>().sprite = characterSprite[0];
                    //帯を表示
                    BandDraw(false);
                }
                //帯を表示
                private void BandDraw(bool isCorrect)
                {
                    //表示時間が０になったら
                    bandDrawTime -= Time.deltaTime;
                    if (bandDrawTime <= 0.0f)
                    {
                        band.SetActive(false);  //帯を非アクティブに
                        bandDrawTime = 1.0f;  //表示時間を初期化
                        nowQuestionCount += 1;  //現在の設問数を加算
                        //正解だったら正解の数を加算
                        if (isCorrect == true) correctCount += 1;
                        //現在の設問数が全設問数に到達したらリザルトへ(違ったら次の設問へ)
                        if (nowQuestionCount == questionAllCount) state = GameState.Result;
                        else state = GameState.Next;
                    }
                }
                //次の設問へ
                private void Next()
                {
                    //問題とボタンを非アクティブに
                    QuestionTextObject.SetActive(false);
                    RightButton.SetActive(false);
                    LeftButton.SetActive(false);
                    //「次の課題です」の帯を表示
                    band.GetComponent<Image>().sprite = bandSprite[2];
                    band.SetActive(true);
                    //キャラの画像を変更
                    characterObject.GetComponent<Image>().sprite = characterSprite[0];
                    //表示時間が０になったら
                    nextImgDrawTime -= Time.deltaTime;
                    if (nextImgDrawTime <= 0.0f)
                    {
                        band.SetActive(false);      //帯を非アクティブに
                        nextImgDrawTime = 1.0f;     //表示時間を初期化
                        isPush = false;              //押されてたを押されてないにする
                        state = GameState.QuestionSet;  //問題をセット
                    }
                }
                //終了ーリザルトへ
                private void Result()
                {
                    //問題とボタンを非アクティブに
                    QuestionTextObject.SetActive(false);
                    RightButton.SetActive(false);
                    LeftButton.SetActive(false);
                    //「終了」の帯を表示
                    band.GetComponent<Image>().sprite = bandSprite[3];
                    band.SetActive(true);
                    //キャラの画像を変更
                    characterObject.GetComponent<Image>().sprite = characterSprite[0];
                    //表示時間が０になったら
                    bandDrawTime -= Time.deltaTime;
                    if (bandDrawTime <= 0.0f)
                    {
                        //正解率を計算
                        correctAvg = correctCount / questionAllCount*100;
                        //平均解答時間を計算
                        answerAvgTime = answerTimer / questionAllCount;
                        //リザルトをセット
                        Settings.Instance.SetResult(correctAvg, answerAvgTime,(int)correctCount);
                        //リザルトへ移行
                        fadeTransit.Transit(SceneData.GetSceneName(SceneData.ID.ChangeConcept, SceneData.Suffix.Result));
                    }
                }
                //ボタンが押されたか
                public void OnClickAnswer(string buttonName)
                {
                    //押されていないかつ解答モードなら
                    if (isPush == false && state == GameState.QuestionAnswer)
                    {
                        selectButton = buttonName;
                        isPush = true;
                    }
                }
            }
        }
    }
}
