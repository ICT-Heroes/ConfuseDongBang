using UnityEngine;
using System.Collections;

namespace Gauge {
	public class Gauge : BaseGauge {

		public MeshRenderer gauge;
		public float delta = 1;

		void Start() {
			StartCoroutine(update());
		}

		public override void Set(int amount, int max) {
			float delta = ((float)amount / (float)max);
			gauge.transform.localPosition = new Vector3(0.5f - delta * 0.5f, 0, 0.03f);
			gauge.transform.localScale = new Vector3(delta * 0.95f, 0.95f, 1);
			gauge.material.SetTextureScale("_MainTex", new Vector2(delta, 1));
			gauge.material.SetTextureOffset("_MainTex", new Vector2(1 - delta, 0));
		}

		IEnumerator update() {
			while (true) {
				transform.LookAt(MainCam.instance.cam.transform.position);
				yield return null;
			}
		}
	}
}
