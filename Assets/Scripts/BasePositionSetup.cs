using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Input;
using UnityEngine.VR.WSA.Persistence;

// ワールドアンカーを探索して、あったら、そのポジションに3Dオブジェクトを配置する
public class BasePositionSetup : MonoBehaviour {
	GestureRecognizer gr;
	public bool _debug;


	public string ID;
	WorldAnchorStore anchorStore;



	void Start(){
		// ジェスチャー(主にタップ)のコンストラクト
		gr = new GestureRecognizer ();
		// 関数をTappedEventに登録
		gr.TappedEvent += SetUp;
		gr.StartCapturingGestures ();

		// ワールドアンカーを取得する
		WorldAnchorStore.GetAsync (AnchorStoreReady);
	}

	// ワールドアンカーの座標を取得する
	void AnchorStoreReady(WorldAnchorStore store){
		anchorStore = store;
		string[] ids = anchorStore.GetAllIds ();
		for (int i = 0; i < ids.Length; i++) {
			if (ids [i].Equals (ID)) {
				anchorStore.Load (ids [i], gameObject);
				gameObject.transform.GetChild(0).gameObject.SetActive (true);
				break;
			}
		}
	}

	void Update(){
		if (_debug) {
			_debug = false;
			SetUp ((InteractionSourceKind)0, 0, new Ray (Camera.main.transform.position, Camera.main.transform.forward));
		}
	}
	void SetUp(InteractionSourceKind isk,int tCount,Ray headRay){

		// 回帰直線の始点、終点を取得し、その方向ベクトルにモデルの壁を揃える
		gameObject.transform.GetChild(0).gameObject.SetActive (true);
		Vector3[] v3 = new Vector3[AVGPointLine.lrComp.numPositions];
		AVGPointLine.lrComp.GetPositions(v3);
		Vector3 dir = (v3 [v3.Length - 1] - v3 [0]).normalized;
		transform.rotation = Quaternion.LookRotation (dir, Vector3.up);

		// 使用者の目線の高さに揃えるために、真下にRayを打ち壁のモデルの高さを揃える
		RaycastHit rh;
		if(Physics.Raycast(headRay,out rh,100)){
			transform.position = rh.point;
			if (Physics.Raycast (headRay.origin, Vector3.down, out rh, 100)) {
				transform.position = new Vector3 (transform.position.x,
					rh.point.y,
					transform.position.z);
			}

		}

		// タップした音を鳴らす。
		AudioSource aus=GetComponent<AudioSource>();
		if (aus) {
			aus.Play ();
		}
		// ワールドアンカーのストアから、毎回IDを削除して再登録を行う。
		// IDが重複するとまずいため。
		WorldAnchor wa = gameObject.GetComponent<WorldAnchor> ();
		if (wa != null)
			DestroyImmediate (wa);

		string[] ids = anchorStore.GetAllIds ();
		int num = Array.FindIndex<string> (ids, x => x.Equals (ID));
		if(num!=-1)
			anchorStore.Delete (ids [num]);

		// 壁との整合が済んでいた場合にはオブジェクトにあるワールドアンカーの座標とIDを保存する
		WorldAnchor anchor=gameObject.AddComponent<WorldAnchor> ();
		if (anchor.isLocated) {
			anchorStore.Save (ID, anchor);
		} else {
			anchor.OnTrackingChanged += AOchanged;
		}



	}
	// ルームトラッキングがずれたら、再登録してイベントから削除
	void AOchanged(WorldAnchor self,bool located){
		if (located) {
			anchorStore.Save (ID, self);
			self.OnTrackingChanged -= AOchanged;

		}
	}



}
