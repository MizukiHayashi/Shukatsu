using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jcores
{
    namespace Attention
    {
        namespace React
        {
            public class ReactResultManager : MonoBehaviour
            {
                [SerializeField]
                private Text displayResult1;
                [SerializeField]
                private Text displayResult2;
                [SerializeField]
                private Text displayResult3;
                // Use this for initialization
                void Start()
                {
                    displayResult1.text = "不正解数: " + PlayerPrefs.GetInt("batuCount").ToString();
                    displayResult2.text = "見逃し数: " + PlayerPrefs.GetInt("missCount").ToString();
                    displayResult3.text = "平均反応時間 前半:" + PlayerPrefs.GetFloat("avgTime_first").ToString("f2") + 
                                          " 後半:" + PlayerPrefs.GetFloat("avgTime_latter").ToString("f2") +
                                          " 全体:" + PlayerPrefs.GetFloat("avgTime_all").ToString("f2");
                }

                // Update is called once per frame
                void Update()
                {

                }
            }
        }
    }
}
