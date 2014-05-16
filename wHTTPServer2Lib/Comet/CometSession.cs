using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib.Comet
{
    public class CometSession
    {
        private string guid;
        public string GUID { get { return guid; } }
        private HttpSession receivingSession = null;
        private HttpSession originalSession = null; //Used to ensure no spoofing occurs.
        public CometSession(HttpSession httpSession)
        {
            ClientSession session = ClientSession.Open(httpSession);
            this.guid = Guid.NewGuid().ToString("N");
            originalSession = httpSession;

            httpSession.Response.outCookies["wcsid"] = guid;
            CometSessionManager.RegisterSession(this, guid);
        }
        private List<string> inBuffer = new List<string>();
        public List<string> InBuffer { get { return inBuffer; } }
        /// <summary>
        /// Add to the in buffer [crap sent from the client to the server]
        /// </summary>
        /// <param name="inMessage"></param>
        public void AddToServerInBuffer(string inMessage)
        {
            inBuffer.Add(inMessage);
        }

        public void SetReceivingSession(HttpSession session) { this.receivingSession = session; }
        public void SendStreamPageHeaders()
        {
            /*
                        .statusCode(StatusCode.Okay)
                        .transferEncoding(TransferEncoding.Chunked)
                        .cacheControl(CacheControl.noCache)
                        .contentType(ContentType.multipartReplace)
                        .date(DateTime.Now)
                        .finalize()
                        .ToBytes()*/
            receivingSession.Socket.Send(
                new HttpResponse()
                    .SetCacheControl(HttpResponse.CacheControl.dontCache)
                    .SetContentType(HttpResponse.ContentType.multipartXMixedReplace)
                    .SetTransferEncoding(HttpResponse.TransferEncoding.Chunked)
                    .SetCacheControl(HttpResponse.CacheControl.dontCache)
                    .ToBytes()
            );
        }

        /// <summary>
        /// Pushes crap to the client...
        /// </summary>
        /// <param name="outMessage"></param>
        public void RealPushToClient(string outMessage)
        {
            while(!Connected) System.Threading.Thread.Sleep(100);
            try
            {
                receivingSession.Socket.Send(
                    (
                        outMessage.Length.ToString("X") + "\r\n" +
                        outMessage + "\r\n"
                    ).ToBytes()
                );
            }
            catch 
            {
                Console.WriteLine("Failed to push to client");
            }
        }
        /// <summary>
        /// Pushes to the receiver window's cometObject.inBuffer
        /// </summary>
        /// <param name="message"></param>
        public void PushToClient(string message)
        {
            RealPushToClient(
                "<script> window.parent.CometSessions.cs{0}.AddToInBuffer('{1}'); </script>".F(this.guid, message)
            );
        }

        public string PullFromClient()
        {
            while (inBuffer.Count == 0) System.Threading.Thread.Sleep(50);
            string inString = inBuffer[0];
            inBuffer.RemoveAt(0);
            return inString;
        }

        public bool RequestHasSameOrigin(HttpSession session)
        {
            return ClientSession.Open(session).MatchesRequest(originalSession.Request);
        }

        public object sessionState = null;

        public bool Connected
        {
            get
            {
                return this.receivingSession != null && this.receivingSession.IsConnected;
            }
        }
    }
}
