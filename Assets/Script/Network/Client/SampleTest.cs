using UnityEngine;
using System.Collections;

public class SampleTest : MonoBehaviour {

	// Use this for initialization
	void Start() {
		StartCoroutine(sendPosition());
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKey("w")) {
			transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
		}

		if (Input.GetKey("s")) {
			transform.position -= new Vector3(1, 0, 0) * Time.deltaTime;
		}
	}

	public IEnumerator sendPosition() {
		while (true) {
			PlayerState data = new PlayerState(ClientNetwork.MyNet.myId, transform.position, transform.rotation);
			string jsonString = JsonUtility.ToJson(data);
			ClientNetwork.MyNet.Send(new NetPacket(DataType.PlayerState, ClientNetwork.MyNet.myId, EchoType.Echo, NetFunc.ChangePlayerData, jsonString));
			Debug.Log("myId : " + ClientNetwork.MyNet.myId + ", jsonString : " + jsonString);
			yield return new WaitForSeconds(0.2f);
		}
	}
}
