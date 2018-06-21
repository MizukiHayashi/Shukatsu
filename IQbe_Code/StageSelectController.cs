using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageSelectController : MonoBehaviour
{
    [SerializeField]
    private GameObject modeSelect;      //ゲームモード選択UI
    [SerializeField]
    private GameObject stageSelect_CD;  //カウントダウンモードUI
    [SerializeField]
    private GameObject stageSelect_CU;  //カウントアップモードUI
    [SerializeField]
    private Image cursor;               //選択時の枠
    [SerializeField]
    private Button initModeButton;      //初期モード選択ボタン
    [SerializeField]
    private Button initStageButton_CU;  //初期カウントアップの初期セレクトボタン
    [SerializeField]
    private Button initStageButton_CD;  //初期カウントダウンの初期セレクトボタン

    private GameObject selectMode;      //現在選択しているモード
    private GameObject prevSelect;      //前回選択したモード

    // Use this for initialization
    void Start()
    {
        initModeButton.Select();
        selectMode = EventSystem.current.currentSelectedGameObject;
        prevSelect = selectMode;
    }

    // Update is called once per frame
    void Update()
    {
        //前回選択したモード
        prevSelect = selectMode;
        //現在選択しているモード
        selectMode = EventSystem.current.currentSelectedGameObject;
        //モードが選択していれば
        if (selectMode != null)
        {
            switch (selectMode.name)
            {
                case "CountDown":
                    stageSelect_CD.SetActive(true);
                    stageSelect_CU.SetActive(false);
                    break;

                case "CountUp":
                    stageSelect_CU.SetActive(true);
                    stageSelect_CD.SetActive(false);
                    break;
            }

            if (selectMode.tag == "ModeSelect")
            {
                if (prevSelect != selectMode)
                {
                    Sound.PlaySE(0);
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    Sound.PlaySE(1);
                    SceneManager.LoadScene("Title");
                }
                cursor.GetComponent<RectTransform>().anchoredPosition=selectMode.GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }
    //カウントダウンを選んだ時
    public void SelectMode_CountDown()
    {
        Sound.PlaySE(2);
        initStageButton_CD.Select();        
    }
    //カウントアップを選んだ時
    public void SelectMode_CoundUp()
    {
        Sound.PlaySE(2);
        initStageButton_CU.Select();
    }
    //ステージ遷移処理
    public void SelectStage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
