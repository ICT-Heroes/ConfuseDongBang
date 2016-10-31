using UnityEngine;

using System;
using System.Collections;
using System.Runtime.InteropServices;

public class GameSystem : MonoBehaviour {

	private bool mouseMoveable = true;
	/// <summary>
	/// 코루틴 실행 중에 0.2초동안 esc 키를 두번 눌러서 아직 코루틴을 빠져나오지 않았는데 새로운 코루틴을 만들어 버리는 것을 방지.
	/// 새로운 코루틴이 만들어지지 않게 기존 코루틴을 유지.
	/// </summary>
	private bool exitMouseMoveableCoroutine = true;

	public static GameSystem instance;

	public GameObject uis, androidUi;

	private bool uiMode = false;
	public bool UIMode {
		get { return uiMode; }
	}

	void Awake() {
		instance = this;
	}

	void Start () {
		SetUIMode(false);
		if (!(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)) {
			Destroy(androidUi);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (uiMode) {
				SetUIMode(false);
			} else {
				SetUIMode(true);
			}
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			if (!uiMode) {
				SetUIMode(true);
				Chattings.Chatting.instance.FocusToInputField();
			}
		}


	}

	public void SetUIMode(bool on) {
		uiMode = on;
		SetMouseCenterPos(!on);
		uis.SetActive(on);
	}

	private void SetMouseCenterPos(bool isCenter) {
		if (isCenter) {
			if (mouseMoveable) {
				mouseMoveable = false;
				if (exitMouseMoveableCoroutine) {
					exitMouseMoveableCoroutine = false;
					StartCoroutine(ToMouseCenterPos());
				}
			}
		} else {
			mouseMoveable = true;
		}
		Cursor.visible = !isCenter;
	}

	private IEnumerator ToMouseCenterPos() {

#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
		while (!mouseMoveable) {
			MouseController.ToMouseCenter();
			yield return new WaitForSeconds(0.2f);
		}
#endif
		exitMouseMoveableCoroutine = true;
		yield return null;
	}


#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IOS)
	public class MouseController {
		private const uint LBUTTONDOWN = 0x00000002;
		private const uint LBUTTONUP = 0x00000004;
		private const uint RBUTTONDOWN = 0x00000008;
		private const uint RBUTTONUP = 0x00000010;
		private const int KEYDOWN = 0x00000000;
		private const int KEYUP = 0x00000002;

		public struct POINT {
			public Int32 x;
			public Int32 y;
		}

		[DllImport("user32.dll")]
		static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);
		[DllImport("user32.dll")]
		static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
		static extern void SetCursorPos(uint X, uint Y);
		[DllImport("user32.dll")]
		static extern Int32 GetCursorPos(out POINT pt);

		/// <summary>
		/// 마우스를 가운데로
		/// </summary>
		public static void ToMouseCenter() {
			SetCursorPos((uint)(Screen.width * 0.5f), (uint)(Screen.height * 0.5f));
		}
	}
#endif
}


