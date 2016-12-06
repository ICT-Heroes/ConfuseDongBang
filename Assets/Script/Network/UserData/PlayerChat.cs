using System;
using UnityEngine;
using System.Text;

public class PlayerChat {
	public int memberSrl;
	public string nick;
	public string text;

	public PlayerChat(int memberSrl, string nick, string text) {
		this.memberSrl = memberSrl;
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
