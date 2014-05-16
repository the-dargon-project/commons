using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.IO;
using System.Threading;

using ItzWarty;

namespace ItzWarty.Sockets
{
    /// <summary>
    /// Internetworked stream socket with the TCP protocol... w-ified
    /// 
    /// Fakes an asynchronous socket by recieving asynchronously nonstop and filling a queue... 
    /// </summary>
    public class wStreamSocket
    {
        Socket s = null;
        NetworkStream ns = null;
        public bool isAsync = false;
        public Socket socket
        {
            get
            {
                return s;
            }
        }
        public wStreamSocket()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.NoDelay = true;
        }
        public wStreamSocket(Socket baseSocket)
        {
            s = baseSocket;
            ns = new NetworkStream(s);
        }
        /// <summary>
        /// Listens to any ip address at the given port
        /// </summary>
        public void Bind(int port)
        {
            s.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen(int backlog)
        {
            s.Listen(backlog);
        }

        public wStreamSocket Accept()
        {
            Socket clientConn = s.Accept();
            clientConn.NoDelay = true;
            wStreamSocket wss_clientConn = new wStreamSocket(clientConn);
            wss_clientConn.BeginSocketManagerThread();

            return wss_clientConn;
        }

        public void Connect(string addr, int port)
        {
            s.Connect(addr, port);
            s.NoDelay = true;
            s.Blocking = true;
            ns = new NetworkStream(s);
            BeginSocketManagerThread();
        }
        public void BeginSocketManagerThread()
        {
            new Thread(SocketManagerThread).Start();
        }
        List<byte> inBuffer = new List<byte>();
        void SocketManagerThread()
        {
            try
            {
                bool receiving = false;
                while (s.Connected)
                {
                    if (!isAsync) { System.Threading.Thread.Sleep(10); continue; }
                    while (receiving) System.Threading.Thread.Sleep(10);
                    byte[] bInBuffer = new byte[2048];
                    receiving = true;
                    s.BeginReceive(bInBuffer, 0, bInBuffer.Length, 0,
                        new AsyncCallback(
                            delegate(IAsyncResult ar)
                            {
                                lock (s)
                                {
                                    if (s.Connected)
                                    {
                                        int length = s.EndReceive(ar);
                                        if (length == 0)
                                        {
                                            s.Shutdown(SocketShutdown.Both);
                                            s.Close();
                                        }
                                        else
                                        {
                                            byte[] final = new byte[length];
                                            Console.Write("Received {0} byte(s):", length.ToString().PadLeft(3));
                                            //for (int i = 0; i < length; i++) Console.Write(" " + bInBuffer[i].ToString("x"));
                                            Array.Copy(bInBuffer, final, length);

                                            inBuffer.AddRange(final);

                                            Console.WriteLine();
                                            receiving = false;
                                        }
                                    }
                                }
                            }
                        ), s
                    );
                }
            }
            catch
            {
                try
                {
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
                catch { }
            }
        }
        public byte[] Recieve()
        {
            byte[] buffer = new byte[1024];
            int length = s.Receive(buffer);
            byte[] final = new byte[length];
            Array.Copy(buffer, final, length);
            return final;
        }
        public byte[] Recieve(int bytes)
        {
            if (isAsync)
            {
                while (inBuffer.Count < bytes) System.Threading.Thread.Sleep(10);
                byte[] final = inBuffer.GetRange(0, bytes).ToArray();
                inBuffer.RemoveRange(0, bytes);

                return final;
            }
            else
            {
                //byte[] buffer = new byte[1024];
                //int length = s.Receive(buffer);
                //byte[] final = new byte[length];
                //Array.Copy(buffer, final, length);

                BinaryReader br = new BinaryReader(new NetworkStream(s));
                byte[] final = br.ReadBytes(bytes);

                Console.Write("Received {0} byte(s):", final.Length.ToString().PadLeft(3));
                for (int i = 0; i < final.Length; i++) Console.Write(" " + final[i].ToString("x"));
                Console.WriteLine();
                return final;
            }
        }
        public int RecieveInt32()
        {
            if (isAsync)
            {
                while (this.inBuffer.Count < 4) System.Threading.Thread.Sleep(100);

                byte[]  inBytes = this.inBuffer.GetRange(0, 4).ToArray();
                this.inBuffer.RemoveRange(0, 4);

                return BitConverter.ToInt32(inBytes, 0);
            }else{
                return new BinaryReader(ns).ReadInt32();
            }
        }

        public long RecieveInt64()
        {
            return new BinaryReader(ns).ReadInt64();
        }

        public string RecieveString()
        {
            return new BinaryReader(ns).ReadString();
        }
        //public byte[] RecieveBytes(int count)
        //{
        //    return new BinaryReader(new NetworkStream(s)).ReadBytes(count);
        //}
        public bool HasNextBytes()
        {
            return inBuffer.Count != 0;
        }
        public void Send(byte[] b)
        {
            /*
            s.BeginSend(b, 0, b.Length, 0,
                new AsyncCallback(
                    delegate(IAsyncResult ar)
                    {
                        s.EndSend(ar);
                    }
                ), s
            );
            */
            //if (isAsync)
            //{
            //    s.Send(b);
            //}else{
            Console.Write("Sending {0} bytes:", b.Length.ToString().PadLeft(4));
            for (int i = 0; i < b.Length; i++)
                Console.Write(" " + b[i].ToString("x").PadLeft(2));
            Console.WriteLine();
            BinaryWriter bw = new BinaryWriter(new NetworkStream(s));
            Console.Write(".. write");
            bw.Write(b);
            Console.Write(".. flush");
            bw.Flush();
            Console.WriteLine("done");
            //}
            //bw.Dispose();
            //s.Send(b);
        }
        public void Send(byte b)
        {
            Send(new byte[] { b });
        }
        public void Send(bool b)
        {
            Send(new byte[] { b?(byte)0x01:(byte)0x00 });
        }
        public void Send(int i)
        {
            Send(BitConverter.GetBytes(i));

            //Console.WriteLine("Sent {0}", i);
            //BinaryWriter bw = new BinaryWriter(new NetworkStream(s));
            //bw.Write(i);
            /*
            bw.Flush();
             */
        }
        public void Send(string s)
        {
            BinaryWriter bw = new BinaryWriter(ns);
            bw.Write(s);
            bw.Flush();
        }
        public bool Connected
        {
            get
            {
                return this.s.Connected;
            }
        }
        public void Disconnect()
        {
            try
            {
                this.s.Shutdown(SocketShutdown.Both);
                this.s.Close();
            }
            catch { }
        }
    }
}
