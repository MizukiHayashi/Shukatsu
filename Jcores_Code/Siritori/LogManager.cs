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
            public class LogManager : MonoBehaviour
            {
                [SerializeField]
                private GameObject inkoLogObject;
                [SerializeField]
                private GameObject oumuLogObject;

                private string inkoLogs = "";
                private string inkoOldLogs = "";
                private string oumuLogs = "";
                private string oumuOldLogs = "";
                [SerializeField]
                private ScrollRect inko_scrollRect;
                [SerializeField]
                private Text inko_textLog;
                [SerializeField]
                private ScrollRect oumu_scrollRect;
                [SerializeField]
                private Text oumu_textLog;
                //ログにインコの言葉を格納
                public void InkoSetLog(string logText)
                {
                    inkoLogs += (logText + "\n\n");
                    inko_textLog.text = inkoLogs;
                    inko_scrollRect.verticalNormalizedPosition = 0.0f;
                }
                //ログにオウムの言葉を格納
                public void OumuSetLog(string logText)
                {
                    oumuLogs += (logText + "\n\n");
                    oumu_textLog.text = oumuLogs;
                    oumu_scrollRect.verticalNormalizedPosition = 0.0f;
                }
                //インコがクリックされた時の処理
                public void OnClickInko()
                {
                    if (inkoLogObject.activeSelf == false)
                    {
                        if (oumuLogObject.activeSelf == true) oumuLogObject.SetActive(false);
          
                        inkoLogObject.SetActive(true);
                    }
                    else inkoLogObject.SetActive(false);
                }
                //オウムがクリックされた時の処理
                public void OnClickOumu()
                {
                    if (oumuLogObject.activeSelf == false)
                    {
                        if (inkoLogObject.activeSelf == true) inkoLogObject.SetActive(false);

                        oumuLogObject.SetActive(true);
                    }
                    else oumuLogObject.SetActive(false);
                }
            }
        }
    }
}
