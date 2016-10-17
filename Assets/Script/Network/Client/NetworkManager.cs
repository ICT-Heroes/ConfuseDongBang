using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientNetwork;
using System.Reflection;
using System;
using PenguinModel;

namespace ClientNetwork {

	public class NetworkManager : MonoBehaviour {
		public string serverAddress = "192.168.0.2";
		public int ID = -1;

		public static NetworkManager instance;

		// Use this for initialization
		void Start() {

			instance = this;
			MyNet.serverAddress = serverAddress;
			MyNet.Start();
		}

		void Update() {
			while (Received.GetCount() > 0) {
				NetPacket packet = Received.DequeueThreadSafe();
				Debug.Log(packet.ToString());
				if (packet.func == NetFunc.SetId) {
					MyNet.myId = packet.clientId;
					Debug.Log("내 아이디 바뀜 : " + MyNet.myId);
				}
			}
		}

		void OnApplicationQuit() {
			Debug.Log("Stop Network");
			MyNet.Stop();
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
}


