using UnityEngine;
using System.Collections;

public class EffectBase : MonoBehaviour {

	public void SetDestroyTimer(float timer) {
		StartCoroutine(update(timer));
	}

	private IEnumerator update(float timer) {
		yield return new WaitForSeconds(timer);
		Destroy(gameObject);
	}
}
