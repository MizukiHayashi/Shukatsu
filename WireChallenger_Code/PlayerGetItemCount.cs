using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UIにプレイヤーの取得しているアイテム数を取得する
public class PlayerGetItemCount : MonoBehaviour
{
    //アイテムのタイプ
    public enum ItemType
    {
        Attack,
        Respawn,
        KnockBack,
        Wire
    }

    public ItemType itemType;

    private PlayerAbility playerAbility;    //プレイヤーアビリティ

    // Use this for initialization
    void Start()
    {
        playerAbility = GameObject.Find("PlayerAbility").GetComponent<PlayerAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        //それぞれ設定されたタイプに応じて取得数を取得し、表示
        switch (itemType)
        {
            case ItemType.Attack:
                this.GetComponent<Text>().text = playerAbility.GetAttackItemCount().ToString();
                break;
            case ItemType.KnockBack:
                this.GetComponent<Text>().text = playerAbility.GetKnockBackItemCount().ToString();
                break;
            case ItemType.Respawn:
                this.GetComponent<Text>().text = playerAbility.GetRespawnItemCount().ToString();
                break;
            case ItemType.Wire:
                this.GetComponent<Text>().text = playerAbility.GetWireItemCount().ToString();
                break;
        }
    }
}
