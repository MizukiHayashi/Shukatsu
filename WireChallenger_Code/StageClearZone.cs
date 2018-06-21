using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ステージクリア範囲に入った時の処理
public class StageClearZone : MonoBehaviour
{
    public Scene_State nextScene;   //次のシーン

    [SerializeField]
    private GameObject rotateItemGetResult; //回転させるリザルトオブジェクト
    [SerializeField]
    private GameObject itemGetResult;       //リザルトオブジェクト

    private GameObject player;  //プレイヤー

    private Scene_Manager_ scene_manager;
    private Scene_Manager_Fade scene_Fade;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        scene_manager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        scene_Fade = Camera.main.GetComponent<Scene_Manager_Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーに向ける
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
    //ポップアップ状態での遷移
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && scene_manager.GetIsPause() == false)
            {
                player.GetComponent<VR_PlayerWireAction>().IsNotPause();
                scene_manager.SelectStage(nextScene);
                scene_Fade.LoadSceenWithFade();
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
