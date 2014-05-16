using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty;
using ItzWarty.wHTTPServer2Lib;
using ItzWarty.wHTTPServer2Lib.Comet;

using System.Net.Sockets;

namespace ItzWarty.wHTTPServer2Lib.wRCon
{
    public class wRConsole:CometSession
    {
        public wRConsole(HttpSession httpSession):base(httpSession)
        {
            //Base would register to CometSessionManager
        }
        public string ReadLine()
        {
            PushToClient("READLINE");
            string inLine = PullFromClient();
            return inLine;
        }
        public string ReadLinePassword()
        {
            PushToClient("READLINE^]PASSWORD");
            string inLine = PullFromClient();
            return inLine;
        }
        public void Write(string s)
        {
            PushToClient("WRITE^]" + s);
        }
        public void WriteLine()
        {
            Write("\r\n");
        }
        public void WriteLine(string s)
        {
            Write(s+"\r\n");
        }
    }
}