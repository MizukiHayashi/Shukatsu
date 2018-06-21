using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRed : MonoBehaviour
{
    private bool flag_red; //emissionの色が赤かどうか

    private new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void FixedUpdate()
    {
        //自信のマテリアルの取得
        Material mat = renderer.material;
        //emissionカラーの取得
        Color baseColor = mat.GetColor("_EmissionColor");
        //色を点滅させる
        if (baseColor.r <= 1.2f)
        {
            baseColor.r = 1.2f;
            baseColor.g = 0.6f;
            baseColor.b = 0.1f;
            flag_red = true;

        }
        else if (baseColor.r >= 2.0f)
        {
            baseColor.r = 2.0f;
            baseColor.g = 1.0f;
            baseColor.b = 0.166f;
            flag_red = false;
        }
        //特定の数値になったら色を修正する
        if (flag_red == true)
        {
            baseColor.r += 0.0133f;
            baseColor.g += 0.00665f;
            baseColor.b += 0.0011f;
        }
        else if (flag_red == false)
        {
            baseColor.r -= 0.0133f;
            baseColor.g -= 0.00665f;
            baseColor.b -= 0.0011f;
        }
        //emissionに設定する
        mat.SetColor("_EmissionColor", baseColor);
    }
}
