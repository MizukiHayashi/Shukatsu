using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Jcores
{
    namespace ExecutiveFunction
    {
        namespace ChangeConcept
        {
            public class ChangeConceptResultManager : MonoBehaviour
            {

                [SerializeField]
                private Image back;             //背景イメージ
                [SerializeField]
                private Sprite[] e_backSprite;    //背景イメージのスプライト(Normal)
                [SerializeField]
                private Sprite[] n_backSprite;    //背景イメージのスプライト(Normal)
                [SerializeField]
                private Sprite[] h_backSprite;    //背景イメージスプライト(Hard)
                [SerializeField]
                private Sprite[] vh_backSprite;    //背景イメージスプライト(VaryHard)

                [SerializeField]
                private Text resultText_1;      //リザルト１
                [SerializeField]
                private Text resultText_2;      //リザルト２

                // Use this for initialization
                void Start()
                {
                    //難易度を取得
                    Settings.Instance.SetSettings();
                    //特定の難易度で正解率によって背景を変える
                    switch (Settings.Instance.DifficultyInt)
                    {
                        case 1: //色と文字の課題(その１)
                            back.sprite = e_backSprite[0];
                            if (Settings.Instance.result_correctNum >= 15) back.sprite = e_backSprite[6];
                            else if (Settings.Instance.result_correctNum >= 12) back.sprite = e_backSprite[5];
                            else if (Settings.Instance.result_correctNum >= 9) back.sprite = e_backSprite[4];
                            else if (Settings.Instance.result_correctNum >= 7) back.sprite = e_backSprite[3];
                            else if (Settings.Instance.result_correctNum >= 4) back.sprite = e_backSprite[2];
                            else if (Settings.Instance.result_correctNum >= 1) back.sprite = e_backSprite[1];
                            break;
                        case 2: //色と文字の課題(その2)
                            back.sprite = n_backSprite[0];
                            if (Settings.Instance.result_correctNum >= 75) back.sprite = n_backSprite[6];
                            else if (Settings.Instance.result_correctNum >= 60) back.sprite = n_backSprite[5];
                            else if (Settings.Instance.result_correctNum >= 45) back.sprite = n_backSprite[4];
                            else if (Settings.Instance.result_correctNum >= 30) back.sprite = n_backSprite[3];
                            else if (Settings.Instance.result_correctNum >= 15) back.sprite = n_backSprite[2];
                            else if (Settings.Instance.result_correctNum >= 5) back.sprite = n_backSprite[1];
                            break;
                        case 3: //位置と文字の課題
                            back.sprite = h_backSprite[0];
                            if (Settings.Instance.result_correctNum >= 15) back.sprite = h_backSprite[5];
                            else if (Settings.Instance.result_correctNum >= 10) back.sprite = h_backSprite[4];
                            else if (Settings.Instance.result_correctNum >= 7) back.sprite = h_backSprite[3];
                            else if (Settings.Instance.result_correctNum >= 4) back.sprite = h_backSprite[2];
                            else if (Settings.Instance.result_correctNum >= 1) back.sprite = h_backSprite[1];
                            break;
                        case 4: //数字と文字の課題
                            back.sprite = vh_backSprite[0];
                            if (Settings.Instance.result_correctNum >= 15) back.sprite = vh_backSprite[6];
                            else if (Settings.Instance.result_correctNum >= 12) back.sprite = vh_backSprite[5];
                            else if (Settings.Instance.result_correctNum >= 9) back.sprite = vh_backSprite[4];
                            else if (Settings.Instance.result_correctNum >= 7) back.sprite = vh_backSprite[3];
                            else if (Settings.Instance.result_correctNum >= 4) back.sprite = vh_backSprite[2];
                            else if (Settings.Instance.result_correctNum >= 1) back.sprite = vh_backSprite[1];
                            break;
                    }
                    //結果を表示
                    resultText_1.text = "正解率:  " + Settings.Instance.result_correctAvg + "% （"+Settings.Instance.result_correctNum+"／"+Settings.Instance.CurrentSettings.QuestionNum+"）";
                    resultText_2.text = "平均解答時間:  " + Settings.Instance.result_answerAvg + "秒";

                }
            }
        }
    }
}
