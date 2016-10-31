using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillUI : MonoBehaviour {


	public static SkillUI instance;

	public Image[] images;
	public Image[] fillImages;
	private float[] timer;
	private float[] maxTimer;

	private bool[] s;

	public bool SkillUseAble(int num) {
		return s[num];
	}
	// Use this for initialization
	void Awake () {
		instance = this;
	}

	public void InitUI(CreateManager.Charic charic) {
		maxTimer = new float[3];
		timer = new float[3];
		s = new bool[3];
		for (int i = 0; i < 3; i++) {
			timer[i] = 0;
			s[i] = false;
		}
		switch (charic) {
			case CreateManager.Charic.penguin:
				images[0].sprite = Resources.Load<Sprite>("Image/penguin/penguin_attack0");
				images[1].sprite = Resources.Load<Sprite>("Image/penguin/penguin_attack1");
				images[2].sprite = Resources.Load<Sprite>("Image/penguin/jumpButton");
				maxTimer[0] = 3;
				maxTimer[1] = 1;
				maxTimer[2] = 1;
				s[2] = true;
				timer[2] = 1;
				break;
		}

		StartCoroutine(CoolTime(0));
		StartCoroutine(CoolTime(1));
		StartCoroutine(CoolTime(2));
	}

	public void OnButtonSkillTouch(int num) {
		if (s[num]) {
			s[num] = false;
			timer[num] = 0;
			StartCoroutine(CoolTime(num));
		}
	}

	IEnumerator CoolTime(int num) {
		while (timer[num] < maxTimer[num]) {
			timer[num] += Time.deltaTime;
			fillImages[num].fillAmount = 1 - (timer[num] / maxTimer[num]);
			yield return null;
		}
		s[num] = true;
	}
}
