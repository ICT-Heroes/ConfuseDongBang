using UnityEngine;
using System.Collections;
using PenguinModel;
using UnityEngine.UI;
using System;
using System.Net;
using ClientNetwork;
using UnityEngine.Experimental.Networking;
using System.Collections.Generic;
using System.Text;

public class CreateAccount : MonoBehaviour{

	public InputField idComponent;
	public InputField passwordComponent;
	public InputField nicknameComponent;
	public InputField emailComponent;
	private string currentJsonString;
    readonly UTF8Encoding encoding = new UTF8Encoding();
    // Use this for initialization
    void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown("f")) {
			//network.CreateSelf("sample", transform.position);
		}
	}


	public void OnCheckIdButtonTouched(){
        if (idComponent.text.Equals("")) {
            Debug.Log("아이디를 입력해 주세요");
        } else {
            Member member = new Member(idComponent.text);
            string jsonString = JsonUtility.ToJson(member);
            NetPacket netPacket = new NetPacket(ClassType.Member, 13579, EchoType.NotEcho, NetFunc.ReadMemberInfo, jsonString);
            CheckIsIdExist(netPacket);
        }
	}

	void CheckIsIdExist(NetPacket netPacket){
        StartCoroutine(NetworkFunctionLibrary.MakeWWWRequest(netPacket, RequestCheckId));
	}

    void RequestCheckId(WWW www) {
        if (www.error == null) {
            currentJsonString = www.text;

            NetPacket netPacket = JsonUtility.FromJson<NetPacket>(currentJsonString);
            Member member = JsonUtility.FromJson<Member>(netPacket.jsonString);
            if (member.id.Equals(idComponent.text)) {
                Debug.Log("이미 존재하는 아이디 입니다.");
            } else {
                Debug.Log("사용 가능한 아이디 입니다.");
            }
        } else {
            Debug.Log("Server error :" + www.error);
        }
    }

}

