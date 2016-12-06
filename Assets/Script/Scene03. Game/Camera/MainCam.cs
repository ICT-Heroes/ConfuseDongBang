using UnityEngine;
using System.Collections;

public class MainCam : MonoBehaviour {

	public Camera cam;
	public GameObject mainCamY, mainCamCenter;
	public static MainCam instance;
	public static float mouseDelta = 5;


	void Awake() {
		instance = this;
	}

	void Update() {

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {

		} else {
			if (!GameSystem.instance.UIMode) {
				transform.Rotate(new Vector3(0, mouseDelta * Input.GetAxis("Mouse X"), 0));
				mainCamY.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * mouseDelta, 0, 0));

				if (Input.GetKeyDown("[")) {
					mouseDelta -= 0.2f;
					Debug.Log("mouse delta down : " + mouseDelta);
				}

				if (Input.GetKeyDown("]")) {
					mouseDelta += 0.2f;
					Debug.Log("mouse delta up   : " + mouseDelta);
				}
			}
		}
	}

	public void OnDragMobileToScreen(Vector2 vec) {
		transform.Rotate(new Vector3(0, vec.x * Time.deltaTime * mouseDelta, 0));
		mainCamY.transform.Rotate(new Vector3(-vec.y * Time.deltaTime * mouseDelta, 0, 0));
	}

	public void SetMyCharacter(GameObject obj) {
		transform.SetParent(obj.transform);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}
}
