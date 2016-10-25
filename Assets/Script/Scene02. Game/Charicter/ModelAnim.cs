using UnityEngine;
using System.Collections;

public class ModelAnim : MonoBehaviour {

	public enum Anim {
		attack0, attack1, attack2, jump, stand, run, dash, dead, hit, air
	}

	public Animator anim;

	public void SetAnim(Anim anim) {
		StartCoroutine(SetBlinkAnimToTrue(GetAnimName(anim)));
	}

	private IEnumerator SetBlinkAnimToTrue(string animName) {
		anim.SetBool(animName, true);
		yield return null;
		anim.SetBool(animName, false);
		yield return null;
	}

	public void Attack_event(int num) {
		Debug.Log("attack event : " + num);
	}

	public void Attack_end(int num) {
		Debug.Log("attack end : " + num);
	}

	private string GetAnimName(Anim anim) {
		switch (anim) {
			case Anim.attack0: return "attack0";
			case Anim.attack1: return "attack1";
			case Anim.attack2: return "attack2";
			case Anim.dash: return "dash_start";
			case Anim.dead: return "dead";
			case Anim.hit: return "damage";
			case Anim.jump: return "jump_start";
			case Anim.run: return "run";
			case Anim.stand: return "stand";
			case Anim.air: return "air";
		}
		return "stand";
	}

}
