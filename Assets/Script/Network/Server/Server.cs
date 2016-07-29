using UnityEngine;
using UnityEngine.UI;

using ServerNetwork;

public class Server : MonoBehaviour {


	public Text textDebug;
	public Text textState;

	public static Server instance;

	void Start() {
		instance = this;
		MyNet.Start();
	}

	void Update() {
		debugString = Print.Debug;
		stateString = Print.State;
		Decode();
	}

	void OnApplicationQuit() {
		Debug.Log("Stop Server");
		MyNet.Stop();
	}


	private void Decode() {
		while (!Received.strs.IsEmpty()) {
			ServerString str = Received.strs.Pop();
			Debug.Log("str.param[1] : " + str.param[1] + ", str : " + str.GetString());
			switch (str.param[1]) {
				case StrProtocol.State.Create:
					ServerString newStr = new ServerString(str.param);
					MyNet.SendAll(newStr);
					break;

				case StrProtocol.State.Position:
					ServerString newStr2 = new ServerString(str.param);
					MyNet.SendAll(newStr2);
					break;
			}
		}
	}

	/*
	public void SendPosition(ServerObject player) {
		Vector3 pos = player.transform.position;
		MyNet.SendAll(new ServerString(player.id, pos.x + "", pos.y + "", pos.z + ""));
	}
	*/

	public string debugString {
		get { return instance.textDebug.text; }
		set { if (value != null) instance.textDebug.text = value; }
	}

	public string stateString {
		get { return instance.textState.text; }
		set { if (value != null) instance.textState.text = value; }
	}
}





/// <summary>
/// 네트워크를 통해 작업을 하다가 그 결과가 유니티에 반영되어야 할 때 전달자로 사용한다.
/// createPlayer : 클라이언트에서 접속할 때, 플레이어를 만들어야 하는데,
///				서버에서도 동일하게 만들어야 한다.
///				그런데 서버에서는 클라가 접속할 때에 즉시 만들어야 하는데 그 시기는
///				네트워크가 동작하는 쓰레드에서만 알 수 있다.
/// </summary>
class LocalDelegate {
	public static MyQueue<Param.CreatePlayer> createPlayer = new MyQueue<Param.CreatePlayer>();
	public static MyQueue<Param.Login> login = new MyQueue<Param.Login>();
}

public class Param {
	public struct CreatePlayer {
		public enum Player {
			penguin
		};
		public int id;
		public Player player;
		public Vector3 pos;
		public Quaternion rot;
		public string nick;
	}

	public struct Login {
		public string id;
		public string pass;
	}

}


