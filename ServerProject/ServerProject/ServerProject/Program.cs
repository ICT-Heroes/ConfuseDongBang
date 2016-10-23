using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using Microsoft.VisualBasic;

namespace ServerProject {
	

	class Program {
		static void Main(string[] args) {
			ServerNetwork.Server server = new ServerNetwork.Server();
			server.Start();
		}
	}
}
