using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Connect
    {
        private Socket m_socket;
        private int i = 0;
        public Connect()
        {

            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress ipa = IPAddress.Parse("127.0.0.1");
            IPEndPoint iep = new IPEndPoint(ipa, 8900);


          
           // while (true)
            {
               // Socket clientSocket = m_socket.Accept();
                if (!m_socket.Connected)
                {
                    Console.WriteLine("Closeed!");
                    m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_socket.Connect(iep);

                    SendMsg(m_socket);
                    //i = 0;
                }
                else
                {
                    Console.WriteLine("start a connect");
                    m_socket.Send(Encoding.ASCII.GetBytes("hello-----ok" + i++.ToString()));
                    Thread.Sleep(500);
                    if (m_socket.Available > 0)
                    {
                        byte[] s = new byte[m_socket.Available];
                        m_socket.Receive(s);
                        Console.WriteLine(ASCIIEncoding.ASCII.GetString(s));

                    }                
                
                }

                
                
                
                Thread.Sleep(5000);
              //  m_socket.Shutdown(SocketShutdown.Both);                
               // m_socket.Close();
                Console.WriteLine("close connect");
                Thread.Sleep(1000);
                /*
                while (true)
                {
                    byte[] byteData = Encoding.ASCII.GetBytes("clent" + i.ToString());
                    
                    try
                    {
                        m_socket.Send(byteData);
                        byteData = new byte[128];
                        m_socket.Receive(byteData);
                        Console.WriteLine(Encoding.ASCII.GetString(byteData));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error a connect:" + e.Message);
                    }

                    if (i >= 5)
                    {
                        m_socket.Shutdown(SocketShutdown.Both);
                        Thread.Sleep(100);
                        m_socket.Close();
                        break;
                    }

                    Thread.Sleep(1000);
                }*/
                
            }
        }


        public void SendMsg(Socket socket)
        {
            int i = 0;
            while (true)
            {
                socket.Send(Encoding.ASCII.GetBytes("-" + i++.ToString() + "-abcdefghijklmnopqrstxyzuvw"));
                Console.WriteLine("send:" + i);
                while (socket.Available > 0)
                {
                    byte[] byteRecive = new byte[100];
                    socket.Receive(byteRecive);
                    Console.WriteLine(Encoding.ASCII.GetString(byteRecive));
                }
                
                Thread.Sleep(10);
            
            }
        
        }
    }
}
