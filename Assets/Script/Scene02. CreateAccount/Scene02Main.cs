using UnityEngine;
using System.Collections;
using System;


namespace Scene02 {
	public class Scene02Main : MonoBehaviour {

		public Transform bigQuad;

		public Transform layers;

		void Start() {
			StartCoroutine(CreateBigQuad());
			DateTime time = DateTime.Now;
		}

		IEnumerator CreateBigQuad() {
			while (true) {
				Transform q = Instantiate(bigQuad);
				q.position = new Vector3(20, -10, 0);
				yield return new WaitForSeconds(1.5f);
			}
		}

		IEnumerator PosCenter() {
			while (true) {
				layers.position = Vector3.Lerp(Vector3.zero, layers.position, Time.deltaTime);
				
			}
		}
	}
}
