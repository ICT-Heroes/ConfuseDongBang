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
	public void Analyze(string st)
	{
			/*
			NetPacket netPacket = JsonUtility.FromJson<NetPacket>(netPacketString);
			Type dataType = DataParser.getDataType(netPacket.DataType);
			StartCoroutine(netPacket.Func.ToString(), netPacket);
            */
	}


}
