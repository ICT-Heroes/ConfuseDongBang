using UnityEngine;
using System.Collections;
using System;

namespace UserData {
	[Serializable]
	public class PlayerState {
		public int clientId;
		public Vec3 pos;
		public Quat rot;
		public int hp;
		public int maxHp;
		public int charKind;

		public PlayerState() {
			clientId = 0;
			pos = new Vec3();
			rot = new Quat();
			this.hp = 100;
			this.maxHp = 100;
			charKind = (int)CreateManager.Charic.penguin;
		}

		public PlayerState(int clientId, Vector3 pos, Quaternion rot, int hp, int maxHp, CreateManager.Charic charic) {
			this.clientId = clientId;
			this.pos = new Vec3(pos);
			this.rot = new Quat(rot);
			this.hp = hp;
			this.maxHp = maxHp;
			charKind = (int)charic;
		}

		public CreateManager.Charic GetCharic() {
			return (CreateManager.Charic)charKind;
		}
	}
}
