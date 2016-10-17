using System;
using UnityEngine;

namespace PenguinModel
{
	public class Member
	{
		public Member(string id, Vector3 pos, string password, string nickname, string emailAddress, bool isAdmin, string regDate, string lastLogin)
		{
			this.id = id;
			this.position = pos;
			this.password = password;
			this.emailAddress = emailAddress;
			this.nickname = nickname;
			this.isAdmin = isAdmin;
			this.regDate = regDate;
			this.lastLogin = lastLogin;
		}

		public string id;
		public string password;
		public string nickname;
		public string emailAddress;
		public Vector3 position;
		public bool isAdmin;
		public string regDate;
		public string lastLogin;
	}
}

