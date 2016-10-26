using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;


namespace Chattings {

	public class Chatting : MonoBehaviour {

		public ChattingText chattingText;

		private List<PlayerChat> list = new List<PlayerChat>();

		public InputField field;

		public static Chatting instance;

		void Awake() {
			instance = this;
		}

		public void OnNetwork(string json) {
			PlayerChat playerChat = JsonUtility.FromJson<PlayerChat>(json);
			Print(playerChat);
		}

		public void OnWriteText(string cont) {
			Debug.Log("context : " + field.text);
			string context = field.text;
			if (!context.Equals("")) { 
				PlayerChat playerChat = new PlayerChat(ClientNetwork.MyNet.myId, "nick" + ClientNetwork.MyNet.myId, context);
				string json = JsonUtility.ToJson(playerChat);
				NetPacket packet = new NetPacket(ClassType.PlayerChat, ClientNetwork.MyNet.myId, EchoType.NotEcho, NetFunc.Chat, json);
				ClientNetwork.MyNet.Send(packet);
				field.text = "";
				field.Select();
				field.ActivateInputField();
			}
		}

		public void Print(PlayerChat chat) {
			list.Add(chat);
			if (list.Count > 13) {
				list.RemoveAt(0);
			}
			chattingText.Print(GetPrintString());
		}

		private string GetPrintString() {
			StringBuilder sb = new StringBuilder();
			for (int i = list.Count - 1; i >= 0; i--) {
				sb.Append(list[i].ToString());
				sb.Append("\n");
			}
			return sb.ToString();
		}


	}

}