using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateManager : MonoBehaviour {

	public Transform penguin;

	public List<Transform> players = new List<Transform>();

	private int sendCount = 0;

	public void Update() {
		if (!LocalDelegate.createPlayer.IsEmpty()) {
			CreatePlayer(LocalDelegate.createPlayer.Pop());
		}
		SendPos();
	}

	void SendPos() {
		sendCount++;
		if (sendCount >= 10) {
			sendCount = 0;
			for(int i = 0; i < players.Count; i ++)
				MyNet.SendUdp(ConvertPlayerToString(players[i]));
		}
	}

	private NetString ConvertPlayerToString(Transform p) {
		NetString ret = new NetString(p.GetComponent<NetObject>().id);
		return ret;
	}

	public void CreatePlayer(Param.CreatePlayer param) {
		Transform newObj;
		if (Param.CreatePlayer.Player.penguin == param.player)
			newObj = (Transform)Instantiate(penguin, param.pos, param.rot);
		else
			newObj = (Transform)Instantiate(penguin, param.pos, param.rot);
		//파라미터의 아이디를 추가.
		if (!newObj.gameObject.GetComponent<NetObject>())
			newObj.gameObject.AddComponent<NetObject>();
		newObj.gameObject.GetComponent<NetObject>().id = param.id;
		newObj.gameObject.GetComponent<NetObject>().nick = param.nick;
		newObj.gameObject.GetComponent<NetObject>().player = param.player;

		players.Add(newObj);

		//어떤 캐릭터를 만들 것인지 전송.
		/*
		NetString str = new NetString(param.id);
		str.AddStruct(new NetStruct(Str.Create.Player.ID, param.id + ""));
		if (Param.CreatePlayer.Player.penguin == param.player)
			str.AddStruct(new NetStruct(Str.Create.Player.player, Str.Create.Player.Penguin));
		else
			str.AddStruct(new NetStruct(Str.Create.Player.player, Str.Create.Player.Penguin));
		str.AddStruct(new NetStruct(Str.Create.Player.pos, param.pos.x + "", param.pos.y + "", param.pos.z + ""));
		str.AddStruct(new NetStruct(Str.Create.Player.rot, param.rot.x + "", param.rot.y + "", param.rot.z + "", param.rot.w + ""));
		str.AddStruct(new NetStruct(Str.Create.Player.nick, param.nick + ""));
		MyNet.SendTcp(str);
		*/
	}

	public void DeletePlayer(int id) {
		for (int i = 0; i < players.Count; i++) {
			if (players[i].GetComponent<NetObject>().id == id) {

			}
		}
	}
}
