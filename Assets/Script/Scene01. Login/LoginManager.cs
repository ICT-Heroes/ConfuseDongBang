﻿using UnityEngine;
using PenguinModel;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {

	public InputField idComponent;
	public InputField passwordComponent;
	private string currentJsonString;
    readonly UTF8Encoding encoding = new UTF8Encoding();
    // Use this for initialization
    void Start() {
	}

	public  void GoGameScene() {
		SceneManager.LoadScene("Scene03. Game");
	}

    public void GoCreateAccountScene() {
		SceneManager.LoadScene("Scene02. CreateAccount");
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

