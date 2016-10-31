using UnityEngine;
using System.Collections;

public class PenguinAttack1_Collider : MonoBehaviour {
	public AttackBase myBase;
	public Transform effect;

	void OnTriggerEnter(Collider coll) {
		Transform newT =  (Transform)Instantiate(effect, transform.position, Quaternion.identity);
		newT.forward = transform.forward;
		Destroy(newT.gameObject, 0.95f);
		myBase.OnAttackEnter(coll);
	}
}
