using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScene : MonoBehaviour
{
    private Scene_Manager_Fade sceneFade;

    private bool isChange;

    // Use this for initialization
    void Start()
    {
        sceneFade = Camera.main.GetComponent<Scene_Manager_Fade>();

        isChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        //特定のボタンでシーン遷移
        if (isChange == false && (Input.GetKeyDown(KeyCode.Space)
            || OVRInput.GetDown(OVRInput.RawButton.A) || OVRInput.GetDown(OVRInput.RawButton.B)
            || OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.Y)
            || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)
            || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.LHandTrigger)))
        {
            sceneFade.LoadSceenWithFade();
            isChange = true;
        }
    }
}
