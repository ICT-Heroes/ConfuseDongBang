using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerState {
	public int clientId;
	public Vec3 pos;
	public Quat rot;

	public PlayerState() {
		clientId = 0;
		pos = new Vec3();
		rot = new Quat();
	}

	public PlayerState(int clientId, Vector3 pos, Quaternion rot) {
		this.clientId = clientId;
		this.pos = new Vec3(pos);
		this.rot = new Quat(rot);
	}
}
