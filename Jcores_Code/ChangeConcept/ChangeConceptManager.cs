using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jcores
{
    namespace ExecutiveFunction
    {
        namespace ChangeConcept
        {
            public class ChangeConceptManager : MonoBehaviour
            {

                private int difficalty;

                [SerializeField]
                private GameObject ve_objects;  //練習で使用するオブジェクト集
                [SerializeField]
                private GameObject e_objects;   //色と文字の課題(その１)で使用するオブジェクト集
                [SerializeField]
                private GameObject n_objects;   //色と文字の課題(その２)で使用するオブジェクト集
                [SerializeField]
                private GameObject h_objects;   //位置と文字の課題で使用するオブジェクト集
                [SerializeField]
                private GameObject vh_objects;  //数字と文字の課題で使用するオブジェクト集

                // Use this for initialization
                void Start()
                {
                    //難易度の取得
                    Settings.Instance.SetSettings();
                    //難易度ごとに起動するオブジェクトとスクリプトを指定
                    switch (Settings.Instance.DifficultyInt)
                    {
                        case 0:
                            ve_objects.SetActive(true);
                            gameObject.GetComponent<VaryEasy>().enabled = true;
                            break;
                        case 1:
                            e_objects.SetActive(true);
                            gameObject.GetComponent<Easy>().enabled = true;
                            break;
                        case 2:
                            n_objects.SetActive(true);
                            gameObject.GetComponent<Normal>().enabled = true;
                            break;
                        case 3:
                            h_objects.SetActive(true);
                            gameObject.GetComponent<Hard>().enabled = true;
                            break;
                        case 4:
                            vh_objects.SetActive(true);
                            gameObject.GetComponent<VaryHard>().enabled = true;
                            break;
                    }
                }

                // Update is called once per frame
                void Update()
                {

                }
            }
        }
    }
}

