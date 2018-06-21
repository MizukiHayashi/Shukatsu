using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{
    [SerializeField]
    private Transform initRestartPoint;             //処理リスタートポイント
    [SerializeField]
    private GameObject respawnEffect;               //リスポーン時のエフェクト
    private Transform m_RestartPoint;               //自分のリスタートポイント
    private Scene_Manager_ sceneManager;            //シーンマネージャー
    private Scene_Manager_Fade sceneManager_Fade;   //フェードシーンマネージャー
    private TimeLimit timeLimit;                    //制限時間
    private bool isRespawn;                         //リスポーンするか

    [SerializeField]
    private GameObject player;   //プレイヤー
    [SerializeField]
    private Transform goalPos;   //ゴール位置
    // Use this for initialization
    void Start()
    {
        //各変数の初期化
        sceneManager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        sceneManager_Fade = Camera.main.GetComponent<Scene_Manager_Fade>();
        timeLimit = GameObject.Find("TimeTex").GetComponent<TimeLimit>();
        m_RestartPoint = initRestartPoint;
        isRespawn = false;
        //goalPos = GameObject.Find("GoalPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.G))
        {
            player.transform.position = goalPos.position;
        }
    }
    //それぞれ触れた時の処理
    public void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            //リスタートポイントに触れたら
            case "RestartPoint":
                m_RestartPoint = other.transform;   //リスタート位置を更新
                break;
            //死亡壁に触れたら
            case "DestroyWall":
                if (isRespawn == false)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    StartCoroutine(Respawn());
                    GetComponent<VR_PlayerWireAction>().JointConnectedBodyRelease();
                    GetComponent<VR_PlayerWireAction>().WireLineDelete();
                    GetComponent<VR_PlayerWireAction>().KnockBackTimeReset();
                    GetComponent<VR_PlayerWireAction>().EntryThrowObjsReset();
                    isRespawn = true;
                }
                break;
            //ゴールポイントに触れたら
            case "GoalPoint":
                sceneManager.GameClear();
                timeLimit.CountStop(true);
                GetComponent<VR_PlayerWireAction>().MenuModeChange(true);
                break;

        }
    }
    //リスポーン時の処理
    IEnumerator Respawn()
    {
        //リスポーンエフェクトを生成
        GameObject effect = Instantiate(respawnEffect, player.transform.position, respawnEffect.transform.rotation, player.transform);
        //3.5秒待つ
        yield return new WaitForSeconds(3.5f);
        //移動量を０にしてリスポーン位置に移動
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = m_RestartPoint.position;
        //1.5秒まつ
        yield return new WaitForSeconds(1.5f);
        //エフェクト削除
        Destroy(effect);
        isRespawn = false;
    }
}
