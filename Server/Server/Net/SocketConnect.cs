using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server.Net
{
    public class SocketConnect
    {
        private IPEndPoint m_ipEndPoint;
        private Socket m_socket;
        private int m_maxconnect;
        private SocketAsyncEventArgsPool m_eventArgPool;
        private BufferManager m_bufferManager;

        private List<AsyncUserToken> m_client;

        public SocketConnect()
        {
           
        }

        public void Init(IPEndPoint point, int maxConn)
        {
            if (point == null)
                return;

            m_client = new List<AsyncUserToken>();

            m_ipEndPoint = point;

            m_maxconnect = maxConn;

            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_socket.Bind(point);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            m_socket.Listen(maxConn);

            m_bufferManager = new BufferManager(1000 * 5 * 2, 5);
            m_bufferManager.InitBuffer();
            m_eventArgPool = new SocketAsyncEventArgsPool(maxConn);

            SocketAsyncEventArgs arg = null;
            for (int i = 0; i < maxConn; i++)
            {
                arg = new SocketAsyncEventArgs();
                arg.Completed += handel_OnEventArgsComplete;
                arg.UserToken = new AsyncUserToken();
                m_bufferManager.SetBuffer(arg);
                m_eventArgPool.Push(arg);
            }


            startAcceptConn(null);


           
        }


        private void startAcceptConn(SocketAsyncEventArgs arg)
        {
            if (arg == null)
            {
                arg = new SocketAsyncEventArgs();
                arg.Completed += handel_OnAcceptComplete;
            }
            else
            {
                arg.AcceptSocket = null;
            }

           bool isRaiseEvent =  m_socket.AcceptAsync(arg);
           if (!isRaiseEvent)
               processAcceptConn(arg);
        }
               

        private void processAcceptConn(SocketAsyncEventArgs arg)
        {
            Console.WriteLine("AcceptConn");
            if (m_eventArgPool.Count > 0)
            {
                SocketAsyncEventArgs e = m_eventArgPool.Pop();
                ((AsyncUserToken)e.UserToken).Socket = arg.AcceptSocket;
                ((AsyncUserToken)e.UserToken).SocketArg = arg;
                
                
                if (e != null)
                {

                    bool isRaiseEvent = arg.AcceptSocket.ReceiveAsync(e);
                    if (!isRaiseEvent)
                    {
                        processRecive(e);
                    }
                }
            }
            
            else
            {
                Console.WriteLine("Pool is Empty");
            }

           
            startAcceptConn(arg);
        }

        private void processRecive(SocketAsyncEventArgs arg)
        {
            Console.WriteLine("processRecive");
            
            if (arg.BytesTransferred > 0 && arg.SocketError == SocketError.Success)
            {

               // if (((AsyncUserToken)arg.UserToken).Socket.Available == 0)
                {
                //    Console.WriteLine("Available " + ((AsyncUserToken)arg.UserToken).Socket.Available);
                    byte[] data = new byte[arg.BytesTransferred];
                    Buffer.BlockCopy(arg.Buffer, arg.Offset, data, 0, arg.BytesTransferred);
                    string s = Encoding.ASCII.GetString(data);
                    Console.WriteLine(s);

            //        ((AsyncUserToken)arg.UserToken).UserId = Int32.Parse(s.Replace("hello",""));

                //    Console.WriteLine("Available " + ((AsyncUserToken)arg.UserToken).Socket.Available);
                }

                Thread.Sleep(10);
                arg.SetBuffer(arg.Offset, arg.BytesTransferred);
               bool isRaiseEvent1 = ((AsyncUserToken)arg.UserToken).Socket.SendAsync(arg);
               if (!isRaiseEvent1)
                   processSend(arg);                

                /*
                bool isRaiseEvent = ((AsyncUserToken)arg.UserToken).Socket.ReceiveAsync(arg);
                if (!isRaiseEvent)
                {
                    processRecive(arg);
                }*/
            }
            else
            {
                //清除
                m_client.Remove((AsyncUserToken)arg.UserToken);
                closeClientConnect(arg);
            }
            
         
        }

        private void processSend(SocketAsyncEventArgs arg)
        {
            Console.WriteLine("processSend");
            if (arg.SocketError != SocketError.Success)
            {
                closeClientConnect(arg);
            }
            else
            {
                bool isRaiseEvent = ((AsyncUserToken)arg.UserToken).Socket.ReceiveAsync(arg);
                if (!isRaiseEvent)
                {
                    processRecive(arg);
                }
            }

            
        }



        private void handel_OnEventArgsComplete(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                if (e.LastOperation == SocketAsyncOperation.Receive)
                {
                    processRecive(e);
                }
                else if (e.LastOperation == SocketAsyncOperation.Send)
                {
                    processSend(e);
                }
                
            }
            else
            {
                Console.WriteLine("error:" + e.SocketError);
            }        
       }

        private void handel_OnAcceptComplete(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Console.WriteLine("e.LastOperation" + e.LastOperation);
                processAcceptConn(e);
            }
            else
            {
                Console.WriteLine("error:" + e.SocketError);
                closeClientConnect(e);
            }

            //e.AcceptSocket.Shutdown();
            // e.AcceptSocket.Close();
        }


        private void closeClientConnect(SocketAsyncEventArgs e)
        {
            Socket socket = ((AsyncUserToken)e.UserToken).Socket;
            Console.WriteLine("Disconnect " + ((AsyncUserToken)e.UserToken).UserId);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            e.AcceptSocket = null;            
            m_eventArgPool.Push(e);
            
        }

    }
}
