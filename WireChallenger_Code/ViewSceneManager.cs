using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
//ボス戦前のシーンの処理
public class ViewSceneManager : MonoBehaviour {

    public Scene_State nextScene;   //次のシーン
    [SerializeField]
    private GameObject door_R;      //右のドア
    [SerializeField]
    private GameObject door_L;      //左のドア

    private Scene_Manager_ scene_manager;
    private Scene_Manager_Fade scene_Fade;
    private bool isSceneChage;

    // Use this for initialization
    void Start () {

        scene_Fade = Camera.main.GetComponent<Scene_Manager_Fade>();
        scene_manager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        isSceneChage = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isSceneChage == false && OVRInput.Get(OVRInput.RawButton.LIndexTrigger) && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            StartCoroutine(GoBossStage());
            isSceneChage = true;
        }
    }
    //遷移開始処理
    IEnumerator GoBossStage()
    {
        yield return new WaitForSeconds(1.0f);
        //左右のドアを開く
        LeanTween.moveX(door_L, -4.4f, 1.5f);
        LeanTween.moveX(door_R, 5.0f, 1.5f);

        yield return new WaitForSeconds(1.5f);
        //ボスシーンへ
        scene_manager.SelectStage(nextScene);
        scene_Fade.LoadSceenWithFade();
    }
}
