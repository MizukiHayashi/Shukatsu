using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//リザルト表示
public class ResultDrawPos : MonoBehaviour
{

    [SerializeField]
    private Canvas resultCanvas;    //リザルトキャンバス

    // Use this for initialization
    void Start()
    {
        //リザルトキャンバスのワールドカメラにメインカメラを設定
        resultCanvas.worldCamera = Camera.main;
    }

}
