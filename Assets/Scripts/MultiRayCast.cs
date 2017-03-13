using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.VR.WSA.Input;

public class MultiRayCast : MonoBehaviour {


	GestureRecognizer gr;
	public LayerMask lm;
	bool holdflg=false;
	[Range(1,100)]public int raycount=10;
	[Range(1,100)]public float depth=5;
	[Range(1,100)]public float width=10;
	// Update is called once per frame
	void Start(){

	}
	void Update () {

		// レイを打って、壁のポイントを取得する
		float hw = width / 2;
		float inte = width / (float)raycount;
		List<Vector3> vects = new List<Vector3> ();
		for (float i = -hw; i <= hw; i += inte) {
			Vector3 vect = new Vector3 (i, 0, depth);
			RaycastHit rh;

			if (Physics.Raycast (transform.position, (transform.rotation * vect).normalized, out rh,100,lm.value,QueryTriggerInteraction.Ignore)) {
				Debug.DrawLine (transform.position, rh.point, Color.blue);
				vects.Add (rh.point);
			} else {
				Debug.DrawRay (transform.position, (transform.rotation * vect).normalized * 10, Color.blue);
				//vects.Add (new Vector3 (100, 100, 100));
			}
		}


		// ここから、取ってきたポイントから回帰直線を計算する
		float[] rtn = Point2AVGLine.point2line (vects.ToArray ());

		List<Vector3> argvect = new List<Vector3> ();

		for (int i = 0; i < vects.Count; i++) {
			if (vects [i].x != 100) {
				argvect.Add (new Vector3 (vects [i].x,
					transform.position.y,
					(vects [i].x * rtn [0]) + rtn [1]));
			}

		}
		// 回帰直線を表示する
		AVGPointLine.SetPoint (argvect.ToArray());
	}
}
