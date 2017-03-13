using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 壁の接近によってソナー音を鳴らす
public class ProximitySensor : MonoBehaviour {
	public LayerMask lm;
	public float distance;
	public GameObject se;
	public int DirectionCount;

	Vector3 bpos;
	// Use this for initialization
	void Start () {
		se = Instantiate (se, transform.position, Quaternion.identity);
		se.SetActive (false);
	}

	// Update is called once per frame
	void FixedUpdate () {
		// DirectionCount本分、周囲にレイを打ち壁に近づいたときに警告(ソナー)音を鳴らす。
		float velocity = ((transform.position - bpos) / Time.fixedDeltaTime).magnitude;
		List<Vector3> hitvect = new List<Vector3> ();
		Vector3 sumvect=Vector3.zero;
		RaycastHit rh;
		for (int i = 0; i < DirectionCount; i++) {
			Quaternion euler = Quaternion.Euler (0,(360/DirectionCount)*i,0);
			Vector3 dirvect = euler * Vector3.forward;

			// 衝突したときの位置を保持する
			if (Physics.Raycast (transform.position, dirvect,out rh,distance, lm.value, QueryTriggerInteraction.Ignore)) {
				hitvect.Add (dirvect*rh.distance);
			}
		}
		// レイが衝突した位置を取得して、平均値を計算する。その値の位置からソナー音を鳴らす。
		// つまり、衝突しそうな壁の方向のベクトルを取得して、その方向から音が聞こえる。
		for (int i = 0; i < hitvect.Count; i++)
			sumvect += hitvect [i];
		if (Physics.Raycast (transform.position, sumvect.normalized, out rh, distance, lm.value, QueryTriggerInteraction.Ignore)) {
			se.SetActive (true);
			se.transform.position = rh.point;
			se.GetComponent<soundInstance> ().normalize = rh.distance / distance;
		} else {
			se.SetActive (false);
		}

		bpos = transform.position;
	}
}
