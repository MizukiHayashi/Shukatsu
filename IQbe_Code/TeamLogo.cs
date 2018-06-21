using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamLogo : MonoBehaviour
{
    [SerializeField]
    private Image logoImage;
    private float alpha;
    private float time;

    // Use this for initialization
    void Start()
    {
        alpha = 0.0f;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        logoImage.color = new Color(1, 1, 1, alpha);
        if (time < 60 * 1.5f)
            alpha += 0.02f;
        else if (time >= 60 * 1.5f)
            alpha -= 0.02f;

        if (time >= 60 * 1.5f && alpha <= 0)
            SceneManager.LoadScene("Title");
    }
}
