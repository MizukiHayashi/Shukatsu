using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Manager : MonoBehaviour {

    private int nowItemCount;       //現在出現しているアイテム個数
    private int createItemCount;    //生成するアイテムの個数
    private int itemGetCount;       //取得したアイテムの個数
    [SerializeField]
    private GameObject[] itemPrefab;    //各アイテムオブジェクトのプレハブ
    private int randomItem;             //各アイテムから一つ選ぶランダム変数

    private GameObject[] itemInstancePos;   //アイテム生成位置配列
    private int randomPos;                  //アイテムを生成する位置を決めるランダム変数

    private Scene_Manager_ sceneManager;    //シーンマネージャー

    private int rand;       //各アイテムの出現確率

	// Use this for initialization
	void Start () {
        //各変数の初期化
        sceneManager = GameObject.Find("ScriptManager").GetComponent<Scene_Manager_>();
        itemInstancePos = GameObject.FindGameObjectsWithTag("ItemPos");
        itemGetCount = 0;
        randomPos = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //現在のアイテム数を取得
        nowItemCount = GameObject.FindGameObjectsWithTag("Item").Length;
        //取得したアイテム数に応じて生成するアイテム数を変更
        if(itemGetCount<2)　createItemCount = 1;
        else if(itemGetCount<4)　createItemCount = 2;
        else　createItemCount = 3;

        //現在のアイテム個数が生成するアイテムの個数より小さければ
        if (nowItemCount < createItemCount)
        {
            //ランダムでアイテムを生成
            rand = Random.Range(0, 1000 + 1);
            if (rand >= 600)
            {
                randomItem = Random.Range(0, 1 + 1);
            }
            else if (rand >= 960)
            {
                randomItem = Random.Range(2, 3 + 1);
            }
            else if (rand >= 1000)
            {
                randomItem = 4;
            }
            randomPos = Random.Range(0, itemInstancePos.Length-1);
            randomItem = Random.Range(0, itemPrefab.Length-1);
            Instantiate(itemPrefab[randomItem],itemInstancePos[randomPos].transform.position+new Vector3(0.0f,3.0f,0.0f),itemPrefab[randomItem].transform.rotation);
        }
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Y))
        {
            itemGetCount = 10;
        }

	}
    //アイテム取得処理
    public void ItemCountUp()
    {
        //アイテム取得数を加算
        itemGetCount += 1;
    }
}
