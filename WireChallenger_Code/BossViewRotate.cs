using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossViewRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //回転させる
        transform.Rotate(new Vector3(0.0f, 45.0f, 0.0f) * Time.deltaTime, Space.World);
    }
}
