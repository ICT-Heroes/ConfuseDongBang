using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateManager : MonoBehaviour {

	public static CreateManager instance;

	public TestCube myCharicter;
	public Transform testCube;



	public Dictionary<int, TestCube> charicters = new Dictionary<int, TestCube>();

	void Awake() {
		instance = this;
	}

	/// <summary>
	/// 네트워크로 나 이외에 누군가가 들어왔을 때 그 객체를 생성한다
	/// </summary>
	public void CreateCharicter(string jsString) {
		PlayerState state = JsonUtility.FromJson<PlayerState>(jsString);
		if (state.clientId != ClientNetwork.MyNet.myId) {
			if (charicters.ContainsKey(state.clientId)) return; //이미 생성된 객체에 대해서는 한번 더 생성하지 않는다.
			Transform newC = (Transform)Instantiate(testCube, state.pos.ToVector3(), state.rot.ToQuaternion());
			TestCube tc = newC.GetComponent<TestCube>();
			tc.SetID(state.clientId);
			charicters.Add(state.clientId, tc);
			Debug.Log("다른놈 캐릭터 생성 : " + state.clientId);
		}
	}

	/// <summary>
	/// 누군가가 접속을 끊었을 때,
	/// </summary>
	public void DeleteCharicter(int id) {
		if (id != ClientNetwork.MyNet.myId) {
			charicters.Remove(id);
		}
	}


	/// <summary>
	/// 네트워크로 받은 나 이외의 다른 아이디의 캐릭터를 움직인다.
	/// </summary>
	public void SetPos(string jsString) {
		PlayerState state = JsonUtility.FromJson<PlayerState>(jsString);
		if (state.clientId != ClientNetwork.MyNet.myId) {
			TestCube cube;
			if (charicters.TryGetValue(state.clientId, out cube)) {
				cube.SetPos(state.pos.ToVector3(), state.rot.ToQuaternion());
			} else {
				NetPacket packet = new NetPacket(ClassType.PlayerState, ClientNetwork.MyNet.myId, EchoType.NotEcho, NetFunc.RequireOtherPlayer, jsString);
				ClientNetwork.MyNet.Send(packet);
			}
		}
	}

	/// <summary>
	/// 네트워크로 받은 나 이외의 다른 아이디의 캐릭터의 애니메이션을 바꾼다.
	/// </summary>
	public void SetAnim(string jsString) {
		PlayerAnim state = JsonUtility.FromJson<PlayerAnim>(jsString);
		if (state.clientId != ClientNetwork.MyNet.myId) {
			TestCube cube;
			if (charicters.TryGetValue(state.clientId, out cube)) {
				cube.SetAnim(ModelAnim.ConvertIntToAnim(state.anim));
			}
		}
	}


}
