using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace NetModule {
	class PacketModule {
		class Server {
			public static void Main() {
				NetworkStream stream = null;
				TcpListener tcpListener = null;
				StreamReader reader = null;
				Socket clientsoket = null;
				try {
					//ip주소를 나타내는 객체 생성, Tcplistener를 생성시 인자로 사용
					IPAddress ipAd = IPAddress.Parse("127.0.0.1");

					//TcpListener Class를 이용하여 클라이언트의 연결을 받아 들임
					tcpListener = new TcpListener(ipAd, 5001);
					tcpListener.Start();

					//Client 의 접속이 올 때 까지 Block되는 부분, Thread
					//백그라운드 Thread 처리 맡김
					clientsoket = tcpListener.AcceptSocket();

					//클라이언트의 데이터를 읽고, 쓰기 위한 스트림 생성
					stream = new NetworkStream(clientsoket);

					Encoding encode = System.Text.Encoding.UTF8;
					reader = new StreamReader(stream, encode);

					while (true) {
						string str = reader.ReadLine();

						if (str.IndexOf("<EOF>") > -1) {
							Console.WriteLine("Bye~ Bye~");
							break;
						}
						Console.WriteLine(str);
						str += "\r\n";
						byte[] dataWrite = Encoding.Default.GetBytes(str);
						stream.Write(dataWrite, 0, dataWrite.Length);
					}
				} catch (Exception e) {
					Console.WriteLine(e.ToString());
				} finally {
					clientsoket.Close();
				}
			}
		}


		class TcpClientTest {
			static void Main(string[] args) {
				TcpClient client = null;

				try {
					//LocalHost에 지정포트로 TCP Connection생성 후 데이터 송수신 스트림 얻음
					client = new TcpClient();
					client.Connect("127.0.0.1", 5001);
					NetworkStream writeStream = client.GetStream();

					Encoding encode = System.Text.Encoding.UTF8;
					StreamReader readerStream = new StreamReader(writeStream, encode);

					//보낼 데이터를 읽어 Default형식의 바이트 스트림으로 변환
					string dataToSend = Console.ReadLine();
					byte[] data = Encoding.Default.GetBytes(dataToSend);

					while (true) {
						dataToSend += "\r\n";
						data = Encoding.Default.GetBytes(dataToSend);
						writeStream.Write(data, 0, data.Length);

						if (dataToSend.IndexOf("<EOF>") > -1)
							break;

						string returnData;
						returnData = readerStream.ReadLine();
						Console.WriteLine("server : " + returnData);

						dataToSend = Console.ReadLine();
					}
				} catch (Exception ex) {
					Console.WriteLine(ex.ToString());
				} finally {
					client.Close();
				}
			}
		}

	}
}
