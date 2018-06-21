using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageSelectController_Dungeon : MonoBehaviour
{
    [SerializeField]
    private GameObject modeSelect;    //ゲームモード選択UI
    [SerializeField]
    private GameObject stageSelect_D; //ダンジョンモードUI
    [SerializeField]
    private Image cursor;             //選択時の枠
    [SerializeField]
    private Button initModeButton;    //初期モード選択ボタン
    [SerializeField]
    private Button initStageButton_D; //初期ステ―ジボタン

    private GameObject select;      //現在選択しているボタン
    private GameObject prevSelect;  //前回選択したボタン

    // Use this for initialization
    void Start()
    {
        initModeButton.Select();
        select = EventSystem.current.currentSelectedGameObject;
        prevSelect = select;
    }

    // Update is called once per frame
    void Update()
    {
        //前回のボタンを設定
        prevSelect = select;
        //現在のボタンを設定
        select = EventSystem.current.currentSelectedGameObject;
        //ボタンが選ばれていたら
        if (select != null)
        {
            if (select.name == "Dungeon")
            {
                stageSelect_D.SetActive(true);
            }

            if (select.tag == "ModeSelect")
            {
                if (prevSelect != select)
                {
                    Sound.PlaySE(0);
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    Sound.PlaySE(1);
                    SceneManager.LoadScene("Title");
                }
                cursor.GetComponent<RectTransform>().anchoredPosition=select.GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }


    //ダンジョンを選んだ時
    public void SelectMode_Dungeon()
    {
        Sound.PlaySE(2);
        initStageButton_D.Select();
    }

    public void SelectStage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
