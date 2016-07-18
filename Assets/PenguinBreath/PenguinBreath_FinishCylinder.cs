using UnityEngine;
using System.Collections;

public class PenguinBreath_FinishCylinder : MonoBehaviour {

	public GameObject realCylinder;
	float timer;
	float maxTimer;
	float firstScale = 0.15f;
	public float speed = 3;

	void Start () {
		timer = 0;
		maxTimer = 0.5f;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (maxTimer < timer)
			Destroy (gameObject);
		realCylinder.GetComponent<MeshRenderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (0, timer * speed));
		realCylinder.GetComponent<MeshRenderer> ().material.SetColor ("_Color", new Color(2, 2, 2, (1 - timer/maxTimer)));
		realCylinder.transform.localScale = new Vector3 (firstScale * (1 - timer / maxTimer), 10, firstScale * (1 - timer / maxTimer));
	}
}
