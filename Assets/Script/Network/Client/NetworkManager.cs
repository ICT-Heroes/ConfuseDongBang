using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientNetwork;
using System.Reflection;
using System;
using PenguinModel;

/// <summary>
/// 싱글톤이다.
/// </summary>
public class NetworkManager : MonoBehaviour {
	public string address = "192.168.0.2";
	public int ID = -1;
	public Dictionary<string, Transform> objects = new Dictionary<string, Transform>();
	public Transform[] prefabs;

	/// <summary>
	/// 싱글톤이다.
	/// </summary>
	public static NetworkManager instance;

	// Use this for initialization
	void Start () {

		instance = this;
		MyNet.serverAddress = address;
		MyNet.Start();
	}

	void Update() {
		while (ClientNetwork.Received.buffer.Count > 0) {
			string netPacketString = ClientNetwork.Received.buffer.Dequeue ();
			NetworkFunctionLibrary.instance.Analyze (netPacketString);
		}
	}




	void OnApplicationQuit() {
		Debug.Log("Stop Network");
		ClientNetwork.MyNet.Stop();
	}

	/// <summary>
	/// 남은 스트링배열을 리턴한다.
	/// </summary>
	/// <param name="param"></param>
	/// <param name="startIndex"></param>
	/// <returns></returns>
	private string[] RemainStr(string[] param, int startIndex) {
		string[] ret = new string[param.Length - startIndex];

		return ret;
	}
}




