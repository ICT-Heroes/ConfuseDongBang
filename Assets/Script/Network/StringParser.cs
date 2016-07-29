using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

public class StrProtocol {
	public class Login {
		public const string SetID = "logSetId";
		public const string Confirm = "logConfirm";
		public const string Create = "logCreate";
		public const string Success = "logs";
		public const string Fail = "logf";
		public const string CreateSuccess = "logCs";
		public const string CreateFail = "logCf";
	}

	public class Flow {
		public const string Exit = "exit";
	}

	public class Require {
		public const string Id = "reqId";
	}

	public class State {
		public const string Create = "stateCrt";
		public const string Position = "statePos";
		public const string Rotation = "stateRot";
	}

	public class Character {
		public const string Penguin = "charPenguin";
	}



	public static bool EqualName(ServerNetwork.ServerString str, string parse) {
		return str.param[0].Equals(parse);
	}
}

class MyQueue<Tem> {
	public LinkedList<Tem> tem = new LinkedList<Tem>();
	public bool IsEmpty() {
		return tem.Count <= 0;
	}
	public Tem Pop() {
		Tem ret = tem.First();
		tem.RemoveFirst();
		return ret;
	}
	public void Push(Tem t) {
		tem.AddLast(t);
	}
}

