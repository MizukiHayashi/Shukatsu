using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jcores
{
    namespace ProcessingSpeed
    {
        namespace WhackAMole
        {
            public class newMoleGameManager : MonoBehaviour
            {
                enum GameState
                {
                    None,
                    MoleSet,
                    Result
                }

                private GameState state;

                private int difficalty; //難易度

                [SerializeField]
                private FadeTransit fadeTransit;

                [SerializeField]
                private GameObject startTimer;      //開始タイマー
                [SerializeField]
                private GameObject endImage;    //終了帯

                [SerializeField]
                private GameObject hummer;      //ハンマー
                private Animator hummerAnim;    //ハンマーアニメーション
                private bool isClick;           //クリックしたか

                //練習で使用するオブジェクト達
                [SerializeField]
                private GameObject ve_Objects;
                [SerializeField]
                private GameObject[] ve_moles;
                [SerializeField]
                private GameObject[] ve_Leg;
                //簡単で使用するオブジェクト達
                [SerializeField]
                private GameObject e_Objects;
                [SerializeField]
                private GameObject[] e_moles;
                [SerializeField]
                private GameObject[] e_Leg;
                //ふつうで使用するオブジェクト達
                [SerializeField]
                private GameObject n_Objects;
                [SerializeField]
                private GameObject[] n_moles;
                [SerializeField]
                private GameObject[] n_Leg;
                //むずかしい・すごくむずかしいで使用するオブジェクト達
                [SerializeField]
                private GameObject h_vh_Objects;
                [SerializeField]
                private GameObject[] h_vh_moles;
                [SerializeField]
                private GameObject[] h_vh_Leg;
                
                private GameObject[] effects;   //星のエフェクト

                private int moleNumOnce;    //モグラ生成数
                public float moleNum = 0;   //もぐらの数
                private int moleID;         //もぐらID

                private int hummerMaxCount; //ハンマーの振れる回数
                private int hummerCount;    //振った回数

                private float resultDelay; //終了の前の待ち時間

                private float appearanceMoleCount; //出現したモグラの数
                private float moleHitCount; //叩いたモグラの数
                private float notMoleHitCount;  //叩いたモグラ以外の数
                private float reactionTime;

                private float correctAvg;       //正答率
                private float moleHitCorrectAvg; //叩いたときの正答率
                private float reactionAvgTime; //平均反応時間
                private float missCount;          //見逃しミス率
                private float endDrawTime;      //終了の表示時間

                // Use this for initialization
                void Start()
                {
                    //難易度・モグラの数を取得
                    Settings.Instance.SetSettings();
                    difficalty = Settings.Instance.DifficultyInt;
                    moleNumOnce = Settings.Instance.CurrentSettings.MoleNumAtOnce;

                    switch (difficalty)
                    {
                        case 0:
                            ve_Objects.SetActive(true);
                            break;
                        case 1:
                            e_Objects.SetActive(true);
                            break;
                        case 2:
                            n_Objects.SetActive(true);
                            break;
                        case 3:
                            h_vh_Objects.SetActive(true);
                            break;
                        case 4:
                            h_vh_Objects.SetActive(true);
                            break;
                    }
                    //ハンマーの振れる回数を取得
                    hummerMaxCount = Settings.Instance.HammerNum;
                    hummerCount = 0;

                    resultDelay = 0.5f;
                    //ハンマーのアニメーションを取得
                    hummerAnim = hummer.GetComponent<Animator>();
                    //各変数の初期化
                    appearanceMoleCount = 0;
                    moleHitCount = 0;
                    notMoleHitCount = 0;
                    reactionTime = 0;

                    correctAvg = 0;
                    moleHitCorrectAvg = 0;
                    reactionAvgTime = 0;
                    missCount = 0;
                    endDrawTime = 1.0f;

                }

                // Update is called once per frame
                void Update()
                {
                    //ハンマーをマウスの位置に
                    hummer.GetComponent<Image>().rectTransform.position = Input.mousePosition;
                    //時間が０になったら
                    if (state == GameState.None && startTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH)
                    {
                        state = GameState.MoleSet;
                    }

                    switch (state)
                    {
                        case GameState.MoleSet:
                            MoleSet();
                            break;
                        case GameState.Result:
                            Result();
                            break;
                    }

                    //スタートタイマーが終わっていて、ステートが終了じゃなく、マウスの左クリックが押されたら
                    if (startTimer.GetComponent<Timer>().TimerState == Timer.TimerStateType.FINISH && state != GameState.Result && Input.GetMouseButtonDown(0))
                    {
                        hummerAnim.SetBool("OnClick", true);    //ハンマーアニメーション起動
                        hummerCount += 1;   //振った回数を加算
                        //振った回数が振れる回数になったら終了
                        if (hummerCount == hummerMaxCount)
                        {
                            state = GameState.Result;
                        }
                    }
                    else
                    {
                        hummerAnim.SetBool("OnClick", false);
                    }
                }
                //モグラの起動
                private void MoleSet()
                {
                    //難易度によって変更
                    switch (difficalty)
                    {
                        case 0:
                            if (moleNum < 1)
                            {
                                moleID = UnityEngine.Random.Range(0, ve_moles.Length); //起動するモグラを決める
                                //指定したモグラが起動していなければ
                                if (ve_moles[moleID].GetComponent<MoleController>().enabled == false)
                                {
                                    //モグラを起動
                                    ve_moles[moleID].GetComponent<MoleController>().enabled = true;
                                    //起動したモグラの数を加算
                                    moleNum += 1;
                                }
                            }
                            break;
                        case 1:
                            if (moleNum < 1)
                            {
                                moleID = UnityEngine.Random.Range(0, e_moles.Length);
                                if (e_moles[moleID].GetComponent<MoleController>().enabled == false)
                                {
                                    e_moles[moleID].GetComponent<MoleController>().enabled = true;
                                    moleNum += 1;
                                }
                            }
                            break;
                        case 2:
                            if (moleNum < 1)
                            {
                                moleID = UnityEngine.Random.Range(0, n_moles.Length);
                                if (n_moles[moleID].GetComponent<MoleController>().enabled == false)
                                {
                                    n_moles[moleID].GetComponent<MoleController>().enabled = true;
                                    moleNum += 1;
                                }
                            }
                            break;
                        case 3:
                            if (moleNum < moleNumOnce)
                            {
                                moleID = UnityEngine.Random.Range(0, h_vh_moles.Length);
                                if (h_vh_moles[moleID].GetComponent<MoleController>().enabled == false)
                                {
                                    h_vh_moles[moleID].GetComponent<MoleController>().enabled = true;
                                    moleNum += 1;
                                }
                            }
                            break;
                        case 4:
                            if (moleNum < moleNumOnce)
                            {
                                moleID = UnityEngine.Random.Range(0, h_vh_moles.Length);
                                if (h_vh_moles[moleID].GetComponent<MoleController>().enabled == false)
                                {
                                    h_vh_moles[moleID].GetComponent<MoleController>().enabled = true;
                                    moleNum += 1;
                                }
                            }
                            break;
                    }
                }
                //出現したモグラを加算
                public void MoleCountPlus()
                {
                    appearanceMoleCount += 1;
                }
                //叩いたモグラを加算
                public void MoleHit(float value)
                {
                    moleHitCount += 1;
                    reactionTime += value;
                }
                //叩いたモグラ以外を加算
                public void NotMoleHit()
                {
                    notMoleHitCount += 1;
                }

                //終了ーリザルトへ
                private void Result()
                {
                    resultDelay -= Time.deltaTime;
                    if (resultDelay <= 0.0f)
                    {
                        switch (difficalty)
                        {
                            case 0:
                                ResultMoleProcess(ve_moles, ve_Leg);
                                break;
                            case 1:
                                ResultMoleProcess(e_moles, e_Leg);
                                break;
                            case 2:
                                ResultMoleProcess(n_moles, n_Leg);
                                break;
                            case 3:
                                ResultMoleProcess(h_vh_moles, h_vh_Leg);
                                break;
                            case 4:
                                ResultMoleProcess(h_vh_moles, h_vh_Leg);
                                break;
                        }
                        //星を全て削除
                        effects = GameObject.FindGameObjectsWithTag("MoleHitEffect");
                        foreach(var effect in effects)
                        {
                            Destroy(effect);
                        }

                        //「終了」を表示
                        endImage.SetActive(true);
                        //表示時間が０になったら
                        endDrawTime -= Time.deltaTime;
                        if (endDrawTime <= 0.0f)
                        {
                            //正解率
                            correctAvg = moleHitCount / appearanceMoleCount * 100;
                            //モグラが１匹でも叩かれていたら
                            if (moleHitCount > 0)
                            {
                                //叩いたモグラの正解率
                                moleHitCorrectAvg = (moleHitCount - notMoleHitCount) / (moleHitCount + notMoleHitCount) * 100;
                                //平均反応時間の計算
                                reactionAvgTime = reactionTime / moleHitCount;
                            }
                            else
                            {
                                moleHitCorrectAvg = 0;
                                reactionAvgTime = 0;
                            }
                            //見逃しミス率の計算
                            missCount = appearanceMoleCount-moleHitCount;

                            //リザルトをセット
                            Settings.Instance.SetResult(correctAvg, moleHitCorrectAvg, reactionAvgTime, missCount);
                            //リザルトへ移行
                            fadeTransit.Transit(SceneData.GetSceneName(SceneData.ID.WhackAMole, SceneData.Suffix.Result));
                        }
                    }
                }
                //終了処理
                private void ResultMoleProcess(GameObject[] moles, GameObject[] legs)
                {
                    //全モグラと足を非アクティブに
                    for (int i = 0; i < moles.Length; i++)
                    {
                        moles[i].SetActive(false);
                        legs[i].SetActive(false);
                    }
                }
            }
        }
    }
}