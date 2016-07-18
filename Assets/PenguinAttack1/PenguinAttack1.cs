using UnityEngine;
using System.Collections;

public class PenguinAttack1 : MonoBehaviour {


	float timer;
	float maxTimer;
	public int damage = 20;

	public int id = -100;

	public MeshRenderer render;

	// Use this for initialization
	void Start () {
		timer = 0;
		maxTimer = 1;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (maxTimer < timer)
			Destroy (transform.parent.gameObject);
		render.material.SetColor ("_TintColor", new Color (255, 255, 255, (1 - timer / maxTimer)));
	}

	void OnTriggerEnter(Collider coll){
	}
}
