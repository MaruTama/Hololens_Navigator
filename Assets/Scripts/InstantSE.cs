using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 効果音の再生機能の読み込み
public class InstantSE : MonoBehaviour {
	AudioSource aus;
	float length;
	// Use this for initialization
	void Start () {
		aus = GetComponent<AudioSource> ();
		length = aus.clip.length;
		aus.Play ();
	}
	void Update(){
		if (length < 0) {
			Destroy (this.gameObject);
		}
		length -= Time.deltaTime;

	}

}
