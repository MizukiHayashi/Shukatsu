using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField]
    private float timeLimit_Up; //上に上がり始めるまでの時間
    [SerializeField]
    private float timeLimit_Down; //下に下がる始めるまでの時間
    [SerializeField]
    private float tweenTime; //Tweenまでの時間
    [SerializeField]
    private GameObject moveBlock; //動かすブロック
    [SerializeField]
    private GameObject seterPoint; //ブロックのセット位置
    [SerializeField]
    private GameObject moveWave; //動く時のエフェクト
    [SerializeField]
    private Material moveBlockWhite; //障害物時のマテリアル
    [SerializeField]
    private Material moveBlockBlack; //床時のマテリアル
    [SerializeField]
    private Texture emissionMap_Up; //障害物時のテクスチャ
    [SerializeField]
    private Texture emissionMap_Down; //床時のテクスチャ

    private float countTime = 0; //時間カウント
    [SerializeField]
    private bool isUp; //上がるフラグ
    private bool isDown; //下がるフラグ

    private Vector3 defaultPos; //デフォルト位置

    private Renderer mRenderer;

    public bool isMove = false; //動き始めているか
    public bool init_Up;

    // Use this for initialization
    void Start()
    {
        if (DungionCsv.dungion == false)
            transform.position = new Vector3(transform.position.x, Csv.getStartposY, transform.position.z);
        else if (DungionCsv.dungion == true)
            transform.position = new Vector3(transform.position.x, DungionCsv.getStartposY, transform.position.z);

        defaultPos = seterPoint.transform.position; //デフォルト位置の取得
        mRenderer = moveBlock.GetComponent<Renderer>(); //レンダラーの取得
    }

    // Update is called once per frame
    void Update()
    {
        //どちらかのプレイヤーキューブが乗っていたらカウントしない
        if ((Player1Move.onMoveBlock == true) || (Player2Move.onMoveBlock == true))
        {
            this.countTime = 0;
        }
        //乗っていなければ加算
        else
        {
            this.countTime += Time.deltaTime;
        }
        //ブロックの位置が上の位置にいなければ
        if (moveBlock.transform.position.y >= defaultPos.y + 1.0f)
        {
            isUp = true; //上がるフラグをtrue
            init_Up = true;
        }
        //ブロックの位置が下の位置にいなければ
        else if (moveBlock.transform.position.y <= defaultPos.y)
        {
            isDown = true; //下がるフラグをtrue
            init_Up = false;
        }
        //上がっていなければ
        if (isUp == false)
        {
            //動きだす0.5秒前には
            if (timeLimit_Up - countTime <= 0.5f)
            {
                //動く判定をtureに
                isMove = true;
            }
            //時間カウントが上に上がり始める時間になったら
            if (timeLimit_Up <= countTime)
            {
                //emissionを操作
                mRenderer.material.EnableKeyword("_EMMISION");
                mRenderer.material.SetTexture("_EmissionMap", emissionMap_Up);
                mRenderer.material.SetColor("_Color", moveBlockBlack.color);
                mRenderer.material.SetColor("_EmissionColor", moveBlockBlack.GetColor("_EmissionColor"));
                //位置を上の位置へ
                LeanTween.moveY(moveBlock, moveBlock.transform.position.y + 1.0f, tweenTime).setOnComplete(CountReset);
                //動くときのエフェクトを生成
                Instantiate(moveWave, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                isUp = true;
            }
        }
        //上がっており、下がっていなければ
        else if (isDown == false)
        {
            //時間カウントが下に下がり始める時間になったら
            if (timeLimit_Down <= countTime)
            {
                //位置を下の位置へ
                LeanTween.moveY(moveBlock, moveBlock.transform.position.y - 1.0f, tweenTime).setOnComplete(DownCountReset);
                //動くときのエフェクトを生成
                Instantiate(moveWave, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                isDown = true;
            }
        }
    }
    //時間カウントのリセット
    public void CountReset()
    {
        countTime = 0;
        isDown = false;
    }
    //下がったときの時間リセット
    public void DownCountReset()
    {
        mRenderer.material.EnableKeyword("_EMMISION");
        mRenderer.material.SetTexture("_EmissionMap", emissionMap_Down);
        mRenderer.material.SetColor("_Color", moveBlockWhite.color);
        mRenderer.material.SetColor("_EmissionColor", moveBlockWhite.GetColor("_EmissionColor"));
        this.countTime = 0;
        isMove = false;
        isUp = false;
    }
}
