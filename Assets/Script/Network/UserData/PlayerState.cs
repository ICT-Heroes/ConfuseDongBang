using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerState {
	public int clientId;
	public Vec3 pos;
	public Quat rot;
	public int hp;
	public int maxHp;

	public PlayerState() {
		clientId = 0;
		pos = new Vec3();
		rot = new Quat();
		this.hp = 100;
		this.maxHp = 100;
	}

	public PlayerState(int clientId, Vector3 pos, Quaternion rot, int hp, int maxHp) {
		this.clientId = clientId;
		this.pos = new Vec3(pos);
		this.rot = new Quat(rot);
		this.hp = hp;
		this.maxHp = maxHp;
	}
}
