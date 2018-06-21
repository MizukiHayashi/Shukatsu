using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3HormingBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;          //弾速
    private GameObject player;          //プレイヤー
    private Quaternion targetRotate;    //ターゲット方向

    // Use this for initialization
    void Start()
    {
        //スクリプト起動時にそれぞれ設定
        transform.tag = TagName.WEAK_ENEMY_ATTACK;
        gameObject.layer = LayerMask.NameToLayer("BossBullet");
        transform.GetComponent<SphereCollider>().isTrigger = true;
        Destroy(gameObject.GetComponent<BossCubeObject>());
        player = GameObject.Find("Player");
        Destroy(this.gameObject, 30.0f);

        //プレイヤーを向く
        targetRotate = Quaternion.LookRotation(player.transform.position - this.transform.position);
        this.transform.rotation = targetRotate;
    }

    // Update is called once per frame
    void Update()
    {
        //正面へ飛ばす
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Boss")
        {
            Debug.Log(gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
