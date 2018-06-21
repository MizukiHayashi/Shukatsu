using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;   //弾速
    [SerializeField]
    private float rotateSpeed;   //回転速度

    private GameObject player;      //プレイヤー
    private Quaternion targetRotate;//ターゲットの方向


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        //12秒後に削除
        Destroy(this.gameObject, 12.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーを向く
        targetRotate = Quaternion.LookRotation(player.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotate, rotateSpeed * Time.deltaTime);
        //正面に飛ばす
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Boss")
        {
            Destroy(this.gameObject);
        }
    }
}
