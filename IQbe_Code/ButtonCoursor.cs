using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonCoursor : MonoBehaviour {

    private RectTransform m_RectTransform; //自分の位置

    // Use this for initialization
    void Start () {
        //自分の位置を取得
        m_RectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        //現在選択されたいるボタンオブジェクトを取得
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        //オブジェクトがなかったら
        if (selectedObject == null)
        {
            return;
        }
        //ボタンの枠を選択されているボタンの位置へ
        m_RectTransform.anchoredPosition =
            selectedObject.GetComponent<RectTransform>().anchoredPosition;

    }
}
