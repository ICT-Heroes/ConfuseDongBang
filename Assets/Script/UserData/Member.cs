using System;
using UnityEngine;

namespace PenguinModel
{
	public class Member
	{
		public Member(string id, Vector3 pos, string password, string nickname, string emailAddress, bool isAdmin, string regDate, string lastLogin)
		{
			this.Id = id;
			this.position = pos;
			this.Password = password;
			this.EmailAddress = emailAddress;
			this.Nickname = nickname;
			this.isAdmin = isAdmin;
			this.regDate = regDate;
			this.lastLogin = lastLogin;
		}
		public string id;
		private string Id{
			get{ return id;}
			set{ id = value;}
		}
		public string password;
		private string Password{
			get{ return password;}
			set{ password = value; }
		}
		public string nickname;
		private string Nickname { 
			get{ return nickname; } 
			set{ nickname = value; }
		}
		public string emailAddress;
		private string EmailAddress { 
			get{ return emailAddress; } 
			set{ emailAddress = value; }
		}
		public Vector3 position{ get; set;}
		public bool isAdmin{ get; set; }
		public string regDate{ get; set; }
		public string lastLogin{get;set;}
	}


}

