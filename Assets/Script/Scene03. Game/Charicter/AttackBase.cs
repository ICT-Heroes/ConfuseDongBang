using UnityEngine;
using System.Collections;

public class AttackBase : MonoBehaviour {
	public int damage;
	public int clientID;
	public void OnAttackEnter(Collider coll) {
		OnAttackSomthingEventStart(coll);
		if (clientID != ClientNetwork.MyNet.myId) {
			if (coll.tag.Equals("Player")) {
				TestCube tc = coll.GetComponent<TestCube>();
				if (tc.id == ClientNetwork.MyNet.myId) {
					tc.Hp -= damage;
					OnAttackOtherCharicEvent();
				}
			}
		}
		OnAttackSomthingEventEnd(coll);
	}

	/// <summary>
	/// 내것과 충돌했을 때만 콜된다.
	/// 다른 플레이어와 다른 공격 오브젝트가 충돌했을 때는 콜되지 않는다.
	/// </summary>
	public virtual void OnAttackOtherCharicEvent() {

	}

	/// <summary>
	/// 나 이외에 뭐든 충돌되면 콜된다.
	/// </summary>
	public virtual void OnAttackSomthingEventStart(Collider coll) {

	}

	/// <summary>
	/// 나 이외에 뭐든 충돌되면 콜된다.
	/// </summary>
	public virtual void OnAttackSomthingEventEnd(Collider coll) {

	}

	public void CreateEffect(Transform effect, float destroyTime) {
		Transform ef = (Transform)Instantiate(effect, transform.position, Quaternion.identity);
		ef.forward = transform.forward;
		ef.GetComponent<EffectBase>().SetDestroyTimer(destroyTime);
	}
}

