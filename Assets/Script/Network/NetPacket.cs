using UnityEngine;
using System.Collections;
using System;
using PenguinModel;


public enum NetFunc {
	Login, Account, ChangePlayerData, Exit, SetId, Success, Failed, Create, RequireOtherPlayer, Chat, Attack
};

public enum EchoType {
	Echo, NotEcho
}

public enum ClassType
{
    GameInfo, Member, PlayerState, None, PlayerAnim, PlayerChat, PlayerAttack
}

public class NetPacket {
	public int clientId;
	public EchoType echoType;
	public NetFunc func;
	public string jsonString;
	public ClassType classType;

    public NetPacket(ClassType dataType,int clientID, EchoType echoType, NetFunc func, String jsonString){
		this.classType = classType;
		this.clientId = clientID;
		this.echoType = echoType;
		this.func = func;
		this.jsonString = jsonString;
	}


    public NetPacket() {
    }

	/*
    public NetPacket(int clientID, EchoType type, NetFunc func, string jsString) {
        this.clientID = clientID;
        this.echoType = type;
        this.func = func;
        this.jsString = jsString;
    }
	*/

	public override string ToString() {
		return string.Format("{0};{1};{2};{3};{4}", (int)classType, clientId, (int)echoType, (int)func, jsonString);
	}

	public static NetPacket Parse(string str){
        string[] ss = str.Split (';');
		NetPacket netPacket = new NetPacket ((ClassType)int.Parse(ss[0]), int.Parse(ss[1]), (EchoType)int.Parse(ss[2]), (NetFunc)int.Parse(ss[3]), ss[4]);
        return netPacket;
	}
}
