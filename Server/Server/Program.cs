using Server.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{  

    class Program
    {
        static void Main(string[] args)
        {
            //new SocketListener();
            IPEndPoint ipend = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8900);
            new SocketConnect().Init(ipend,5);
            Console.ReadKey();
        }
    }
}
