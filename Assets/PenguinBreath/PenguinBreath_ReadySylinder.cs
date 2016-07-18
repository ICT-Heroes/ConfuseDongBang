using UnityEngine;
using System.Collections;

public class PenguinBreath_ReadySylinder : MonoBehaviour {

	private float timer = 0;
	private float maxTimer = 0.7f;
	public GameObject realCylinder;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		realCylinder.GetComponent<MeshRenderer> ().material.SetColor ("_TintColor", new Color (255, 0, 0, (1 - (timer / maxTimer)) * 0.5f));
		if (maxTimer < timer) {
			Destroy (gameObject);
		}
	}
}
