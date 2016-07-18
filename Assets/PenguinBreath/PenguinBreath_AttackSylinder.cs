using UnityEngine;
using System.Collections;

public class PenguinBreath_AttackSylinder : MonoBehaviour {

	float timer;
	float maxTimer;
	public float speed = 3;
	public int damage = 30;

	public int id = -100;

	void Start () {
		timer = 0;
		maxTimer = 1;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (maxTimer < timer)
			Destroy (gameObject);
		GetComponent<MeshRenderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (0, timer * speed));
	}

	void OnTriggerEnter(Collider coll){
	}
}
