using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleImageRotation : MonoBehaviour
{
    [SerializeField]
    private Image circle_UIa1; //内側から1つめの円
    [SerializeField]
    private Image circle_UIa3; //内側から3つめの円
    [SerializeField]
    private Image circle_UIa5; //内側から5つめの円
    [SerializeField]
    private Image circle_UIa7; //内側から7つめの円

    float UIa7timer; //7つめの円を回転させるタイミング

    // Use this for initialization
    void Start()
    {
        UIa7timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //1つめの円を回転させる
        circle_UIa1.rectTransform.Rotate(new Vector3(0, 0, 1));

        UIa7timer += 1 * Time.deltaTime;

        circle_UIa3.rectTransform.Rotate(new Vector3(0, 0, -2));

        //5つめの円を回転させる
        circle_UIa5.rectTransform.Rotate(new Vector3(0, 0, 2));

        //1秒ごとに7つめの円を回転させる
        if (UIa7timer >= 1.0f)
        {
            circle_UIa7.rectTransform.Rotate(new Vector3(0, 0, -10));
            UIa7timer = 0;
        }
    }
}
