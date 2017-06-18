using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SocketListener
    {
        Socket m_socket;
        int i = 0;

        public SocketListener()
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipa = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(ipa,9000);
            m_socket.Bind(iep);
            m_socket.Listen(5);

            while (true)
            {
                Console.WriteLine("start accept a connect ");
                Socket clientSocket = m_socket.Accept();
                
                while (true)
                {
                    if (!clientSocket.Connected)
                    {
                        Console.WriteLine("Client Close");
                        break;
                    }

                    try
                    {
                        byte[] byteData = new byte[128];
                        clientSocket.Receive(byteData);
                        Console.WriteLine("get a connect" + Encoding.ASCII.GetString(byteData));

                        byteData = Encoding.ASCII.GetBytes(i++.ToString());
                        clientSocket.Send(byteData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error" + e.ToString());
                    }
                }
                
            }

        }
    }
}
