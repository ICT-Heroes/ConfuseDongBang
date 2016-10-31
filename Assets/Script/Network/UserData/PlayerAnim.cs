using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerAnim {
	public int anim;
	public int clientId;

	public PlayerAnim(int id, ModelAnim.Anim anim) {
		this.anim = ModelAnim.ConvertAnimToInt(anim);
		this.clientId = id;
	}
}
