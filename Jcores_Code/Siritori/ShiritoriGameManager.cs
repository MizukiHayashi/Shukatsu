using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Jcores
{
    namespace Fluency
    {
        namespace Shiritori
        {
            public class ShiritoriGameManager : MonoBehaviour
            {
                enum GameState
                {
                    None,
                    OumuAnswer,
                    OumuAnswerDraw,
                    InkoAnswer,
                    Correct,
                    Miss,
                    Result
                }
                private GameState state;

                [SerializeField]
                private FadeTransit fadeTransit;

                [SerializeField]
                private GameObject keyboard;    //キーボード

                [SerializeField]
                private GameObject startTimer;  //開始タイマー
                [SerializeField]
                private GameObject gameTimer;   //経過タイマー
                [SerializeField]
                private GameObject answerImage; //○・×イメージ
                [SerializeField]
                private Sprite[] answerSprite;  //○・×スプライト
                [SerializeField]
                private GameObject endImage;    //終了帯

                private Shiritori_ShiritoriDictionary dictionary;   //しりとりで使う辞書

                private int difficalty; //難易度
                private int option;     //オプション

                private string dictionary_word; //辞書の言葉 

                //全ての辞書言葉
                private List<string> allWords = new List<string>();
                //2文字の言葉
                private List<string> twoWords = new List<string>();
                //3文字の言葉
                private List<string> threeWords = new List<string>();

                //五十音で各文字を格納
                private List<string> a_words = new List<string>();
                private List<string> i_words = new List<string>();
                private List<string> u_words = new List<string>();
                private List<string> e_words = new List<string>();
                private List<string> o_words = new List<string>();

                private List<string> ka_words = new List<string>();
                private List<string> ki_words = new List<string>();
                private List<string> ku_words = new List<string>();
                private List<string> ke_words = new List<string>();
                private List<string> ko_words = new List<string>();

                private List<string> sa_words = new List<string>();
                private List<string> si_words = new List<string>();
                private List<string> su_words = new List<string>();
                private List<string> se_words = new List<string>();
                private List<string> so_words = new List<string>();

                private List<string> ta_words = new List<string>();
                private List<string> ti_words = new List<string>();
                private List<string> tu_words = new List<string>();
                private List<string> te_words = new List<string>();
                private List<string> to_words = new List<string>();

                private List<string> na_words = new List<string>();
                private List<string> ni_words = new List<string>();
                private List<string> nu_words = new List<string>();
                private List<string> ne_words = new List<string>();
                private List<string> no_words = new List<string>();

                private List<string> ha_words = new List<string>();
                private List<string> hi_words = new List<string>();
                private List<string> hu_words = new List<string>();
                private List<string> he_words = new List<string>();
                private List<string> ho_words = new List<string>();

                private List<string> ma_words = new List<string>();
                private List<string> mi_words = new List<string>();
                private List<string> mu_words = new List<string>();
                private List<string> me_words = new List<string>();
                private List<string> mo_words = new List<string>();

                private List<string> ya_words = new List<string>();
                private List<string> yu_words = new List<string>();
                private List<string> yo_words = new List<string>();

                private List<string> ra_words = new List<string>();
                private List<string> ri_words = new List<string>();
                private List<string> ru_words = new List<string>();
                private List<string> re_words = new List<string>();
                private List<string> ro_words = new List<string>();

                private List<string> wa_words = new List<string>();
                private List<string> wo_words = new List<string>();

                //答えの言葉
                private List<string> answerWords = new List<string>();

                [SerializeField]
                private GameObject inko;    //インコオブジェクト
                [SerializeField]
                private Text inkoText;      //インコのテキスト
                private string inkoWord;    //インコの言葉
                private string inkoStartChar;   //インコの言葉の最初の文字
                private string inkoEndChar; //インコの言葉の最後の文字

                [SerializeField]
                private GameObject oumu;    //オウムオブジェクト
                [SerializeField]
                private Text oumuText;      //オウムのテキスト
                [SerializeField]
                private Sprite oumuLose;    //参りましたの時の画像
                private string oumuWord;    //オウムの言葉
                private string oumuEndChar; //オウムの言葉の最後の文字
                private int indexCount;     //辞書から検索した回数                

                [SerializeField]
                private float intervalCharDraw = 0.05f; //1文字の表示にかける時間
                private float wordDrawTime = 0;   //表示にかかる時間
                private float wordDrawBeginTime = 1;  //文字列の表示を開始した時間
                private int lastDrawCharacter = -1; //表示中の文字数

                private bool isAnswer;   //解答可能か

                private float answerDrawTime;   //解答の表示時間
                private float endDrawTime;      //終了の表示時間

                private float questionAllCount; //すべての問題数
                private float correctCount;     //正解数
                private float correctAvg;       //正解率
                private float elapsedTime;      //経過時間

                void Start()
                {
                    state = GameState.None;
                    //しりとり辞書を取得
                    dictionary = Resources.Load("ShiritoriDictionary") as Shiritori_ShiritoriDictionary;

                    //難易度・オプションを取得
                    Settings.Instance.SetSettings();
                    difficalty = Settings.Instance.DifficultyInt;
                    option = Prefs.GetOption();
                    //解答回数を取得
                    questionAllCount = Settings.Instance.CurrentSettings.AnswerMaxCount;

                    //各変数を初期化
                    answerDrawTime = 1.0f;
                    endDrawTime = 1.0f;
                    correctCount = 0;
                    correctAvg = 0;
                    elapsedTime = 0;
                    indexCount = 0;

                    //オプションによって格納を変更
                    if (option == 1)  //２文字制限
                    {
                        //辞書の文字分
                        for (int i = 0; i < dictionary.sheets[0].list.Count; i++)
                        {
                            dictionary_word = dictionary.sheets[0].list[i].word;
                            if (dictionary_word.Length == 2) //文字の大きさが２文字なら
                            {
                                if (dictionary_word.Substring(dictionary_word.Length - 1, 1) != "ん") //最後の文字が「ん」でなければ
                                {
                                    twoWords.Add(dictionary_word);  //2文字の言葉に格納
                                    SetWordClass(dictionary_word);  //各辞書に格納
                                }
                            }
                        }
                    }
                    else if (option == 2)  //３文字制限
                    {
                        for (int i = 0; i < dictionary.sheets[0].list.Count; i++)
                        {
                            dictionary_word = dictionary.sheets[0].list[i].word;
                            if (dictionary_word.Length == 3)  //文字の大きさが３文字なら
                            {
                                if (dictionary_word.Substring(dictionary_word.Length - 1, 1) != "ん")  //最後の文字が「ん」でなければ
                                {
                                    threeWords.Add(dictionary_word);  //3文字の言葉に格納
                                    SetWordClass(dictionary_word);    //各辞書に格納
                                }
                            }
                        }
                    }
                    else //制限なし
                    {
                        for (int i = 0; i < dictionary.sheets[0].list.Count; i++)
                        {
                            dictionary_word = dictionary.sheets[0].list[i].word;
                            if (dictionary_word.Substring(dictionary_word.Length - 1, 1) != "ん")  //最後の文字が「ん」でなければ
                            {
                                allWords.Add(dictionary_word); //全ての言葉に格納
                                SetWordClass(dictionary_word); //各辞書に格納
                            }
                        }
                    }
                    //解答可能かを初期化
                    isAnswer = false;
                }
                /// <summary>
                /// 各言葉の最初の文字から五十音に振り分ける
                /// </summary>
                /// <param name="word">言葉</param>
                private void SetWordClass(string word)
                {
                    if (word.StartsWith("あ")) a_words.Add(word);
                    else if (word.StartsWith("い")) i_words.Add(word);
                    else if (word.StartsWith("う")) u_words.Add(word);
                    else if (word.StartsWith("え")) e_words.Add(word);
                    else if (word.StartsWith("お")) o_words.Add(word);

                    else if (word.StartsWith("か") || word.StartsWith("が")) ka_words.Add(word);
                    else if (word.StartsWith("き") || word.StartsWith("ぎ")) ki_words.Add(word);
                    else if (word.StartsWith("く") || word.StartsWith("ぐ")) ku_words.Add(word);
                    else if (word.StartsWith("け") || word.StartsWith("げ")) ke_words.Add(word);
                    else if (word.StartsWith("こ") || word.StartsWith("ご")) ko_words.Add(word);

                    else if (word.StartsWith("さ") || word.StartsWith("ざ")) sa_words.Add(word);
                    else if (word.StartsWith("し") || word.StartsWith("じ")) si_words.Add(word);
                    else if (word.StartsWith("す") || word.StartsWith("ず")) su_words.Add(word);
                    else if (word.StartsWith("せ") || word.StartsWith("ぜ")) se_words.Add(word);
                    else if (word.StartsWith("そ") || word.StartsWith("ぞ")) so_words.Add(word);

                    else if (word.StartsWith("た") || word.StartsWith("だ")) ta_words.Add(word);
                    else if (word.StartsWith("ち") || word.StartsWith("ぢ")) ti_words.Add(word);
                    else if (word.StartsWith("つ") || word.StartsWith("づ")) tu_words.Add(word);
                    else if (word.StartsWith("て") || word.StartsWith("で")) te_words.Add(word);
                    else if (word.StartsWith("と") || word.StartsWith("ど")) to_words.Add(word);

                    else if (word.StartsWith("な")) na_words.Add(word);
                    else if (word.StartsWith("に")) ni_words.Add(word);
                    else if (word.StartsWith("ぬ")) nu_words.Add(word);
                    else if (word.StartsWith("ね")) ne_words.Add(word);
                    else if (word.StartsWith("の")) no_words.Add(word);

                    else if (word.StartsWith("は") || word.StartsWith("ば") || word.StartsWith("ぱ")) ha_words.Add(word);
                    else if (word.StartsWith("ひ") || word.StartsWith("び") || word.StartsWith("ぴ")) hi_words.Add(word);
                    else if (word.StartsWith("ふ") || word.StartsWith("ぶ") || word.StartsWith("ぷ")) hu_words.Add(word);
                    else if (word.StartsWith("へ") || word.StartsWith("べ") || word.StartsWith("ぺ")) he_words.Add(word);
                    else if (word.StartsWith("ほ") || word.StartsWith("ぼ") || word.StartsWith("ぽ")) ho_words.Add(word);

                    else if (word.StartsWith("ま")) ma_words.Add(word);
                    else if (word.StartsWith("み")) mi_words.Add(word);
                    else if (word.StartsWith("む")) mu_words.Add(word);
                    else if (word.StartsWith("め")) me_words.Add(word);
                    else if (word.StartsWith("も")) mo_words.Add(word);

                    else if (word.StartsWith("や")) ya_words.Add(word);
                    else if (word.StartsWith("ゆ")) yu_words.Add(word);
                    else if (word.StartsWith("よ")) yo_words.Add(word);

                    else if (word.StartsWith("ら")) ra_words.Add(word);
                    else if (word.StartsWith("り")) ri_words.Add(word);
                    else if (word.StartsWith("る")) ru_words.Add(word);
                    else if (word.StartsWith("れ")) re_words.Add(word);
                    else if (word.StartsWith("ろ")) ro_words.Add(word);

                    else if (word.StartsWith("わ")) wa_words.Add(word);
                    else if (word.StartsWith("を")) wo_words.Add(word);
                }

                void Update()
                {
                    //ステートが初期でスタートタイマーが終わったら
                    if (state == GameState.None && startTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                    {
                        state = GameState.OumuAnswer;
                    }
                    //ステートがインコの解答で経過時間が終わったら
                    if (state == GameState.InkoAnswer && gameTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                    {
                        state = GameState.Result;
                    }

                    switch (state)
                    {
                        case GameState.OumuAnswer:
                            OumuAnswer();
                            break;
                        case GameState.OumuAnswerDraw:
                            OumuAnswerDraw();
                            break;
                        case GameState.InkoAnswer:
                            //経過時間を加算
                            elapsedTime += Time.deltaTime;
                            break;
                        case GameState.Correct:
                            Correct();
                            break;
                        case GameState.Miss:
                            Miss();
                            break;
                        case GameState.Result:
                            Result();
                            break;
                    }

                    switch (option)
                    {
                        case 0:
                            break;
                        case 1:  //２文字制限
                            if (inkoText.text.Length > 2) //インコの言葉が2文字より多ければ
                            {
                                keyboard.GetComponent<KeyBoardName.KeyBoard>().DeleteLast();  //最後の文字を消す
                            }
                            break;
                        case 2:
                            if (inkoText.text.Length > 3) //インコの言葉が3文字より多ければ
                            {
                                keyboard.GetComponent<KeyBoardName.KeyBoard>().DeleteLast();  //最後の文字を消す
                            }
                            break;
                    }
                }
                /// オウムの解答
                private void OumuAnswer()
                {
                    //インコの最後の言葉から各コレクションを選択
                    if (inkoEndChar == null && option == 1) OumuAnswerSet(twoWords);
                    else if (inkoEndChar == null && option == 2) OumuAnswerSet(threeWords);
                    else if (inkoEndChar == null) OumuAnswerSet(allWords);
                    else if (inkoEndChar == "あ") OumuAnswerSet(a_words);
                    else if (inkoEndChar == "い") OumuAnswerSet(i_words);
                    else if (inkoEndChar == "う") OumuAnswerSet(u_words);
                    else if (inkoEndChar == "え") OumuAnswerSet(e_words);
                    else if (inkoEndChar == "お") OumuAnswerSet(o_words);

                    else if (inkoEndChar == "か" || inkoEndChar == "が") OumuAnswerSet(ka_words);
                    else if (inkoEndChar == "き" || inkoEndChar == "ぎ") OumuAnswerSet(ki_words);
                    else if (inkoEndChar == "く" || inkoEndChar == "ぐ") OumuAnswerSet(ku_words);
                    else if (inkoEndChar == "け" || inkoEndChar == "げ") OumuAnswerSet(ke_words);
                    else if (inkoEndChar == "こ" || inkoEndChar == "ご") OumuAnswerSet(ko_words);

                    else if (inkoEndChar == "さ" || inkoEndChar == "ざ") OumuAnswerSet(sa_words);
                    else if (inkoEndChar == "し" || inkoEndChar == "じ") OumuAnswerSet(si_words);
                    else if (inkoEndChar == "す" || inkoEndChar == "ず") OumuAnswerSet(su_words);
                    else if (inkoEndChar == "せ" || inkoEndChar == "ぜ") OumuAnswerSet(se_words);
                    else if (inkoEndChar == "そ" || inkoEndChar == "ぞ") OumuAnswerSet(so_words);

                    else if (inkoEndChar == "た" || inkoEndChar == "だ") OumuAnswerSet(ta_words);
                    else if (inkoEndChar == "ち" || inkoEndChar == "ぢ") OumuAnswerSet(ti_words);
                    else if (inkoEndChar == "つ" || inkoEndChar == "づ") OumuAnswerSet(tu_words);
                    else if (inkoEndChar == "て" || inkoEndChar == "で") OumuAnswerSet(te_words);
                    else if (inkoEndChar == "と" || inkoEndChar == "ど") OumuAnswerSet(to_words);

                    else if (inkoEndChar == "な") OumuAnswerSet(na_words);
                    else if (inkoEndChar == "に") OumuAnswerSet(ni_words);
                    else if (inkoEndChar == "ぬ") OumuAnswerSet(nu_words);
                    else if (inkoEndChar == "ね") OumuAnswerSet(ne_words);
                    else if (inkoEndChar == "の") OumuAnswerSet(no_words);

                    else if (inkoEndChar == "は" || inkoEndChar == "ば" || inkoEndChar == "ぱ") OumuAnswerSet(ha_words);
                    else if (inkoEndChar == "ひ" || inkoEndChar == "び" || inkoEndChar == "ぴ") OumuAnswerSet(hi_words);
                    else if (inkoEndChar == "ふ" || inkoEndChar == "ぶ" || inkoEndChar == "ぷ") OumuAnswerSet(hu_words);
                    else if (inkoEndChar == "へ" || inkoEndChar == "べ" || inkoEndChar == "ぺ") OumuAnswerSet(he_words);
                    else if (inkoEndChar == "ほ" || inkoEndChar == "ぼ" || inkoEndChar == "ぽ") OumuAnswerSet(ho_words);

                    else if (inkoEndChar == "ま") OumuAnswerSet(ma_words);
                    else if (inkoEndChar == "み") OumuAnswerSet(mi_words);
                    else if (inkoEndChar == "む") OumuAnswerSet(mu_words);
                    else if (inkoEndChar == "め") OumuAnswerSet(me_words);
                    else if (inkoEndChar == "も") OumuAnswerSet(mo_words);

                    else if (inkoEndChar == "や") OumuAnswerSet(ya_words);
                    else if (inkoEndChar == "ゆ") OumuAnswerSet(yu_words);
                    else if (inkoEndChar == "よ") OumuAnswerSet(yo_words);

                    else if (inkoEndChar == "ら") OumuAnswerSet(ra_words);
                    else if (inkoEndChar == "り") OumuAnswerSet(ri_words);
                    else if (inkoEndChar == "る") OumuAnswerSet(ru_words);
                    else if (inkoEndChar == "れ") OumuAnswerSet(re_words);
                    else if (inkoEndChar == "ろ") OumuAnswerSet(ro_words);

                    else if (inkoEndChar == "わ") OumuAnswerSet(wa_words);
                    else if (inkoEndChar == "を") OumuAnswerSet(wo_words);
                }
                /// <summary>
                /// オウムの解答を設定
                /// </summary>
                /// <param name="words">言葉のコレクション</param>
                private void OumuAnswerSet(List<string> words)
                {
                    //オウムの言葉を指定されたコレクションからランダムで設定
                    oumuWord = words[UnityEngine.Random.Range(0, words.Count)];
                    indexCount += 1;    //検索回数を加算

                    //解答ができなくなったら(50回検索したら)
                    if (indexCount >= 50)
                    {
                        oumuText.text = "参りました…";
                        oumu.GetComponent<Image>().sprite = oumuLose;
                        state = GameState.Result;
                    }
                    //既に解答で出ていなければ
                    else if (answerWords.Contains(oumuWord) == false)
                    {
                        answerWords.Add(oumuWord);  //解答の言葉に格納
                        oumuEndChar = oumuWord.Substring(oumuWord.Length - 1, 1);  //相手の言葉の最後の文字に指定
                        //最後の文字が「ー」ならその前の文字を取得
                        if (oumuEndChar == "ー") oumuEndChar = oumuWord.Substring(oumuWord.Length - 2, 1);
                        //最後の文字が小文字なら大文字に変換
                        if (oumuEndChar == "ぁ") oumuEndChar = ChangeCharAt(oumuEndChar, 'あ');
                        else if (oumuEndChar == "ぃ") oumuEndChar = ChangeCharAt(oumuEndChar, 'い');
                        else if (oumuEndChar == "ぅ") oumuEndChar = ChangeCharAt(oumuEndChar, 'う');
                        else if (oumuEndChar == "ぇ") oumuEndChar = ChangeCharAt(oumuEndChar, 'え');
                        else if (oumuEndChar == "ぉ") oumuEndChar = ChangeCharAt(oumuEndChar, 'お');
                        else if (oumuEndChar == "っ") oumuEndChar = ChangeCharAt(oumuEndChar, 'つ');
                        else if (oumuEndChar == "ゃ") oumuEndChar = ChangeCharAt(oumuEndChar, 'や');
                        else if (oumuEndChar == "ゅ") oumuEndChar = ChangeCharAt(oumuEndChar, 'ゆ');
                        else if (oumuEndChar == "ょ") oumuEndChar = ChangeCharAt(oumuEndChar, 'よ');
                   
                        wordDrawTime = oumuWord.Length + intervalCharDraw;   //表示にかかる時間を計算
                        wordDrawBeginTime = Time.time; //文字列の表示を開始した時間を設定
                        lastDrawCharacter = 0; //表示中の文字数を初期化
                        oumu.GetComponent<OumuAnimation>().AnswerAnimStart();  //オウムの解答アニメーションをスタート
                        indexCount = 0;
                        state = GameState.OumuAnswerDraw;
                    }
                }
                // オウムの言葉を表示する処理
                private void OumuAnswerDraw()
                {
                    //文字の表示処理が終了していれば
                    if (OumuAnsDrawComp() == true)
                    {
                        //経過タイマーをスタート
                        gameTimer.GetComponent<Timer>().TimerStart();
                        //オウムの解答をログに保存
                        gameObject.GetComponent<LogManager>().OumuSetLog(oumuWord);
                        //オウムの正解アニメーションをスタート
                        oumu.GetComponent<OumuAnimation>().CorrectAnimStart();
                        //インコの正解アニメーションをスタート
                        inko.GetComponent<InkoAnimation>().AnswerAnimStart();
                        state = GameState.InkoAnswer;
                    }
                    //表示される文字数を計算
                    int drawCharCount = (int)(Mathf.Clamp01((Time.time - wordDrawBeginTime) / wordDrawTime) * oumuWord.Length);
                    if (drawCharCount != lastDrawCharacter)
                    {
                        //オウムの文字を一文字ずつ表示
                        oumuText.text = oumuWord.Substring(0, drawCharCount);
                        //表示している文字数の更新
                        lastDrawCharacter = drawCharCount;
                    }
                }
                //文字の表示処理が終了したか
                private bool OumuAnsDrawComp()
                {
                    return Time.time > wordDrawBeginTime + wordDrawTime;
                }
                /// <summary>
                /// 決定をクリックした時の処理
                /// </summary>
                /// <param name="text">キーボードの入力テキスト</param>
                public void OnClickEnter(Text text)
                {
                    //解答可能でステートがインコ解答で文字が０じゃなければ
                    if (isAnswer == false && state == GameState.InkoAnswer && text.text.Length != 0)
                    {
                        //テキストの文字をひらがなに変換してインコの言葉に格納
                        inkoWord = ToHiragana(text.text);
                        //インコの言葉の最初の文字を取得
                        inkoStartChar = inkoWord.Substring(0, 1);
                        //最初の文字が小文字なら大文字に変換
                        if (inkoStartChar == "ぁ") inkoWord = ChangeCharAt(inkoWord, 'あ');
                        else if (inkoStartChar == "ぃ") inkoWord = ChangeCharAt(inkoWord, 'い');
                        else if (inkoStartChar == "ぅ") inkoWord = ChangeCharAt(inkoWord, 'う');
                        else if (inkoStartChar == "ぇ") inkoWord = ChangeCharAt(inkoWord, 'え');
                        else if (inkoStartChar == "ぉ") inkoWord = ChangeCharAt(inkoWord, 'お');
                        else if (inkoStartChar == "っ") inkoWord = ChangeCharAt(inkoWord, 'つ');
                        else if (inkoStartChar == "ゃ") inkoWord = ChangeCharAt(inkoWord, 'や');
                        else if (inkoStartChar == "ゅ") inkoWord = ChangeCharAt(inkoWord, 'ゆ');
                        else if (inkoStartChar == "ょ") inkoWord = ChangeCharAt(inkoWord, 'よ');
                        //最初の文字を変換した言葉で再格納
                        inkoStartChar = inkoWord.Substring(0, 1);
                        //インコの始めの文字と相手の最後の文字が一緒なら
                        if (inkoStartChar == oumuEndChar)
                        {
                            CheckWord(inkoStartChar);   //辞書に言葉があるか検索
                        }
                        //代行文字で判定
                        else if ((inkoStartChar == "か" && oumuEndChar == "が") || (inkoStartChar == "が" && oumuEndChar == "か") ||
                                 (inkoStartChar == "き" && oumuEndChar == "ぎ") || (inkoStartChar == "ぎ" && oumuEndChar == "き") ||
                                 (inkoStartChar == "く" && oumuEndChar == "ぐ") || (inkoStartChar == "ぐ" && oumuEndChar == "く") ||
                                 (inkoStartChar == "け" && oumuEndChar == "げ") || (inkoStartChar == "げ" && oumuEndChar == "け") ||
                                 (inkoStartChar == "こ" && oumuEndChar == "ご") || (inkoStartChar == "ご" && oumuEndChar == "こ") ||
                                 (inkoStartChar == "さ" && oumuEndChar == "ざ") || (inkoStartChar == "ざ" && oumuEndChar == "さ") ||
                                 (inkoStartChar == "し" && oumuEndChar == "じ") || (inkoStartChar == "じ" && oumuEndChar == "し") ||
                                 (inkoStartChar == "す" && oumuEndChar == "ず") || (inkoStartChar == "ず" && oumuEndChar == "す") ||
                                 (inkoStartChar == "せ" && oumuEndChar == "ぜ") || (inkoStartChar == "ぜ" && oumuEndChar == "せ") ||
                                 (inkoStartChar == "そ" && oumuEndChar == "ぞ") || (inkoStartChar == "ぞ" && oumuEndChar == "そ") ||
                                 (inkoStartChar == "た" && oumuEndChar == "だ") || (inkoStartChar == "だ" && oumuEndChar == "た") ||
                                 (inkoStartChar == "ち" && oumuEndChar == "ぢ") || (inkoStartChar == "ぢ" && oumuEndChar == "ち") ||
                                 (inkoStartChar == "つ" && oumuEndChar == "づ") || (inkoStartChar == "づ" && oumuEndChar == "つ") ||
                                 (inkoStartChar == "て" && oumuEndChar == "で") || (inkoStartChar == "で" && oumuEndChar == "て") ||
                                 (inkoStartChar == "と" && oumuEndChar == "ど") || (inkoStartChar == "ど" && oumuEndChar == "と") ||
                                 (inkoStartChar == "は" && oumuEndChar == "ば") || (inkoStartChar == "は" && oumuEndChar == "ぱ") || (inkoStartChar == "ば" && oumuEndChar == "は") || (inkoStartChar == "ば" && oumuEndChar == "ぱ") || (inkoStartChar == "ぱ" && oumuEndChar == "は") || (inkoStartChar == "ぱ" && oumuEndChar == "ば") ||
                                 (inkoStartChar == "ひ" && oumuEndChar == "び") || (inkoStartChar == "ひ" && oumuEndChar == "ぴ") || (inkoStartChar == "び" && oumuEndChar == "ひ") || (inkoStartChar == "び" && oumuEndChar == "ぴ") || (inkoStartChar == "ぴ" && oumuEndChar == "ひ") || (inkoStartChar == "ぴ" && oumuEndChar == "び") ||
                                 (inkoStartChar == "ふ" && oumuEndChar == "ぶ") || (inkoStartChar == "ふ" && oumuEndChar == "ぷ") || (inkoStartChar == "ぶ" && oumuEndChar == "ふ") || (inkoStartChar == "ぶ" && oumuEndChar == "ぷ") || (inkoStartChar == "ぷ" && oumuEndChar == "ふ") || (inkoStartChar == "ぷ" && oumuEndChar == "ぶ") ||
                                 (inkoStartChar == "へ" && oumuEndChar == "べ") || (inkoStartChar == "へ" && oumuEndChar == "ぺ") || (inkoStartChar == "べ" && oumuEndChar == "へ") || (inkoStartChar == "べ" && oumuEndChar == "ぺ") || (inkoStartChar == "ぺ" && oumuEndChar == "へ") || (inkoStartChar == "ぺ" && oumuEndChar == "べ") ||
                                 (inkoStartChar == "ほ" && oumuEndChar == "ぼ") || (inkoStartChar == "ほ" && oumuEndChar == "ぽ") || (inkoStartChar == "ぼ" && oumuEndChar == "ほ") || (inkoStartChar == "ぼ" && oumuEndChar == "ぽ") || (inkoStartChar == "ぽ" && oumuEndChar == "ほ") || (inkoStartChar == "ぽ" && oumuEndChar == "ぼ"))
                        {
                            CheckWord(inkoStartChar);  //辞書に言葉があるか検索
                        }
                        else
                        {
                            //インコの不正解アニメーションをスタート
                            inko.GetComponent<InkoAnimation>().MissAnimStart();
                            state = GameState.Miss;
                        }
                        isAnswer = true;
                    }
                }
                /// <summary>
                /// 最初の文字から検索するリストを指定
                /// </summary>
                /// <param name="startChar">最初の文字</param>
                private void CheckWord(string startChar)
                {
                    if (startChar == "あ") CheckAnswer(a_words);
                    else if (startChar == "い") CheckAnswer(i_words);
                    else if (startChar == "う") CheckAnswer(u_words);
                    else if (startChar == "え") CheckAnswer(e_words);
                    else if (startChar == "お") CheckAnswer(o_words);

                    else if (startChar == "か" || startChar == "が") CheckAnswer(ka_words);
                    else if (startChar == "き" || startChar == "ぎ") CheckAnswer(ki_words);
                    else if (startChar == "く" || startChar == "ぐ") CheckAnswer(ku_words);
                    else if (startChar == "け" || startChar == "げ") CheckAnswer(ke_words);
                    else if (startChar == "こ" || startChar == "ご") CheckAnswer(ko_words);

                    else if (startChar == "さ" || startChar == "ざ") CheckAnswer(sa_words);
                    else if (startChar == "し" || startChar == "じ") CheckAnswer(si_words);
                    else if (startChar == "す" || startChar == "ず") CheckAnswer(su_words);
                    else if (startChar == "せ" || startChar == "ぜ") CheckAnswer(se_words);
                    else if (startChar == "そ" || startChar == "ぞ") CheckAnswer(so_words);

                    else if (startChar == "た" || startChar == "だ") CheckAnswer(ta_words);
                    else if (startChar == "ち" || startChar == "ぢ") CheckAnswer(ti_words);
                    else if (startChar == "つ" || startChar == "づ") CheckAnswer(tu_words);
                    else if (startChar == "て" || startChar == "で") CheckAnswer(te_words);
                    else if (startChar == "と" || startChar == "ど") CheckAnswer(to_words);

                    else if (startChar == "な") CheckAnswer(na_words);
                    else if (startChar == "に") CheckAnswer(ni_words);
                    else if (startChar == "ぬ") CheckAnswer(nu_words);
                    else if (startChar == "ね") CheckAnswer(ne_words);
                    else if (startChar == "の") CheckAnswer(no_words);

                    else if (startChar == "は" || startChar == "ば" || startChar == "ぱ") CheckAnswer(ha_words);
                    else if (startChar == "ひ" || startChar == "び" || startChar == "ぴ") CheckAnswer(hi_words);
                    else if (startChar == "ふ" || startChar == "ぶ" || startChar == "ぷ") CheckAnswer(hu_words);
                    else if (startChar == "へ" || startChar == "べ" || startChar == "ぺ") CheckAnswer(he_words);
                    else if (startChar == "ほ" || startChar == "ぼ" || startChar == "ぽ") CheckAnswer(ho_words);

                    else if (startChar == "ま") CheckAnswer(ma_words);
                    else if (startChar == "み") CheckAnswer(mi_words);
                    else if (startChar == "む") CheckAnswer(mu_words);
                    else if (startChar == "め") CheckAnswer(me_words);
                    else if (startChar == "も") CheckAnswer(mo_words);

                    else if (startChar == "や") CheckAnswer(ya_words);
                    else if (startChar == "ゆ") CheckAnswer(yu_words);
                    else if (startChar == "よ") CheckAnswer(yo_words);

                    else if (startChar == "ら") CheckAnswer(ra_words);
                    else if (startChar == "り") CheckAnswer(ri_words);
                    else if (startChar == "る") CheckAnswer(ru_words);
                    else if (startChar == "れ") CheckAnswer(re_words);
                    else if (startChar == "ろ") CheckAnswer(ro_words);

                    else if (startChar == "わ") CheckAnswer(wa_words);
                    else if (startChar == "を") CheckAnswer(wo_words);
                }
                //辞書に載っているかどうか検索
                private void CheckAnswer(List<string> words)
                {
                    //すでに解答されていないかつ言葉がリストに登録されていれば
                    if (answerWords.Contains(inkoWord) == false && words.Contains(inkoWord) == true)
                    {
                        gameTimer.GetComponent<Timer>().ResetTimer();   //タイマーを初期化
                        inkoEndChar = inkoWord.Substring(inkoWord.Length - 1, 1); //インコの言葉の最後の文字を格納                                                                                 
                        if (inkoEndChar == "ー") inkoEndChar = inkoWord.Substring(inkoWord.Length - 2, 1);  //最後の文字が「ー」ならその前の文字を取得
                        oumuText.text = ""; //オウムの言葉を初期化
                        inko.GetComponent<InkoAnimation>().CorrectAnimStart();  //インコの正解アニメーションをスタート
                        state = GameState.Correct;
                    }
                    else
                    {
                        inko.GetComponent<InkoAnimation>().MissAnimStart();  //インコの不正解アニメーションをスタート
                        state = GameState.Miss;
                    }
                }
                //正解
                private void Correct()
                {
                    //「正解」を表示
                    answerImage.SetActive(true);
                    answerImage.GetComponent<Image>().sprite = answerSprite[0];
                    //帯を表示
                    BandDraw(true);
                }
                //おしい
                private void Miss()
                {
                    //「おしい」を表示
                    answerImage.SetActive(true);
                    answerImage.GetComponent<Image>().sprite = answerSprite[1];
                    //帯を表示
                    BandDraw(false);
                }
                //帯を表示
                private void BandDraw(bool isCorrect)
                {
                    //表示時間が０になったら
                    answerDrawTime -= Time.deltaTime;
                    if (answerDrawTime <= 0.0f)
                    {
                        answerImage.SetActive(false);  //帯を非アクティブに
                        answerDrawTime = 1.0f;  //表示時間を初期化
                        isAnswer = false;    //解答可能に
                        if (isCorrect == true)　　//正解なら
                        {
                            answerWords.Add(inkoWord); //解答された言葉に格納
                            gameObject.GetComponent<LogManager>().InkoSetLog(inkoWord);  //インコの言葉をログに格納
                            keyboard.GetComponent<KeyBoardName.KeyBoard>().DeleteAll();  //キーボード入力を全て削除
                            correctCount += 1;  //正解数を１プラス
                            //正解数が解答回数になったらリザルトへ
                            if (correctCount == questionAllCount) state = GameState.Result;
                            //違ったらオウムの解答へ
                            else state = GameState.OumuAnswer;
                        }
                        else  //不正解なら
                        {
                            //インコの解答アニメーションをスタート
                            inko.GetComponent<InkoAnimation>().AnswerAnimStart();
                            state = GameState.InkoAnswer;
                        }
                    }
                }
                //終了-リザルトへ
                private void Result()
                {
                    //「終了」を表示
                    endImage.SetActive(true);
                    //表示時間が０になったら
                    endDrawTime -= Time.deltaTime;
                    if (endDrawTime <= 0.0f)
                    {
                        //正解率を計算
                        correctAvg = correctCount / questionAllCount * 100;

                        //リザルトをセット
                        Settings.Instance.SetResult(correctAvg, correctCount, questionAllCount, elapsedTime);
                        //リザルトへ移行
                        fadeTransit.Transit(SceneData.GetSceneName(SceneData.ID.Shiritori, SceneData.Suffix.Result));
                    }
                }
                //言葉を全てひらがなに変換
                private string ToHiragana(string word)
                {
                    return new string(word.Select(c => (c >= 'ァ' && c <= 'ヶ') ? (char)(c + 'ぁ' - 'ァ') : c).ToArray());
                }
                //一文字目を別の文字に変換
                private string ChangeCharAt(string word, char newChar)
                {
                    return word.Remove(0, 1).Insert(0, newChar.ToString());
                }
            }
        }

    }
}

