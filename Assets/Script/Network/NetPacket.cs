using UnityEngine;
using System.Collections;
using System;
using PenguinModel;


public enum NetFunc {
	Login, Account, ChangePlayerData, Exit, SetId, Success, Failed
};

public enum EchoType {
	Echo, NotEcho
}

public enum DataType{
    GameInfo,Member,PlayerState, None
}

public class NetPacket {
    public int clientId {get; set;}
	public EchoType echoType { get; set; }
    public NetFunc func { get; set; }
    public string jsString { get; set; }
    public DataType dataType { get; set; }

    public NetPacket(DataType dataType,int clientID, EchoType echoType, NetFunc func, String jsString){
		this.dataType = dataType;
		this.clientId = clientID;
		this.echoType = echoType;
		this.func = func;
		this.jsString = jsString;
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


	public override string ToString ()
	{
		return string.Format ("{0},{1},{2},{3},{4}", (int)dataType, clientId, (int)echoType, (int)func, jsString);
	}

	public static NetPacket Parse(string str){
        string[] split = str.Split (',');
        NetPacket netPacket = new NetPacket ((DataType)int.Parse(split[0]), int.Parse(split[1]), (EchoType)int.Parse(split[2]), (NetFunc)int.Parse(split[3]), split[4]);
        return netPacket;
	}
}
