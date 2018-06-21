using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseDrawPos : MonoBehaviour
{

    [SerializeField]
    private Canvas pauseCanvas; //ポーズ時に表示するキャンパス

    // Use this for initialization
    void Start()
    {
        //ポーズキャンバスのワールドカメラを現在のメインカメラに
        pauseCanvas.worldCamera = Camera.main;

    }

}
