using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jcores
{
    namespace Fluency
    {
        namespace Shiritori
        {
            public class ShiritoriResultManager : MonoBehaviour
            {
                [SerializeField]
                private Text resultTex1;
                [SerializeField]
                private Text resultTex2;

                // Use this for initialization
                void Start()
                {
                    Settings.Instance.SetSettings();
                    resultTex1.text = "正解率:  " + Settings.Instance.result_correctAvg + "% (" + Settings.Instance.result_correctCount + "/" + Settings.Instance.result_questionAllCount + ")";
                    resultTex2.text = "経過時間:  " + Settings.Instance.result_elapsedTime;
                }

                // Update is called once per frame
                void Update()
                {

                }
            }
        }
    }
}
