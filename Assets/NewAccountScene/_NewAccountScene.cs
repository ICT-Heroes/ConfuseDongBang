using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class _NewAccountScene : MonoBehaviour {


    public Text id, password, password2, nickName, state;

    public void OnButtonTouch() {
        if (id.text.Equals("") || password.text.Equals("") || password2.text.Equals("") || nickName.text.Equals(""))
        {
            state.text = "전부 적어주세요";
        }
        else {
            if (password.text.Equals(password2.text))
            {
                UserDataCreate();
            }
            else {
                state.text = "두 비밀번호가 다릅니다";
            }
        }
    }

    private void UserDataCreate() {
        //여기서 서버에 아이디를 만들어달라고 요청해야함.
        UserData.Create(id.text, password.text, nickName.text);
        Application.LoadLevel("LoginScene");
    }
}
