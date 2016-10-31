using System;
using UnityEngine;
using System.Text;

public class PlayerChat {
	public int clientId;
	public string nick;
	public string text;

	public PlayerChat(int clientId, string nick, string text) {
		this.clientId = clientId;
		this.nick = nick;
		this.text = text;
	}

	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		sb.Append("[");
		sb.Append(nick);
		sb.Append("] ");
		sb.Append(text);
		return sb.ToString();
	}
}
