using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace Chattings {
	public class ChattingText : MonoBehaviour {
		public Text text;
		private float a;

		void Start() {
			StartCoroutine(DisapearText());
		}

		public void Print(string text) {
			this.text.text = text;
			a = 2;
		}

		private IEnumerator DisapearText() {
			while (true) {
				if (a > 0) {
					a -= Time.deltaTime * 0.5f;
					text.color = new Color(1, 1, 1, a);
				}
				yield return null;
			}
		}

	}
}
