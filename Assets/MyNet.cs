using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class MyNet {
	/// <summary>
	/// udp 로 모든 클라이언트에게 보내고 싶을 때 사용.
	/// </summary>
	/// <param name="str"></param>
	public static void SendUdp(NetString str) {
		UdpNet.Send(str);
	}

	/// <summary>
	/// udp 로 특정 id 의 클라이언트에게 보내고 싶을 때 사용.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="str"></param>
	public static void SendUdp(int id, NetString str) {
		UdpNet.Send(id, str);
	}

	/// <summary>
	/// tcp 로 모든 클라이언트에게 보내고 싶을 때 사용.
	/// </summary>
	/// <param name="str"></param>
	public static void SendTcp(NetString str) {
		_MyNet.TcpSendClients(str);
	}

	/// <summary>
	/// tcp 로 특정 id 의 클라이언트에게 보내고 싶을 때 사용.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="str"></param>
	public static void SendTcp(int id, NetString str) {
		TcpStringWriter.SendForId(id, str);
	}
}

class Received {
	public static LocalQueue<NetString> tcp = new LocalQueue<NetString>();
	public static LocalQueue<NetString> udp = new LocalQueue<NetString>();
}

class _MyNet {
	public static int TcpPort = 1900;
	public static int UdpPort = 1138;

	private static int idCount = 0;
	private static bool[] bID = new bool[2500];
	public static TcpListener tcpListener = null;
	public static bool isClientOver = true;
	public static ClientSet senderManager = new ClientSet();

	public static string stateMessage = "";
	public static bool isServerRun = true;

	public static int PopID() {
		int index = 0;
		while (index++ < 2500) {
			idCount++;
			if (!bID[idCount]) {
				bID[idCount] = true;
				return idCount;
			}
			if (idCount >= 2500 - 10) {
				idCount = 0;
			}
		}
		return -1;
	}

	//다쓴 아이디를 리턴하여 다시 사용할 수 있는 아이디로 만든다.
	public static void ReturnID(int id) {
		if (bID[id])
			bID[id] = false;
	}

	public static void TcpSendClients(NetString message) {
		TcpStringWriter.messageBuffer.AddLast(message);
	}

	public static void StartServer() {
		_MyNet server = new _MyNet();
		for (int i = 0; i < bID.Count(); i++) bID[i] = false;
		Thread thread = new Thread(server.Run);
		thread.Start();
	}

	public static void StopServer() {
		isServerRun = false;
		UdpNet.StopThread();
	}


	public void Run() {
		//수신할 서버 소켓을 연다.
		IPEndPoint ipep = new IPEndPoint(IPAddress.Any, TcpPort);
		tcpListener = new TcpListener(ipep);
		tcpListener.Start();

		//Tcp 송신 쓰레드를 시작한다.
		TcpStringWriter writer = new TcpStringWriter();
		Thread writherSendingThread = new Thread(writer.SendString);
		writherSendingThread.Start();

		UdpNet.StartThread();

		//서버소켓에 연결하는 클라이언트가 생길 때마다 새로운 쓰레드를 만들어 열어준다.
		//그리고 수신한다.
		while (isServerRun) {
			if (isClientOver) {
				isClientOver = false;
				AcceptSocket soc = new AcceptSocket();
				Thread thread = new Thread(soc.Accept);
				thread.Start();
			}
		}
	}
}


/// <summary>
/// 들어온 클라이언트를 연결한 후,
/// 클라이언트를 클라이언트 센더에 추가한다.
/// 그리고 클라이언트가 보내주는 것을 무한정 듣는다.
/// </summary>

class AcceptSocket {
	//들어온 클라이언트와 연결한다.
	public void Accept() {
		Socket clientsocket = null;
		NetworkStream stream = null;
		StreamReader reader = null;
		string clientEndpoint = "";
		try {
			clientsocket = _MyNet.tcpListener.AcceptSocket();
			stream = new NetworkStream(clientsocket);
			reader = new StreamReader(stream, System.Text.Encoding.UTF8);

			int id = _MyNet.PopID();
			clientEndpoint = ConvertEndPointToString(clientsocket.RemoteEndPoint.ToString());
			ClientSet.AddClient(clientEndpoint, id);
			_MyNet.isClientOver = true;
			while (true) {
				//클라이언트가 보내주는 것을 받는 곳.
				NetString str = NetString.Get(reader.ReadLine());
				str.id = id;
				if (str.param[0].Equals(Str.Flow.Exit)) break;
				Received.tcp.Push(str);
				Thread.Sleep(1);
			}
		} catch (Exception e) {
			Debug.Log(e.ToString());
		} finally {
			ClientSet.DeleteClient(clientEndpoint);
			clientsocket.Close();
		}
	}

