

namespace ServerProject {
	

	class Program {
		static void Main(string[] args) {
			ServerNetwork.Server server = new ServerNetwork.Server();
			server.Start();
		}
	}
}
