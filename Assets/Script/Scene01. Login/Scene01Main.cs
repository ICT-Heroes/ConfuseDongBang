using UnityEngine;
using System.Collections;


namespace Scene01 {
	public class Scene01Main : MonoBehaviour {

		public Transform bigQuad;


		void Start() {
			StartCoroutine(CreateBigQuad());
		}

		IEnumerator CreateBigQuad() {
			while (true) {
				Transform q = Instantiate(bigQuad);
				q.position = new Vector3(20, -10, 0);
				yield return new WaitForSeconds(1.5f);
			}
		}
	}
}