	private string ConvertEndPointToString(string localEndPoint) {
		char[] ch = localEndPoint.ToCharArray();
		for (int i = 0; i < ch.Length; i++) {
			if (ch[i] == ':') {
				return localEndPoint.Substring(0, i);
			}
		}
		return "localhost";
	}
}

class UdpNet {
	private static Socket sendServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
	private static LocalQueue<NetString> buffer = new LocalQueue<NetString>();
	private static UdpNet myUdp;

	private static Thread udpRecieve, udpSend;

	public static void StartThread() {
		//스테틱 객체 생성.
		myUdp = new UdpNet();

		//수신 쓰레드를 시작한다.
		udpRecieve = new Thread(myUdp.Reciever);
		udpRecieve.Start();

		//송신 쓰레드를 시작한다.
		udpSend = new Thread(myUdp.Sender);
		udpSend.Start();
	}

	public static void StopThread() {
		if (udpRecieve.IsAlive) { udpRecieve.Abort(); }
		if (udpSend.IsAlive) udpSend.Abort();
	}

	private void Reciever() {
		int recv = 0;
		byte[] data = new byte[1024];

		IPEndPoint ep = new IPEndPoint(IPAddress.Any, _MyNet.UdpPort);
		Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		server.Bind(ep);

		IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
		EndPoint remoteEP = (EndPoint)sender;

		while (_MyNet.isServerRun) {
			recv = server.ReceiveFrom(data, ref remoteEP);
			Received.udp.Push(NetString.Get(Encoding.UTF8.GetString(data, 0, recv)));
			/*
			Console.WriteLine("[first] Message received from {0}", remoteEP.ToString());
			Console.WriteLine("[first] received data : {0}", Encoding.UTF8.GetString(data, 0, recv));
			*/
		}
		server.Close();
		sendServer.Close();
	}

	//보낼 메세지 버퍼에 추가하여, 자동으로 전송되도록한다.
	public static void Send(NetString message) {
		buffer.Push(message);
	}

	//보낼 메세지를 버퍼에 추가하지 않고 직접 해당 아이디로 전송한다.
	public static void Send(int id, NetString message) {
		myUdp.SendTo(id, message);
	}

	private void Sender() {
		while (_MyNet.isServerRun) {
			if(!buffer.IsEmpty())
				SendTo(buffer.Pop());
			Thread.Sleep(1);
		}
		Thread.Sleep(10);
	}

	private void SendTo(NetString message) {
		SendTo(message.GetString());
	}

	private void SendTo(int id, NetString message) {
		SendTo(id, message.GetString());
	}

	private void SendTo(string message) {
		byte[] data = new byte[1024];
		data = Encoding.UTF8.GetBytes(message);
		for (int i = 0; i < ClientSet.clients.Count; i++) {
			sendServer.SendTo(data, ClientSet.clients.ElementAt(i).endPoint);
		}
	}

	private void SendTo(int id, string message) {
		byte[] data = new byte[1024];
		data = Encoding.UTF8.GetBytes(message);
		for (int i = 0; i < ClientSet.clients.Count; i++) {
			if (ClientSet.clients.ElementAt(i).GetId() == id) {
				sendServer.SendTo(data, ClientSet.clients.ElementAt(i).endPoint);
				break;
			}
		}
	}

}

//보낼 메세지를 쌓아두는 곳.
class TcpStringWriter {
	public static LinkedList<NetString> messageBuffer = new LinkedList<NetString>();

	//쓰레드로 돌림.
	public void SendString() {
		while (_MyNet.isServerRun) {
			_MyNet.stateMessage = "Clients Count : " + ClientSet.clients.Count;
			if (ClientSet.clients.Count > 0) {
				if (messageBuffer.Count > 0) {
					NetString str = messageBuffer.ElementAt(0);
					messageBuffer.RemoveFirst();
					for (int i = 0; i < ClientSet.clients.Count; i++) {
						ClientSet.clients.ElementAt(i).Push(str);
					}
				}
			}
		}
	}

	//특정 id 의 sender 에게만 보내고 싶을 때,
	public static void SendForId(int id, NetString message) {
		for (int i = 0; i < ClientSet.clients.Count; i++) {
			if (ClientSet.clients.ElementAt(i).GetId() == id) {
				ClientSet.clients.ElementAt(i).Push(message);
				break;
			}
		}
	}
}

