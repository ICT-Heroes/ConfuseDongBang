using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Collections.Generic;

using ServerNetwork;

public class Server : MonoBehaviour {
	
	public static Server instance;

	void Start() {
		URLStart();
		instance = this;
		ServerModule.Start();
	}

	void Update() {
		Decode();
	}

	void OnApplicationQuit() {
		Debug.Log("Stop Server");
        ServerModule.Stop();
	}


	private void Decode() {
		while (Received.GetCount() > 0) {
			NetPacket str = Received.DequeueThreadSafe();
			switch (str.func) {
                case NetFunc.Account:
                    break;
                case NetFunc.Login:
                    break;
            }
		}
	}

    /// <summary>
    /// tcp 로 모든 클라이언트에게 보내고 싶을 때 사용.
    /// </summary>
    /// <param name="str"></param>
    public static void SendAll(DataType type, NetFunc func, string jsString) {
        ServerModule.SendAll(type, func, jsString);
    }

    /// <summary>
    /// tcp 로 특정 id 의 클라이언트에게 보내고 싶을 때 사용.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="str"></param>
    public static void Send(DataType type, int id, NetFunc func, string jsString) {
        ServerModule.Send(type, id, func, jsString);
    }

    /*
	public void SendPosition(ServerObject player) {
		Vector3 pos = player.transform.position;
		MyNet.SendAll(new ServerString(player.id, pos.x + "", pos.y + "", pos.z + ""));
	}
	*/

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


