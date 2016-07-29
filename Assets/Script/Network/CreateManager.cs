using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateManager : MonoBehaviour {

	public Transform penguin;

	public List<Transform> players = new List<Transform>();

	private int sendCount = 0;

	public void Update() {
		if (!LocalDelegate.createPlayer.IsEmpty()) {
			CreatePlayer(LocalDelegate.createPlayer.Pop());
		}
		SendPos();
	}

	void SendPos() {
		sendCount++;
		if (sendCount >= 10) {
			sendCount = 0;
			for (int i = 0; i < players.Count; i++) {
				//각 플레이어의 위치를 전송해야 한다.
			}
		}
	}


	public void CreatePlayer(Param.CreatePlayer param) {
	}

	public void DeletePlayer(int id) {
	}
}
