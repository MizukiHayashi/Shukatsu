using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//チュートリアル用ボス
public class TutorialBoss : MonoBehaviour
{
    public float HP;                       //ヒットポイント
    [SerializeField]
    private GameObject bossExplosionPrefab; //爆発エフェクト

    private bool isDie; //死亡フラグ

    private Scene_Manager_ sceneManager;

    // Use this for initialization
    void Start()
    {
        isDie = false;
        sceneManager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
    }

    // Update is called once per frame
    void Update()
    {
        //HPが０になったら死亡
        if (HP <= 0.0f && isDie == false)
        {
            BossDie();
            isDie = true;
        }
    }
    //死亡時処理
    private void BossDie()
    {
        Instantiate(bossExplosionPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject, 2.5f);
        sceneManager.GameClear();
        GameObject.FindGameObjectWithTag(TagName.PLAYER).GetComponent<VR_PlayerWireAction>().MenuModeChange(true);
    }
}
