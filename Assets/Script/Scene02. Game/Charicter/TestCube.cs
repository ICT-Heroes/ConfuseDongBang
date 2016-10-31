using UnityEngine;
using System.Collections;

public class TestCube : MonoBehaviour {

	public int id = -1;
	bool myCharic = false;
	private Vector3 virtualVec, exVec, movingForward, attackForward, currVec;
	public ModelAnim model;

	public CreateManager.Charic charic = CreateManager.Charic.penguin;

	private Rigidbody rigidbody;
	private GameObject lookAtObj;
	public CapsuleCollider capsuleCol;

	public Gauge.BaseGauge hpGauge;

	private int flying = 0;
	private bool dead = false;

	private int maxHp = 1000;
	private int hp = 1000;
	public int Hp {
		get {
			return hp;
		}
		set {
			if (!dead) {
				hp = value;
				if (hp <= 0) {
					hp = 0;
					if (ClientNetwork.MyNet.myId == id) {
						dead = true;
						StartCoroutine(deadUpdate());
					}
				}
				hpGauge.Set(hp, maxHp);
			}
		}
	}

	/// <summary>
	/// 초기화.
	/// </summary>
	private void Init() {
		transform.position = new Vector3(0.7f, 1, 0);
		Hp = 1000;
		virtualVec = Vector3.zero;
	}

