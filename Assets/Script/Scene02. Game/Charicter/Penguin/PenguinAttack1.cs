using UnityEngine;
using System.Collections;

public class PenguinAttack1 : AttackBase {

	public GameObject obj;
	public MeshRenderer render;
	private float degree = 5;

	void Start() {
		render.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0));
		StartCoroutine(RollingCollider());
		transform.position += new Vector3(0, 0.15f, 0);
		obj.SetActive(false);
	}

	IEnumerator RollingCollider() {
		float time = 0;
		yield return new WaitForSeconds(0.2f);
		obj.SetActive(true);
		while (time < 0.085f) {
			time += Time.deltaTime;
			obj.transform.RotateAround(transform.position, transform.up, -Time.deltaTime * 2000);
			yield return null;
		}
		Destroy(obj.gameObject);
		time = 1;
		while (time > 0) {
			time -= Time.deltaTime;
			render.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, time));
			yield return null;
		}
		Destroy(gameObject);
	}
}

public class AttackBase : MonoBehaviour{
	public int damage;
	public int clientID;
	public void OnAttackEnter(Collider coll) {
		if (clientID != ClientNetwork.MyNet.myId) {
			if (coll.tag.Equals("Player")) {
				TestCube tc = coll.GetComponent<TestCube>();
				if (tc.id == ClientNetwork.MyNet.myId) {
					tc.Hp -= damage;
				}
			}
		}
	}
}
