using UnityEngine;
using System.Collections;
using PenguinModel;
using UnityEngine.UI;
using System;
using System.Net;

public class SampleClient : MonoBehaviour{

	public InputField idComponent;
	public InputField passwordComponent;
	public InputField nicknameComponent;
	public InputField emailComponent;
	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown("f")) {
			//network.CreateSelf("sample", transform.position);
		}
	}
	/*
	public virtual void NetworkInit(params string[] param) {
		
	}*/


	public void OnButtonTouched(){
		string id;
		string password;
		string email;
		string nickname;


		HttpWebRequest header = new HttpWebRequest();
		id = idComponent.text;
		password = passwordComponent.text;
		email = emailComponent.text;
		nickname = nicknameComponent.text;
		Member member = new Member (id, new Vector3(1, 1, 1.12356f), password, nickname, email , true, "0808", "0808");
		string jsonString = JsonUtility.ToJson (member);

		Debug.Log (jsonString);

		Member member1 = JsonUtility.FromJson<Member>(jsonString);
		//Debug.Log(member1.id);
	
	}
}

