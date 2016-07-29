using UnityEngine;
using System.Collections;

public class SampleClient : FieldObject {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown("f")) {
			network.CreateSelf("sample", transform.position);
		}
	}

	public virtual void NetworkInit(params string[] param) {

	}
}

