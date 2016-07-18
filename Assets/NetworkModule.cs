using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class NetworkModule : MonoBehaviour {

	public Text receiveText;

	// Use this for initialization
	void Start() {
		_MyNet.StartServer();
	}

	// Update is called once per frame
	void Update() {
		IfAndroidToQuit();
		if (Input.GetKeyDown("t"))
			StopServer();
		SetConsolText();
		
		PerformTCPString();

	}

	void IfAndroidToQuit() {
		if (Application.platform == RuntimePlatform.Android)
			if (Input.GetKey(KeyCode.Escape))
				Application.Quit();
	}

	void StopServer() {
		_MyNet.StopServer();
		_MyNet.stateMessage = "Exit Server";
	}

	void SetConsolText() {
		receiveText.text = _MyNet.stateMessage;
	}



	//매 프레임마다 실행됨.
	void PerformTCPString() {
		if (!Received.tcp.IsEmpty()) {
			NetString str = Received.tcp.Pop();
			if (Str.EqualName(str, Str.Login.Confirm))
				Login(str);
			if (Str.EqualName(str, Str.Login.Create))
				CreateId(str);
			if (Str.EqualName(str, Str.Require.id))
				RequiredId(str);

		}
	}

	/*
	void PerformUDPString() {
		if (!Received.udp.IsEmpty()) {
			NetString str = Received.udp.Pop();
			if (Str.EqualName(str.strings[0], Str.Update.pos))
				Login(str.id, str.strings[0]);
		}
	}
	*/

	void RequiredId(NetString str) {
		//스트링의 id : 보내온 아이디
		//스트링의 strings[0] : 리콰이어 스트럭트
		//스트링의 param[0] : 요청 아이디
		int id = int.Parse(str.param[1]);	//요청 아이디
		for (int i = 0; i < GameObject.Find("Main").GetComponent<CreateManager>().players.Count; i++) {
			if (GameObject.Find("Main").GetComponent<CreateManager>().players[i].GetComponent<NetObject>().id == id) {
				GameObject obj = GameObject.Find("Main").GetComponent<CreateManager>().players[i].gameObject;
				NetString sendStr = new NetString(
					id,
					Str.Create.player,	//0
					Str.Character.penguin,
					obj.transform.position.x + "",
					obj.transform.position.y + "",
					obj.transform.position.z + "",

					obj.transform.rotation.x + "",
					obj.transform.rotation.y + "",
					obj.transform.rotation.z + "",
					obj.transform.rotation.w + "",
					obj.GetComponent<NetObject>().nick
					);
				MyNet.SendTcp(str.id, sendStr);
				break;
			}
		}
	}

	void Login(NetString str) {
		PlayerData data;
		if (UserData.LogIn(str.param[1], str.param[2], out data)) {
			//로그인 성공했다고 보냄.
			NetString sendStr = new NetString(str.id, Str.Login.Success,
				data.pos.x + "", data.pos.y + "", data.pos.z + "",
				data.rot.x + "", data.rot.y + "", data.rot.z + "", data.rot.w + "",
				data.GetNickName()
				);
			MyNet.SendTcp(str.id, sendStr);

			//플레이어를 생성하기위해 파라미터를 지정, 생성
			Param.CreatePlayer param = new Param.CreatePlayer();
			param.id = str.id;
			param.player = Param.CreatePlayer.Player.penguin;
			param.pos = data.pos;
			param.rot = data.rot;
			LocalDelegate.createPlayer.Push(param);

		} else {
			NetString sendStr = new NetString(str.id, Str.Login.Fail);
			MyNet.SendTcp(str.id, sendStr);
		}
	}

	void CreateId(NetString str) {
		if (UserData.Create(str.param[0], str.param[1], str.param[2])) {
			NetString sendStr = new NetString(str.id, Str.Login.CreateSuccess);
			MyNet.SendTcp(str.id, sendStr);
		} else {
			NetString sendStr = new NetString(str.id, Str.Login.CreateFail);
			MyNet.SendTcp(str.id, sendStr);
		}
	}

	void UpdatePos(NetString str) {

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
	public static LocalQueue<Param.CreatePlayer> createPlayer = new LocalQueue<Param.CreatePlayer>();
	public static LocalQueue<Param.Login> login = new LocalQueue<Param.Login>();
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


