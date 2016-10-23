using UnityEngine;

[System.Serializable]
public class Quat {
	public float x;
	public float y;
	public float z;
	public float w;

	public Quat(Quaternion quat) {
		x = quat.x;
		y = quat.y;
		z = quat.z;
		w = quat.w;
	}

	public Quat() {
		x = 0;
		y = 0;
		z = 0;
		w = 1;
	}
}
