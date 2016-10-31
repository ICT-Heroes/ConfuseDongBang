using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientNetwork;
using System.Reflection;
using System;
using PenguinModel;


public class NetworkFunctionLibrary : MonoBehaviour {

	public IEnumerator MyFunc(string json) {
		Debug.Log ("fdsafdsa");
		yield return null;
	}

	public static NetworkFunctionLibrary instance;

	void Awake(){
		instance = this;
	
	}

	/// <summary>
	/// 해석
	/// </summary>
	public void Analyze(string netPacketString)
	{	
		NetPacket netPacket = JsonUtility.FromJson<NetPacket>(netPacketString);
		Type dataType = DataParser.getDataType(netPacket.classType);
		StartCoroutine(netPacket.func.ToString(), netPacket);
	}
	
	public IEnumerator Login(NetPacket np){
		yield return null;

		string url = "http://minus-one.co.kr/penguin/insertMemberInfo.php";

		Member member = JsonUtility.FromJson<Member> (np.jsString);
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