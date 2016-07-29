using UnityEngine;
using System.Collections;

public class PlayerData {


	private string login_id;
	private string passward;
	private string nick_name;
	public Vector3 pos;
	public Quaternion rot;

	public PlayerData() {
		login_id = null;
		passward = null;
		nick_name = "null";
		pos = new Vector3();
		rot = new Quaternion();
	}

	public PlayerData(string userId, string pass, string nick) {
		login_id = userId;
		passward = pass;
		nick_name = nick;
		pos = new Vector3();
		rot = new Quaternion();
	}

	public PlayerData(PlayerData other) {
		login_id = other.login_id;
		passward = other.passward;
		nick_name = other.nick_name;
		pos = other.pos;
		rot = other.rot;
	}

	public void SetPassword(string pass) {
		passward = pass;
	}

	public void SetNickName(string nick) {
		nick_name = nick;
	}

	public void SetUserId(string id) {
		login_id = id;
	}

	public string GetPassword() {
		return passward;
	}

	public string GetNickName() {
		return nick_name;
	}

	public string GetUserId() {
		return login_id;
	}
}
