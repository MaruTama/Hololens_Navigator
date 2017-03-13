using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 距離によって音の生成速度を変える
public class soundInstance : MonoBehaviour {
	public AudioClip ac;
	public GameObject instance;
	[Range(0.05f,1)]public float normalize=1;
	float length;
	float c;
	// Use this for initialization
	void Start () {
		length = ac.length;
		c = length*normalize+1;
	}
	// Update is called once per frame
	void Update () {
		normalize = normalize < 0.05f ? 0.05f : normalize;
		if (c > length*normalize) {
			c = 0;
			GameObject obj=Instantiate (instance, transform.position, Quaternion.identity)as GameObject;
			obj.GetComponent<AudioSource> ().clip = ac;
		}
		c += Time.deltaTime;
	}
}
