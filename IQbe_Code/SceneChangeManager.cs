using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneChangeManager : MonoBehaviour
{
    [SerializeField]
    private Button initSelectButtun;    //初期ボタン
    [SerializeField]
    private GameObject countUI;         //カウントモードUI
    [SerializeField]
    private GameObject dungeonUI;       //ダンジョンモードUI
    [SerializeField]
    private GameObject modeSelectUI;    //モードセレクトUI
    [SerializeField]
    private Image cursor;               //選択時の枠
    [SerializeField]
    private Button initButton_C;        //カウントモード選択時の次の初期ボタン
    [SerializeField]
    private Button initButton_D;        //ダンジョンモード選択時の次の初期ボタン

    private GameObject prevSelect;      //前回選択されたボタン
    private GameObject select;          //現在選ばれているボタン

    // Use this for initialization
    void Start()
    {
        initSelectButtun.Select();
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
            //ボタンのタグによってそれぞれ処理
            switch (select.tag)
            {
                case "ModeSelect":
                    if (select != prevSelect)
                    {
                        Sound.PlaySE(0);
                    }
                    cursor.GetComponent<RectTransform>().anchoredPosition = select.GetComponent<RectTransform>().anchoredPosition;
                    break;

                case "CountMode":
                    if (select != prevSelect)
                    {
                        Sound.PlaySE(0);
                    }
                    if (Input.GetButtonDown("Cancel"))
                    {
                        Sound.PlaySE(1);
                        SelectMode();
                    }
                    cursor.GetComponent<RectTransform>().anchoredPosition = select.GetComponent<RectTransform>().anchoredPosition;
                    break;

                case "DungeonMode":
                    if (select != prevSelect)
                    {
                        Sound.PlaySE(0);
                    }
                    if (Input.GetButtonDown("Cancel"))
                    {

                        SelectMode();
                    }
                    cursor.GetComponent<RectTransform>().anchoredPosition = select.GetComponent<RectTransform>().anchoredPosition;
                    break;
            }
        }
    }
    //シーン切替処理
    public void SceneChange(string sceneName)
    {
        Sound.PlaySE(3);
        Sound.StopBGM();
        SceneManager.LoadScene(sceneName);
    }
    //モードセレクト処理
    public void SelectMode()
    {
        Sound.PlaySE(1);
        countUI.SetActive(false);
        dungeonUI.SetActive(false);
        modeSelectUI.SetActive(true);
        initSelectButtun.Select();
    }
    //CountMode選択時処理
    public void SelectMode_Count()
    {
        Sound.PlaySE(2);
        modeSelectUI.SetActive(false);
        countUI.SetActive(true);
        initButton_C.Select();
    }
    //DungeonMode選択時処理
    public void SelectMode_Dungeon()
    {
        Sound.PlaySE(2);
        modeSelectUI.SetActive(false);
        dungeonUI.SetActive(true);
        initButton_D.Select();
    }
    //Exit選択時処理
    public void GameExit()
    {
        Application.Quit();
    }

}
