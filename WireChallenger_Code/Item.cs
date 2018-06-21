using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        AttackPower,    //攻撃力
        MaxWireCount,   //最大ワイヤー数
        ResponeTime,    //オブジェクト復活時間
        KnockBack,      //飛弾時の吹っ飛び量
        AllUp           //全ステータス上昇(レア)
    }
    public ItemType itemType;

    [SerializeField]
    private GameObject arrowPrefab; //矢印オブジェクトプレハブ
    [SerializeField]
    private float itemDestroyTime;  //アイテム消失時間

    private Item_Manager item_manger;   //アイテムマネージャー
    private GameObject arrow;           //矢印オブジェクト
    private Transform arrowInstantPos;  //矢印生成位置
    private GameObject maskObj;         //マスクオブジェクト

    private Quaternion targetRotation;  //ターゲットの方向

    private PlayerAbility playerAbility;    //プレイヤーアビリティ

    private SoundsManager soundsManager;

    // Use this for initialization
    void Start()
    {
        soundsManager = GameObject.Find("ScriptManager").GetComponent<SoundsManager>();

        //アイテムが生成されると同時に矢印を生成
        arrowInstantPos = GameObject.Find("ArrowInstantPos").transform;
        arrow = Instantiate(arrowPrefab, arrowInstantPos);
        //アイテムマネージャーを取得
        item_manger = GameObject.Find("ItemManager").GetComponent<Item_Manager>();
        //アイテムと矢印をアイテム削除時間後で削除
        Destroy(this.gameObject, itemDestroyTime);
        Destroy(arrow, itemDestroyTime);
        //マスクオブジェクトの取得
        maskObj = arrow.transform.Find("Mask_Base").gameObject;
        //マスクの位置をアイテム削除時間に合わせてずらしていく
        LeanTween.scaleZ(maskObj, 0, itemDestroyTime);
        //プレイヤーアビリティの取得
        playerAbility = GameObject.Find("PlayerAbility").GetComponent<PlayerAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットの方向の計算
        targetRotation = Quaternion.LookRotation(this.transform.position - arrow.transform.position);
        //矢印をターゲットに向ける
        arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, targetRotation, Time.deltaTime * 5);
    }

    public void OnTriggerEnter(Collider other)
    {
        //プレイヤーと当たったら
        if (other.tag == TagName.PLAYER)
        {
            soundsManager.PlaySE("ItemGet", 6);
            //アイテム取得数加算
            item_manger.ItemCountUp();
            //アイテムタイプによってそれぞれ効果付与
            switch (itemType)
            {
                case ItemType.AttackPower:
                    playerAbility.CountUp_AttackPower();
                    break;
                case ItemType.KnockBack:
                    playerAbility.CountUp_KnockBack();
                    break;
                case ItemType.MaxWireCount:
                    playerAbility.CountUp_WireCount();
                    break;
                case ItemType.ResponeTime:
                    playerAbility.CountUp_ResporneTime();
                    break;
                case ItemType.AllUp:
                    playerAbility.CountUp_All();
                    break;
            }
            //矢印とアイテムを削除
            Destroy(arrow);
            Destroy(this.gameObject);
        }
    }
}
