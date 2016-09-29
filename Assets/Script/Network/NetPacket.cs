using UnityEngine;
using System.Collections;
using System;
using PenguinModel;


public enum NetFunc {
	LOGIN, ACCOUNT
};

public enum EchoType {
	ECHO, NOT_ECHO
}

public enum DataType{
    GAME_INFO, MEMBER
}

public class NetPacket {
	private int clientID;
	private EchoType echoType;
	private NetFunc func;
	private string jsString;
    private DataType dataType;

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
}
