using UnityEngine;
using System.Collections.Generic;
using ClientNetwork;

/// <summary>
/// 싱글톤이다.
/// </summary>
public class NetworkManager : MonoBehaviour {
	public string address = "192.168.0.2";
	public int ID = -1;
	public Dictionary<string, Transform> objects = new Dictionary<string, Transform>();
	public Transform[] prefabs;

	/// <summary>
	/// 싱글톤이다.
	/// </summary>
	public static NetworkManager instance;
	public List<ClientObject> createObjectes = new List<ClientObject>();

	// Use this for initialization
	void Start () {
		for (int i = 0; i < prefabs.Length; i++) {
			objects.Add(prefabs[i].GetComponent<FieldObject>().network.key, prefabs[i]);
		}
		instance = this;
		MyNet.serverAddress = address;
		MyNet.Start();
	}

	void Update() {
		SeaStone();
	}

	/// <summary>
	/// 해석
	/// </summary>
	private void SeaStone() {
		while (!ClientNetwork.Received.buffer.IsEmpty()) {
			ClientNetwork.ClientString str = ClientNetwork.Received.buffer.Pop();

			switch (str.param[0]) {
				case StrProtocol.State.Create:
					CreateObject(str.id, str.param[1], float.Parse(str.param[2]), float.Parse(str.param[3]), float.Parse(str.param[4]), RemainStr(str.param, 5));
					break;
				case StrProtocol.Login.SetID:
					ClientNetwork.MyNet.myId = str.id;
					break;
				case StrProtocol.State.Position:
					MoveObject(str.id, float.Parse(str.param[1]), float.Parse(str.param[2]), float.Parse(str.param[3]));
					break;
			}
		}
	}

	public void CreateObject(int id, string key, float pos_x, float pos_y, float pos_z, params string[] param) {
		Vector3 pos = new Vector3(pos_x, pos_y, pos_z);
		Transform newT = (Transform)Instantiate(objects[key], pos, Quaternion.identity);
		FieldObject newF = newT.GetComponent<FieldObject>();
		newF.NetworkInit(param);
		newF.network.ID = id;
		createObjectes.Add(newF.network);
	}

	public void MoveObject(int id, float pos_x, float pos_y, float pos_z) {
		for (int i = 0; i < createObjectes.Count; i++) {
			if (createObjectes[i].ID == id) {
				//createObjectes[i].gameObject.GetComponent<TgNetworkCharacter>().SetDestPos(new Vector2(pos_x, pos_y));
				createObjectes[i].gameObject.transform.position = new Vector3(pos_x, pos_y, pos_z);
			}
		}
	}

	void OnApplicationQuit() {
		Debug.Log("Stop Network");
		ClientNetwork.MyNet.Stop();
	}

	/// <summary>
	/// 남은 스트링배열을 리턴한다.
	/// </summary>
	/// <param name="param"></param>
	/// <param name="startIndex"></param>
	/// <returns></returns>
	private string[] RemainStr(string[] param, int startIndex) {
		string[] ret = new string[param.Length - startIndex];

		return ret;
	}
}




