using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MyButton : Button {

	private float speed = 1.5f;
	private bool initTag = false;
	private float multTag = 1;
	public override void OnPointerDown(PointerEventData eventData) {
		base.OnPointerDown(eventData);
		gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 1) * GetMultTag();
	}

	private float GetMultTag() {
		if (initTag) return multTag;
		else {
			if (transform.tag.Equals("ShopPointButton")) {
				multTag = 0.5f;
			} else {
				multTag = 1;
			}
			initTag = true;
		}
		return multTag;
	}

	public override void OnPointerClick(PointerEventData eventData) {
		base.OnPointerClick(eventData);
		if (gameObject.activeInHierarchy) StartCoroutine("CoroutineButtonTouch");
	}


	public override void OnPointerExit(PointerEventData eventData) {
		base.OnPointerExit(eventData);
		gameObject.transform.localScale = new Vector3(1, 1, 1) * GetMultTag();
	}

	public IEnumerator CoroutineButtonTouch() {
		for (float i = 0; i < 1; i += Time.deltaTime * speed * 5) {
			float ss = Mathf.Sin((i * 1.5f - 0.5f) * 3.141592f) * 0.1f + 1;
			gameObject.transform.localScale = new Vector3(ss, ss, 1) * GetMultTag();
			yield return null;
		}
		gameObject.transform.localScale = new Vector3(1, 1, 1) * GetMultTag();
	}
}
