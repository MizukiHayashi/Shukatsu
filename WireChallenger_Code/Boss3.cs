using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Boss3 : MonoBehaviour
{

    //ボス状態遷移
    public enum BossState
    {
        Move,       //回転
        Stop,       //停止
        Counter,    //弱点にダメージをくらった時に反撃
        Die         //死亡
    }
    //攻撃状態
    public enum AttackState
    {
        None,       //初期
        ShotGun,    //散弾攻撃
        Horming,    //追尾弾攻撃
        Laser       //レーザー攻撃
    }
    private BossState bossState;            //ボスの状態
    private AttackState attackState;        //攻撃の状態
    public float HP;    //ヒットポイント
    private BossPlusHP plusHP;

    private GameObject player;  //プレイヤー
    [SerializeField]
    private GameObject[] panels; //ステージの床の各パネル

    private Quaternion targetRotate;    //ターゲットの方向
    [SerializeField]
    private float rotateSpeed;   //回転速度

    private bool isOnlyRotate;  //回転動作のみ

    private bool isStop;    //停止フラグ
    private float stopTime; //停止時間

    [SerializeField]
    private float attackStartTime;   //攻撃までの時間
    private float attackStartTimeCount; //攻撃の時間カウント
    private float laserStartTime;   //レーザー攻撃までの時間
    private bool isAttackStart;     //攻撃開始フラグ

    //ショットガン攻撃に使う変数
    private Quaternion shotTargetRotate;
    [SerializeField]
    private GameObject boss3Bullet;  //ショットガンの弾
    [SerializeField]
    private GameObject RightArm;     //右アーム
    [SerializeField]
    private GameObject LeftArm;      //左アーム
    [SerializeField]
    private Transform rightShotPos;  //攻撃部分(右腕部分)
    [SerializeField]
    private Transform leftShotPos;   //攻撃部分(左腕部分)
    [SerializeField]
    private int shotGunMaxShot = 4;      //最大ショットガン発射数
    private int shotGunShotCount;   //ショットガン発射カウント
    private float shotGundelay_R;    //弾発射間隔(右)
    private float shotGundelay_L;    //弾発射間隔(左)
    private Quaternion shotTarget_R; //攻撃場所(右腕)
    private Quaternion shotTarget_L; //攻撃場所(左腕)   

    //ホーミング攻撃に使う変数
    [SerializeField]
    private Transform centerAttackPos;   //攻撃部部(中心部)
    [SerializeField]
    private GameObject boss3Horming; //ホーミング弾
    [SerializeField]
    private Transform[] hormingStopPos; //ホーミング待機場所(3箇所)
    private GameObject horming1;    //ホーミング１発目
    private GameObject horming2;    //ホーミング２発目
    private GameObject horming3;    //ホーミング３発目
    private bool hormingShotStart;  //ホーミング弾発射開始フラグ
    private bool hormingShotEnd;    //ホーミング弾発射終了フラグ
    private float hormingStopTime;  //ホーミング弾待機時間

    //レーザー攻撃に使う変数
    [SerializeField]
    private Transform attackPos_RU;          //レーザー発射位置(右上)
    [SerializeField]
    private Transform attackPos_RD;          //レーザー発射位置(右下)
    [SerializeField]
    private Transform attackPos_LU;          //レーザー発射位置(左上)
    [SerializeField]
    private Transform attackPos_LD;          //レーザー発射位置(右下)
    [SerializeField]
    private Transform laserAttackPos;        //レーザー攻撃開始位置
    [SerializeField]
    private Transform bossDefaultPos;        //ボスの基本位置
    [SerializeField]
    private GameObject LaserPrefab;          //レーザープレハブ
    [SerializeField]
    private GameObject LaserChargePrefab;    //レーザーチャージプレハブ
    private GameObject laserCharge_RU;       //レーザーチャージ(右上)
    private GameObject laserCharge_LU;       //レーザーチャージ(左上)
    private GameObject laserCharge_RD;       //レーザーチャージ(右下)
    private GameObject laserCharge_LD;       //レーザーチャージ(左下)
    private bool laserStart;        //レーザー開始フラグ
    private bool laserEnd;          //レーザー終了フラグ
    private bool isPanelBreak;      //パネル破壊フラグ
    private float panelBreakTime;   //パネル破壊時間

    //カウンター攻撃に使う変数
    private bool isCounter;
    private bool counterStart;
    private bool counterEnd;

    [SerializeField]
    private GameObject bossExplosionPrefab; //爆発エフェクト

    private Scene_Manager_ sceneManager;
    private bool isDie;

    private SoundsManager soundsManager;

    // Use this for initialization
    void Start()
    {
        //各変数の初期化
        bossState = BossState.Move;
        attackState = AttackState.ShotGun;

        player = GameObject.Find("Player");

        plusHP = GameObject.Find("BossPlusHP").GetComponent<BossPlusHP>();
        HP = HP + plusHP.GetPlusHP();

        isOnlyRotate = true;

        stopTime = 5.0f;

        attackStartTimeCount = 0.0f;
        laserStartTime = 60.0f;
        isAttackStart = false;

        shotGunShotCount = 0;
        shotGundelay_R = 0.0f;
        shotGundelay_L = 2.0f;

        hormingShotStart = false;
        hormingShotEnd = false;
        hormingStopTime = 5.0f;

        laserStart = false;
        laserEnd = false;
        isPanelBreak = false;
        panelBreakTime = 30.0f;

        sceneManager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
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
        //パネル破壊フラグがたったら
        if (isPanelBreak == true)
        {
            panelBreakTime -= Time.deltaTime;
            //破壊時間が０になったらパネル復活
            if (panelBreakTime < 0.0f)
            {
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(true);
                }
                panelBreakTime = 30.0f;
                isPanelBreak = false;
            }
        }
    }
    //ボスの動作処理
    void BossMove()
    {
        //カウンター攻撃に移行
        if (isCounter == true) bossState = BossState.Counter;
        //回転動作のみ
        if (isOnlyRotate == true)
        {
            //プレイヤーを向く    
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotate, rotateSpeed * Time.deltaTime);
        }

        //攻撃間隔
        attackStartTimeCount += Time.deltaTime;
        laserStartTime -= Time.deltaTime;
        //レーザーカウントが０じゃなければ
        if (attackStartTimeCount > attackStartTime && isAttackStart == false && laserStartTime > 0.0f)
        {
            //ランダムでショットガンかホーミングを決定
            int rand = UnityEngine.Random.Range(1, 100);
            if (rand < 60) attackState = AttackState.ShotGun;
            else attackState = AttackState.Horming;
            isAttackStart = true;
        }
        //レーザーカウントが０になったら
        else if (laserStartTime < 0.0f && isAttackStart == false && attackStartTimeCount > attackStartTime)
        {
            attackState = AttackState.Laser;
            isAttackStart = true;
        }
        //攻撃可能なら
        if (isAttackStart == true)
        {
            switch (attackState)
            {
                case AttackState.ShotGun:
                    BossAttack_ShotGun();
                    break;
                case AttackState.Horming:
                    BossAttack_Horming();
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

    //ショットガン攻撃
    void BossAttack_ShotGun()
    {
        isOnlyRotate = false;
        //カウンターフラグがたったらカウンター攻撃に移行
        if (isCounter == true)
        {
            //パネルエフェクトを消す
            for (int n = 0; n < panels.Length; n++)
            {
                panels[n].GetComponent<PanelEffectScript>().PanelEffect_OFF();
            }
            shotGunShotCount = 0;
            bossState = BossState.Counter;
        }
        //ショットガン発射回数が最大回数以下なら
        if (shotGunShotCount < shotGunMaxShot)
        {
            //全てのパネルと比較
            for (int i = 0; i < panels.Length; i++)
            {
                //プレイヤーがパネルから離れすぎていなければ
                if (player.transform.position.y <= 10)
                {
                    //腕を向ける方向をプレイヤーに近いパネルに設定
                    if (Vector3.Distance(panels[i].transform.position, player.transform.position) <= 20.0f)
                    {
                        shotTargetRotate = Quaternion.LookRotation(panels[i].transform.position - this.transform.position);
                        shotTarget_R = Quaternion.LookRotation(panels[i].transform.position - RightArm.transform.position);
                        shotTarget_L = Quaternion.LookRotation(panels[i].transform.position - LeftArm.transform.position);
                        //パネルエフェクトを起動
                        panels[i].GetComponent<PanelEffectScript>().PanelEffect_ON();
                    }
                    else
                    {
                        //パネルエフェクトを消す
                        panels[i].GetComponent<PanelEffectScript>().PanelEffect_OFF();
                    }
                }
                else
                {
                    //腕を向ける方向をプレイヤーに設定
                    shotTargetRotate = Quaternion.LookRotation(player.transform.position - this.transform.position);
                    shotTarget_R = Quaternion.LookRotation(player.transform.position - RightArm.transform.position);
                    shotTarget_L = Quaternion.LookRotation(player.transform.position - LeftArm.transform.position);
                    panels[i].GetComponent<PanelEffectScript>().PanelEffect_OFF();
                }
            }
            //腕を向ける
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, shotTargetRotate, rotateSpeed * Time.deltaTime);
            RightArm.transform.rotation = Quaternion.Slerp(RightArm.transform.rotation, shotTarget_R, rotateSpeed * Time.deltaTime);
            LeftArm.transform.rotation = Quaternion.Slerp(LeftArm.transform.rotation, shotTarget_L, rotateSpeed * Time.deltaTime);

            shotGundelay_R -= Time.deltaTime;
            shotGundelay_L -= Time.deltaTime;
            //攻撃待機時間を越えたら(右)
            if (shotGundelay_R < 0.0f)
            {
                //弾を100発生成
                for (int n = 0; n < 100; n++)
                {
                    Instantiate(boss3Bullet, rightShotPos.position, rightShotPos.rotation);
                }
                soundsManager.PlaySE("Shotgun", 12);

                shotGunShotCount += 1;
                shotGundelay_R = 4.0f;
            }
            //攻撃待機時間を越えたら(左)
            if (shotGundelay_L < 0.0f)
            { 
                //弾を100発生成
                for (int n = 0; n < 100; n++)
                {
                    Instantiate(boss3Bullet, leftShotPos.position, leftShotPos.rotation);
                }
                soundsManager.PlaySE("Shotgun", 12);

                shotGunShotCount += 1;
                shotGundelay_L = 4.0f;
            }

        }
        else
        {
            for (int n = 0; n < panels.Length; n++)
            {
                panels[n].GetComponent<PanelEffectScript>().PanelEffect_OFF();
            }
            shotGunShotCount = 0;
            bossState = BossState.Stop;
        }
    }

    //ホーミング攻撃
    void BossAttack_Horming()
    {
        isOnlyRotate = false;
        //攻撃が始まっていなければ
        if (hormingShotStart == false)
        {
            //正面を向かせる
            LeanTween.rotate(this.gameObject, new Vector3(0, 180, 0), 1.0f).setOnComplete(() =>
            {
                //斜め上を向かせる
                LeanTween.rotateX(this.gameObject, -45, 2.0f).setOnComplete(() =>
                {
                    //ホーミング発射スタート
                    StartCoroutine(HormingShot());
                });
            });
            hormingShotStart = true;
        }
        //ホーミング発射終了
        if (hormingShotEnd == true)
        {
            //ホーミング待機時間が０になったらホーミング起動
            hormingStopTime -= Time.deltaTime;
            if (hormingStopTime < 0.0f)
            {
                if (horming1 != null && horming1.GetComponent<LineRenderer>().enabled == false)
                {
                    soundsManager.PlaySE("Missile2", 21);
                    try
                    {
                        horming1.GetComponent<Boss3HormingBullet>().enabled = true;
                    }
                    catch
                    { }
                }

                if (horming2 != null && horming2.GetComponent<LineRenderer>().enabled == false)
                {
                    soundsManager.PlaySE("Missile2", 22);
                    try
                    {
                        horming2.GetComponent<Boss3HormingBullet>().enabled = true;
                    }
                    catch { }
                }
                if (horming3 != null && horming3.GetComponent<LineRenderer>().enabled == false)
                {
                    soundsManager.PlaySE("Missile2", 23);
                    try
                    {
                        horming3.GetComponent<Boss3HormingBullet>().enabled = true;
                    }
                    catch { }
                }

                hormingShotStart = false;
                hormingShotEnd = false;
                hormingStopTime = 5.0f;
                bossState = BossState.Stop;
            }
        }
    }

    //ホーミング発射
    IEnumerator HormingShot()
    {
        //1秒後にホーミング１を発射
        LeanTween.delayedCall(1.0f, () =>
        {
            horming1 = Instantiate(boss3Horming, centerAttackPos.position, centerAttackPos.rotation);
            soundsManager.PlaySE("Missile1", 21);
            LeanTween.move(horming1, hormingStopPos[0].position, 0.5f);
        });
        //2秒待機
        yield return new WaitForSeconds(2.0f);
        //1秒後にホーミング2を発射
        LeanTween.delayedCall(1.0f, () =>
        {
            horming2 = Instantiate(boss3Horming, centerAttackPos.position, centerAttackPos.rotation);
            soundsManager.PlaySE("Missile1", 22);
            LeanTween.move(horming2, hormingStopPos[1].position, 0.5f);
        });
        //2秒待機
        yield return new WaitForSeconds(2.0f);
        //1秒後にホーミング3を発射
        LeanTween.delayedCall(1.0f, () =>
        {
            horming3 = Instantiate(boss3Horming, centerAttackPos.position, centerAttackPos.rotation);
            soundsManager.PlaySE("Missile1", 23);
            LeanTween.move(horming3, hormingStopPos[2].position, 0.5f);
        });

        hormingShotEnd = true;
        //2秒待機
        yield return new WaitForSeconds(2.0f);
        //正面を向ける
        LeanTween.rotate(this.gameObject, new Vector3(0, 180, 0), 2.0f);
    }

    //レーザー攻撃
    void BossAttack_Laser()
    {
        isOnlyRotate = false;
        //攻撃が始まっていなければ
        if (laserStart == false)
        {
            //正面を向ける
            LeanTween.rotate(this.gameObject, new Vector3(0.0f, 180.0f, 0.0f), 1.0f).setOnComplete(() =>
            {
                //斜め上を向ける
                LeanTween.rotateX(this.gameObject, -45.0f, 1.5f).setOnComplete(() =>
                {
                    //レーザー発射位置に移動
                    LeanTween.move(this.gameObject, laserAttackPos.position, 2.5f).setOnComplete(() =>
                    {
                        //斜め下を向ける
                        LeanTween.rotateX(this.gameObject, 60.0f, 2.0f).setOnComplete(() =>
                        {
                            //レーザー発射
                            StartCoroutine(LaserShot());
                        });
                    });
                });
            });
            laserStart = true;
        }
        //レーザー攻撃が終了したら
        if (laserEnd == true)
        {
            //斜め後ろ下を向ける
            LeanTween.rotate(this.gameObject, new Vector3(120.0f, 180.0f, 0.0f), 2.0f).setOnComplete(() =>
              {
                  //ボス初期位置に移動
                  LeanTween.move(this.gameObject, bossDefaultPos, 2.5f).setOnComplete(() =>
                  {
                      //正面を向かせる
                      LeanTween.rotate(this.gameObject, new Vector3(0, 180, 0), 2.0f).setOnComplete(() =>
                      {
                          laserStart = false;
                          laserStartTime = 60.0f;
                          bossState = BossState.Stop;
                      });
                  });
              });
            laserEnd = false;
        }
    }

    //レーザー発射
    IEnumerator LaserShot()
    {
        //破壊するパネルをランダムソートで選ぶ
        var ary = Enumerable.Range(0, panels.Length).OrderBy(n => Guid.NewGuid()).Take(4).ToArray();
        //それぞれパネルに向ける
        attackPos_RU.rotation = Quaternion.LookRotation(panels[ary[0]].transform.position - attackPos_RU.position);
        attackPos_LU.rotation = Quaternion.LookRotation(panels[ary[1]].transform.position - attackPos_LU.position);
        attackPos_RD.rotation = Quaternion.LookRotation(panels[ary[2]].transform.position - attackPos_RD.position);
        attackPos_LD.rotation = Quaternion.LookRotation(panels[ary[3]].transform.position - attackPos_LD.position);
        //0.5秒待機
        yield return new WaitForSeconds(0.5f);
        //狙っているパネルのパネルエフェクトを起動
        panels[ary[0]].GetComponent<PanelEffectScript>().PanelEffect_ON();
        panels[ary[1]].GetComponent<PanelEffectScript>().PanelEffect_ON();
        panels[ary[2]].GetComponent<PanelEffectScript>().PanelEffect_ON();
        panels[ary[3]].GetComponent<PanelEffectScript>().PanelEffect_ON();
        //レーザーチャージを生成
        laserCharge_RU = Instantiate(LaserChargePrefab, attackPos_RU.position, attackPos_RU.rotation);
        laserCharge_LU = Instantiate(LaserChargePrefab, attackPos_LU.position, attackPos_LU.rotation);
        laserCharge_RD = Instantiate(LaserChargePrefab, attackPos_RD.position, attackPos_RD.rotation);
        laserCharge_LD = Instantiate(LaserChargePrefab, attackPos_LD.position, attackPos_LD.rotation);
        soundsManager.PlaySE("Charge", 12);
        //徐々に大きくする
        LeanTween.scale(laserCharge_RU, new Vector3(3.0f, 3.0f, 3.0f), 2.0f);
        LeanTween.scale(laserCharge_LU, new Vector3(3.0f, 3.0f, 3.0f), 2.0f);
        LeanTween.scale(laserCharge_RD, new Vector3(3.0f, 3.0f, 3.0f), 2.0f);
        LeanTween.scale(laserCharge_LD, new Vector3(3.0f, 3.0f, 3.0f), 2.0f);
        //2秒待機
        yield return new WaitForSeconds(2.0f);
        //0.5秒毎にレーザーを発射
        //チャージを消してレーザー発射
        Destroy(laserCharge_RU);
        Instantiate(LaserPrefab, attackPos_RU.position, attackPos_RU.rotation);
        soundsManager.PlaySE("ChargeShot", 12);
        //パネルエフェクトを消してパネルを非表示に
        panels[ary[0]].GetComponent<PanelEffectScript>().PanelEffect_OFF();
        panels[ary[0]].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Destroy(laserCharge_LU);
        Instantiate(LaserPrefab, attackPos_LU.position, attackPos_LU.rotation);
        soundsManager.PlaySE("ChargeShot", 12);
        panels[ary[1]].GetComponent<PanelEffectScript>().PanelEffect_OFF(); ;
        panels[ary[1]].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Destroy(laserCharge_RD);
        Instantiate(LaserPrefab, attackPos_RD.position, attackPos_RD.rotation);
        soundsManager.PlaySE("ChargeShot", 12);
        panels[ary[2]].GetComponent<PanelEffectScript>().PanelEffect_OFF();
        panels[ary[2]].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Destroy(laserCharge_LD);
        Instantiate(LaserPrefab, attackPos_LD.position, attackPos_LD.rotation);
        soundsManager.PlaySE("ChargeShot", 12);
        panels[ary[3]].GetComponent<PanelEffectScript>().PanelEffect_OFF();
        panels[ary[3]].SetActive(false);
        //1秒待機
        yield return new WaitForSeconds(1.0f);
        isPanelBreak = true;
        laserEnd = true;
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
            isOnlyRotate = true;
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
        if (bossState == BossState.Stop || (bossState == BossState.Move && attackState == AttackState.ShotGun) ||
            ((bossState == BossState.Move && attackState != AttackState.Horming) && (bossState == BossState.Move && attackState != AttackState.Laser)))
            isCounter = true;
        else isCounter = false;
    }
    //カウンター攻撃処理
    void CounterAttack()
    {
        if (counterStart == false)
        {
            //腕をプレイヤーに向ける
            shotTarget_R = Quaternion.LookRotation(player.transform.position - RightArm.transform.position);
            shotTarget_L = Quaternion.LookRotation(player.transform.position - LeftArm.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotate, 1.0f);
            RightArm.transform.rotation = Quaternion.Slerp(RightArm.transform.rotation, shotTarget_R, 1.0f);
            LeftArm.transform.rotation = Quaternion.Slerp(LeftArm.transform.rotation, shotTarget_L, 1.0f);
            //両腕から50発ずつ弾を発射
            for (int n = 0; n < 50; n++)
            {
                Instantiate(boss3Bullet, rightShotPos.position, rightShotPos.rotation);
                Instantiate(boss3Bullet, leftShotPos.position, leftShotPos.rotation);
            }
            soundsManager.PlaySE("Shotgun", 12);
            counterStart = true;
            counterEnd = true;
        }

        if (counterEnd == true)
        {
            for (int n = 0; n < panels.Length; n++)
            {
                panels[n].GetComponent<PanelEffectScript>().PanelEffect_OFF();
            }
            isCounter = false;
            isOnlyRotate = true;
            isAttackStart = false;
            attackStartTimeCount = 0.0f;
            counterStart = false;
            counterEnd = false;
            bossState = BossState.Move;
        }
    }
}
