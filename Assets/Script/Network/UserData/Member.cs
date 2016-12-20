using System;
using UnityEngine;

namespace UserData
{
	public class Member
	{
		public Member(string id, int memberSrl, string password, string nickname, string emailAddress, bool isAdmin, string regDate, string lastLogin)
		{
			this.id = id;
			this.memberSrl = memberSrl;
			this.password = password;
			this.emailAddress = emailAddress;
			this.nickname = nickname;
			this.isAdmin = isAdmin;
			this.regDate = regDate;
			this.lastLogin = lastLogin;
		}
		public Member(string id)
		{
			this.id = id;
		}

		public string id;
		public int memberSrl;
		public string password;
		public string nickname;
		public string emailAddress;
		public bool isAdmin;
		public string regDate;
		public string lastLogin;
	}
}

