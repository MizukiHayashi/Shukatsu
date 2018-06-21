using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1Move : MonoBehaviour
{
    public static bool onMoveBlock;     //動く障害物に乗っているか
    public static bool onMoveBlock_Up;  
    public static bool isDropDown;      //落下しているか

    /****色障害物に触れている時のエフェクト****/
    [SerializeField]
    private GameObject wallEffect_R; //右側面のエフェクト
    [SerializeField]
    private GameObject wallEffect_L; //左側面のエフェクト
    [SerializeField]
    private GameObject wallEffect_U; //上側面のエフェクト
    [SerializeField]
    private GameObject wallEffect_D; //下側面のエフェクト
    //各エフェクトの初期回転値
    private Quaternion defaultRotate_R;
    private Quaternion defaultRotate_L;
    private Quaternion defaultRotate_U;
    private Quaternion defaultRotate_D;

    [SerializeField]
    private Material playerRed;              //赤のマテリアル
    [SerializeField]
    private Material playerBlue;             //青のマテリアル

    public int colorCheck;                  //色の判定用数値
    int layerMask = ~(1 << 10 | 1 << 9);    //レイヤーマスク

    public bool enemyForwerd;   //エネミーの正面
    public bool enemyBack;      //エネミーの後方
    public bool enemyLeft;      //エネミーの左
    public bool enemyRight;     //エネミーの右

    public bool isStopLeft;     //左が行き止まりか
    public bool isStopRight;    //右が行き止まりか
    public bool isStopUp;       //上が行き止まりか
    public bool isStopDown;     //下が行き止まりか

    private bool emit_white;    //白発光
    private bool emit_red;      //赤発光
    private bool emit_blue;     //青発光
    private bool flag_white;    //白発光開始
    private bool flag_red;      //赤発光開始
    private bool flag_blue;     //青発光開始

    private float distance = 0.8f;    //レイの大きさ

    private bool oneShot_ChangeColor;       //色変更時にSEが一回鳴ったか
    private bool oneShot_HitColorBlockSE_R; //色ブロックに当たった時にSEが一回鳴ったか(右)
    private bool oneShot_HitColorBlockSE_L; //色ブロックに当たった時にSEが一回鳴ったか(左)
    private bool oneShot_HitColorBlockSE_U; //色ブロックに当たった時にSEが一回鳴ったか(上)
    private bool oneShot_HitColorBlockSE_D; //色ブロックに当たった時にSEが一回鳴ったか(下)

    // Use this for initialization
    void Start()
    {
        //各変数の初期化
        isDropDown = false;
        isStopLeft = false;
        isStopRight = false;
        isStopUp = false;
        isStopDown = false;
        defaultRotate_R = wallEffect_R.transform.localRotation;
        defaultRotate_L = wallEffect_L.transform.localRotation;
        defaultRotate_U = wallEffect_U.transform.localRotation;
        defaultRotate_D = wallEffect_D.transform.localRotation;

        colorCheck = 0;

        oneShot_HitColorBlockSE_R = false;
        oneShot_HitColorBlockSE_L = false;
        oneShot_HitColorBlockSE_U = false;
        oneShot_HitColorBlockSE_D = false;
        oneShot_ChangeColor = false;
    }

    public void FixedUpdate()
    {
        //プレイヤーの発光処理
        PlayerFlashing();
        //側面エフェクト処理
        WallEffectPosition();
    }

    // Update is called once per frame
    void Update()
    {
        //レイの判定処理
        CubeRayCheck();
    }

    //プレイヤーの点滅処理
    public void PlayerFlashing()
    {
        //自分のレンダラーを取得
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        Color baseColor = mat.GetColor("_EmissionColor");
        //RGBの色の値がすべて一緒なら白発光
        if (baseColor.r == baseColor.g && baseColor.r == baseColor.b)
            emit_white = true;
        else
            emit_white = false;
        //RGBの値でRがGより大きい
        if (baseColor.r >= baseColor.g)
        {
            //RGBの値でRがBより大きい
            if (baseColor.r >= baseColor.b)
            {
                //白発光でRが0.4より小さければ
                if (baseColor.r <= 0.4f && emit_white == true)
                {
                    //値を修正
                    baseColor.r = 0.4f;
                    baseColor.g = 0.4f;
                    baseColor.b = 0.4f;
                    //白発光開始
                    flag_white = true;
                    emit_red = false;
                    emit_blue = false;
                }
                //白発光でRが0.6より大きければ
                else if (baseColor.r >= 0.6f && emit_white == true)
                {
                    //値を修正
                    baseColor.r = 0.6f;
                    baseColor.g = 0.6f;
                    baseColor.b = 0.6f;
                    //白発光終了
                    flag_white = false;
                    emit_red = false;
                    emit_blue = false;

                }
                //白発光でなくRが0.5より小さければ
                else if (baseColor.r <= 0.5f && emit_white == false)
                {
                    //値を修正
                    baseColor.r = 0.5f;
                    baseColor.g = 0.25f;
                    baseColor.b = 0.04f;
                    //赤発光
                    emit_red = true;
                    //赤発光開始
                    flag_red = true;

                    emit_blue = false;
                    emit_white = false;
                }
                //白発光でなくRが1.2より大きければ
                else if (baseColor.r >= 1.2f && emit_white == false)
                {
                    //値を修正
                    baseColor.r = 1.2f;
                    baseColor.g = 0.6f;
                    baseColor.b = 0.1f;
                    //赤発光
                    emit_red = true;
                    //赤発光終了
                    flag_red = false;
                    emit_blue = false;

                    emit_white = false;
                }
            }
        }
        //RGBの値でGがBより大きい
        else if (baseColor.g >= baseColor.b)
        {
            //とくになし
        }
        //RGBの値でBがGより大きい
        else
        {
            //白発光でなくBが0.5より小さければ
            if (baseColor.b <= 0.5f && emit_white == false)
            {
                //値を修正
                baseColor.r = 0.041f;
                baseColor.g = 0.208f;
                baseColor.b = 0.5f;
                //青発光
                emit_blue = true;
                //青発光開始
                flag_blue = true;

                emit_red = false;
                emit_white = false;
            }
            //白発光でなくBが1.2より大きければ
            else if (baseColor.b >= 1.2f && emit_white == false)
            {
                //値を修正
                baseColor.r = 0.1f;
                baseColor.g = 0.5f;
                baseColor.b = 1.2f;
                //青発光
                emit_blue = true;
                //発光終了
                emit_red = false;
                flag_blue = false;
                emit_white = false;
            }
        }

        if (flag_white == true && emit_white == true)
        {
            baseColor.r += 0.003f;
            baseColor.g += 0.003f;
            baseColor.b += 0.003f;
        }
        else if (flag_white == false && emit_white == true)
        {
            baseColor.r -= 0.003f;
            baseColor.g -= 0.003f;
            baseColor.b -= 0.003f;
        }

        if (flag_red == true && emit_red == true)
        {
            baseColor.r += 0.012f;
            baseColor.g += 0.006f;
            baseColor.b += 0.0012f;
        }
        else if (flag_red == false && emit_red == true)
        {
            baseColor.r -= 0.012f;
            baseColor.g -= 0.006f;
            baseColor.b -= 0.0012f;
        }

        if (flag_blue == true && emit_blue == true)
        {
            baseColor.b += 0.012f;
            baseColor.r += 0.0012f;
            baseColor.g += 0.0048f;
        }
        else if (flag_blue == false && emit_blue == true)
        {
            baseColor.b -= 0.012f;
            baseColor.r -= 0.0012f;
            baseColor.g -= 0.0048f;
        }

        mat.SetColor("_EmissionColor", baseColor);
    }


    //レイによる当たり判定チェック
    public void CubeRayCheck()
    {
        RayCheckRight();
        RayCheckLeft();
        RayCheckDown();
        RayCheckUp();
        RayCheckBlockDown();

        //Debug.DrawRay(transform.position, Vector3.forward * distance, Color.red);
        //Debug.DrawRay(transform.position, -Vector3.forward * distance, Color.red);
        //Debug.DrawRay(transform.position, Vector3.right * distance, Color.red);
        //Debug.DrawRay(transform.position, -Vector3.right * distance, Color.red);
    }

    //カメラから見てブロックの右方向のレイ
    public void RayCheckRight()
    {
        //レイの生成
        RaycastHit hitR;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitR, distance, layerMask))
        {
            //Debug.Log(hitF.collider.gameObject);
            //障害物に当たったら
            if (hitR.collider.gameObject.tag == "Obstacle")
                isStopRight = true;
            //上下に動く障害物に当たったら
            if ((hitR.collider.gameObject.tag == "MoveBlock") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopRight = true;
            else if ((hitR.collider.gameObject.tag == "MoveBlock") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopRight = false;

            if ((hitR.collider.gameObject.tag == "MoveBlock_Up") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitR.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopRight = true;
            else if ((hitR.collider.gameObject.tag == "MoveBlock_Up") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopRight = false;
            //赤障害物に当たったら
            if (hitR.collider.gameObject.tag == "RedObstacle")
            {
                //自分が赤色だったら
                if (this.GetComponent<Renderer>().material.name == "PlayerRed")
                {
                    wallEffect_R.SetActive(true);
                    //一度だけ音を鳴らす
                    if (oneShot_HitColorBlockSE_R == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_R = true;
                    }
                    isStopRight = true;
                }
                else
                {
                    wallEffect_R.SetActive(false);
                    isStopRight = false;
                }
            }
            //青障害物に当たったら
            else if (hitR.collider.gameObject.tag == "BlueObstacle")
            {
                //自分が青色だったら
                if (this.GetComponent<Renderer>().material.name == "PlayerBlue")
                {
                    wallEffect_R.SetActive(true);
                    //チュートリアル専用処理
                    if (SceneManager.GetActiveScene().name == "C_T_Stage2" || SceneManager.GetActiveScene().name == "D_T_Stage2")
                    {
                        EnemyHit.isGameOver = true;
                    }
                    //一度だけ音を鳴らす
                    if (oneShot_HitColorBlockSE_R == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_R = true;
                    }
                    isStopRight = true;
                }
                else
                {
                    wallEffect_R.SetActive(false);
                    isStopRight = false;
                }
            }
            else
            {
                wallEffect_R.SetActive(false);
                oneShot_HitColorBlockSE_R = false;
            }
            //ターゲットに当たっていたら
            if (hitR.collider.gameObject.tag == "Enemy")
                enemyForwerd = true;
        }
        else
        {
            isStopRight = false;
            enemyForwerd = false;
            wallEffect_R.SetActive(false);
            oneShot_HitColorBlockSE_R = false;
        }
    }

    //カメラから見てブロックの左方向のレイ
    public void RayCheckLeft()
    {
        RaycastHit hitL;

        if (Physics.Raycast(transform.position, -Vector3.forward, out hitL, distance, layerMask))
        {
            //Debug.Log(hitB.collider.gameObject);
            if (hitL.collider.gameObject.tag == "Obstacle")
                isStopLeft = true;

            if ((hitL.collider.gameObject.tag == "MoveBlock") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopLeft = true;
            else if ((hitL.collider.gameObject.tag == "MoveBlock") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopLeft = false;

            if ((hitL.collider.gameObject.tag == "MoveBlock_Up") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitL.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopLeft = true;
            else if ((hitL.collider.gameObject.tag == "MoveBlock_Up") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopLeft = false;

            if (hitL.collider.gameObject.tag == "RedObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerRed")
                {
                    wallEffect_L.SetActive(true);
                    if (oneShot_HitColorBlockSE_L == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_L = true;
                    }
                    isStopLeft = true;
                }
                else
                {
                    wallEffect_L.SetActive(false);
                    isStopLeft = false;
                }
            }
            else if (hitL.collider.gameObject.tag == "BlueObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerBlue")
                {
                    wallEffect_L.SetActive(true);
                    if (oneShot_HitColorBlockSE_L == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_L = true;
                    }
                    isStopLeft = true;
                }
                else
                {
                    wallEffect_L.SetActive(false);
                    isStopLeft = false;
                }
            }
            else
            {
                wallEffect_L.SetActive(false);
                oneShot_HitColorBlockSE_L = false;
            }

            if (hitL.collider.gameObject.tag == "Enemy")
                enemyBack = true;
        }
        else
        {
            isStopLeft = false;
            enemyBack = false;
            wallEffect_L.SetActive(false);
            oneShot_HitColorBlockSE_L = false;
        }
    }

    //カメラから見てブロックの下方向のレイ
    public void RayCheckDown()
    {
        RaycastHit hitD;

        if (Physics.Raycast(transform.position, Vector3.right, out hitD, distance, layerMask))
        {
            //Debug.Log(hitR.collider.gameObject);
            if (hitD.collider.gameObject.tag == "Obstacle")
                isStopDown = true;

            if ((hitD.collider.gameObject.tag == "MoveBlock") && (hitD.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopDown = true;
            else if ((hitD.collider.gameObject.tag == "MoveBlock") && (hitD.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopDown = false;

            if ((hitD.collider.gameObject.tag == "MoveBlock_Up") && (hitD.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitD.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopDown = true;
            else if ((hitD.collider.gameObject.tag == "MoveBlock_Up") && (hitD.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopDown = false;

            if (hitD.collider.gameObject.tag == "RedObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerRed")
                {
                    wallEffect_D.SetActive(true);
                    if (oneShot_HitColorBlockSE_D == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_D = true;
                    }
                    isStopDown = true;
                }
                else
                {
                    wallEffect_D.SetActive(false);
                    isStopDown = false;
                }
            }
            else if (hitD.collider.gameObject.tag == "BlueObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerBlue")
                {
                    wallEffect_D.SetActive(true);
                    if (oneShot_HitColorBlockSE_D == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_D = true;
                    }
                    isStopDown = true;
                }
                else
                {
                    wallEffect_D.SetActive(false);
                    isStopDown = false;
                }
            }
            else
            {
                wallEffect_D.SetActive(false);
                oneShot_HitColorBlockSE_D = false;
            }

            if (hitD.collider.gameObject.tag == "Enemy")
                enemyRight = true;
        }
        else
        {
            isStopDown = false;
            enemyRight = false;
            wallEffect_D.SetActive(false);
            oneShot_HitColorBlockSE_D = false;
        }
    }

    //カメラから見てブロックの上方向のレイ
    public void RayCheckUp()
    {
        RaycastHit hitU;

        if (Physics.Raycast(transform.position, -Vector3.right, out hitU, distance, layerMask))
        {
            //Debug.Log(hitL.collider.gameObject);
            if (hitU.collider.gameObject.tag == "Obstacle")
                isStopUp = true;

            if ((hitU.collider.gameObject.tag == "MoveBlock") && (hitU.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopUp = true;
            else if ((hitU.collider.gameObject.tag == "MoveBlock") && (hitU.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopUp = false;

            if ((hitU.collider.gameObject.tag == "MoveBlock_Up") && (hitU.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitU.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopUp = true;
            else if ((hitU.collider.gameObject.tag == "MoveBlock_Up") && (hitU.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopUp = false;


            if (hitU.collider.gameObject.tag == "RedObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerRed")
                {
                    wallEffect_U.SetActive(true);
                    if (oneShot_HitColorBlockSE_U == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_U = true;
                    }
                    isStopUp = true;
                }
                else
                {
                    wallEffect_U.SetActive(false);
                    if (oneShot_HitColorBlockSE_U == false)
                    {
                        Sound.PlaySE(9);
                        oneShot_HitColorBlockSE_U = true;
                    }
                    isStopUp = false;
                }
            }
            else if (hitU.collider.gameObject.tag == "BlueObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerBlue")
                {
                    wallEffect_U.SetActive(true);
                    isStopUp = true;
                }
                else
                {
                    wallEffect_U.SetActive(false);
                    isStopUp = false;
                }
            }
            else
            {
                wallEffect_U.SetActive(false);
                oneShot_HitColorBlockSE_U = false;
            }

            if (hitU.collider.gameObject.tag == "Enemy")
                enemyLeft = true;
        }
        else
        {
            isStopUp = false;
            enemyLeft = false;
            wallEffect_U.SetActive(false);
            oneShot_HitColorBlockSE_U = false;
        }
    }

    //ブロックの下方向のレイ
    public void RayCheckBlockDown()
    {
        RaycastHit hitD;

        if (Physics.Raycast(transform.position, -Vector3.up, out hitD, distance))
        {
            //Debug.Log(hitD.collider.gameObject);
            if (hitD.collider.gameObject.tag == "MoveBlock")
                onMoveBlock = true;
            else
                onMoveBlock = false;

            if (hitD.collider.gameObject.tag == "MoveBlock_Up")
                onMoveBlock_Up = true;
            else
                onMoveBlock_Up = false;

            //赤色床に乗っていて色が赤じゃなければ
            if (hitD.collider.gameObject.tag == "RedSquare" && colorCheck != 1)
            {
                //自分を赤色にする
                this.GetComponent<Renderer>().material = playerRed;
                this.GetComponent<Renderer>().material.name = "PlayerRed";
                if (oneShot_ChangeColor == false)
                {
                    Sound.PlaySE(5);
                    oneShot_ChangeColor = true;
                }
                colorCheck = 1;
            }
            //青色床に乗っていて色が青じゃなければ
            if (hitD.collider.gameObject.tag == "BlueSquare" && colorCheck != 2)
            {
                //自分を青色にする
                this.GetComponent<Renderer>().material = playerBlue;
                this.GetComponent<Renderer>().material.name = "PlayerBlue";
                if (oneShot_ChangeColor == false)
                {
                    Sound.PlaySE(6);
                    oneShot_ChangeColor = true;
                }
                colorCheck = 2;
            }

            if (hitD.collider.gameObject.tag != "BlueSquare" && hitD.collider.gameObject.tag != "RedSquare")
                oneShot_ChangeColor = false;

        }
        else
        {
            //ステージ生成終了
            if (CsvMessage.startend == true)
            {
                //床がないので落ちてゲームオーバー
                DropDown();
            }
        }
    }
    //落下ゲームオーバー処理
    public void DropDown()
    {
        //落下した
        isDropDown = true;
        PlayerController.isNotInput = true;
        //回転しながら落ちる
        this.gameObject.transform.Rotate(new Vector3(3, 3, 3));
        LeanTween.moveY(this.gameObject, this.gameObject.transform.position.y - 10.0f, 2.0f).setOnComplete(() =>
        {
            EnemyHit.isGameOver = true;
        });
    }

    //色付き障害物が通れない時のエフェクト表示位置
    public void WallEffectPosition()
    {
        //赤と青の障害物に触れている時のエフェクト(通れない時)
        //カメラから見た時のエフェクトの位置
        //左
        wallEffect_R.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
        wallEffect_R.transform.localRotation = Quaternion.Inverse(transform.rotation) * defaultRotate_R;
        //右
        wallEffect_L.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
        wallEffect_L.transform.localRotation = Quaternion.Inverse(transform.rotation) * defaultRotate_L;
        //上
        wallEffect_U.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
        wallEffect_U.transform.localRotation = Quaternion.Inverse(transform.rotation) * defaultRotate_U;
        //下
        wallEffect_D.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        wallEffect_D.transform.localRotation = Quaternion.Inverse(transform.rotation) * defaultRotate_D;
    }


}
