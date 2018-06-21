using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{

    //ボス行動推移
    public enum BossState
    {
        Move,   　//回転
        Stop,   　//停止
        Counter,　//弱点にダメージをくらった時に反撃
        Die     　//死亡
    }

    public enum AttackState
    {
        None,     //初期
        Bullet,   //弾丸攻撃
        Homing,   //追尾弾攻撃
        Laser,    //レーザー攻撃
    }

    private GameObject player;          //プレイヤー
    [SerializeField]
    private GameObject RightArm;        //右アーム
    [SerializeField]
    private GameObject LeftArm;         //左アーム
    [SerializeField]
    private Transform attackPos_R;     //攻撃部分（右）
    [SerializeField]
    private Transform attackPos_L;     //攻撃部分（左）
    [SerializeField]
    private Transform attackPos_Laser; //攻撃部分（中心）
    [SerializeField]
    private GameObject bulletPrefab;    //弾
    [SerializeField]
    private GameObject homingPrefab;    //追尾弾
    [SerializeField]
    private GameObject laserPrefab;     //レーザー
    [SerializeField]
    private Transform bossDefaultPos;   //ボス位置
    private Quaternion defaultRotate;   //初期回転位置

    private BossState bossState;    //ボスの状態

    private AttackState attackState;        //攻撃の状態
    private AttackState prevAttackState;    //前回の攻撃

    public float HP;           //ヒットポイント
    private BossPlusHP plusHP;  //追加ヒットポイント

    private Quaternion targetRotate;        //ターゲット方向(本体)
    private Quaternion targetRotate_R_Arm;  //ターゲット方向(右手)
    private Quaternion targetRotate_L_Arm;  //ターゲット方向(左手)
    [SerializeField]
    private float rotateSpeed;              //回転速度

    [SerializeField]
    private float attackStartTime;       //攻撃までの時間
    private float attackStartTimeCount;  //攻撃の時間カウント
    private bool isAttackStart;          //攻撃開始

    private float stopTime; //停止時間

    [SerializeField]
    private float bulletEndTime;      //通常弾終了時間
    private float bulletTimeCount;   //通常弾終了カウント
    private float bulletdelay;       //弾の発射間隔カウント

    [SerializeField]
    private int homingMaxShot;       //ホーミング弾数
    private int homingShotCount;     //ホーミング発射弾数
    private float homingMaxDelay_R;  //ホーミング弾待機時間(右手)
    private float homingMaxDelay_L;  //ホーミング弾待機時間(左手)
    private float homingdelay_R;     //ホーミング弾の発射間隔(右手)
    private float homingdelay_L;     //ホーミング弾の発射間隔(左手) 

    private bool laserStart;             //レーザー開始フラグ
    private bool laserEnd;               //レーザー終了フラグ
    [SerializeField]
    private Transform laserPos_Right;     //レーザー位置(右)
    [SerializeField]
    private Transform laserPos_Left;      //レーザー位置(左)
    private Transform laserStartPos;     //レーザー開始時位置
    private Transform laserEndPos;       //レーザー終了時位置
    private bool isLaserAttackStart;     //レーザースタート
    private int randomLaser;             //左右どちらか開始するかランダム変数

    private bool isCounter;     //カウンター攻撃フラグ
    private bool counterStart;  //カウンター攻撃開始フラグ
    private bool counterEnd;    //カウンター攻撃終了フラグ

    private Renderer m_Renderer;    //自身のレンダラー
    private Material m_Material;    //自身のマテリアル
    private Color emissionColor;    //自己発光カラー
    private Color defautEmissionColor;  //初期時の発光カラー
    private bool changeColor;       //色変更フラグ

    [SerializeField]
    private GameObject bossExplosionPrefab; //爆発プレハブ
    private bool isDie; //死亡フラグ

    private Scene_Manager_ sceneManager;
    private SoundsManager soundsManager;

    // Use this for initialization
    void Start()
    {
        //各変数の初期化
        defaultRotate = this.transform.rotation;

        bossState = BossState.Move;
        attackState = AttackState.Bullet;
        prevAttackState = AttackState.None;

        player = GameObject.Find("Player");

        plusHP = GameObject.Find("BossPlusHP").GetComponent<BossPlusHP>();
        HP = HP + plusHP.GetPlusHP();

        attackStartTimeCount = 0.0f;
        isAttackStart = false;

        stopTime = 5.0f;

        bulletTimeCount = 0.0f;
        bulletdelay = 0.0f;

        homingShotCount = 0;
        homingdelay_R = 0.0f;
        homingdelay_L = 0.0f;
        homingMaxDelay_R = 2.0f;
        homingMaxDelay_L = 4.0f;

        laserStart = false;
        laserEnd = false;
        isLaserAttackStart = false;

        isCounter = false;
        counterStart = false;
        counterEnd = false;

        m_Renderer = GetComponent<Renderer>();
        m_Material = m_Renderer.material;
        emissionColor = m_Material.GetColor("_EmissionColor");
        defautEmissionColor = m_Material.GetColor("_EmissionColor");
        changeColor = false;

        sceneManager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        isDie = false;

        soundsManager = GameObject.Find("ScriptManager").GetComponent<SoundsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの向きを設定
        targetRotate = Quaternion.LookRotation(player.transform.position - this.transform.position);

        if (HP <= 0)
        {
            bossState = BossState.Die;
            HP = 0;
        }
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.D))
        {
            bossState = BossState.Die;
        }
        //状態によって変更
        switch (bossState)
        {
            case BossState.Move:
                BossMove();
                break;
            case BossState.Stop:
                BossStop();
                break;
            case BossState.Counter:
                CounterAttack();
                break;
            case BossState.Die:
                BossDie();
                break;
        }

        if (changeColor == true)
        {
            FlashingBeforeLaser();
        }
        else
        {
            FlashingAfterLaser();
        }

    }
    //ボスの動作処理
    void BossMove()
    {
        //カウンター攻撃に移行
        if (isCounter == true) bossState = BossState.Counter;

        //プレイヤーを向く(レーザー攻撃以外)
        if (isLaserAttackStart == false)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotate, rotateSpeed * Time.deltaTime);
        }
        //プレイヤーの向き(右手、左手)
        targetRotate_L_Arm = Quaternion.LookRotation(player.transform.position - LeftArm.transform.position);
        targetRotate_R_Arm = Quaternion.LookRotation(player.transform.position - RightArm.transform.position);

        //攻撃間隔
        attackStartTimeCount += Time.deltaTime;
        if (attackStartTimeCount > attackStartTime && isAttackStart == false)
        {
            //前回の攻撃によって攻撃内容を変更
            switch (prevAttackState)
            {
                case AttackState.None:
                    attackState = AttackState.Bullet;
                    break;
                case AttackState.Bullet:
                    attackState = AttackState.Homing;
                    break;
                case AttackState.Homing:
                    randomLaser = Random.Range(1, 2 + 1);
                    attackState = AttackState.Laser;
                    break;
                case AttackState.Laser:
                    attackState = AttackState.Bullet;
                    break;
            }
            isAttackStart = true;
        }
        //攻撃可能なら
        if (isAttackStart == true)
        {
            switch (attackState)
            {
                case AttackState.Bullet:
                    BossAttack_Bullet();
                    break;
                case AttackState.Homing:
                    BossAttack_Homing();
                    break;
                case AttackState.Laser:
                    BossAttack_Laser();
                    break;
            }
        }
        else
        {
            //腕の回転を初期へ
            RightArm.transform.localRotation = Quaternion.Slerp(
                RightArm.transform.localRotation,
                new Quaternion(
                    RightArm.transform.localRotation.x,
                    0,
                    RightArm.transform.localRotation.z,
                    RightArm.transform.localRotation.w),
                rotateSpeed * Time.deltaTime
                );
            LeftArm.transform.localRotation = Quaternion.Slerp(
                LeftArm.transform.localRotation,
                new Quaternion(
                    LeftArm.transform.localRotation.x,
                    0,
                    LeftArm.transform.localRotation.z,
                    LeftArm.transform.localRotation.w),
                rotateSpeed * Time.deltaTime
                );
        }
    }
    //通常弾攻撃処理
    void BossAttack_Bullet()
    {
        if (bulletTimeCount == 0)
            soundsManager.PlaySE("Balkan", 10);
        //カウンターフラグがたったらカウンター攻撃に移行
        if (isCounter == true)
        {
            bulletTimeCount = 0.0f;
            prevAttackState = attackState;
            bossState = BossState.Counter;
        }
        //通常弾攻撃時間カウントの加算
        bulletTimeCount += Time.deltaTime;
        if (bulletTimeCount <= bulletEndTime)
        {
            //徐々に腕をプレイヤーに向ける
            RightArm.transform.rotation = Quaternion.Slerp(RightArm.transform.rotation, targetRotate_R_Arm, rotateSpeed * Time.deltaTime);
            LeftArm.transform.rotation = Quaternion.Slerp(LeftArm.transform.rotation, targetRotate_L_Arm, rotateSpeed * Time.deltaTime);
            bulletdelay += Time.deltaTime;
            //0.1秒毎に弾を生成
            if (bulletdelay > 0.1f)
            {
                Instantiate(bulletPrefab, attackPos_R.position, attackPos_R.rotation);
                Instantiate(bulletPrefab, attackPos_L.position, attackPos_L.rotation);
                bulletdelay = 0.0f;

            }
        }
        else
        {
            bulletTimeCount = 0.0f;
            prevAttackState = attackState;
            bossState = BossState.Stop;
        }
    }
    //ホーミング弾攻撃処理
    void BossAttack_Homing()
    {
        //カウンターフラグがたったらカウンター攻撃に移行
        if (isCounter == true)
        {
            homingShotCount = 0;
            prevAttackState = attackState;
            bossState = BossState.Counter;
        }
        //ホーミング弾の発射数が最大発射数以下なら
        if (homingShotCount < homingMaxShot)
        {
            homingdelay_L += Time.deltaTime;
            homingdelay_R += Time.deltaTime;
            //攻撃待機時間を越えたら(右)
            if (homingdelay_R > homingMaxDelay_R)
            {
                //弾生成
                Instantiate(homingPrefab, attackPos_R.position, attackPos_R.rotation);
                homingShotCount += 1;
                homingdelay_R = 0.0f;
                homingMaxDelay_R = 4.0f;
                soundsManager.PlaySE("Beamgun", 10);
            }
            //攻撃待機時間を越えたら(左)
            if (homingdelay_L > homingMaxDelay_L)
            {
                //弾生成
                Instantiate(homingPrefab, attackPos_L.position, attackPos_L.rotation);
                homingShotCount += 1;
                homingdelay_L = 0.0f;
                soundsManager.PlaySE("Beamgun", 10);
            }
            //Debug.Log("Attack2");
        }
        else
        {
            homingShotCount = 0;
            prevAttackState = attackState;
            bossState = BossState.Stop;

        }

    }
    //レーザー攻撃処理
    void BossAttack_Laser()
    {
        isLaserAttackStart = true;
        //レーザー開始位置の設定
        if (randomLaser == 1)
        {
            laserStartPos = laserPos_Right;
            laserEndPos = laserPos_Left;
        }
        else if (randomLaser == 2)
        {
            laserStartPos = laserPos_Left;
            laserEndPos = laserPos_Right;
        }
        //レーザーが始まっていなければ
        if (laserStart == false)
        {
            changeColor = true;
            //正面を向く
            LeanTween.rotate(this.gameObject, new Vector3(0, 180, 0), 1.0f).setOnComplete(() =>
             {
                 //レーザー開始位置に移動
                 LeanTween.move(this.gameObject, laserStartPos, 1.0f).setOnComplete(() =>
                 {
                     //レーザー生成
                     GameObject laser = Instantiate(laserPrefab, attackPos_Laser.position, attackPos_Laser.rotation, this.transform);
                     soundsManager.PlaySE("BossBeam", 10);
                     //レーザー終了位置に移動
                     LeanTween.move(this.gameObject, laserEndPos, 3.0f).setOnComplete(() =>
                     {
                         //レーザー消去
                         Destroy(laser);
                         changeColor = false;
                         //初期位置に移動
                         LeanTween.move(this.gameObject, bossDefaultPos, 1.0f).setOnComplete(() =>
                         {
                             prevAttackState = attackState;
                             laserStart = false;
                             isLaserAttackStart = false;
                             bossState = BossState.Stop;
                         });
                     });
                 });
             });
            laserStart = true;
        }
        // Debug.Log("Attack3");
    }
    //レーザー前の発光処理
    public void FlashingBeforeLaser()
    {
        if (emissionColor.r <= 3.0f && emissionColor.g <= 3.0f && emissionColor.b <= 3.0f)
        {
            emissionColor = Color.Lerp(emissionColor, new Color(3.0f, 3.0f, 3.0f), 0.1f);
        }
        m_Material.SetColor("_EmissionColor", emissionColor);
    }
    //レーザー後の発光処理
    public void FlashingAfterLaser()
    {
        if (emissionColor.r >= 1.1f && emissionColor.g >= 1.1f && emissionColor.b >= 1.1f)
        {
            emissionColor = Color.Lerp(emissionColor, defautEmissionColor, 0.1f);
        }
        m_Material.SetColor("_EmissionColor", emissionColor);
    }
    //攻撃後の動作の停止処理
    void BossStop()
    {
        stopTime -= Time.deltaTime;

        if (isCounter == true)
        {
            stopTime = 5.0f;
            bossState = BossState.Counter;
        }

        if (stopTime < 0.0f)
        {
            attackStartTimeCount = 0.0f;
            isAttackStart = false;
            stopTime = 5.0f;
            bossState = BossState.Move;
        }
    }
    //死亡時処理
    void BossDie()
    {
        if (isDie == false)
        {
            Instantiate(bossExplosionPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject, 2.5f);
            sceneManager.GameClear();
            GameObject.FindGameObjectWithTag(TagName.PLAYER).GetComponent<VR_PlayerWireAction>().MenuModeChange(true);
            isDie = true;
        }
    }
    //弱点に当たった時の処理
    public void HitWeakPoint()
    {
        if (bossState == BossState.Stop || (bossState == BossState.Move && attackState == AttackState.Bullet) ||
            (bossState == BossState.Move && attackState == AttackState.Homing) || (bossState == BossState.Move && attackState != AttackState.Laser))
            isCounter = true;
        else isCounter = false;
    }
    //カウンター攻撃処理
    void CounterAttack()
    {
        if (counterStart == false)
        {
            changeColor = true;
            //プレイヤーを向かせる
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotate, 1.0f);

            LeanTween.delayedCall(1.0f, () =>
            {
                //レーザー発射
                GameObject laser = Instantiate(laserPrefab, attackPos_Laser.position, attackPos_Laser.rotation, this.transform);
                Destroy(laser, 1.0f);
                counterEnd = true;
            });

            counterStart = true;
        }

        if (bossState == BossState.Die)
        {
            LeanTween.cancelAll();
        }

        if (counterEnd == true)
        {
            changeColor = false;
            counterEnd = false;
            counterStart = false;
            isCounter = false;
            attackStartTimeCount = 0.0f;
            isAttackStart = false;
            bossState = BossState.Move;
        }
    }


}
