using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerState {
	public int clientId;
	public Vector3 pos;
	public Quaternion rot;

	public PlayerState() {
		clientId = 0;
		pos = new Vector3();
		rot = new Quaternion();
	}

	public PlayerState(int clientId, Vector3 pos, Quaternion rot) {
		this.clientId = clientId;
		this.pos = pos;
		this.rot = rot;
	}
}
