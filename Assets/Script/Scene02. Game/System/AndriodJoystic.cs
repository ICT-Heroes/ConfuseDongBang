using UnityEngine;
using System.Collections;

public class AndriodJoystic : MonoBehaviour {

	public GameObject joystic;

	public static AndriodJoystic instance;

	public Vector3 vec;

	// Use this for initialization
	void Start() {
#if !(UNITY_ANDROID || UNITY_IOS)
		Destroy(joystic, 0.1f);	
#else
		instance = this;
#endif
	}
	
	// Update is called once per frame
	void Update () {
		vec = transform.localPosition;
		float magitude = vec.sqrMagnitude;
		vec = Vector3.Normalize(vec);
		if (magitude < 1000) {
			vec *= magitude;
			vec *= 0.001f;
		}
	}
}
