using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UserData;

public class CreateManager : MonoBehaviour {

	public enum Character {
		penguin
	}

	public static CreateManager instance;

	public TestCube myCharacter;
	public Transform testCube;
	public AttackTransform penguinAttack;



	public Dictionary<int, TestCube> characters = new Dictionary<int, TestCube>();

	void Awake() {
		instance = this;
	}

	/// <summary>
	/// 네트워크로 나 이외에 누군가가 들어왔을 때 그 객체를 생성한다
	/// </summary>
	public void CreateCharacter(string jsString) {
		PlayerState state = JsonUtility.FromJson<PlayerState>(jsString);
		if (state.memberSrl != ClientNetwork.MyNet.myId) {
			if (characters.ContainsKey(state.memberSrl)) return; //이미 생성된 객체에 대해서는 한번 더 생성하지 않는다.
			Transform newC = (Transform)Instantiate(testCube, state.pos.ToVector3(), state.rot.ToQuaternion());
			TestCube tc = newC.GetComponent<TestCube>();
			tc.SetID(state.memberSrl);
			characters.Add(state.memberSrl, tc);
			Debug.Log("다른놈 캐릭터 생성 : " + state.memberSrl);
		}
	}

	public void CreateAttack(string jsString) {
		PlayerAttack atk = JsonUtility.FromJson<PlayerAttack>(jsString);
		Transform newT = (Transform)Instantiate(GettAttackTransform(atk.GetCharacter(), atk.atkNum), atk.pos.ToVector3(), Quaternion.identity);
		AttackBase attackBase = newT.GetComponent<AttackBase>();
		attackBase.damage = atk.damage;
		attackBase.memberSrl = atk.memberSrl;
		newT.LookAt(atk.target.ToVector3());
	}

	private Transform GettAttackTransform(CreateManager.Character character, int num) {
		switch (character) {
			case Character.penguin:
				if (num == 0) return penguinAttack.atk0;
				if (num == 1) return penguinAttack.atk1;
				if (num == 2) return penguinAttack.atk2;
				break;
		}
		return penguinAttack.atk1;
	}

	/// <summary>
	/// 누군가가 접속을 끊었을 때,
	/// </summary>
	public void DeleteCharacter(int id) {
		if (id != ClientNetwork.MyNet.myId) {
			characters.Remove(id);
		}
	}


	/// <summary>
	/// 네트워크로 받은 나 이외의 다른 아이디의 캐릭터를 움직인다.
	/// </summary>
	public void SetPos(string jsString) {
		PlayerState state = JsonUtility.FromJson<PlayerState>(jsString);
		if (state.memberSrl != ClientNetwork.MyNet.myId) {
			TestCube cube;
			if (characters.TryGetValue(state.memberSrl, out cube)) {
				cube.SetPos(state.pos.ToVector3(), state.rot.ToQuaternion(), state.hp, state.maxHp);
			} else {
				NetPacket packet = new NetPacket(ClassType.PlayerState, ClientNetwork.MyNet.myId, EchoType.NotEcho, NetFunc.RequireOtherPlayer, jsString);
				ClientNetwork.MyNet.Send(packet);
			}
		}
	}

	/// <summary>
	/// 네트워크로 받은 나 이외의 다른 아이디의 캐릭터의 애니메이션을 바꾼다.
	/// </summary>
	public void SetAnim(string jsString) {
		PlayerAnim state = JsonUtility.FromJson<PlayerAnim>(jsString);
		if (state.memberSrl != ClientNetwork.MyNet.myId) {
			TestCube cube;
			if (characters.TryGetValue(state.memberSrl, out cube)) {
				cube.SetAnim(ModelAnim.ConvertIntToAnim(state.anim));
			}
		}
	}

	/// <summary>
	/// UI 를 통해 안드로이드에서 공격버튼을 눌렀을 때 발동.
	/// </summary>
	/// <param name="num"></param>
	public void OnButtonAttack(int num) {
		if (num == 2) {
			myCharacter.Jump();
		} else {
			myCharacter.SendAttack(num);
		}
	}


}

[Serializable]
public class AttackTransform{
	public Transform atk0, atk1, atk2;
}