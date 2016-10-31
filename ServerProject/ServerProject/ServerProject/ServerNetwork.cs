using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

namespace ServerNetwork {

	/// <summary>
	/// 받은 NetString 을 저장하는 곳
	/// </summary>
	class Received {
		private static Queue<NetPacket> receiverBuffer = new Queue<NetPacket>();

		public static int GetCount() {
			return receiverBuffer.Count;
		}
		public static void EnqueueThreadSafe(NetPacket nStr) {
			lock (receiverBuffer) {
				receiverBuffer.Enqueue(nStr);
			}
		}
		public static NetPacket DequeueThreadSafe() {
			NetPacket nStr;
			lock (receiverBuffer) {
				nStr = receiverBuffer.Dequeue();
			}
			return nStr;
		}
	}

	class RealNetwork {
		public static int TcpPort = 1900;
		private const int MAXCLIENT = 2500;

		/// <summary>
		/// 연결이 하나 들어올 때마다 아이디 카운트를 하나씩 늘린다.
		/// 그런데 만약 bID[idCount]가 true 이면 하나를 더 더하여 false 인 곳을 찾는다.
		/// 그리고 연결된 클라이언트에게 해당 아이디를 넘겨준다.
		/// 이로써 클라이언트들은 전부 다른 id 를 부여받게 된다.
		/// idCount 가 2500 을 넘어가게 되면 0으로 되돌아온다.
		/// </summary>
		private static int idCount = -1;

		/// <summary>
		/// 연결이 하나 들어올 때마다 bID[idCount] 를 true 로 하고
		/// 어떤 클라이언트의 연결이 끊기면 bID[부여받은 idCount] 를 false 로 바꾼다.
		/// </summary>
		private static bool[] bID = new bool[MAXCLIENT];

		public static TcpListener tcpListener = null;
		public static ClientSet senderManager = new ClientSet();

		public static bool isServerRun = true;

		public static int PopID() {
			int index = 0;
			while (index++ < MAXCLIENT) {
				idCount++;
				if (idCount >= MAXCLIENT - 10)
					idCount = 0;
				if (!bID[idCount]) {
					bID[idCount] = true;
					return idCount;
				}

			}
			return -1;
		}

		//다쓴 아이디를 리턴하여 다시 사용할 수 있는 아이디로 만든다.
		public static void ReturnID(int id) {
			if (bID[id])
				bID[id] = false;
		}

		/*
		public static void SendClients(string message) {

		}
        */

		public static void StartServer() {
			Console.WriteLine("Clients Count : " + ClientSet.clients.Count);
			RealNetwork server = new RealNetwork();
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
			/*
			Thread writherSendingThread = new Thread(writer.SendString);
			writherSendingThread.Start();
            */
			//서버소켓에 연결하는 클라이언트가 생길 때마다 새로운 쓰레드를 만들어 열어준다.
			//그리고 수신한다.
			while (isServerRun) {
				try {
					Console.WriteLine("WAITING FOR CLIENTS...");
					Socket welcomeSocket = tcpListener.AcceptSocket();
					TcpClientThread tcpClientThread = new TcpClientThread(welcomeSocket);
					Thread welcome = new Thread(tcpClientThread.BeginConnection);
					welcome.Start();
				} catch (Exception e) {
					Console.WriteLine("WELCOME THREAD: " + e.Message);
					break;
				}
			}
		}
	}


	/// <summary>
	/// 들어온 클라이언트를 연결한 후,
	/// 클라이언트를 클라이언트 센더에 추가한다.
	/// 그리고 클라이언트가 보내주는 것을 무한정 듣는다.
	/// </summary>
	class TcpClientThread {
		private Socket clientSocket = null;
		private NetworkStream stream = null;
		private StreamReader reader = null;
		private StreamWriter writer = null;
		private int id = -1;
		private string clientEndpoint = "";
		public bool isAlive = false;

