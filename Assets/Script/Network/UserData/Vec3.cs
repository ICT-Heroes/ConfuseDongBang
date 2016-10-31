using UnityEngine;

[System.Serializable]
public class Vec3 {
	public float x;
	public float y;
	public float z;

	public Vec3(Vector3 vec) {
		x = vec.x;
		y = vec.y;
		z = vec.z;
	}

	public Vec3() {
		x = 0;
		y = 0;
		z = 0;
	}

	public void Copy(Vec3 v) {
		x = v.x;
		y = v.y;
		z = v.z;
	}

	public Vector3 ToVector3() {
		return new Vector3(x, y, z);
	}
}