//연결 들어온 클라이언트들을 모아두는 곳
class ClientSet {
	public static LinkedList<TcpSender> clients = new LinkedList<TcpSender>();

	public static void AddClient(string hostAddress, int id) {
		TcpSender sender = new TcpSender(id);
		sender.SetHostAddress(hostAddress);
		clients.AddLast(sender);
		Thread senderThread = new Thread(sender.Run);
		senderThread.Start();
	}

	public static void DeleteClient(string hostAddress) {
		for (int i = 0; i < clients.Count; i++) {
			if (clients.ElementAt(i).GetHostAddress().Equals(hostAddress)) {
				clients.ElementAt(i).ExitSending();
				clients.Remove(clients.ElementAt(i));
				break;
			}
		}
	}
}


class TcpSender {
	private string hostAddress = "localhost";
	public LinkedList<NetString> localSendBuffer = new LinkedList<NetString>();
	public EndPoint endPoint;			//udp 전용.
	private bool exit = false;
	private TcpClient client = null;	//원래 있는 클래스
	private NetworkStream writeStream = null;
	private int id = -1;

	public TcpSender(int id) {
		this.id = id;
	}

	public string GetHostAddress() {
		return hostAddress;
	}

	public void SetHostAddress(string host) {
		hostAddress = host;
		IPEndPoint sender = new IPEndPoint(IPAddress.Parse(host), _MyNet.UdpPort);
		endPoint = (EndPoint)sender;
	}

	public void ExitSending() {
		exit = true;
	}

	public int GetId() {
		return id;
	}

	public void Push(NetString message) {
		if (localSendBuffer.Count < 1000) {
			localSendBuffer.AddLast(message);
		}
	}

	private NetString Pop() {
		if (localSendBuffer.Count != 0) {
			NetString str = localSendBuffer.ElementAt(0);
			localSendBuffer.RemoveFirst();
			return str;
		}

		//종료시키는 스트링
		NetString ret = new NetString(-10, "exit");
		return ret;
	}

	private bool IsEmpty() {
		return !(localSendBuffer.Count > 0);
	}

	public void Run() {
		client = new TcpClient();
		client.Connect(hostAddress, 1901);
		writeStream = client.GetStream();

		try {

			//클라이언트에게 아이디를 보내줌
			//클라이언트는 받은 아이디를 자신의 아이디로 세팅함.
			NetString sendString = new NetString(id, Str.Login.SetID, id + "");
			Debug.Log(sendString.GetString());
			string str = sendString.GetString() + "\r\n";
			byte[] data = Encoding.UTF8.GetBytes(str);
			writeStream.Write(data, 0, data.Length);

			while (!exit) {
				if (!IsEmpty()) {
					str = Pop().GetString() + "\r\n";
					data = Encoding.UTF8.GetBytes(str);
					writeStream.Write(data, 0, data.Length);
					if (str.Equals("-10,exit;\r\n")) exit = true;
				}
			}
			if (exit) {
				data = Encoding.UTF8.GetBytes("-10,exit;\r\n");
				writeStream.Write(data, 0, data.Length);

			}
		} catch (Exception ex) {
			//Console.WriteLine(ex.ToString());
			Debug.Log(ex.ToString());
		} finally {
			client.Close();
			_MyNet.ReturnID(id);
		}
	}
}


public class ReceiveBuffer {
	//인풋이 들어오면 여기에 그 값이 저장되어있음
	public static POINT mPoint;

	private const uint LBUTTONDOWN = 0x00000002;
	private const uint LBUTTONUP = 0x00000004;
	private const uint RBUTTONDOWN = 0x00000008;
	private const uint RBUTTONUP = 0x00000010;
	private const int KEYDOWN = 0x00000000;
	private const int KEYUP = 0x00000002;

	public struct POINT {
		public Int32 x;
		public Int32 y;
	}

	[DllImport("user32.dll")]
	static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);
	[DllImport("user32.dll")]
	static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
	static extern void SetCursorPos(uint X, uint Y);
	[DllImport("user32.dll")]
	static extern Int32 GetCursorPos(out POINT pt);


	/*
    * 네트워크를 통해 인풋을 받게 되면
    * receive 변수에 어떤 인풋이 들어왔는지 저장이 되고,
    * 이 함수가 실행됨.
    */
	public static void Received(string buffer) {
		Debug.Log("fdsa");
	}

	public static void MoveCursor(int x_, int y_) {
		GetCursorPos(out mPoint);
		double CursorX = mPoint.x + x_;
		double CursorY = mPoint.y + y_;
		SetCursorPos((uint)CursorX, (uint)CursorY);
	}
}
