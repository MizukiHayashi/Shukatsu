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
            public class MoleController : MonoBehaviour
            {
                enum MoleState
                {
                    Init,
                    Move
                }

                enum MoleType
                {
                    Mole,
                    Mushroom,
                    Girl
                }

                private MoleState state = MoleState.Init;

                private MoleType type;

                private int difficalty; //難易度

                private Image mole; //モグラのイメージ

                [SerializeField]
                private Sprite[] moleSprite;　//モグラの画像類
                [SerializeField]
                private Sprite[] mushroomSprite;    //キノコの画像類
                [SerializeField]
                private Sprite[] moleGirlSprite;    //モグラ（メス）の画像類
                [SerializeField]
                private GameObject moleLeg; //モグラの足

                [SerializeField]
                private GameObject hitEffectPrefab; //叩いたときに出す星のプレハブ
                private GameObject hitEffect;       //叩いたときに出す星

                private newMoleGameManager moleGameManager;

                private int rand_char;  //キャラ決定用変数
                private int rand_notMole; //きのことモグラ（メス）を選ぶための変数

                private float upPosition_Y = -30f;
                private float downPostion_Y = -150f;

                [SerializeField]
                private float[] moveSpeeds=new float[5]; //頭を出すスピード(設定元)
                private float moveSpeed;    //頭を出すスピード

                private bool isUp;  //上がりきったか
                private bool onClick; //クリックされたか

                private float moleBeingTime; //出現時間
                private float hitAnimTime;  //叩かれた時の挙動時間

                private float moleAppearanceTime; //反応時間

                // Use this for initialization
                void Start()
                {
                    //難易度の取得
                    Settings.Instance.SetSettings();
                    difficalty = Settings.Instance.DifficultyInt;
                   
                    moleGameManager = GameObject.Find("GameManager").GetComponent<newMoleGameManager>();
                    //モグライメージを取得
                    mole = gameObject.GetComponent<Image>();
                    moleAppearanceTime = 0.0f;

                    switch (difficalty)
                    {
                        case 0:
                            moveSpeed = moveSpeeds[0];
                            break;
                        case 1:
                            moveSpeed = moveSpeeds[1];
                            break;
                        case 2:
                            moveSpeed = moveSpeeds[2];
                            break;
                        case 3:
                            moveSpeed = moveSpeeds[3];
                            break;
                        case 4:
                            moveSpeed = moveSpeeds[4];
                            break;
                    }

                    MoleInit();
                }

                // Update is called once per frame
                void Update()
                {
                    switch (state)
                    {
                        case MoleState.Init:
                            MoleInit();
                            break;
                        case MoleState.Move:
                            MoleMove();
                            break;
                    }
                }
                //モグラの初期化処理
                private void MoleInit()
                {
                    isUp = false;
                    onClick = false;

                    moleAppearanceTime = 0.0f;

                    Settings.Instance.SetSettings();
                    moleBeingTime = Settings.Instance.CurrentSettings.BeingTime;
                    hitAnimTime = 1.0f;

                    //キャラのランダム設定(難易度によって確率変更)
                    if (difficalty == 3) rand_char = Random.Range(1, 3 + 1);
                    else if (difficalty == 4) rand_char = Random.Range(1, 2 + 1);
                    else rand_char = Random.Range(1, 5 + 1);

                    if (rand_char == 1)
                    {
                        rand_notMole = Random.Range(0, 2);
                        if (rand_notMole == 0)
                        {
                            //キノコに設定
                            mole.sprite = mushroomSprite[0];
                            type = MoleType.Mushroom;
                        }
                        else
                        {
                            //モグラ（メス）に設定
                            mole.sprite = moleGirlSprite[0];
                            type = MoleType.Girl;
                        }
                    }
                    else
                    {
                        //モグラに設定
                        mole.sprite = moleSprite[0];
                        type = MoleType.Mole;
                    }

                    if (mole.sprite == moleSprite[0]) moleGameManager.MoleCountPlus();
                    state = MoleState.Move;
                }
                //モグラを動かす
                private void MoleMove()
                {
                    moleAppearanceTime += Time.deltaTime;
                    //叩かれてないかつ上がりきってなければ
                    if (onClick == false && isUp == false)
                    {
                        //上がる
                        mole.rectTransform.localPosition = Vector2.MoveTowards(mole.rectTransform.localPosition, new Vector2(mole.rectTransform.localPosition.x, upPosition_Y), moveSpeed);
                        if (mole.rectTransform.localPosition.y >= upPosition_Y - 0.5f)
                        {
                            //モグラの足をアクティブに
                            if (rand_char != 1) moleLeg.SetActive(true);
                            isUp = true;
                        }
                    }
                    else if (onClick == false && isUp == true)
                    {
                        moleBeingTime -= Time.deltaTime;
                        if (moleBeingTime <= 0.0f)
                        {
                            //下がる
                            mole.rectTransform.localPosition = Vector2.MoveTowards(mole.rectTransform.localPosition, new Vector2(mole.rectTransform.localPosition.x, downPostion_Y), moveSpeed);
                            if (mole.rectTransform.localPosition.y <= downPostion_Y + 0.5f)
                            {
                                //モグラの足を非表示
                                if (rand_char != 1) moleLeg.SetActive(false);
                                state = MoleState.Init;
                                //生成数を-１
                                moleGameManager.moleNum -= 1;
                                //停止
                                gameObject.GetComponent<MoleController>().enabled = false;
                            }
                        }
                    }

                    if (onClick == true)
                    {
                        HitMole();
                    }
                }
                //叩かれた
                private void HitMole()
                {
                    //モグラなら足を表示
                    if (rand_char != 1) moleLeg.SetActive(true);
                    //モグラを出し切る
                    mole.rectTransform.localPosition = new Vector2(mole.rectTransform.localPosition.x, upPosition_Y);
                    //表示時間が０なら
                    hitAnimTime -= Time.deltaTime;
                    if (hitAnimTime <= 0.0f)
                    {
                        //位置を一番下へ
                        mole.rectTransform.localPosition = new Vector2(mole.rectTransform.localPosition.x, downPostion_Y);
                        //モグラの足を非表示
                        if (rand_char != 1) moleLeg.SetActive(false);
                        //星を消す
                        Destroy(hitEffect);
                        state = MoleState.Init;
                        //生成数を-１
                        moleGameManager.moleNum -= 1;
                        //モグラを停止
                        gameObject.GetComponent<MoleController>().enabled = false;
                    }
                }
                //クリック
                public void OnClickMole()
                { 
                    //クリックされていなければ
                    if (onClick == false)
                    {
                        //叩かれたときの星を生成
                        hitEffect = Instantiate(hitEffectPrefab);
                        hitEffect.transform.parent = GameObject.Find("Back").transform;
                        hitEffect.GetComponent<Image>().rectTransform.position = mole.rectTransform.position;
                        if (type == MoleType.Mole)
                        {
                            //叩かれた画像
                            mole.sprite = moleSprite[4];
                            //スコアを加算(モグラ)
                            moleGameManager.MoleHit(moleAppearanceTime);
                            onClick = true;
                        }
                        else if (type == MoleType.Mushroom)
                        {
                            //叩かれた画像
                            mole.sprite = mushroomSprite[4];
                            //スコア加算(モグラ以外)
                            moleGameManager.NotMoleHit();
                            onClick = true;
                        }
                        else if (type == MoleType.Girl)
                        {
                            //叩かれた画像
                            mole.sprite = moleGirlSprite[4];
                            //スコア加算(モグラ以外)
                            moleGameManager.NotMoleHit();
                            onClick = true;
                        }
                    }
                }             
            }
        }
    }
}
