using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
class PlayerAttack {
	public int clientID;
	public int damage;
	public int charic;
	public int atkNum;
	public Vec3 pos;
	public Vec3 target;

	public PlayerAttack(int clientID, CreateManager.Charic charic, int attackNum, int damage, Vec3 pos, Vec3 target) {
		atkNum = attackNum;
		this.charic = (int)charic;
		this.clientID = clientID;
		this.damage = damage;
		this.pos = pos;
		this.target = target;
	}

	public CreateManager.Charic GetCharic() {
		return (CreateManager.Charic)charic;
	}
}

