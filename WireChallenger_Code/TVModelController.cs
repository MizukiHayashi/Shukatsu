using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ステージ選択シーンのテレビの処理
public class TVModelController : MonoBehaviour
{
    [SerializeField]
    private GameObject TVs_Stage;       //ステージ選択のテレビの親オブジェクト
    [SerializeField]
    private GameObject TVs_Tutorial;    //チュートリアル選択のテレビの親オブジェクト
    [SerializeField]
    private GameObject TV1Joint;        //TV1のジョイント
    [SerializeField]
    private GameObject TV2Joint;        //TV2のジョイント
    [SerializeField]
    private GameObject TV3Joint;        //TV3のジョイント
    [SerializeField]
    private GameObject TVTutoYJoin;     //TutorialYesのジョイント
    [SerializeField]
    private GameObject TVTutoNJoin;     //TutorialNoのジョイント

    [SerializeField]
    private GameObject Door_L;  //左のドア
    [SerializeField]
    private GameObject Door_R;  //右のドア

    [SerializeField]
    private GameObject Door_Center_L;   //真ん中の左ドア
    [SerializeField]
    private GameObject Door_Center_R;   //真ん中の右ドア

    private Scene_Manager_Fade scene_Fade;
    private bool isComplete_Stage;  //ステージのゲージが溜まったか
    private bool isComplete_TutoY;  //チュートリアルYESのゲージが溜まったか
    private bool isComplete_TutoN;  //チュートリアルNOのゲージが溜まったか
    // Use this for initialization
    void Start()
    {
        scene_Fade = Camera.main.GetComponent<Scene_Manager_Fade>();
        isComplete_Stage = false;
        isComplete_TutoY = false;
        isComplete_TutoN = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isComplete_Stage == true)
        {
            StartCoroutine(TVAnimation_Stage());
            isComplete_Stage = false;
        }

        if (isComplete_TutoY == true)
        {
            StartCoroutine(TVAnimation_TutoY());
            isComplete_TutoY = false;
        }

        if (isComplete_TutoN == true)
        {
            StartCoroutine(TVAnimation_TutoN());
            isComplete_TutoN = false;
        }
    }

    public void OnCompleteGauge_Stage()
    {
        isComplete_Stage = true;
    }

    public void OnCompleteGauge_TutoY()
    {
        isComplete_TutoY = true;
    }

    public void OnCompleteGauge_TutoN()
    {
        isComplete_TutoN = true;
    }
    //ステージ選択TVのアニメーション処理
    IEnumerator TVAnimation_Stage()
    {
        //3つのTVジョイントを90度回転
        LeanTween.rotateX(TV1Joint, 90, 1.0f);
        LeanTween.rotateX(TV2Joint, 90, 1.0f);
        LeanTween.rotateX(TV3Joint, 90, 1.0f);

        yield return new WaitForSeconds(1.5f);
        //ステージ選択のテレビの親オブジェクトの位置を下に下げて見えない位置に
        LeanTween.moveLocalY(TVs_Stage, TVs_Stage.transform.position.y - 5.0f, 1.0f);

        yield return new WaitForSeconds(1.5f);
        //奥の左右のドアを開ける
        LeanTween.moveX(Door_L, -4.45f, 1.5f);
        LeanTween.moveX(Door_R, 5.0f, 1.5f);

        yield return new WaitForSeconds(1.5f);

        //シーン遷移開始
        scene_Fade.LoadSceenWithFade();
    }
    //チュートリアルYesを選んだ時のアニメーション処理
    IEnumerator TVAnimation_TutoY()
    {
        //全てのTVのジョイントを90度回転
        LeanTween.rotateX(TV1Joint, 90, 1.0f);
        LeanTween.rotateX(TV2Joint, 90, 1.0f);
        LeanTween.rotateX(TV3Joint, 90, 1.0f);
        LeanTween.rotateX(TVTutoYJoin, 90, 1.0f);
        LeanTween.rotateX(TVTutoNJoin, 90, 1.0f);

        yield return new WaitForSeconds(1.5f);
        //位置を下に下げて見えないように
        LeanTween.moveLocalY(TVs_Tutorial, TVs_Tutorial.transform.position.y - 5.0f, 1.0f);
        LeanTween.moveLocalY(TVs_Stage, TVs_Stage.transform.position.y - 5.0f, 1.0f);

        yield return new WaitForSeconds(1.5f);
        //真ん中の左右のドアを開ける
        LeanTween.moveX(Door_Center_L, -4.45f, 1.5f);
        LeanTween.moveX(Door_Center_R, 5.0f, 1.5f);

        yield return new WaitForSeconds(1.0f);
        //奥の左右のドアを開ける
        LeanTween.moveX(Door_L, -4.45f, 1.5f);
        LeanTween.moveX(Door_R, 5.0f, 1.5f);

        yield return new WaitForSeconds(1.5f);

        //シーン遷移開始
        scene_Fade.LoadSceenWithFade();
    }
    //チュートリアルNoを選んだ時のアニメーション処理
    IEnumerator TVAnimation_TutoN()
    {
        //チュートリアル選択TVのジョイントを90度回転
        LeanTween.rotateX(TVTutoYJoin, 90, 1.0f);
        LeanTween.rotateX(TVTutoNJoin, 90, 1.0f);

        yield return new WaitForSeconds(1.5f);
        //チュートリアルのTVの位置を下げて見えないように
        LeanTween.moveLocalY(TVs_Tutorial, TVs_Tutorial.transform.position.y - 5.0f, 1.0f);

        yield return new WaitForSeconds(1.5f);
        //真ん中の左右のドアを開く
        LeanTween.moveX(Door_Center_L, -4.45f, 1.5f);
        LeanTween.moveX(Door_Center_R, 5.0f, 1.5f);

    }
}
