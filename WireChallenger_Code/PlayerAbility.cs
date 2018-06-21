using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{

    [SerializeField, TooltipAttribute("攻撃力")]
    private float attackPower;
    [SerializeField, TooltipAttribute("ワイヤー数")]
    private float wireCount;
    [SerializeField, TooltipAttribute("オブジェクト復活時間")]
    private float resporneTime;
    [SerializeField, TooltipAttribute("吹っ飛び(小攻撃)")]
    private float knockBack_weak;
    [SerializeField, TooltipAttribute("吹っ飛び(大攻撃)")]
    private float knockBack_strong;

    //各アイテムの取得数
    private int getAttackItem;   
    private int getWireItem;
    private int getRespawneItem;
    private int getknockBackItem;

    // Use this for initialization
    void Start()
    {
        //変数の初期化
        getAttackItem = 0;
        getWireItem = 0;
        getRespawneItem = 0;
        getknockBackItem = 0;
    }
    //攻撃力上昇アイテム取得時の処理
    public void CountUp_AttackPower()
    {
        getAttackItem += 1;
        attackPower += 1.0f;
    }
    //攻撃力の上昇量を渡す
    public float GetAttackPower()
    {
        return attackPower;
    }
    //攻撃力上昇アイテムの取得数を渡す
    public int GetAttackItemCount()
    {
        return getAttackItem;
    }
    //ワイヤー数上昇処理
    public void CountUp_WireCount()
    {
        getWireItem += 1;
        wireCount += 1.0f;
        //10個より多くならない
        if (wireCount > 10)
        {
            getWireItem = 10;
            wireCount = 10;
        }
    }
    //ワイヤー数の上昇値を渡す
    public float GetWireCount()
    {
        return wireCount;
    }
    //ワイヤー数上昇アイテムの取得数を渡す
    public int GetWireItemCount()
    {
        return getWireItem;
    }
    //オブジェクト復活時間の短縮処理
    public void CountUp_ResporneTime()
    {
        getRespawneItem += 1;
        resporneTime -= 0.5f;
        //0より小さくならない
        if (resporneTime < 0.0f)
        {
            resporneTime = 0.0f;
        }
    }
    //オブジェクト復活時間の短縮値を渡す
    public float GetResporneTime()
    {
        return resporneTime;
    }
    //オブジェクト復活時間短縮アイテムの取得数を渡す
    public int GetRespawnItemCount()
    {
        return getRespawneItem;
    }
    //吹っ飛び率の軽減処理
    public void CountUp_KnockBack()
    {
        getknockBackItem += 1;
        knockBack_weak -= 0.5f;
        knockBack_strong -= 0.5f;
        //０より小さくはならない
        if (knockBack_weak < 0.0f)
        {
            knockBack_weak = 0.0f;
        }
        if (knockBack_strong < 0.0f)
        {
            knockBack_strong = 0.0f;
        }
    }
    //吹っ飛び軽減率(小攻撃)を渡す
    public float GetKenockBack_Weak()
    {
        return knockBack_weak;
    }
    //吹っ飛び軽減率(大攻撃)を渡す
    public float GetKenockBack_Strong()
    {
        return knockBack_strong;
    }
    //吹っ飛び軽減アイテムの取得数を渡す
    public int GetKnockBackItemCount()
    {
        return getknockBackItem;
    }
    //オールアップ取得時の処理
    public void CountUp_All()
    {
        CountUp_AttackPower();
        CountUp_WireCount();
        CountUp_ResporneTime();
        CountUp_KnockBack();
    }
    //全てのアビリティのリセット処理
    public void ResetAll()
    {

        getAttackItem = 0;
        getWireItem = 0;
        getRespawneItem = 0;
        getknockBackItem = 0;
        knockBack_strong = 100.0f;
        knockBack_weak = 10.0f;
        resporneTime = 10.0f;
        wireCount = 1.0f;
        attackPower = 1.0f;
    }
}
