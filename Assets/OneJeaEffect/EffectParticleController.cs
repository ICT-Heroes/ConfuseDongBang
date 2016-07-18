using UnityEngine;
using System.Collections;

public class EffectParticleController : MonoBehaviour {

	private float timer = 0;

	void Start () {
		StartCoroutine (ParticleDestroyRoutine ());
	}

	IEnumerator ParticleDestroyRoutine(){
		while (timer < 0.2f) {
			timer += Time.deltaTime;
			yield return null;
		}

		gameObject.GetComponent<ParticleSystem> ().emissionRate = 0;

		while (timer < 1.5f) {
			timer += Time.deltaTime;
			yield return null;
		}

		Destroy (gameObject);
	}
}
