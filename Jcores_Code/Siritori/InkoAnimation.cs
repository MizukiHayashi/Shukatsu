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
            public class InkoAnimation : MonoBehaviour
            {
                private Image inko;

                [SerializeField]
                private Sprite[] inkoAnswerSprites;
                [SerializeField]
                private Sprite[] inkoCorrectSprites;
                [SerializeField]
                private Sprite[] inkoMissSprites;


                // Use this for initialization
                void Start()
                {
                    inko = gameObject.GetComponent<Image>();
                }

                public void AnswerAnimStart()
                {
                    StartCoroutine(AnswerAnim());
                }

                public void CorrectAnimStart()
                {
                    StartCoroutine(CorrectAnim());
                }

                public void MissAnimStart()
                {
                    StartCoroutine(MissAnim());
                }

                IEnumerator AnswerAnim()
                {
                    inko.sprite = inkoAnswerSprites[0];
                    yield return new WaitForSeconds(0.25f);
                    inko.sprite = inkoAnswerSprites[1];
                    yield return new WaitForSeconds(0.25f);
                    inko.sprite = inkoAnswerSprites[2];

                }

                IEnumerator CorrectAnim()
                {
                    inko.sprite = inkoCorrectSprites[0];
                    yield return new WaitForSeconds(0.25f);
                    inko.sprite = inkoCorrectSprites[1];
                    yield return new WaitForSeconds(0.25f);
                    inko.sprite = inkoCorrectSprites[2];
                }

                IEnumerator MissAnim()
                {
                    inko.sprite = inkoMissSprites[0];
                    yield return new WaitForSeconds(0.25f);
                    inko.sprite = inkoMissSprites[1];
                    yield return new WaitForSeconds(0.25f);
                    inko.sprite = inkoMissSprites[2];
                }
            }
        }
    }
}
