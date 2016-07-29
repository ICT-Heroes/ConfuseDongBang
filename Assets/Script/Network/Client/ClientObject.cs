using UnityEngine;
using System.Collections;

[System.Serializable]
public class ClientObject {

	public int ID;
	public string key;

	public GameObject gameObject;

	/// <summary>
	/// 클라이언트에서 오브젝트를 새로 생성하고자 할 때 만든다.
	/// </summary>
	/// <param name="key">생성할 오브젝트의 키</param>
	/// <param name="pos">생성할 오브젝트의 위치</param>
	/// <param name="param">초기화 할 파라미터 값들</param>
	public void CreateSelf(string key, Vector3 pos, params string[] param) {
		string ss = "";
		for (int i = 0; i < param.Length; i++) ss += "," + param[i];
		ClientNetwork.MyNet.Send(new ClientNetwork.ClientString(StrProtocol.State.Create, key, pos.x + "", pos.y + "", pos.z + "", ss));
	}

	public void SendPosition() {
		ClientNetwork.MyNet.Send(new ClientNetwork.ClientString(StrProtocol.State.Position, gameObject.transform.position.x + "", gameObject.transform.position.y + "", gameObject.transform.position.z + ""));
	}
}


