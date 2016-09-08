using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Collections.Generic;

using ServerNetwork;

public class Server : MonoBehaviour {


	public Text textDebug;
	public Text textState;

	public static Server instance;

	void Start() {
		URLStart();
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
		while (Received.strs.Count > 0) {
			string str = Received.strs.Dequeue();
			switch (str) {
				case "":
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

	public void URLStart() {
		string url = "http://minus-one.co.kr/penguin/insertMemberInfo.php";

		WWWForm wform = new WWWForm();
		wform.AddField("id", "myIDDDD");
		wform.AddField("password", "Pass");
		wform.AddField("nick_name", "ThisIsNickName");
		wform.AddField("email_address", "email@google.com");
		wform.AddField("is_admin", 1);
		wform.AddField("regdate", "regdate");
		//wform.AddField("last_login", "1");

		WWW www = new WWW(url, wform);
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www) {
		yield return www;

		if (www.error == null) {
			Debug.Log("WWW OK! : " + www.text);
		} else {
			Debug.Log("WWW Error : " + www.error);
		}
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
	public static Queue<Param.CreatePlayer> createPlayer = new Queue<Param.CreatePlayer>();
	public static Queue<Param.Login> login = new Queue<Param.Login>();
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


