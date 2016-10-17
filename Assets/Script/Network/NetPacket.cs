using UnityEngine;
using System.Collections;
using System;
using PenguinModel;


public enum NetFunc {
	Login, Account, ChangePlayerData, Exit, SetId
};

public enum EchoType {
	Echo, NotEcho
}

public enum DataType{
    GameInfo,Member,PlayerState, None
}

public class NetPacket {
	private int clientID;
	private EchoType echoType;
	private NetFunc func;
	private string jsString;
    private DataType dataType;

	public NetPacket(DataType dataType,int clientID, EchoType echoType, NetFunc func, String jsString){
		this.dataType = dataType;
		this.clientID = clientID;
		this.echoType = echoType;
		this.func = func;
		this.jsString = jsString;
	}


    public DataType DataType {
		get {
			return dataType;
		}
		set {
			dataType = value;
		}
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

    public int ClientID {
		get {
			return clientID;
		}

		set {
			clientID = value;
		}
	}

	public EchoType EchoType {
		get {
			return echoType;
		}

		set {
			echoType = value;
		}
	}

	public NetFunc Func {
		get {
			return func;
		}

		set {
			func = value;
		}
	}

	public string JsString {
		get {
			return jsString;
		}

		set {
			jsString = value;
		}
	}

	public override string ToString ()
	{
		return string.Format ("{0},{1},{2},{3},{4}", (int)DataType, ClientID, (int)EchoType, (int)Func, JsString);
	}

	public static NetPacket Parse(string str){
        string[] ss = str.Split (',');
        NetPacket netPacket = new NetPacket ((DataType)int.Parse(ss[0]), int.Parse(ss[1]), (EchoType)int.Parse(ss[2]), (NetFunc)int.Parse(ss[3]), ss[4]);
        return netPacket;
	}
}
