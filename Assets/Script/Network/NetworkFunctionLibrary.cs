using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientNetwork;
using System.Reflection;
using System;
using PenguinModel;
using System.Text;


public class NetworkFunctionLibrary : MonoBehaviour {

	public IEnumerator MyFunc(string json) {
		Debug.Log ("fdsafdsa");
		yield return null;
	}

	public static NetworkFunctionLibrary instance;

	void Awake(){
		instance = this;
	
	}

    public delegate void EmptyVoid(WWW www);

    public static IEnumerator MakeWWWRequest(NetPacket netPacket, EmptyVoid func)
    {
        
        string netPacketString = JsonUtility.ToJson(netPacket);
        Debug.Log("js String : " + netPacketString);

        Dictionary<String, String> postHeader = new Dictionary<string, string>();
        postHeader.Add("Content-Type", "application/json");

        //WWWForm form = new WWWForm ();
        //form.AddField ("netPacket", jsonString);
        WWWForm wwwForm = new WWWForm();
        var formData = System.Text.Encoding.UTF8.GetBytes(netPacketString);
        wwwForm.AddField("netPacket", netPacketString, Encoding.UTF8);
        string url = MakeURL(netPacket.func);
        WWW www = new WWW(url, formData, postHeader);
        yield return www;
        if (www.isDone)
        {
            func(www);
        }
       
    }

    public static String MakeURL(NetFunc netFunc)
    {
        switch(netFunc)
        {
            case NetFunc.ReadMemberInfo:
                return HttpRequestData.PENGUIN_HOST + HttpRequestData.URL_READ_MEMBER_INFO;
            default:
                return "";
        }
    }

	/// <summary>
	/// 해석
	/// </summary>
	public void Analyze(string netPacketString)
	{	
		NetPacket netPacket = JsonUtility.FromJson<NetPacket>(netPacketString);
		Type classType = DataParser.getDataType(netPacket.classType);
		StartCoroutine(netPacket.func.ToString(), netPacket);
	}
	
	public IEnumerator Login(NetPacket np){
		yield return null;

		string url = "http://minus-one.co.kr/penguin/insertMemberInfo.php";

		Member member = JsonUtility.FromJson<Member> (np.jsonString);
		WWWForm wform = new WWWForm ();
		wform.AddField("id", member.id);
		wform.AddField ("password", member.password);

		WWW www = new WWW (url, wform);
		StartCoroutine (WaitForRequest (www));
	}

	IEnumerator WaitForRequest(WWW www) {
		yield return www;

		if (www.error == null) {
			NetPacket netPacket = JsonUtility.FromJson<NetPacket> (www.text);
			StartCoroutine (netPacket.func.ToString(), netPacket);
		} else {
			Debug.Log("Error!");
		}
	}
}