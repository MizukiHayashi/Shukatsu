using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jcores
{
    namespace ProcessingSpeed
    {
        namespace WhackAMole
        {
            public class MoleResultManager : MonoBehaviour
            {
                [SerializeField]
                private Text resultText_1;
                [SerializeField]
                private Text resultText_2;
                [SerializeField]
                private Text resultText_3;
                [SerializeField]
                private Text resultText_4;

                // Use this for initialization
                void Start()
                {
                    Settings.Instance.SetSettings();
                    resultText_1.text = "正解率:  " + Settings.Instance.result_correctAvg + "％";
                    resultText_2.text = "叩いた時の正解率:  " + Settings.Instance.result_moleHitCorrectAvg + "％";
                    resultText_3.text = "平均反応時間:  " + Settings.Instance.result_reactionAvgTime+"秒";
                    resultText_4.text = "見逃し数:  " + Settings.Instance.result_missCount;
                }

                // Update is called once per frame
                void Update()
                {

                }
            }
        }
    }
}
