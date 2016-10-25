﻿using UnityEngine;
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
				//if(packet.ClientID != MyNet.myId)	Debug.Log(packet.ToString());
				switch (packet.Func) {
					case NetFunc.Account:
						break;
					case NetFunc.ChangePlayerData:      //캐릭터 움직임
						if(packet.DataType == ClassType.PlayerState)
							CreateManager.instance.SetPos(packet.JsString);
						else
							CreateManager.instance.SetAnim(packet.JsString);
						break;
					case NetFunc.Create:				//캐릭터 생성
						CreateManager.instance.CreateCharicter(packet.JsString);
						break;
					case NetFunc.Exit:
						break;
					case NetFunc.Login:
						break;
					case NetFunc.SetId:					//처음 접속 시 내 아이디 셋팅
						MyNet.myId = packet.ClientID;
						Debug.Log("내 아이디 바뀜 : " + MyNet.myId);
						CreateManager.instance.myCharicter.id = MyNet.myId;
						CreateManager.instance.myCharicter.StartEndOfLoading();
						break;
				}
			}
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


