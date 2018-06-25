using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//練習
namespace Jcores
{
    namespace ExecutiveFunction
    {
        namespace ChangeConcept
        {
            public class VaryEasy : MonoBehaviour
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
                private Sprite[] characterSprite;     //キャラクターのスプライト(0:普通顔 1:正解顔 2:不正解顔)

                [SerializeField]
                private GameObject RightButton;     //右の看板ボタン
                private Image RightWordImage;       //右の単語イメージ
                [SerializeField]
                private GameObject LeftButton;      //左の看板ボタン           
                private Image LeftWordImage;        //左の単語イメージ

                //単語(0:青色のあか 1:赤色のあお)
                [SerializeField]
                private Sprite[] WordSprite;

                //色選択配列
                private string[] colorWord = { "あか", "あお" };
                //問題選択配列
                private string[] questionWord = { "の文字なのはどっち？", "の色なのはどっち？" };
                private int color_Rand;     //色を選ぶランダム変数
                private int word_Rand;      //文字を選ぶランダム変数
                private string correctButton = "";  //正解のボタンを指定する変数

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
                    color_Rand = Random.Range(0, colorWord.Length);
                    word_Rand = Random.Range(0, questionWord.Length);
                    //問題を表示
                    questionText.text = colorWord[color_Rand] + questionWord[word_Rand];
                    QuestionTextObject.SetActive(true);

                    //ボタンのアクティブに
                    RightButton.SetActive(true);
                    LeftButton.SetActive(true);

                    //正解のボタンをランダムで設定(0:右 1:左)
                    int correctButton_rand = Random.Range(0, 2);

                    if (color_Rand == 0 && word_Rand == 0)  //あかの文字なのはどっち？
                    {
                        if (correctButton_rand == 0)
                        {
                            RightWordImage.sprite = WordSprite[0];
                            LeftWordImage.sprite = WordSprite[1];
                            correctButton = "Right";
                        }
                        else if (correctButton_rand == 1)
                        {
                            RightWordImage.sprite = WordSprite[1];
                            LeftWordImage.sprite = WordSprite[0];
                            correctButton = "Left";
                        }
                    }
                    else if (color_Rand == 0 && word_Rand == 1)  //あかの色なのはどっち？
                    {
                        if (correctButton_rand == 0)
                        {
                            RightWordImage.sprite = WordSprite[1];
                            LeftWordImage.sprite = WordSprite[0];
                            correctButton = "Right";
                        }
                        else if (correctButton_rand == 1)
                        {
                            RightWordImage.sprite = WordSprite[0];
                            LeftWordImage.sprite = WordSprite[1];
                            correctButton = "Left";
                        }
                    }
                    else if (color_Rand == 1 && word_Rand == 0)  //あおの文字なのはどっち？
                    {
                        if (correctButton_rand == 0)
                        {
                            RightWordImage.sprite = WordSprite[1];
                            LeftWordImage.sprite = WordSprite[0];
                            correctButton = "Right";
                        }
                        else if (correctButton_rand == 1)
                        {
                            RightWordImage.sprite = WordSprite[0];
                            LeftWordImage.sprite = WordSprite[1];
                            correctButton = "Left";
                        }
                    }
                    else if (color_Rand == 1 && word_Rand == 1)  //あおの色なのはどっち？
                    {
                        if (correctButton_rand == 0)
                        {
                            RightWordImage.sprite = WordSprite[0];
                            LeftWordImage.sprite = WordSprite[1];
                            correctButton = "Right";
                        }
                        else if (correctButton_rand == 1)
                        {
                            RightWordImage.sprite = WordSprite[1];
                            LeftWordImage.sprite = WordSprite[0];
                            correctButton = "Left";
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
                            state = GameState.Correct;  //正解
                        }
                        else
                        {
                            state = GameState.Close;    //おしい
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
                    //帯の表示を終了
                    BandDrawEnd(true);
                }
                //おしい
                private void Close()
                {
                    //「おしい」を表示
                    band.GetComponent<Image>().sprite = bandSprite[1];
                    band.SetActive(true);
                    //キャラの画像を変更
                    characterObject.GetComponent<Image>().sprite = characterSprite[2];
                    //帯の表示を終了
                    BandDrawEnd(false);
                }
                //帯の表示を終了
                private void BandDrawEnd(bool isCorrect)
                {
                    //表示時間が０になったら
                    bandDrawTime -= Time.deltaTime;
                    if (bandDrawTime <= 0.0f)
                    {
                        band.SetActive(false);  //帯を非アクティブに
                        bandDrawTime = 1.0f;  //表示時間を初期化
                        nowQuestionCount += 1;  //現在の設問数を加算
                        //正解だったら正解の数を加算
                        if(isCorrect==true) correctCount += 1;
                        //現在の設問数が全設問数に到達したらリザルトへ(違ったら次の設問へ)
                        if (nowQuestionCount == questionAllCount) state = GameState.Result;
                        else state = GameState.Next;
                    }
                }
                //次の課題です
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
                        isPush = false;             //押されてたを押されてないにする
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
                public void OnPushButton(string buttonName)
                {
                    //押されていないかつ解答モードなら
                    if (isPush == false　&& state==GameState.QuestionAnswer)    
                    {
                        selectButton = buttonName;
                        isPush = true;
                    }
                }
            }
        }
    }
}

