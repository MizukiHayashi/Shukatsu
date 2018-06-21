using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTexScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //アイテムのテキストを回す
        transform.Rotate(new Vector3(0.0f, 45.0f, 0.0f) * Time.deltaTime, Space.World);
	}
}