		public TcpClientThread(Socket s) {
			clientSocket = s;
			stream = new NetworkStream(clientSocket);
			reader = new StreamReader(stream, Encoding.UTF8);
			writer = new StreamWriter(stream, Encoding.UTF8);
			id = RealNetwork.PopID();
			clientEndpoint = ConvertEndPointToString(clientSocket.RemoteEndPoint.ToString());
			Console.WriteLine("Begin TcpClientThread: " + id);
		}
		//들어온 클라이언트와 연결한다.
		public void BeginConnection() {
			if (id >= 0) {
				ClientSet.AddClient(clientEndpoint, id);
				isAlive = true;
				while (true) {//클라이언트로 부터 수신
					try {
						string recieveString = reader.ReadLine();
						if (recieveString != null) {
							NetPacket packet = NetPacket.Parse(recieveString);
							if (packet.echoType == EchoType.Echo) {
								Server.SendAll(recieveString);
							}
							Received.EnqueueThreadSafe(packet);
							if (packet.func == NetFunc.Exit) {
								/*
								 * 밑에서 같은 동작을 하드라. DeleteClient 에서
                                for (int i = 0; i < ClientSet.clients.Count; i++) {
                                    if (ClientSet.clients.ElementAt(i).GetId() == id) {
                                        ClientSet.clients.ElementAt(i).ExitSending();
                                    }
                                }*/
								break;
							}
							//Console.WriteLine(packet.ToString());
						}
					} catch (Exception e) {
						Console.WriteLine("CLIENT SOCKET: " + id + " / " + e.Message);
						break;
					}
				}
				ClientSet.DeleteClient(clientEndpoint);
				//클라이언트가 종료될 때 반드시 에코로 보내야 한다.
				isAlive = false;
			}

			Console.WriteLine("Close TcpClientThread: " + id);
			clientSocket.Close();
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
		private static string usingMessage = "";

		/// <summary>
		/// 셋 하자마자 모든 클라이언트 큐에 메세지를 넣는다.
		/// </summary>
		public static string messageBuffer {
			set {
				lock (usingMessage) {
					for (int i = 0; i < ClientSet.clients.Count; i++) {
						ClientSet.clients.ElementAt(i).SendMessageForClient(value);
					}
				}
			}
		}


		/*
        //어디선가 쓰레드로 돌림.
        public void SendString() {
			while (_MyNet.isServerRun) {
				//Thread.Sleep(1);
				if (ClientSet.clients.Count > 0) {
					if (messageBuffer.Count > 0) {
						string str = messageBuffer.Dequeue();
						for (int i = 0; i < ClientSet.clients.Count; i++) {
							ClientSet.clients.ElementAt(i).Push(str);
						}
					}
				}
			}
		}
        */

		//특정 id 의 sender 에게만 보내고 싶을 때,
		public static void SendForId(int id, string message) {
			for (int i = 0; i < ClientSet.clients.Count; i++) {
				if (ClientSet.clients.ElementAt(i).GetId() == id) {
					ClientSet.clients.ElementAt(i).SendMessageForClient(message);
					break;
				}
			}
		}
	}

	/// <summary>
	/// 연결 들어온 클라이언트들을 모아두는 곳
	/// </summary>
	class ClientSet {
		public static int clientCount;
		public static List<TcpSender> clients = new List<TcpSender>();

		public static void AddClient(string hostAddress, int id) {
			clientCount++;
			TcpSender sender = new TcpSender(id);
			sender.SetHostAddress(hostAddress);
			clients.Add(sender);
			Thread senderThread = new Thread(sender.Run);
			senderThread.Start();
			Console.WriteLine("Clients Count : " + clients.Count);
		}

		public static void DeleteClient(string hostAddress) {
			for (int i = 0; i < clients.Count; i++) {
				if (clients[i].GetHostAddress().Equals(hostAddress)) {
					clients[i].ExitSending();
					clients.RemoveAt(i);
					clientCount--;
					Console.WriteLine("Clients Count : " + clients.Count);
					break;
				}
			}
		}
	}


	class TcpSender {
		private string hostAddress = "localhost";
		//public Queue<string> localSendBuffer = new Queue<string>();
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
			IPEndPoint sender = new IPEndPoint(IPAddress.Parse(host), RealNetwork.TcpPort);
		}

		public void ExitSending() {
			Console.WriteLine("종료시키겠다는 메세지를 받음 : " + id);
			exit = true;
			ConnectExit();
		}

		public int GetId() {
			return id;
		}

		/*
		public void Push(string message) {
			if (localSendBuffer.Count < 1000) {
				localSendBuffer.Enqueue(message);
			}
		}

		private string Pop() {
			if (localSendBuffer.Count != 0) {
                return localSendBuffer.Dequeue();
			}

            /*
			//종료시키는 스트링
			string ret = StrProtocol.Flow.Exit + "\r\n";
			return ret;
            
            return "fdsafdsafdsa";
		}



		private bool IsEmpty() {
			return !(localSendBuffer.Count > 0);
		}
                */
		/// <summary> 
		/// 클라이언트에게 아이디를 보내줌
		/// 클라이언트는 받은 아이디를 자신의 아이디로 세팅함.
		/// </summary>
		public void Run() {
			client = new TcpClient();
			client.Connect(hostAddress, 1901);
			writeStream = client.GetStream();
			try {
				//클라이언트에게 아이디를 보내줌
				//클라이언트는 받은 아이디를 자신의 아이디로 세팅함.
				string str = (new NetPacket(ClassType.None, id, EchoType.NotEcho, NetFunc.SetId, "")).ToString() + "\r\n";
				Console.WriteLine("send set id : " + str);
				byte[] data = Encoding.UTF8.GetBytes(str);
				writeStream.Write(data, 0, data.Length);
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// 보낼 메세지를 받았을 때.
		/// </summary>
		public void SendMessageForClient(string message) {
			try {
				byte[] data = Encoding.UTF8.GetBytes(message + "\r\n");
				writeStream.Write(data, 0, data.Length);
				if (exit) {
					//ConnectExit();
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// 연결을 종료시킴.
		/// </summary>
		private void ConnectExit() {
			try {
				string str = new NetPacket(ClassType.None, -100, EchoType.NotEcho, NetFunc.Exit, "").ToString();
				byte[] data = Encoding.UTF8.GetBytes(str + "\r\n");
				writeStream.Write(data, 0, data.Length);
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			} finally {
				writeStream.Close();
				client.Close();
				RealNetwork.ReturnID(id);
			}
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
		Console.WriteLine("fdsa");
	}

	public static void MoveCursor(int x_, int y_) {
		GetCursorPos(out mPoint);
		double CursorX = mPoint.x + x_;
		double CursorY = mPoint.y + y_;
		SetCursorPos((uint)CursorX, (uint)CursorY);
	}
}
