using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputRetryScene: MonoBehaviour
{
    public string retrySceneName;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //リトライ処理
        if(Input.GetButton("R")&&Input.GetButton("L"))
        {
            Sound.StopBGM();
            SceneManager.LoadScene(retrySceneName);          
        }

    }
}
