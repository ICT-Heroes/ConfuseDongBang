using System.IO;
using UnityEngine;
using System.Collections;

public class UserData {

	static public bool LogIn(string userId, string userPassword, out PlayerData data) {
		FileInfo info = null;
		StreamReader reader = null;
		data = new PlayerData();
		string text = "";

		if (Directory.Exists("\\UserID"))
			if (File.Exists("\\UserID\\" + userId + ".user_id")) {

				info = new FileInfo("\\UserID\\" + userId + ".user_id");
				reader = info.OpenText();
				while (true) {
					text = reader.ReadLine();
					if (text == null) break;
					if (text.Equals(userPassword)) {
						//아이디와 패스워드가 모두 일치할 때 데이터를 저장한다.
						data.SetNickName(reader.ReadLine());
						StringParser.IsMyString(reader.ReadLine(), out data.pos);
						StringParser.IsMyString(reader.ReadLine(), out data.rot);
						return true;
					}
				}
				reader.Close();
			}

		return false;
	}

	static public bool Create(string userId, string pass, string nick) {
		if (Directory.Exists("\\UserID"))
			if (File.Exists("\\UserID\\" + userId + ".user_id"))
				return false;
		PlayerData newData = new PlayerData(userId, pass, nick);
		Save(newData);
		return true;
	}

	static public void Save(PlayerData data) {
		if (!Directory.Exists("\\UserID"))
			Directory.CreateDirectory("\\UserID");

		//기존의 유저데이타를 지운다.
		if (File.Exists("\\UserID\\" + data.GetUserId() + ".user_id"))
			File.Delete("\\UserID\\" + data.GetUserId() + ".user_id");

		//새 데이타를 만든다.
		StreamWriter writer = File.CreateText("\\UserID\\" + data.GetUserId() + ".user_id");
		writer.WriteLine(data.GetPassword());
		writer.WriteLine(data.GetNickName());
		writer.WriteLine(StringParser.ToString(data.pos));
		writer.WriteLine(StringParser.ToString(data.rot));
		writer.Close();
	}

}
