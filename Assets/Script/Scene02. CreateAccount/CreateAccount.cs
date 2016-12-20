using UnityEngine;
using System.Collections;
using UserData;
using UnityEngine.UI;
using System;
using System.Net;
using ClientNetwork;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

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
    public void OnCancelButtonTouched() {
        SceneManager.LoadScene("Scene01. Login");
    }

    public void OnSubmitButtonTouched() {
        if (idComponent.text.Equals("")) {
            Debug.Log("아이디를 입력해 주세요");
        }else if (passwordComponent.text.Equals("")) {
            Debug.Log("비밀번호를 입력해 주세요");
        }else {
            Member member = new Member(idComponent.text, 0, passwordComponent.text, nicknameComponent.text, emailComponent.text, false, "2016-12-20", "2016-12-20");
            string jsonString = JsonUtility.ToJson(member);
            NetPacket netPacket = new NetPacket(ClassType.Member, 13579, EchoType.NotEcho, NetFunc.CreateMemberInfo, jsonString);
            CreateMemberInfo(netPacket);
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

    void CreateMemberInfo(NetPacket netPacket) {
        StartCoroutine(NetworkFunctionLibrary.MakeWWWRequest(netPacket, RequestCreateMember));
    }

    void RequestCreateMember(WWW www) {
        if (www.error == null) {
            Debug.Log("babo");
            currentJsonString = www.text;
            Debug.Log("호우 : " +www.text);
            NetPacket netPacket = JsonUtility.FromJson<NetPacket>(currentJsonString);
            Debug.Log(netPacket.jsonString);
            Member member = JsonUtility.FromJson<Member>(netPacket.jsonString);
            if (member.id.Equals(idComponent.text)) {
                Debug.Log("회원 가입 성공!");
                SceneManager.LoadScene("Scene01. Login");
            } else {
                Debug.Log("회원 가입 실패!");
            }
        } else {
            Debug.Log("Server error :" + www.error);
        }
    }

    void RequestCheckId(WWW www) {
        if (www.error == null) {
            currentJsonString = www.text;
            NetPacket netPacket = JsonUtility.FromJson<NetPacket>(currentJsonString);
            Debug.Log(netPacket.jsonString);
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

