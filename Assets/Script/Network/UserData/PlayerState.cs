using UnityEngine;
using System.Collections;
using System;

namespace PenguinModel {
    [Serializable]
    public class PlayerState {
        public int memberSrl;
        public Vec3 pos;
        public Quat rot;
        public int hp;
        public int maxHp;

        public PlayerState() {
            memberSrl = 0;
            pos = new Vec3();
            rot = new Quat();
            this.hp = 100;
            this.maxHp = 100;
        }

        public PlayerState(int memberSrl, Vector3 pos, Quaternion rot, int hp, int maxHp) {
            this.memberSrl = memberSrl;
            this.pos = new Vec3(pos);
            this.rot = new Quat(rot);
            this.hp = hp;
            this.maxHp = maxHp;
        }
    }
}