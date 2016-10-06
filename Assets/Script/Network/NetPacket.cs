using UnityEngine;
using System.Collections;
using System;



public enum NetFunc {
	LOGIN, ACCOUNT, EXIT, SETID
};

public enum EchoType {
	ECHO, NOT_ECHO
}

public class NetPacket {
	private int clientID;
	private EchoType echoType;
	private NetFunc func;
	private string jsString;

    public NetPacket() {
    }

    public NetPacket(int clientID, EchoType type, NetFunc func, string jsString) {
        this.clientID = clientID;
        this.echoType = type;
        this.func = func;
        this.jsString = jsString;
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

    public static NetPacket Parse(string ss) {
        return new NetPacket();
    }
}
