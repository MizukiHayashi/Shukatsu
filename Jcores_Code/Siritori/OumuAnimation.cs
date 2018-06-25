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
            public class OumuAnimation : MonoBehaviour
            {
                private Image oumu;

                [SerializeField]
                private Sprite[] oumuAnswerSprites;
                [SerializeField]
                private Sprite[] oumuCorrectSprites;

                // Use this for initialization
                void Start()
                {
                    oumu = gameObject.GetComponent<Image>();
                }

                public void AnswerAnimStart()
                {
                    StartCoroutine(AnswerAnim());
                }

                public void CorrectAnimStart()
                {
                    StartCoroutine(CorrectAnim());
                }

                IEnumerator AnswerAnim()
                {
                    oumu.sprite = oumuAnswerSprites[0];
                    yield return new WaitForSeconds(0.25f);
                    oumu.sprite = oumuAnswerSprites[1];
                    yield return new WaitForSeconds(0.25f);
                    oumu.sprite = oumuAnswerSprites[2];
                }

                IEnumerator CorrectAnim()
                {
                    oumu.sprite = oumuCorrectSprites[0];
                    yield return new WaitForSeconds(0.25f);
                    oumu.sprite = oumuCorrectSprites[1];
                    yield return new WaitForSeconds(0.25f);
                    oumu.sprite = oumuCorrectSprites[2];
                }

            }
        }
    }
}
