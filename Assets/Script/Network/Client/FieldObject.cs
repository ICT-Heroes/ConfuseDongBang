using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldObject : MonoBehaviour {
	protected float hp;
	protected float fullHP = 100;
	protected int fieldObjectId;

	public ClientObject network;

	protected SpriteRenderer spriteRenderer;

	protected virtual void OnAwake(){
		hp = fullHP;
		network.gameObject = gameObject;
	}

	protected void Awake(){
		OnAwake();
	}

	/// <summary>
	/// 네트워크를 통해 들어온 파라미터를 이용하여 초기화.
	/// </summary>
	/// <param name="param"></param>
	public virtual void NetworkInit(params string[] param) {

	}

	public virtual void OnAttack(Collider2D col){//공격시 콜

	}


	public virtual void OnHit() {
		hp -= 21;
		if (hp < 0) OnDeath();
		spriteRenderer.color = new Color(1, hp / fullHP, hp / fullHP);
		//Debug.Log(gameObject.name + " is Hit");
	}

	public virtual void OnHit(float damage) {
		hp -= damage;
		if (hp < 0) OnDeath();
		spriteRenderer.color = new Color(1, hp / fullHP, hp / fullHP);
		//Debug.Log(gameObject.name + " is Hit");
	}

	public virtual void OnDeath(){
		Destroy(gameObject);
	}
}