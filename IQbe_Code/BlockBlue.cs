using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBlue : MonoBehaviour
{
    private bool flag_blue; //emissionの色が青かどうか
    private new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        flag_blue = false;
        renderer = GetComponent<Renderer>();
    }

    public void FixedUpdate()
    {
        //自信のマテリアルの取得
        Material mat = renderer.material;
        //emissionカラーの取得
        Color baseColor = mat.GetColor("_EmissionColor");
        //色を点滅させる
        if (baseColor.b <= 1.2f)
        {
            baseColor.r = 0.06f;
            baseColor.g = 0.6f;
            baseColor.b = 1.2f;
            flag_blue = true;

        }
        else if (baseColor.b >= 2.0f)
        {
            baseColor.r = 0.1f;
            baseColor.g = 1.0f;
            baseColor.b = 2.0f;
            flag_blue = false;
        }
        //特定の数値になったら色を修正する
        if (flag_blue == true)
        {
            baseColor.b += 0.0133f;
            baseColor.r += 0.000665f;
            baseColor.g += 0.00665f;
        }
        else if (flag_blue == false)
        {
            baseColor.b -= 0.0133f;
            baseColor.r -= 0.000665f;
            baseColor.g -= 0.00665f;
        }
        //emissionに設定する
        mat.SetColor("_EmissionColor", baseColor);
    }
}
