using UnityEngine;
using System.Collections;
using PenguinModel;
using UnityEngine.UI;
using System;
using System.Net;
using ClientNetwork;
using UnityEngine.Experimental.Networking;


public class LoginManager : MonoBehaviour{

	public InputField idComponent;
	public InputField passwordComponent;
	public InputField nicknameComponent;
	public InputField emailComponent;
	private string currentJsonString;
	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown("f")) {
			//network.CreateSelf("sample", transform.position);
		}
	}


	public void OnButtonTouched(){

		string id = "ThisIsID";
		Member member = new Member (id);
		string jsonString = JsonUtility.ToJson (member);
		NetPacket netPacket = new NetPacket (ClassType.Member, 13579, EchoType.NotEcho, NetFunc.Login, jsonString);

		string netPacketString = JsonUtility.ToJson (netPacket);
		Debug.Log ("js String : " + netPacketString);

		StartCoroutine(Login(netPacketString));
	
	}

	IEnumerator Login(string jsonString){
		WWWForm form = new WWWForm ();
		form.AddField ("netPacket", jsonString);
		WWW www = new WWW ("http://minus-one.co.kr/penguin/readMemberInfo.php", form);
		yield return www;

        if (www.isDone)
        {
            currentJsonString = www.text;
            Debug.Log("currentJsString : " + currentJsonString);
            //Member member1 = JsonUtility.FromJson<Member>(currentJsonString);
        }
	}

}

