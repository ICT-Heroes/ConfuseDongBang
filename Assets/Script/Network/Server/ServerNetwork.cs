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


namespace ServerNetwork {
	public class Print {
		private static MyQueue<string> debug = new MyQueue<string>();
		private static MyQueue<string> state = new MyQueue<string>();

		public static string Debug {
			get {
				if (debug.IsEmpty()) return null;
				else return debug.Pop();
			}
			set {
				debug.Push(value);
			}

		}

		public static string State {
			get {
				if (state.IsEmpty()) return null;
				else return state.Pop();
			}
			set {
				state.Push(value);
			}
		}
	}

	public class MyNet {

		/// <summary>
		/// tcp 로 모든 클라이언트에게 보내고 싶을 때 사용.
		/// </summary>
		/// <param name="str"></param>
		public static void SendAll(ServerString str) {
			StringWriter.messageBuffer.AddLast(str);
		}

		/// <summary>
		/// tcp 로 특정 id 의 클라이언트에게 보내고 싶을 때 사용.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="str"></param>
		public static void Send(int id, ServerString str) {
			StringWriter.SendForId(id, str);
		}

		/// <summary>
		/// 생성자 없이 이걸로 시작한다.
		/// 윈도우가 생성된 후에 쓰레드가 시작되어야 하기 때문에 10ms 뒤에 시작한다.
		/// </summary>
		public static void Start() {
			Thread.Sleep(10);
			_MyNet.StartServer();
		}

		/// <summary>
		/// 파괴자 없이 이걸로 그만둔다.
		/// </summary>
		public static void Stop() {
			_MyNet.StopServer();
		}
	}

	class Received {
		public static MyQueue<ServerString> strs = new MyQueue<ServerString>();
	}

	/// <summary>
	/// 실제 연결부. 숨기기 위해 앞에 _ 를 붙였다.
	/// </summary>
	class _MyNet {
		public static int TcpPort = 1900;

		/// <summary>
		/// 연결이 하나 들어올 때마다 아이디 카운트를 하나씩 늘린다.
		/// 그런데 만약 bID[idCount]가 true 이면 하나를 더 더하여 false 인 곳을 찾는다.
		/// 그리고 연결된 클라이언트에게 해당 아이디를 넘겨준다.
		/// 이로써 클라이언트들은 전부 다른 id 를 부여받게 된다.
		/// idCount 가 2500 을 넘어가게 되면 0으로 되돌아온다.
		/// </summary>
		private static int idCount = 0;

		/// <summary>
		/// 연결이 하나 들어올 때마다 bID[idCount] 를 true 로 하고
		/// 어떤 클라이언트의 연결이 끊기면 bID[부여받은 idCount] 를 false 로 바꾼다.
		/// </summary>
		private static bool[] bID = new bool[2500];

		public static TcpListener tcpListener = null;
		public static bool isClientOver = true;
		public static ClientSet senderManager = new ClientSet();

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

		public static void SendClients(ServerString message) {

		}

		public static void StartServer() {
			Print.Debug = "Clients Count : " + ClientSet.clients.Count;
			_MyNet server = new _MyNet();
			for (int i = 0; i < bID.Count(); i++) bID[i] = false;
			Thread thread = new Thread(server.Run);
			thread.Start();
		}

		public static void StopServer() {
			isServerRun = false;
		}


