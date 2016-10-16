using System;
using UnityEngine;

namespace PenguinModel
{
	public class Member
	{
		public Member(string id, Vector3 pos, string password, string nickname, string emailAddress, bool isAdmin, string regDate, string lastLogin)
		{
			this.Id = id;
			this.Position = pos;
			this.Password = password;
			this.EmailAddress = emailAddress;
			this.Nickname = nickname;
			this.IsAdmin = isAdmin;
			this.RegDate = regDate;
			this.LastLogin = lastLogin;
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
		private Vector3 position;
		public Vector3 Position { 
			get{ return position; }
			set{ position = value; }
		}
		private bool isAdmin;
		public bool IsAdmin { 
			get{ return isAdmin; } 
			set{ isAdmin = value; }
		}
		private string regDate;
		public string RegDate {
			get{ return regDate; } 
			set{ regDate = value; }
		}
		private string lastLogin;
		public string LastLogin {
			get{ return lastLogin; }
			set{ lastLogin = value; }
		}
	}


}

