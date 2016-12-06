using UnityEngine;
using PenguinModel;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;
using UserData;

public class LoginManager : MonoBehaviour {
    public static PlayerState playerState;
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

    public void OnJoinButtonTouched() {
		SceneManager.LoadScene("Scene02. CreateAccount");
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown("f")) {
			//network.CreateSelf("sample", transform.position);
		}
	}

    public void OnLoginButtonTouched() {
        PlayerState requestedPlayerState = new PlayerState();
        playerState = requestedPlayerState;
        SceneManager.LoadScene("Scene03. Game");
    }

    public void OnLoginButtonTouched_For_Implementation() {
        if (idComponent.text.Equals("")) {
            Debug.Log("아이디를 입력해 주세요");
        }else {
            if (passwordComponent.text.Equals("")) {
                Debug.Log("비밀번호를 입력해주세요");
            }else {
                Member member = new Member(idComponent.text);
                string jsonString = JsonUtility.ToJson(member);
                NetPacket netPacket = new NetPacket(ClassType.Member, 13579, EchoType.NotEcho, NetFunc.ReadMemberInfo, jsonString);
                RequestLoginProcess(netPacket);
            }
        }
    }

    void RequestLoginProcess(NetPacket netPacket) {
        StartCoroutine(NetworkFunctionLibrary.MakeWWWRequest(netPacket, CheckIdForLogin));
    }

    void CheckIdForLogin(WWW www) {
        if (www.error == null) {
            currentJsonString = www.text;

            NetPacket netPacket = JsonUtility.FromJson<NetPacket>(currentJsonString);
            Member member = JsonUtility.FromJson<Member>(netPacket.jsonString);
            if (member.id.Equals(idComponent.text)) {
                Debug.Log("접속 성공!.");
            } else {
                Debug.Log("존재하지 않는 아이디입니다.");
            }
        } else {
            Debug.Log("Server error :" + www.error);
        }
    }

    void RequestPlayerStateInfo(NetPacket netPacket) {
        StartCoroutine(NetworkFunctionLibrary.MakeWWWRequest(netPacket, CheckIdForLogin));
    }

    void InjectPlayerState(WWW www) {
        if (www.error == null) {
            currentJsonString = www.text;

            NetPacket netPacket = JsonUtility.FromJson<NetPacket>(currentJsonString);
            PlayerState requestedPlayerState = JsonUtility.FromJson<PlayerState>(netPacket.jsonString);
            playerState = requestedPlayerState;
        } else {
            Debug.Log("Server error :" + www.error);
        }
    }

}

