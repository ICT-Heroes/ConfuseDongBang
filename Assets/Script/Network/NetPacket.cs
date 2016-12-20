using UnityEngine;
using System.Collections;
using System;
using UserData;


public enum NetFunc {
	Login, Account, ChangePlayerData, Exit, SetId, Success, Failed, Create, RequireOtherPlayer, Chat, Attack, ReadMemberInfo, CreateMemberInfo
};

public enum EchoType {
	Echo, NotEcho
}

public enum ClassType
{
    GameInfo, Member, PlayerState, None, PlayerAnim, PlayerChat, PlayerAttack
}

public class NetPacket {
	public int memberSrl;
	public EchoType echoType;
	public NetFunc func;
    public ClassType classType;
    public string jsonString;
	

    public NetPacket(ClassType classType,int memberSrl, EchoType echoType, NetFunc func, String jsonString){
		this.classType = classType;
		this.memberSrl = memberSrl;
		this.echoType = echoType;
		this.func = func;
		this.jsonString = jsonString;
	}


    public NetPacket() {
    }

	/*
    public NetPacket(int memberSrl, EchoType type, NetFunc func, string jsString) {
        this.memberSrl = memberSrl;
        this.echoType = type;
        this.func = func;
        this.jsString = jsString;
    }
	*/

	public override string ToString() {
		return string.Format("{0};{1};{2};{3};{4}", (int)classType, memberSrl, (int)echoType, (int)func, jsonString);
	}

	public static NetPacket Parse(string str){
        string[] ss = str.Split (';');
		NetPacket netPacket = new NetPacket ((ClassType)int.Parse(ss[0]), int.Parse(ss[1]), (EchoType)int.Parse(ss[2]), (NetFunc)int.Parse(ss[3]), ss[4]);
        return netPacket;
	}
}
