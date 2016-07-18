using UnityEngine;
using System.Collections;

public class StringParser {


	static public string ToString(Vector3 vec) {
		string ret = "v" + vec.x + "," + vec.y + "," + vec.z + ";";
		return ret;
	}
	static public bool IsMyString(string str, out Vector3 vec) {
		char[] ch = str.ToCharArray();
		float[] f = new float[6];
		if (ch[0] == 'v') {
			int fIndex = 1;
			int count1 = 0, count2 = 0;
			for (int i = 1; i < ch.Length; i++) {
				if (ch[i] == ',') {
					f[count1] = float.Parse(str.Substring(fIndex, i - fIndex));
					fIndex = i + 1;
					count1++;
				}
				if (ch[i] == ';') {
					count2++;
					break;
				}
			}
			if (count1 == 2 && count2 == 1) {
				vec = new Vector3(f[0], f[1], f[2]);
				return true;
			}
		}
		vec = new Vector3();
		return false;
	}

	static public string ToString(Quaternion quat) {
		string ret = "q" + quat.x + "," + quat.y + "," + quat.z + "," + quat.w + ";";
		return ret;
	}
	static public bool IsMyString(string str, out Quaternion quat) {
		char[] ch = str.ToCharArray();
		float[] f = new float[6];
		if (ch[0] == 'q') {
			int fIndex = 1;
			int count1 = 0, count2 = 0;
			for (int i = 1; i < ch.Length; i++) {
				if (ch[i] == ',') {
					f[count1] = float.Parse(str.Substring(fIndex, i - fIndex));
					fIndex = i + 1;
					count1++;
				}
				if (ch[i] == ';') {
					count2++;
					break;
				}
			}
			if (count1 == 3 && count2 == 1) {
				quat = new Quaternion(f[0], f[1], f[2], f[3]);
				return true;
			}
		}
		quat = new Quaternion();
		return false;
	}
}

public class Str {
	public class Login {
		public static string SetID = "logSetId";
		public static string Confirm = "logConfirm";
		public static string Create = "logCreate";
		public static string Success = "logs";
		public static string Fail = "logf";
		public static string CreateSuccess = "logCs";
		public static string CreateFail = "logCf";
	}

	public class Flow {
		public static string Exit = "exit";
	}

	public class Update {
		public static string pos = "upos";
		public static string rot = "urot";
	}

	public class Require {
		public static string id = "reqId";
	}

	public class Create {
		public static string player = "crtPlayer";
	}

	public class Character {
		public static string penguin = "charPenguin";
	}



	public static bool EqualName(NetString str, string parse) {
		return str.param[0].Equals(parse);
	}
}
