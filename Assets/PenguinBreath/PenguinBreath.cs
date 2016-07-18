using UnityEngine;
using System.Collections;

public class PenguinBreath : MonoBehaviour {

	public Transform readyCylinder;
	public Transform attackCylinder;
	public Transform finishCylinder;
	private float timer = 0;
	private float maxTimer = 1;
	public int id = -100;


	bool attack = false;

	// Use this for initialization
	void Start () {
		Instantiate (readyCylinder, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (0.7f < timer) {
			if (!attack) {
				Transform newAttack = (Transform)Instantiate (attackCylinder, transform.position, transform.rotation);
				newAttack.FindChild("realCylinder Outer").GetComponent<PenguinBreath_AttackSylinder> ().id = id;
				attack = true;
			}
		}
		if (1.7f < timer) {
			Instantiate (finishCylinder, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
