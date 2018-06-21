using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateSkyBox : MonoBehaviour {

    public int color;   //スカイボックスの色指定数値(0:白 1:緑 2:黄色 3:赤)
    public float angleParFrame=0.1f; //回転割合
    private float rotate = 0.0f; //回転量

	// Use this for initialization
	void Start () {
        Pause.pause = false;
        switch (color) //色指定数値が
        {
            case 0:
                RenderSettings.skybox.SetColor("_Tint", new Color32(255,255,255,128)); //スカイボックスを白色に
                break;
            case 1:
                RenderSettings.skybox.SetColor("_Tint", new Color32(0, 255, 0, 128));  //スカイボックスを緑色に
                break;
            case 2:
                RenderSettings.skybox.SetColor("_Tint", new Color32(255, 255, 0, 128));//スカイボックスを黄色に
                break;
            case 3:
                RenderSettings.skybox.SetColor("_Tint", new Color32(255, 0, 0, 128));  //スカイボックスを赤色に
                break;
            default:
                RenderSettings.skybox.SetColor("_Tint", new Color32(255, 255, 255, 128)); //デフォルトは白色
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //ポーズじゃなければ
        if(Pause.pause==false)
            rotate += angleParFrame;
        if (rotate >= 360.0f)
        {    
            rotate -= 360.0f;
        }
        RenderSettings.skybox.SetFloat("_Rotation", rotate);

	}
}
