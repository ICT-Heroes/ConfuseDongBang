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
				if(packet.ClientID != MyNet.myId)	Debug.Log(packet.ToString());
				if (packet.Func == NetFunc.SetId) {
					MyNet.myId = packet.ClientID;
					Debug.Log("내 아이디 바뀜 : " + MyNet.myId);
				}
			}

#if UNITY_ANDROID
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
			}
#endif
		}


#if UNITY_ANDROID && !UNITY_EDITOR
		/// <summary>
		/// 홈버튼 눌렀을 때
		/// </summary>
		/// <param name="pause"></param>
		void OnApplicationPause(bool pause) {
			if (pause) {
				OnApplicationQuit();
			}
		}
#endif

		/// <summary>
		/// 백버튼 눌렀을 때
		/// </summary>
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


