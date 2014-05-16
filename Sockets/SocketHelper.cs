using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace ItzWarty.Misc
{
    public static partial class SocketHelper
    {
        public static bool PortIsFree(int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Bind(new IPEndPoint(IPAddress.Any, port));
                s.Listen(100);
                s.Shutdown(SocketShutdown.Both);
                s.Close();
                return true;
            }
            catch
            {
                try
                {
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }catch{};
                return false;
            }
        }
    }
}
