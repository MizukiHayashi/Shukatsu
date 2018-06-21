using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlusHP : MonoBehaviour
{

    private float plusBossHP;   //追加ボスHP
    private PlayerAbility playerAbility;    //プレイヤーのステータス

    private float m_AttackMultipleCount;    //プレイヤーの攻撃上昇率

    // Use this for initialization
    void Start()
    {
        plusBossHP = 0.0f;
        playerAbility = GameObject.Find("PlayerAbility").GetComponent<PlayerAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.H))
        {
            plusBossHP += 20.0f;
        }
    }
    //プレイヤーの総攻撃力を計算
    private float GetPlayerAttackPower()
    {
        //ワイヤーで取得できるブロックの数に応じた上昇率を設定
        if (playerAbility.GetWireItemCount() <= 3)
            m_AttackMultipleCount = 0.05f;
        else if (playerAbility.GetWireItemCount() <= 5)
            m_AttackMultipleCount = 0.5f;
        else if (playerAbility.GetWireItemCount() <= 7)
            m_AttackMultipleCount = 2.0f;
        else if (playerAbility.GetWireItemCount() <= 9)
            m_AttackMultipleCount = 5.0f;
        else
            m_AttackMultipleCount = 10.0f;
        //現在のプレイヤーの最大攻撃力を計算（攻撃力*ワイヤーで取得できるオブジェクトの数*(1+上昇率)*10回攻撃した場合）
        return playerAbility.GetAttackPower() * playerAbility.GetWireCount() * (1 + m_AttackMultipleCount) * 10;
    }
    //追加ボスHP加算処理
    public void PlusHPUp(float value)
    {
        plusBossHP += value;
    }
    //追加ボスHP適応処理
    public float GetPlusHP()
    {
        //プレイヤーの最大攻撃力と追加ボスHPを比較して追加ボスHPを修正
        if (GetPlayerAttackPower() < plusBossHP / 10)
        {
            return plusBossHP = plusBossHP * 0.1f;
        }
        else if (GetPlayerAttackPower() < plusBossHP / 8)
        {
            return plusBossHP = plusBossHP * 0.25f;
        }
        else if (GetPlayerAttackPower() < plusBossHP / 6)
        {
            return plusBossHP = plusBossHP * 0.4f;
        }
        else if (GetPlayerAttackPower() < plusBossHP / 4)
        {
            return plusBossHP = plusBossHP * 0.55f;
        }
        else if (GetPlayerAttackPower() < plusBossHP / 2)
        {
            return plusBossHP = plusBossHP * 0.7f;
        }
        else if (GetPlayerAttackPower() < plusBossHP / 1.5)
        {
            return plusBossHP = plusBossHP * 0.85f;
        }

        return plusBossHP;

    }
    //追加ボスHPをリセット
    public void ResetPlusHP()
    {
        plusBossHP = 0.0f;
    }
}
