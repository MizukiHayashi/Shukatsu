using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//処理内容はPlayer1Moveを参考
public class Player2Move : MonoBehaviour
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

    public bool enemyForward;   //エネミーの正面
    public bool enemyBack;      //エネミーの後方
    public bool enemyRight;     //エネミーの左
    public bool enemyLeft;      //エネミーの右

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

    private float distance = 0.8f;                     //レイの大きさ

    private bool oneShot_ChangeColorSE;     //色変更時にSEが一回鳴ったか
    private bool oneShot_HitColorBlockSE_R; //色ブロックに当たった時にSEが一回鳴ったか(右)
    private bool oneShot_HitColorBlockSE_L; //色ブロックに当たった時にSEが一回鳴ったか(左)
    private bool oneShot_HitColorBlockSE_U; //色ブロックに当たった時にSEが一回鳴ったか(上)
    private bool oneShot_HitColorBlockSE_D; //色ブロックに当たった時にSEが一回鳴ったか(下)

    int layerMask = ~(1 << 10 | 1 << 8);

    // Use this for initialization
    void Start()
    {
        isDropDown = false;
        onMoveBlock = false;
        isStopLeft = false;
        isStopRight = false;
        isStopUp = false;
        isStopDown = false;
        defaultRotate_R = wallEffect_R.transform.localRotation;
        defaultRotate_L = wallEffect_L.transform.localRotation;
        defaultRotate_U = wallEffect_U.transform.localRotation;
        defaultRotate_D = wallEffect_D.transform.localRotation;

        oneShot_ChangeColorSE = false;
        oneShot_HitColorBlockSE_R = false;
        oneShot_HitColorBlockSE_L = false;
        oneShot_HitColorBlockSE_U = false;
        oneShot_HitColorBlockSE_D = false;

        // 重心の回転軌道半径を計算
        colorCheck = 0;
    }

    public void FixedUpdate()
    {
        PlayerFlashing();
        WallEffectPosition();
    }

    // Update is called once per frame
    void Update()
    {
        CubeRayCheck();

    }

    //プレイヤーの点滅処理
    public void PlayerFlashing()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        Color baseColor = mat.GetColor("_EmissionColor");

        if (baseColor.r == baseColor.g && baseColor.r == baseColor.b)
            emit_white = true;
        else
            emit_white = false;

        if (baseColor.r >= baseColor.g)
        {
            if (baseColor.r >= baseColor.b)
            {
                if (baseColor.r <= 2.0f && emit_white == true)
                {
                    baseColor.r = 2.0f;
                    baseColor.g = 2.0f;
                    baseColor.b = 2.0f;
                    flag_white = true;
                    emit_red = false;
                    emit_blue = false;
                }
                else if (baseColor.r >= 4.0f && emit_white == true)
                {
                    baseColor.r = 4.0f;
                    baseColor.g = 4.0f;
                    baseColor.b = 4.0f;
                    flag_white = false;
                    emit_red = false;
                    emit_blue = false;

                }
                else if (baseColor.r <= 1.2f && emit_white == false)
                {
                    baseColor.r = 1.2f;
                    baseColor.g = 0.6f;
                    baseColor.b = 0.1f;
                    emit_red = true;
                    emit_blue = false;
                    flag_red = true;
                    emit_white = false;
                }
                else if (baseColor.r >= 4.0f && emit_white == false)
                {
                    baseColor.r = 4.0f;
                    baseColor.g = 2.0f;
                    baseColor.b = 0.33f;
                    emit_red = true;
                    emit_blue = false;
                    flag_red = false;
                    emit_white = false;
                }
            }
        }
        else if (baseColor.g >= baseColor.b)
        {

        }
        else
        {
            if (baseColor.b <= 1.2f && emit_white == false)
            {
                baseColor.r = 0.1f;
                baseColor.g = 0.6f;
                baseColor.b = 1.2f;
                emit_red = false;
                emit_blue = true;
                flag_blue = true;
                emit_white = false;
            }
            else if (baseColor.b >= 4.0f && emit_white == false)
            {
                baseColor.r = 0.33f;
                baseColor.g = 2.0f;
                baseColor.b = 4.0f;
                emit_red = false;
                emit_blue = true;
                flag_blue = false;
                emit_white = false;
            }
        }

        if (flag_white == true && emit_white == true)
        {
            baseColor.r += 0.03f;
            baseColor.g += 0.03f;
            baseColor.b += 0.03f;
        }
        else if (flag_white == false && emit_white == true)
        {
            baseColor.r -= 0.03f;
            baseColor.g -= 0.03f;
            baseColor.b -= 0.03f;
        }

        if (flag_red == true && emit_red == true)
        {
            baseColor.r += 0.047f;
            baseColor.g += 0.025f;
            baseColor.b += 0.004f;
        }
        else if (flag_red == false && emit_red == true)
        {
            baseColor.r -= 0.047f;
            baseColor.g -= 0.025f;
            baseColor.b -= 0.004f;
        }

        if (flag_blue == true && emit_blue == true)
        {
            baseColor.b += 0.047f;
            baseColor.r += 0.004f;
            baseColor.g += 0.025f;
        }
        else if (flag_blue == false && emit_blue == true)
        {
            baseColor.b -= 0.047f;
            baseColor.r -= 0.004f;
            baseColor.g -= 0.025f;
        }

        mat.SetColor("_EmissionColor", baseColor);
    }

    //レイによる当たり判定チェック
    public void CubeRayCheck()
    {

        RayCheckRight();
        RayCheckLeft();
        RayCheckUp();
        RayCheckDown();
        RayCheckBlockDown();

        //Debug.DrawRay(transform.position, Vector3.forward * distance, Color.red);
        //Debug.DrawRay(transform.position, -Vector3.forward * distance, Color.red);
        //Debug.DrawRay(transform.position, Vector3.right * distance, Color.red);
        //Debug.DrawRay(transform.position, -Vector3.right * distance, Color.red);
    }

    //カメラから見てブロックの右方向のレイ
    public void RayCheckRight()
    {
        RaycastHit hitF;

        if (Physics.Raycast(transform.position, Vector3.forward, out hitF, distance, layerMask))
        {
            //Debug.Log(hitF.collider.gameObject);
            if (hitF.collider.gameObject.tag == "Obstacle")
                isStopRight = true;


            if ((hitF.collider.gameObject.tag == "MoveBlock") && (hitF.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopRight = true;
            else if ((hitF.collider.gameObject.tag == "MoveBlock") && (hitF.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopRight = false;

            if ((hitF.collider.gameObject.tag == "MoveBlock_Up") && (hitF.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitF.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopRight = true;
            else if ((hitF.collider.gameObject.tag == "MoveBlock_Up") && (hitF.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopRight = false;

            if (hitF.collider.gameObject.tag == "RedObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerRed")
                {
                    wallEffect_R.SetActive(true);
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
            else if (hitF.collider.gameObject.tag == "BlueObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerBlue")
                {
                    wallEffect_R.SetActive(true);
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

            if (hitF.collider.gameObject.tag == "Enemy")
                enemyForward = true;
        }
        else
        {
            isStopRight = false;
            enemyForward = false;
            wallEffect_R.SetActive(false);
            oneShot_HitColorBlockSE_R = false;
        }
    }

    //カメラから見てブロックの左方向のレイ
    public void RayCheckLeft()
    {
        RaycastHit hitB;
      
        if (Physics.Raycast(transform.position, -Vector3.forward, out hitB, distance, layerMask))
        {
            //Debug.Log(hitB.collider.gameObject);
            if (hitB.collider.gameObject.tag == "Obstacle")
                isStopLeft = true;

            if ((hitB.collider.gameObject.tag == "MoveBlock") && (hitB.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopLeft = true;
            else if ((hitB.collider.gameObject.tag == "MoveBlock") && (hitB.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopLeft = false;

            if ((hitB.collider.gameObject.tag == "MoveBlock_Up") && (hitB.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitB.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopLeft = true;
            else if ((hitB.collider.gameObject.tag == "MoveBlock_Up") && (hitB.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopLeft = false;

            if (hitB.collider.gameObject.tag == "RedObstacle")
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
            else if (hitB.collider.gameObject.tag == "BlueObstacle")
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

            if (hitB.collider.gameObject.tag == "Enemy")
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

    //カメラから見てブロックの上方向のレイ
    public void RayCheckDown()
    {
        RaycastHit hitR;
       
        if (Physics.Raycast(transform.position, Vector3.right, out hitR, distance, layerMask))
        {
            //Debug.Log(hitR.collider.gameObject);
            if (hitR.collider.gameObject.tag == "Obstacle")
                isStopDown = true;

            if ((hitR.collider.gameObject.tag == "MoveBlock") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopDown = true;
            else if ((hitR.collider.gameObject.tag == "MoveBlock") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopDown = false;

            if ((hitR.collider.gameObject.tag == "MoveBlock_Up") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitR.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopDown = true;
            else if ((hitR.collider.gameObject.tag == "MoveBlock_Up") && (hitR.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopDown = false;


            if (hitR.collider.gameObject.tag == "RedObstacle")
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
            else if (hitR.collider.gameObject.tag == "BlueObstacle")
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

            if (hitR.collider.gameObject.tag == "Enemy")
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

    //カメラから見てブロックの下方向のレイ
    public void RayCheckUp()
    {
        RaycastHit hitL;
        
        if (Physics.Raycast(transform.position, -Vector3.right, out hitL, distance, layerMask))
        {
            //Debug.Log(hitL.collider.gameObject);
            if (hitL.collider.gameObject.tag == "Obstacle")
                isStopUp = true;

            if ((hitL.collider.gameObject.tag == "MoveBlock") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == true))
                isStopUp = true;
            else if ((hitL.collider.gameObject.tag == "MoveBlock") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopUp = false;

            if ((hitL.collider.gameObject.tag == "MoveBlock_Up") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == true || hitL.collider.gameObject.GetComponent<MoveBlock>().init_Up == true))
                isStopUp = true;
            else if ((hitL.collider.gameObject.tag == "MoveBlock_Up") && (hitL.collider.gameObject.GetComponent<MoveBlock>().isMove == false))
                isStopUp = false;

            if (hitL.collider.gameObject.tag == "RedObstacle")
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
                    isStopUp = false;
                }
            }
            else if (hitL.collider.gameObject.tag == "BlueObstacle")
            {
                if (this.GetComponent<Renderer>().material.name == "PlayerBlue")
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
                    isStopUp = false;
                }
            }
            else
            {
                wallEffect_U.SetActive(false);
                oneShot_HitColorBlockSE_U = false;
            }

            if (hitL.collider.gameObject.tag == "Enemy")
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
            if (hitD.collider.gameObject.tag == "MoveBlock")
                onMoveBlock = true;
            else
                onMoveBlock = false;

            if (hitD.collider.gameObject.tag == "MoveBlock_Up")
                onMoveBlock_Up = true;
            else
                onMoveBlock_Up = false;

            if (hitD.collider.gameObject.tag == "RedSquare" && colorCheck != 1)
            {

                this.GetComponent<Renderer>().material = playerRed;
                this.GetComponent<Renderer>().material.name = "PlayerRed";

                if (oneShot_ChangeColorSE == false)
                {
                    Sound.PlaySE(5);
                    oneShot_ChangeColorSE = true;
                }
                colorCheck = 1;
            }

            if (hitD.collider.gameObject.tag == "BlueSquare" && colorCheck != 2)
            {

                this.GetComponent<Renderer>().material = playerBlue;
                this.GetComponent<Renderer>().material.name = "PlayerBlue";
                if (oneShot_ChangeColorSE == false)
                {
                    Sound.PlaySE(6);
                    oneShot_ChangeColorSE = true;
                }
                colorCheck = 2;
            }

            if (hitD.collider.gameObject.tag != "BlueSquare" && hitD.collider.gameObject.tag != "RedSquare")
                oneShot_ChangeColorSE = false;
        }
        else
        {
            if (CsvMessage.startend)
            {
                DropDown();
            }
        }
    }

    public void DropDown()
    {
        isDropDown = true;
        PlayerController.isNotInput = true;
        this.gameObject.transform.Rotate(new Vector3(3, 3, 3));     
        LeanTween.moveY(this.gameObject, this.gameObject.transform.position.y - 10.0f, 2.0f).setOnComplete(() => {
            EnemyHit.isGameOver = true;
        });
    }



    //色付き障害物が通れない時のエフェクトの表示位置
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
