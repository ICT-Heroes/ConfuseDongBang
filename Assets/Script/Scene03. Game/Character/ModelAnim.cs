using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelAnim : MonoBehaviour {

	public enum Anim {
		attack0, attack1, attack2, jump, stand, run, dash, dead, hit, air
	}


	public TestCube mother;
	public Animator anim;
	private Anim exAnim = Anim.stand;
	private bool attacking = false;
	public bool Attacking {
		get { return attacking; }
	}

	private Queue<Struct_Anim> q = new Queue<Struct_Anim>();

	void Start() {
		StartCoroutine(AnimDequeue());
	}

	public void SetAnim(Anim anim) {
		if (exAnim != anim) {
			exAnim = anim;
			if (!attacking) {
				if (anim == Anim.attack0 || anim == Anim.attack1 || anim == Anim.attack2) attacking = true;
				if (q.Count > 1) {
					q.Clear();
				}
				Struct_Anim local = new Struct_Anim();
				local.anim = anim;
				q.Enqueue(local);
			}
		}
	}

	private IEnumerator AnimDequeue() {
		while (true) {
			if (q.Count == 0) {
				yield return null;
			} else {
				Struct_Anim local = q.Dequeue();
				StartCoroutine(SetBlinkAnimToTrue(GetAnimName(local.anim)));
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	private IEnumerator SetBlinkAnimToTrue(string animName) {
		anim.SetBool(animName, true);
		yield return null;
		anim.SetBool(animName, false);
		yield return null;
	}

	/// <summary>
	/// 애니메이션이 콜을 날려줌
	/// </summary>
	public void Attack_event(int num) {
		mother.AttackEvent(num);
	}

	/// <summary>
	/// 애니메이션이 콜을 날려줌
	/// </summary>
	public void Attack_end(int num) {
		attacking = false;
		exAnim = Anim.stand;
		Struct_Anim local = new Struct_Anim();
		local.anim = exAnim;
		q.Enqueue(local);
		mother.AttackEnd(num);
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

	public static int ConvertAnimToInt(Anim anim) {
		switch (anim) {
			case Anim.attack0: return 0;
			case Anim.attack1: return 1;
			case Anim.attack2: return 2;
			case Anim.dash: return 3;
			case Anim.dead: return 4;
			case Anim.hit: return 5;
			case Anim.jump: return 6;
			case Anim.run: return 7;
			case Anim.stand: return 8;
			case Anim.air: return 9;
		}
		return 8;
	}

	public static Anim ConvertIntToAnim(int anim) {
		switch (anim) {
			case 0: return Anim.attack0;
			case 1: return Anim.attack1;
			case 2: return Anim.attack2;
			case 3: return Anim.dash;
			case 4: return Anim.dead;
			case 5: return Anim.hit;
			case 6: return Anim.jump;
			case 7: return Anim.run;
			case 8: return Anim.stand;
			case 9: return Anim.air;
		}
		return Anim.stand;
	}

	private struct Struct_Anim {
		public Anim anim;
	}

}
