using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]

// 回帰直線を表示する
public class AVGPointLine : MonoBehaviour {
	public Shader SelectShader;
	public static LineRenderer lrComp {
		private set;
		get;

	}
	// startよりも先に呼び出される
	void Awake(){
		// オブジェクト上のLineRendererを取得する
		lrComp = GetComponent<LineRenderer> ();

		Material mat = lrComp.material;
		mat.shader = SelectShader;
		//mat.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
		mat.color = Color.red;

	}
	public static void SetPoint(Vector3[] vect){
		if (vect.Length != 0) {
			// 配列の長さを先に渡しておく。
			lrComp.numPositions = vect.Length;
			// 座標を指定して、それらを線で結ぶ。
			lrComp.SetPositions (vect);
		}
	}
}
