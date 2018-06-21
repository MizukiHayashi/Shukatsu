using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;      //弾の速度

    private Vector3 randomRotate;   //ランダムで回転させる変数

    // Use this for initialization
    void Start()
    {
        //3秒後に削除
        Destroy(this.gameObject, 3.0f);
        //飛ぶ角度をランダム設定
        float randRenge = 0.5f;
        randomRotate = new Vector3(Random.Range(-randRenge, randRenge), Random.Range(-randRenge, randRenge), 0);
    }

    // Update is called once per frame
    void Update()
    {
        //前に飛ばす
        transform.position += transform.forward+randomRotate * bulletSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        //ボス以外に当たったら消す
        if (other.gameObject.tag != "Boss")
        {
            Destroy(this.gameObject);
        }
        //Debug.Log(other.gameObject.name);
    }
}
