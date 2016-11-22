using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class AndriodCameraJoystic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public GameObject joystic;

	public static AndriodCameraJoystic instance;

	private Vector2 exPos, currPos;

	// Use this for initialization
	void Start() {
#if !(UNITY_ANDROID || UNITY_IOS)
		Destroy(joystic, 0.1f);	
#else
		instance = this;
#endif
	}

	public void OnBeginDrag(PointerEventData eventData) {
		exPos = eventData.position;
	}

	public void OnDrag(PointerEventData eventData) {
		currPos = eventData.position - exPos;
		exPos = eventData.position;
		MainCam.instance.OnDragMobileToScreen(currPos);
	}

	public void OnEndDrag(PointerEventData eventData) {

	}
	
}
