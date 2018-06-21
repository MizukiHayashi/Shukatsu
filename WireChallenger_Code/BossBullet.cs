using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;  //弾速

    Vector3 randomRotate;   //弾の向きをランダムに設定する変数

    // Use this for initialization
    void Start()
    {
        //3秒後に削除
        Destroy(this.gameObject, 3.0f);
        //向きをランダムに設定
        randomRotate = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        //正面に飛ばす
        transform.position += transform.forward+randomRotate * bulletSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Boss")
        {
            Destroy(this.gameObject);
        }
        //Debug.Log(other.gameObject.name);
    }
}
