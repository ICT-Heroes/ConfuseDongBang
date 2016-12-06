using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
class PlayerAttack {
	public int memberSrl;
	public int damage;
	public int character;
	public int atkNum;
	public Vec3 pos;
	public Vec3 target;

	public PlayerAttack(int memberSrl, CreateManager.Character character, int attackNum, int damage, Vec3 pos, Vec3 target) {
		atkNum = attackNum;
		this.character = (int)character;
		this.memberSrl = memberSrl;
		this.damage = damage;
		this.pos = pos;
		this.target = target;
	}

	public CreateManager.Character GetCharacter() {
		return (CreateManager.Character)character;
	}
}

