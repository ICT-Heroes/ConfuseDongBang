using UnityEngine;
using System.Collections;

namespace Scene01 {
	public class BigQuad : MonoBehaviour {

		// Use this for initialization
		void Start() {
			StartCoroutine(MyUpdate());
		}

		IEnumerator MyUpdate() {
			while (true) {
				transform.Translate((Vector3.up - Vector3.right * 0.5f) * Time.deltaTime);
				if (transform.position.x < -20) Destroy(gameObject);
				yield return null;
			}
		}
	}
}