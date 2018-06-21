using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//チュートリアル用ステージクリア範囲に入った時の処理
public class TutorialBossZone : MonoBehaviour {

    public Scene_State nextScene;

    [SerializeField]
    private GameObject rotateItemGetResult;
    [SerializeField]
    private GameObject itemGetResult;
    private GameObject player;

    private TutorialFade fade;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        fade = Camera.main.GetComponent<TutorialFade>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotate = Quaternion.LookRotation(player.transform.position - rotateItemGetResult.transform.position);
        rotateItemGetResult.transform.rotation =
            new Quaternion(
                rotateItemGetResult.transform.rotation.x,
                targetRotate.y,
                rotateItemGetResult.transform.rotation.z,
                rotateItemGetResult.transform.rotation.w);
    }

    //ポップアップ条件(表示)
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LeanTween.scale(itemGetResult, new Vector3(1.0f, 1.0f, 0.5f), 0.1f);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                fade.BossWithFadeOut(player);
            }
        }
    }

    //ポップアップ条件(閉じる)
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LeanTween.scale(itemGetResult, new Vector3(0.0f, 0.0f, 0.0f), 0.1f);
        }
    }
}
