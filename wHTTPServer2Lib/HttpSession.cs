using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace ItzWarty.wHTTPServer2Lib
{
    public class HttpSession
    {
        Socket socket               = null;
        HttpRequest _request        = null;
        HttpResponse _response      = new HttpResponse();
        public HttpRequest Request { get { return _request; } }
        public HttpResponse Response { get { return _response; } }
        public HttpSession(Socket s)
        {
            this.socket = s;
        }
        /// <summary>
        /// Receives the incoming request from the client and processes it,
        /// storing the parsed request header to the this.request field
        /// </summary>
        public void ReceiveRequest()
        {
            byte[] requestHeaderBuffer = new byte[2048];
            int length = socket.Receive(requestHeaderBuffer);
            string requestHeaderString = Encoding.ASCII.GetString(requestHeaderBuffer).Substring(0, length);
            Console.WriteLine("Got Request!");

            _request = HttpRequest.Parse(requestHeaderString);
        }

        /// <summary>
        /// Outputs our response.
        /// </summary>
        public void RespondAndClose()
        {
            if (this.IsConnected)
            {
                byte[] finalResponse = Response.ToBytes();
                socket.Send(
                    finalResponse
                );
                Close();
            }
        }
        public void Close()
        {
            if (this.IsConnected)
            {
                socket.Disconnect(false);
                socket.Close();
                socket = null;
            }
        }
        public bool Closed
        {
            get
            {
                return socket == null;
            }
        }
        public bool IsConnected
        {
            get
            {
                return socket != null && socket.Connected;
            }
        }
        public Socket Socket { get { return this.socket; } }
    }
}
