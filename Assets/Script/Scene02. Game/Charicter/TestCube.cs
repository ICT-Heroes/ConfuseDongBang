using UnityEngine;
using System.Collections;

public class TestCube : MonoBehaviour {

	public int id = -1;
	bool myCharic = false;
	private Vector3 virtualVec, exVec;
	public ModelAnim model;

	private GameObject lookAtObj;


	public void SetID(int id) {
		this.id = id;
	}

	// Use this for initialization
	void Start() {
		virtualVec = Vector3.zero;
		StartCoroutine(movePosition());
	}

	public void StartEndOfLoading() {
		if (id == ClientNetwork.MyNet.myId) {
			myCharic = true;
			MainCam.instance.SetMyCharicter(gameObject);
			Rigidbody rig = gameObject.AddComponent<Rigidbody>();
			rig.freezeRotation = true;

			PlayerState data = new PlayerState(ClientNetwork.MyNet.myId, transform.position, transform.rotation);
			string jsonString = JsonUtility.ToJson(data);
			NetPacket packet = new NetPacket(ClassType.PlayerState, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.Create, jsonString);
			ClientNetwork.MyNet.Send(packet);
			lookAtObj = new GameObject();
			lookAtObj.transform.SetParent(transform);
			lookAtObj.transform.localPosition = transform.forward;
		}
		if (myCharic) StartCoroutine(sendPosition());
	}

	// Update is called once per frame
	void Update() {
		if (!GameSystem.instance.UIMode) {
			if (myCharic) {
				if (Input.GetKey("w")) {
					transform.position += MainCam.instance.transform.forward * Time.deltaTime;
				}

				if (Input.GetKey("s")) {
					transform.position -= MainCam.instance.transform.forward * Time.deltaTime;
				}

				if (Input.GetKey("d")) {
					transform.position += MainCam.instance.transform.right * Time.deltaTime;
				}

				if (Input.GetKey("a")) {
					transform.position -= MainCam.instance.transform.right * Time.deltaTime;
				}
#if (UNITY_ANDROID || UNITY_IOS)
				transform.position += MainCam.instance.transform.forward * AndriodJoystic.instance.vec.y * Time.deltaTime;
				transform.position += MainCam.instance.transform.right * AndriodJoystic.instance.vec.x * Time.deltaTime;
				//transform.position += AndriodJoystic.instance.vec * Time.deltaTime;
#endif
				Vector3 currVec = transform.position - exVec;
				exVec = transform.position;
				float dist = Vector3.SqrMagnitude(currVec) * 50000;
				if (dist < 1) {
					model.SetAnim(ModelAnim.Anim.stand);
				} else if (dist < 5) {
					model.SetAnim(ModelAnim.Anim.run);
				} else {
					model.SetAnim(ModelAnim.Anim.dash);
				}
				currVec = Vector3.Normalize(currVec);
				lookAtObj.transform.localPosition = Vector3.Lerp(lookAtObj.transform.localPosition, currVec, Time.deltaTime * 10);
				model.transform.localPosition = Vector3.zero;
				model.transform.LookAt(lookAtObj.transform.position);
			}
		}
	}

	/// <summary>
	/// 네트워크를 통해 받은 다른 캐릭터의 상태 이동 정보를 저장.
	/// </summary>
	public void SetPos(Vector3 pos, Quaternion rot) {
		virtualVec = pos - exVec;
		transform.position = exVec;
		model.transform.rotation = rot;
		exVec = pos;
	}

	public IEnumerator movePosition() {
		while (!myCharic) {
			transform.position += virtualVec * Time.deltaTime * 10;
			yield return null;
		}	
	}

	public IEnumerator sendPosition() {
		while (true) {
			PlayerState data = new PlayerState(ClientNetwork.MyNet.myId, transform.position, model.transform.rotation);
			string jsonString = JsonUtility.ToJson(data);
			ClientNetwork.MyNet.Send(new NetPacket(ClassType.PlayerState, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.ChangePlayerData, jsonString));
			//Debug.Log("myId : " + ClientNetwork.MyNet.myId + ", jsonString : " + jsonString);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
