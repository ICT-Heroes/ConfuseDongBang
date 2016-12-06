using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
class PlayerAttack {
	public int clientID;
	public int damage;
	public int character;
	public int atkNum;
	public Vec3 pos;
	public Vec3 target;

	public PlayerAttack(int clientID, CreateManager.Character character, int attackNum, int damage, Vec3 pos, Vec3 target) {
		atkNum = attackNum;
		this.character = (int)character;
		this.clientID = clientID;
		this.damage = damage;
		this.pos = pos;
		this.target = target;
	}

	public CreateManager.Character GetCharacter() {
		return (CreateManager.Character)character;
	}
}