		public void Run() {
			//수신할 서버 소켓을 연다.
			IPEndPoint ipep = new IPEndPoint(IPAddress.Any, TcpPort);
			tcpListener = new TcpListener(ipep);
			tcpListener.Start();

			//Tcp 송신 쓰레드를 시작한다.

			StringWriter writer = new StringWriter();
			Thread writherSendingThread = new Thread(writer.SendString);
			writherSendingThread.Start();

			//서버소켓에 연결하는 클라이언트가 생길 때마다 새로운 쓰레드를 만들어 열어준다.
			//그리고 수신한다.
			while (isServerRun) {
				if (isClientOver) {
					isClientOver = false;
					AcceptSocketListen soc = new AcceptSocketListen();
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
	class AcceptSocketListen {
		//들어온 클라이언트와 연결한다.
		public void Accept() {
			Socket clientsocket = null;
			NetworkStream stream = null;
			StreamReader reader = null;
			string clientEndpoint = "";
			int id = -1;
			try {
				//여기서 쓰레드가 남는다.
				//그래서 서버 프로그램 종료시 프로그램이 종료되지 않는다.
				//다른 클라이언트의 접속을 받는 부분이다.
				clientsocket = _MyNet.tcpListener.AcceptSocket();
				stream = new NetworkStream(clientsocket);
				reader = new StreamReader(stream, System.Text.Encoding.UTF8);

				id = _MyNet.PopID();
				clientEndpoint = ConvertEndPointToString(clientsocket.RemoteEndPoint.ToString());
				ClientSet.AddClient(clientEndpoint, id);
				_MyNet.isClientOver = true;
				while (true) {
					//클라이언트가 보내주는 것을 받는 곳.
					ServerString str = ServerString.Get(reader.ReadLine());
					str.id = id;
					if (str.param[0].Equals(StrProtocol.Flow.Exit)) break;
					Received.strs.Push(str);
					Debug.Log(str.GetString());
					Thread.Sleep(1);
				}
			} catch (Exception e) {
				Debug.Log(e.ToString());
			} finally {
				ClientSet.DeleteClient(id);
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

	/// <summary>
	/// 보낼 메세지를 쌓아두는 곳.
	/// 넣자마자 모든 client, 혹은 특정 client 에게 전송된다.
	/// </summary>
	class StringWriter {
		public static LinkedList<ServerString> messageBuffer = new LinkedList<ServerString>();

		//어디선가 쓰레드로 돌림.
		public void SendString() {
			while (_MyNet.isServerRun) {
				//Thread.Sleep(1);
				if (ClientSet.clients.Count > 0) {
					if (messageBuffer.Count > 0) {
						ServerString str = messageBuffer.ElementAt(0);
						messageBuffer.RemoveFirst();
						for (int i = 0; i < ClientSet.clients.Count; i++) {
							ClientSet.clients.ElementAt(i).Push(str);
						}
					}
				}
			}
		}

		//특정 id 의 sender 에게만 보내고 싶을 때,
		public static void SendForId(int id, ServerString message) {
			for (int i = 0; i < ClientSet.clients.Count; i++) {
				if (ClientSet.clients.ElementAt(i).GetId() == id) {
					ClientSet.clients.ElementAt(i).Push(message);
					break;
				}
			}
		}
	}

	/// <summary>
	/// 연결 들어온 클라이언트들을 모아두는 곳
	/// </summary>
	class ClientSet {
		public static LinkedList<TcpSender> clients = new LinkedList<TcpSender>();

		public static void AddClient(string hostAddress, int id) {
			TcpSender sender = new TcpSender(id);
			sender.SetHostAddress(hostAddress);
			clients.AddLast(sender);
			Thread senderThread = new Thread(sender.Run);
			senderThread.Start();
			Print.State = "Clients Count : " + ClientSet.clients.Count;
		}

		public static void DeleteClient(int id) {
			if (id < 0) return;
			for (int i = 0; i < clients.Count; i++) {
				if (clients.ElementAt(i).GetId() == id) {
					clients.ElementAt(i).ExitSending();
					clients.Remove(clients.ElementAt(i));
					Print.Debug = "Delete Client id : " + id;
					Print.State = "Clients Count : " + ClientSet.clients.Count;
					break;
				}
			}
		}
	}


	class TcpSender {
		private string hostAddress = "localhost";
		public LinkedList<ServerString> localSendBuffer = new LinkedList<ServerString>();
		private bool exit = false;
		private TcpClient client = null;    //원래 있는 클래스
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
			IPEndPoint sender = new IPEndPoint(IPAddress.Parse(host), _MyNet.TcpPort);
		}

		public void ExitSending() {
			exit = true;
		}

		public int GetId() {
			return id;
		}

		public void Push(ServerString message) {
			if (localSendBuffer.Count < 1000) {
				localSendBuffer.AddLast(message);
			}
		}

		private ServerString Pop() {
			if (localSendBuffer.Count != 0) {
				ServerString str = localSendBuffer.ElementAt(0);
				localSendBuffer.RemoveFirst();
				return str;
			}

			//종료시키는 스트링
			ServerString ret = new ServerString(-10, "exit");
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
				ServerString sendString = new ServerString(id, StrProtocol.Login.SetID, id + "");
				string str = sendString.GetString() + "\r\n";
				byte[] data = Encoding.UTF8.GetBytes(str);
				writeStream.Write(data, 0, data.Length);

				while (!exit) {
					if (!IsEmpty()) {
						str = Pop().GetString() + "\r\n";
						data = Encoding.UTF8.GetBytes(str);
						writeStream.Write(data, 0, data.Length);
						if (str.Equals("-10,exit;\r\n")) {
							exit = true;
						}
					}
				}
				if (exit) {
					data = Encoding.UTF8.GetBytes("-10,exit;\r\n");
					writeStream.Write(data, 0, data.Length);
				}
			} catch (Exception ex) {
				Debug.Log(ex.ToString());
			} finally {
				writeStream.Close();
				client.Close();
				_MyNet.ReturnID(id);
			}
		}
	}

	public class ServerString {
		public int id;
		public string[] param;

		public string GetString() {
			string ret = id + "";
			if (param != null) for (int i = 0; i < param.Length; i++) ret += "," + param[i];
			return ret + ";";
		}
		public static ServerString Get(string str) {
			char[] sp = new char[2];
			sp[0] = ','; sp[1] = ';';
			string[] strs = str.Split(sp);
			return new ServerString(strs);
		}
		public ServerString(int id) {
			this.id = id;
			this.param = null;
		}
		public ServerString(string[] strs) {
			this.id = int.Parse(strs[0]);
			this.param = new string[strs.Length - 1];
			for (int i = 1; i < param.Length; i++)
				this.param[i - 1] = strs[i];
		}
		public ServerString(int id, params string[] strs) {
			this.id = id;
			param = new string[strs.Length];
			for (int i = 0; i < param.Length; i++)
				param[i] = strs[i];
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

}