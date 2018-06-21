using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//クリア時メニュー選択処理
public class GameClearMenu : MonoBehaviour
{
    [SerializeField]
    private Button initSelectButton;    //初期選択ボタン
    [SerializeField]
    private Image cursor;               //選択中のボタンの枠

    private GameObject selectButton;        //選択中のボタン
    private GameObject prevSelectButton;    //前回のボタン

    // Use this for initialization
    void Start()
    {
        //各変数の初期化
        initSelectButton.Select();
        selectButton = EventSystem.current.currentSelectedGameObject;
        prevSelectButton = selectButton;
    }

    // Update is called once per frame
    void Update()
    {
        //前回のボタンを更新
        prevSelectButton = selectButton;
        //選択中ボタンを更新
        selectButton = EventSystem.current.currentSelectedGameObject;
        //ボタンが選択されていたら
        if (selectButton != null)
        {
            if (prevSelectButton != selectButton)
            {
                Sound.PlaySE(1);
            }
            //ボタン枠の位置を修正
            cursor.GetComponent<RectTransform>().anchoredPosition = selectButton.GetComponent<RectTransform>().anchoredPosition;
        }
    }
    //シーン遷移処理
    public void SceneChange(string sceneName)
    {
        Sound.PlaySE(0);
        Sound.StopBGM();
        SceneManager.LoadScene(sceneName);
    }
}