	private IEnumerator deadUpdate() {
		SendAnim(ModelAnim.Anim.dead);
		GameObject obj = new GameObject();
		Rigidbody modelRig = obj.AddComponent<Rigidbody>();
		capsuleCol.height = 0;
		capsuleCol.center = Vector3.zero;
		obj.transform.SetParent(transform, true);
		obj.transform.localPosition = new Vector3(0, 0.73f, 0);
		rigidbody.freezeRotation = false;
		model.transform.SetParent(obj.transform, true);
		modelRig.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.VelocityChange);
		float timer = 0;
		while (timer < 5) {
			timer += Time.deltaTime;
			obj.transform.localPosition = Vector3.zero;
			yield return null;
		}
		dead = false;
		model.transform.SetParent(transform, true);
		rigidbody.freezeRotation = true;
		transform.rotation = Quaternion.identity;
		capsuleCol.height = 1.84f;
		capsuleCol.center = new Vector3(0, 0.9f, 0);
		Destroy(obj);
		Init();
	}

	public void SetID(int id) {
		this.id = id;
	}

	// Use this for initialization
	void Start() {
		Init();
		StartCoroutine(movePosition());
	}

	public void StartEndOfLoading() {
		if (id == ClientNetwork.MyNet.myId) {
			myCharic = true;
			MainCam.instance.SetMyCharicter(gameObject);
			rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.freezeRotation = true;

			PlayerState data = new PlayerState(ClientNetwork.MyNet.myId, transform.position, transform.rotation, hp, maxHp);
			string jsonString = JsonUtility.ToJson(data);
			NetPacket packet = new NetPacket(ClassType.PlayerState, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.Create, jsonString);
			ClientNetwork.MyNet.Send(packet);
			lookAtObj = new GameObject();
			lookAtObj.transform.SetParent(transform);
			lookAtObj.transform.localPosition = transform.forward;
			SkillUI.instance.InitUI(charic);
			Destroy(hpGauge.gameObject);
			hpGauge = Gauge.MyHpGauge.instance;
		}
		if (myCharic) StartCoroutine(sendPosition());
	}

	private bool GetAttackForward(out Vector3 result) {
		Ray ray = new Ray(MainCam.instance.mainCamCenter.transform.position, MainCam.instance.cam.transform.forward);
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
			if (myCharic && !dead) {
				movingForward = Vector3.zero;
				if (Input.GetKeyDown("l")) {
					Hp = 0;
				}

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

				if (flying > 0 && Input.GetKeyDown(KeyCode.Space)) {
					Jump();
				}

				if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) {
					if (Input.GetMouseButtonDown(0)) {
						SendAttack(0);
					}

					if (Input.GetMouseButtonDown(1)) {
						SendAttack(1);
					}
				}

#if (UNITY_ANDROID || UNITY_IOS)
				movingForward += MainCam.instance.transform.forward * AndriodJoystic.instance.vec.y * Time.deltaTime;
				movingForward += MainCam.instance.transform.right * AndriodJoystic.instance.vec.x * Time.deltaTime;
				//transform.position += AndriodJoystic.instance.vec * Time.deltaTime;
#endif
				float dist = 0;
				if (!model.Attacking) {
					transform.position += movingForward;				//때리는 중이 아니라면 위치가 움직여야 한다.
					currVec = transform.position - exVec;				//현재 움직이는 방향
					currVec = new Vector3(currVec.x, 0, currVec.z);		//펭귄이라면 y축으로 움직이는 것은 그만둔다.
					dist = Vector3.SqrMagnitude(currVec) * 50000;		//얼마만큼의 속도로 움직이고 있는지 dist 를 구한다. 달릴지 걷는지 쉬는중인지 알기위해
					currVec = Vector3.Normalize(currVec);				//단순방향만을 알고싶어서 노멀라이즈 시킨다.
					exVec = transform.position;							//다음 프레임에서도 움직이는 위치를 알고 싶어서 현재 위치를 저장한다.

					SendMyAnim(dist);
					lookAtObj.transform.localPosition = Vector3.Lerp(lookAtObj.transform.localPosition, currVec, Time.deltaTime * 10);
				} else {
					currVec = Vector3.Normalize(attackForward - transform.position);
					lookAtObj.transform.localPosition = new Vector3(currVec.x, 0, currVec.z);
					dist = 1;
				}
				
				if(dist > 0.5f)	model.transform.LookAt(lookAtObj.transform.position);
			}
		}
		if (!dead) {
			model.transform.localPosition = Vector3.zero;
		} else {
			model.transform.localPosition = new Vector3(0, -0.1f, 0);
		}
	}

	public void Jump() {
		rigidbody.AddForce(new Vector3(0, 200, 0));
	}

	public void SendAttack(int num) {
		if (SkillUI.instance.SkillUseAble(num)) {
			SkillUI.instance.OnButtonSkillTouch(num);
			PlayerAttack atk;
			string json;
			if (GetAttackForward(out attackForward)) {
				switch (num) {
					case 0:
						SendAnim(ModelAnim.Anim.attack0);
						atk = new PlayerAttack(ClientNetwork.MyNet.myId, charic, 0, 100, new Vec3(transform.position), new Vec3(attackForward));
						json = JsonUtility.ToJson(atk);
						ClientNetwork.MyNet.Send(new NetPacket(ClassType.PlayerAttack, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.Attack, json));
						break;
					case 1:
						SendAnim(ModelAnim.Anim.attack1);
						atk = new PlayerAttack(ClientNetwork.MyNet.myId, charic, 1, 100, new Vec3(transform.position), new Vec3(attackForward));
						json = JsonUtility.ToJson(atk);
						ClientNetwork.MyNet.Send(new NetPacket(ClassType.PlayerAttack, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.Attack, json));
						break;
					case 2:
						SendAnim(ModelAnim.Anim.attack2);
						atk = new PlayerAttack(ClientNetwork.MyNet.myId, charic, 2, 100, new Vec3(transform.position), new Vec3(attackForward));
						json = JsonUtility.ToJson(atk);
						ClientNetwork.MyNet.Send(new NetPacket(ClassType.PlayerAttack, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.Attack, json));
						break;
				}

			}
		}
	}

	/// <summary>
	/// 애니메이션을 실행시키고 서버에 보낸다.
	/// </summary>
	/// <param name="distance"></param>
	private void SendMyAnim(float distance) {
		if (flying == 0) {
			SendAnim(ModelAnim.Anim.air);
		} else {
			if (distance < 1) {
				SendAnim(ModelAnim.Anim.stand);
			} else if (distance < 2) {
				SendAnim(ModelAnim.Anim.run);
			} else {
				SendAnim(ModelAnim.Anim.dash);
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
	public void SetPos(Vector3 pos, Quaternion rot, int hp, int maxHp) {
		virtualVec = pos - exVec;
		transform.position = exVec;
		model.transform.rotation = rot;
		exVec = pos;
		if (this.maxHp != maxHp) this.maxHp = maxHp;
		if (Hp != hp) Hp = hp;
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

	void OnTriggerEnter(Collider coll) {
		flying++;
	}

	void OnTriggerExit(Collider coll) {
		flying--;
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
			PlayerState data = new PlayerState(ClientNetwork.MyNet.myId, transform.position, model.transform.rotation, hp, maxHp);
			string jsonString = JsonUtility.ToJson(data);
			ClientNetwork.MyNet.Send(new NetPacket(ClassType.PlayerState, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.ChangePlayerData, jsonString));
			//Debug.Log("myId : " + ClientNetwork.MyNet.myId + ", jsonString : " + jsonString);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
