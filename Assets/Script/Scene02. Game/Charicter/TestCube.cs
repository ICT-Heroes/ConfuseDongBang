using UnityEngine;
using System.Collections;

public class TestCube : MonoBehaviour {

	public int id = -1;
	bool myCharic = false;
	private Vector3 virtualVec, exVec, movingForward, attackForward, currVec;
	public ModelAnim model;

	private GameObject lookAtObj;

	public GameObject test;


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

	private bool GetAttackForward(out Vector3 result) {
		Ray ray = new Ray(MainCam.instance.cam.transform.position, MainCam.instance.cam.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			result = hit.point;
			return true;
		}
		result = Vector3.zero;
		return false;
	}

	// Update is called once per frame
	void Update() {
		if (!GameSystem.instance.UIMode) {
			if (myCharic) {
				movingForward = Vector3.zero;
				if (Input.GetKey("w")) {
					movingForward += MainCam.instance.transform.forward * Time.deltaTime;
				}

				if (Input.GetKey("s")) {
					movingForward -= MainCam.instance.transform.forward * Time.deltaTime;
				}

				if (Input.GetKey("d")) {
					movingForward += MainCam.instance.transform.right * Time.deltaTime;
				}

				if (Input.GetKey("a")) {
					movingForward -= MainCam.instance.transform.right * Time.deltaTime;
				}

				if (Input.GetMouseButtonDown(0)) {
					SendAnim(ModelAnim.Anim.attack0);
					if (GetAttackForward(out attackForward)) {

					}
				}

				if (Input.GetMouseButtonDown(1)) {
					SendAnim(ModelAnim.Anim.attack1);
					if (GetAttackForward(out attackForward)) {

					}
				}

#if (UNITY_ANDROID || UNITY_IOS)
				movingForward += MainCam.instance.transform.forward * AndriodJoystic.instance.vec.y * Time.deltaTime;
				movingForward += MainCam.instance.transform.right * AndriodJoystic.instance.vec.x * Time.deltaTime;
				//transform.position += AndriodJoystic.instance.vec * Time.deltaTime;
#endif
				float dist = 0;
				if (!model.Attacking) {
					transform.position += movingForward;
					currVec = transform.position - exVec;
					exVec = transform.position;
					dist = Vector3.SqrMagnitude(currVec) * 50000;
					if (dist < 1) {
						SendAnim(ModelAnim.Anim.stand);
					} else if (dist < 5) {
						SendAnim(ModelAnim.Anim.run);
					} else {
						SendAnim(ModelAnim.Anim.dash);
					}
					currVec = Vector3.Normalize(currVec);
					lookAtObj.transform.localPosition = Vector3.Lerp(lookAtObj.transform.localPosition, currVec, Time.deltaTime * 10);
				} else {
					currVec = Vector3.Normalize(attackForward - transform.position);
					lookAtObj.transform.localPosition = new Vector3(currVec.x, 0, currVec.z);
					dist = 1;
				}
				model.transform.localPosition = Vector3.zero;
				if(dist > 0.5f)	model.transform.LookAt(lookAtObj.transform.position);



			}
		}
	}

	/// <summary>
	/// 어택을 실행함.
	/// </summary>
	/// <param name="num"></param>
	public void AttackEvent(int num) {

	}

	/// <summary>
	/// 어택이 끝남
	/// </summary>
	/// <param name="num"></param>
	public void AttackEnd(int num) {

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

	/// <summary>
	/// 캐릭터가 내것이 아닐 때, 네트워크를 통해 받은대로 애니메이션을 바꾼다.
	/// </summary>
	public void SetAnim(ModelAnim.Anim anim) {
		model.SetAnim(anim);
	}

	/// <summary>
	/// 캐릭터가 내것일때 애니메이션을 보낸다.
	/// </summary>
	private void SendAnim(ModelAnim.Anim anim) {
		model.SetAnim(anim);
		PlayerAnim data = new PlayerAnim(ClientNetwork.MyNet.myId, anim);
		string jsonString = JsonUtility.ToJson(data);
		ClientNetwork.MyNet.Send(new NetPacket(ClassType.PlayerAnim, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.ChangePlayerData, jsonString));
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
