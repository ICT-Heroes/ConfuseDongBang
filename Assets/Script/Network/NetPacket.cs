using UnityEngine;
using System.Collections;
using System;
using PenguinModel;


public enum NetFunc {
	Login, Account, ChangePlayerData
};

public enum EchoType {
	Echo, EchoType
}

public enum DataType{
    GameInfo,Member,PlayerData
}

public class NetPacket {
	private int clientID;
	private EchoType echoType;
	private NetFunc func;
	private string jsString;
    private DataType dataType;

	public NetPacket(DataType dataType, EchoType echoType, Func func, String jsString){
		this.dataType = dataType;
		this.echoType = echoType;
		this.func = func;
		this.jsString = jsString;
	}


    public DataType DataType{
        get
        {
            return dataType;
        }
        set
        {
            dataType = value;
        }
    }

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
		return string.Format ("{0},{1},{2},{3},{4}", DataType, ClientID, EchoType, Func, JsString);
	}

	public static NetPacket Parse(string str){
		str.Split (",");
		NetPacket netPacket = new NetPacket (str [0], str [1], str [2], str [3], str [4]);
	}
}
