using UnityEngine;
using System.Collections;

public class PenguinAttack0 : AttackBase {

	public SphereCollider coll;
	public Transform effect0, effect1, effect2;

	void Start() {
		transform.position += new Vector3(0, 0.15f, 0);
		transform.localScale = Vector3.zero;
		StartCoroutine(update());
		Rigidbody rig = gameObject.AddComponent<Rigidbody>();
		rig.useGravity = false;
		rig.isKinematic = true;
		CreateEffect(effect0, 1);
	}

	IEnumerator update() {
		yield return new WaitForSeconds(0.5f);
		transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		CreateEffect(effect1, 1);
		while (true) {
			transform.position += transform.forward * Time.deltaTime * 4;
			yield return null;
		}
	}

	public override void OnAttackSomthingEventEnd(Collider coll) {
		if (coll.tag.Equals("Player")) {
			if (coll.GetComponent<TestCube>().id == memberSrl) return;
		}
		CreateEffect(effect2, 1);
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider coll) {
		OnAttackEnter(coll);
	}

}
