using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isNotInput;  //入力制御
    [SerializeField]
    private float rotationPeriod = 0.3f;     // 隣に移動するのにかかる時間
    [SerializeField]
    private float sideLength = 1f;           // Cubeの辺の長さ

    public float tweenTime;         //Tweenにかける時間
    public static float tweenTimer; //Tweenにかかった時間

    private float inputX;   //入力(X軸)
    private float inputY;   //入力(y軸)

    /******Player1に関する変数****************************************/
    private GameObject player1;         //player1
    private Player1Move player1Move;    //Player1Moveスクリプト
    private Vector2 player1_xy;         //player1の入力数値
    private bool p1_isRotate;           // Cubeが回転中かどうかを検出するフラグ
    private float p1_directionX = 0;    // 回転方向フラグ
    private float p1_directionZ = 0;    // 回転方向フラグ

    private Vector3 p1_startPos;        // 回転前のCubeの位置
    private float p1_rotationTime = 0;  // 回転中の時間経過
    private float p1_radius;            // 重心の軌道半径
    private Quaternion p1_fromRotation; // 回転前のCubeのクォータニオン
    private Quaternion p1_toRotation;   // 回転後のCubeのクォータニオン
    /*****************************************************************/

    /******Player2に関する変数****************************************/
    private GameObject player2;         //プレイヤー2
    private Player2Move player2Move;    //Player2Moveスクリプト
    private Vector2 player2_xy;         //player2の入力数値
    private bool p2_isRotate = false;   // Cubeが回転中かどうかを検出するフラグ
    private float p2_directionX = 0;    // 回転方向フラグ
    private float p2_directionZ = 0;    // 回転方向フラグ

    private Vector3 p2_startPos;        // 回転前のCubeの位置
    private float p2_rotationTime = 0;  // 回転中の時間経過
    private float p2_radius;            // 重心の軌道半径
    private Quaternion p2_fromRotation; // 回転前のCubeのクォータニオン
    private Quaternion p2_toRotation;   // 回転後のCubeのクォータニオン
    /*****************************************************************/

    public static bool isTweenMove; //Tween中か？


    // Use this for initialization
    void Start()
    {
        //変数の初期化
        tweenTimer = tweenTime;
        inputX = 0;
        inputY = 0;
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        player1_xy = Vector2.zero;
        player2_xy = Vector2.zero;
        isNotInput = false;
        isTweenMove = false;
        p1_rotationTime = 0;
        p1_radius = sideLength * Mathf.Sqrt(2f) / 2f;
        p2_rotationTime = 0;
        p2_radius = sideLength * Mathf.Sqrt(2f) / 2f;

    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーがnullでなければPlayer1Moveを取得
        if (player1 != null) player1Move = player1.GetComponent<Player1Move>();
        if (player2 != null) player2Move = player2.GetComponent<Player2Move>();
        //移動量を初期化
        player1_xy = Vector2.zero;
        player2_xy = Vector2.zero;

        //一定条件で入力を受け付けない
        if ((p1_isRotate == true || p2_isRotate == true) ||
            Pause.pause == true ||
            (Player1Move.isDropDown == true) ||
            (Player2Move.isDropDown == true) ||
            CsvMessage.startend==false)
        {
            if (Pause.pause == false) Pause.rotate = true;
            isNotInput = true;
            inputX = 0;
            inputY = 0;
        }
        else if ((EnemyHit.isGameClear == false) &&
                 (EnemyHit.isGameOver == false) &&
                 (p1_isRotate == false && p2_isRotate == false) &&
                 (isTweenMove == false) && Pause.pause == false
                 )
        {
            Pause.rotate = false;
            isNotInput = false;
        }
        //入力可能なら
        if (isNotInput == false)
        {
            //各入力方向の取得
            if (Input.GetAxisRaw("Horizontal") >= 0.5)
            {
                inputX = 1;
                inputY = 0;
            }
            else if (Input.GetAxisRaw("Horizontal") <= -0.5f)
            {
                inputX = -1;
                inputY = 0;
            }
            else if (Input.GetAxisRaw("Vertical") >= 0.5f)
            {
                inputY = 1;
                inputX = 0;
            }
            else if (Input.GetAxisRaw("Vertical") <= -0.5f)
            {
                inputY = -1;
                inputX = 0;
            }
            else
            {
                inputX = 0;
                inputY = 0;
            }

            /********Player1の入力処理****************/
            //右方向入力時に障害物に触れていなければ
            if (player1Move.isStopRight == false && inputX >= 1f)
            {
                //ターゲットを挟めむ条件がそろっていれば
                if ((player1Move.enemyForwerd == true) &&
                    (player2Move.enemyBack == true) &&
                    (player1Move.colorCheck == player2Move.colorCheck))
                {
                    //挟むアニメーション処理
                    isNotInput = true;
                    isTweenMove = true;
                    LeanTween.moveZ(player1, player1.transform.position.z + 0.5f, tweenTime).setOnComplete(TweenRightEnd);
                }
                else
                {
                    player1_xy = new Vector2(1, 0);
                }
            }
            //左方向入力時に障害物に触れていなければ
            if (player1Move.isStopLeft == false && inputX <= -1f)
            {
                //ターゲットを挟めむ条件がそろっていれば
                if ((player1Move.enemyBack == true) &&
                    (player2Move.enemyForward == true) &&
                    (player1Move.colorCheck == player2Move.colorCheck))
                {
                    //挟むアニメーション処理
                    isNotInput = true;
                    isTweenMove = true;
                    LeanTween.moveZ(player1, player1.transform.position.z - 0.5f, tweenTime).setOnComplete(TweenLeftEnd);
                }
                else
                {
                    player1_xy = new Vector2(-1, 0);
                }
            }
            //ｘに入力が入っていなければ
            if (player1_xy.x == 0)
            {
                //上方向入力時に障害物に触れていなければ
                if (player1Move.isStopUp == false && inputY >= 1f)
                {
                    //ターゲットを挟めむ条件がそろっていれば
                    if ((player1Move.enemyLeft == true) &&
                        (player2Move.enemyRight == true) &&
                        (player1Move.colorCheck == player2Move.colorCheck))
                    {
                        //挟むアニメーション処理
                        isNotInput = true;
                        isTweenMove = true;
                        LeanTween.moveX(player1, player1.transform.position.x - 0.5f, tweenTime).setOnComplete(TweenUpEnd);
                    }
                    else
                    {
                        player1_xy = new Vector2(0, 1);
                    }
                }
                //下方向入力時に障害物に触れていなければ
                if (player1Move.isStopDown == false && inputY <= -1f)
                {
                    //ターゲットを挟めむ条件がそろっていれば
                    if ((player1Move.enemyRight == true) &&
                        (player2Move.enemyLeft == true) &&
                        (player1Move.colorCheck == player2Move.colorCheck))
                    {
                        //挟むアニメーション処理
                        isNotInput = true;
                        isTweenMove = true;
                        LeanTween.moveX(player1, player1.transform.position.x + 0.5f, tweenTime).setOnComplete(TweenDownEnd);
                    }
                    else
                    {
                        player1_xy = new Vector2(0, -1);
                    }
                }
            }

            /**********Player2の入力処理******************/
            if (player2Move.isStopLeft == false && inputX >= 1f)
            {
                if ((player2Move.enemyBack == true) &&
                    (player1Move.enemyForwerd == true) &&
                    (player1Move.colorCheck == player2Move.colorCheck))
                {
                    isNotInput = true;
                    isTweenMove = true;
                    LeanTween.moveZ(player2, player2.transform.position.z - 0.5f, tweenTime);
                }
                else
                {
                    player2_xy = new Vector2(-1, 0);
                }
            }
            if (player2Move.isStopRight == false && inputX <= -1f)
            {
                if ((player2Move.enemyForward == true) &&
                    (player1Move.enemyBack == true) &&
                    (player1Move.colorCheck == player2Move.colorCheck))
                {
                    isNotInput = true;
                    isTweenMove = true;
                    LeanTween.moveZ(player2, player2.transform.position.z + 0.5f, tweenTime);
                }
                else
                {
                    player2_xy = new Vector2(1, 0);
                }
            }

            if (player2_xy.x == 0)
            {
                if (player2Move.isStopUp == false && inputY <= -1f)
                {
                    if ((player2Move.enemyLeft == true) &&
                        (player1Move.enemyRight == true) &&
                        (player1Move.colorCheck == player2Move.colorCheck))
                    {
                        isNotInput = true;
                        isTweenMove = true;
                        LeanTween.moveX(player2, player2.transform.position.x - 0.5f, tweenTime);
                    }
                    else
                    {
                        player2_xy = new Vector2(0, 1);
                    }
                }
                if (player2Move.isStopDown == false && inputY >= 1f)
                {
                    if ((player2Move.enemyRight == true) &&
                        (player1Move.enemyLeft == true) &&
                        (player1Move.colorCheck == player2Move.colorCheck))
                    {
                        isNotInput = true;
                        isTweenMove = true;
                        LeanTween.moveX(player2, player2.transform.position.x + 0.5f, tweenTime);
                    }
                    else
                    {
                        player2_xy = new Vector2(0, -1);
                    }
                }
            }
        }
        //プレイヤー１の回転処理
        if ((player1_xy.x != 0 || player1_xy.y != 0) && p1_isRotate == false)
        {
            p1_directionX = player1_xy.y;  // 回転方向セット (x,yどちらかは必ず0)
            p1_directionZ = player1_xy.x;  // 回転方向セット (x,yどちらかは必ず0)
            p1_startPos = player1.transform.position;       // 回転前の座標を保持
            p1_fromRotation = player1.transform.rotation;   // 回転前のクォータニオンを保持
            player1.transform.Rotate(p1_directionZ * 90, 0, p1_directionX * 90, Space.World);   // 回転方向に90度回転させる
            p1_toRotation = player1.transform.rotation;     // 回転後のクォータニオンを保持
            player1.transform.rotation = p1_fromRotation;   // CubeのRotationを回転前に戻す。（transformのシャローコピーとかできないんだろうか…。）
            p1_rotationTime = 0;    // 回転中の経過時間を0に。
            p1_isRotate = true;     // 回転中フラグをたてる。
        }
        //プレイヤー２の回転処理
        if ((player2_xy.x != 0 || player2_xy.y != 0) && p2_isRotate == false)
        {
            p2_directionX = player2_xy.y;  // 回転方向セット (x,yどちらかは必ず0)
            p2_directionZ = player2_xy.x;  // 回転方向セット (x,yどちらかは必ず0)
            p2_startPos = player2.transform.position;       // 回転前の座標を保持
            p2_fromRotation = player2.transform.rotation;   // 回転前のクォータニオンを保持
            player2.transform.Rotate(p2_directionZ * 90, 0, p2_directionX * 90, Space.World);   // 回転方向に90度回転させる
            p2_toRotation = player2.transform.rotation;     // 回転後のクォータニオンを保持
            player2.transform.rotation = p2_fromRotation;   // CubeのRotationを回転前に戻す。（transformのシャローコピーとかできないんだろうか…。）
            p2_rotationTime = 0;     // 回転中の経過時間を0に。
            p2_isRotate = true;      // 回転中フラグをたてる。
        }

    }
    void LateUpdate()
    {
        /*********Player1****************************/
        if (p1_isRotate == true)
        {
            p1_rotationTime += Time.fixedDeltaTime; // 経過時間を増やす
            float ratio = Mathf.Lerp(0, 1, p1_rotationTime / rotationPeriod);   // 回転の時間に対する今の経過時間の割合

            // 移動
            float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);  // 回転角をラジアンで。
            float distanceX = -p1_directionX * p1_radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));    // X軸の移動距離。 -の符号はキーと移動の向きを合わせるため。
            float distanceY = p1_radius * (Mathf.Sin(45f * Mathf.Deg2Rad + thetaRad) - Mathf.Sin(45f * Mathf.Deg2Rad));                     // Y軸の移動距離
            float distanceZ = p1_directionZ * p1_radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));     // Z軸の移動距離
            player1.transform.position = new Vector3(p1_startPos.x + distanceX, p1_startPos.y + distanceY, p1_startPos.z + distanceZ);      // 現在の位置をセット

            // 回転
            player1.transform.rotation = Quaternion.Lerp(p1_fromRotation, p1_toRotation, ratio);    // Quaternion.Lerpで現在の回転角をセット

            // 移動・回転終了時に各パラメータを初期化。isRotateフラグを下ろす。
            if (ratio == 1)
            {
                Sound.PlaySE(8);
                p1_isRotate = false;
                p1_directionX = 0;
                p1_directionZ = 0;
                p1_rotationTime = 0;
                Result.resultRotate1 = true;
            }
        }

        /**********Player2**********************************/
        if (p2_isRotate == true)
        {
            p2_rotationTime += Time.fixedDeltaTime; // 経過時間を増やす
            float ratio = Mathf.Lerp(0, 1, p2_rotationTime / rotationPeriod);   // 回転の時間に対する今の経過時間の割合

            // 移動
            float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);   // 回転角をラジアンで。
            float distanceX = -p2_directionX * p2_radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));    // X軸の移動距離。 -の符号はキーと移動の向きを合わせるため。
            float distanceY = p2_radius * (Mathf.Sin(45f * Mathf.Deg2Rad + thetaRad) - Mathf.Sin(45f * Mathf.Deg2Rad));                     // Y軸の移動距離
            float distanceZ = p2_directionZ * p2_radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));     // Z軸の移動距離
            player2.transform.position = new Vector3(p2_startPos.x + distanceX, p2_startPos.y + distanceY, p2_startPos.z + distanceZ);      // 現在の位置をセット

            // 回転
            player2.transform.rotation = Quaternion.Lerp(p2_fromRotation, p2_toRotation, ratio);    // Quaternion.Lerpで現在の回転角をセット

            // 移動・回転終了時に各パラメータを初期化。isRotateフラグを下ろす。
            if (ratio == 1)
            {
                Sound.PlaySE(8);

                p2_isRotate = false;
                p2_directionX = 0;
                p2_directionZ = 0;
                p2_rotationTime = 0;
                Result.resultRotate2 = true;
            }
        }
    }
    //右からターゲットを挟んだアニメーション終了後のアニメーション処理
    public void TweenRightEnd()
    {
        isNotInput = true;
        EnemyHit.enemyDestroy = true;
        LeanTween.moveZ(player1, player1.transform.position.z - 0.5f, tweenTime).setOnComplete(TweenAllEnd);
        LeanTween.moveZ(player2, player2.transform.position.z + 0.5f, tweenTime);
    }
    //左からターゲットを挟んだアニメーション終了後のアニメーション処理
    public void TweenLeftEnd()
    {
        isNotInput = true;
        EnemyHit.enemyDestroy = true;
        LeanTween.moveZ(player1, player1.transform.position.z + 0.5f, tweenTime).setOnComplete(TweenAllEnd);
        LeanTween.moveZ(player2, player2.transform.position.z - 0.5f, tweenTime);
    }
    //上からターゲットを挟んだアニメーション終了後のアニメーション処理
    public void TweenUpEnd()
    {
        isNotInput = true;
        EnemyHit.enemyDestroy = true;
        LeanTween.moveX(player1, player1.transform.position.x + 0.5f, tweenTime).setOnComplete(TweenAllEnd);
        LeanTween.moveX(player2, player2.transform.position.x - 0.5f, tweenTime);
    }
    //下からターゲットを挟んだアニメーション終了後のアニメーション処理
    public void TweenDownEnd()
    {
        isNotInput = true;
        EnemyHit.enemyDestroy = true;
        LeanTween.moveX(player1, player1.transform.position.x - 0.5f, tweenTime).setOnComplete(TweenAllEnd);
        LeanTween.moveX(player2, player2.transform.position.x + 0.5f, tweenTime);
    }

    public void TweenAllEnd()
    {
        isNotInput = false;
        isTweenMove = false;
    }

}
