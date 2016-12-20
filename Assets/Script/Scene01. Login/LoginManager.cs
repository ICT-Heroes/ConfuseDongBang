using UnityEngine;
using UserData;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {
    public static PlayerState playerState;
    public static Member playerMemberInfo;
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

    public void Login(Member member) {
        PlayerState requestedPlayerState = new PlayerState(member.memberSrl, new Vector3(0,0,0), new Quaternion(0,0,0,1), 1000, 1000, CreateManager.Character.penguin, member.nickname);
        playerMemberInfo = member;
        playerState = requestedPlayerState;
        SceneManager.LoadScene("Scene03. Game");
    }

    public void OnLoginButtonTouched() {
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
                if (member.password.Equals(passwordComponent.text)) {
                    Debug.Log("접속 성공!.");
                    Login(member);
                } else {
                    Debug.Log("비밀번호가 잘못되었습니다.");
                   
                }
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

