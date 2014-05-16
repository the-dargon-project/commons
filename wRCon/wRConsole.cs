using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.IO;

using ItzWarty;
using ItzWarty.Web.HTTP.Headers;
namespace ItzWarty.wRCon
{
    public static class wRConsoleSessions
    {
        public static Dictionary<string, wRConsole> sessions = new Dictionary<string, wRConsole>();
    }
    public class wRConsole
    {
        public Socket streamSocket = null;
        string sessionId = Guid.NewGuid().ToString();
        public bool sessionAlive = true;
        public wRConsole()
        {
            wRConsoleSessions.sessions.Add(this.sessionId, this);
        }
        public void GenerateMainPage(RequestHeader reqH, ResponseHeader respH, System.Net.Sockets.Socket socket, string streamAddr, string sendAddr)
        {
            socket.Send(
                respH
                    .statusCode(StatusCode.Okay)
                    .transferEncoding(TransferEncoding.Whole)
                    .cacheControl(CacheControl.noCache)
                    .contentType(ContentType.textHtml)
                    .date(DateTime.Now)
                    .setCookie("wrcon_sid", sessionId, DateTime.Now + TimeSpan.FromMinutes(30))
                    .finalize()
                    .toBytes()
            );
            
            socket.Send(   
                System.IO.File.ReadAllText("wRCon/RConFrontend.htm").Replace("{$streamaddr}", streamAddr).Replace("{$sendaddr}", sendAddr).toBytes()
            );
            socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            socket.Close();
        }
        public bool WaitUntilConnected(int timeoutMS)
        {
            DateTime startTime = DateTime.Now;
            while (this.streamSocket == null &&
                   DateTime.Now < startTime+TimeSpan.FromMilliseconds(timeoutMS))
            {
                System.Threading.Thread.Sleep(10);
            }
            if (DateTime.Now > startTime + TimeSpan.FromMilliseconds(timeoutMS))
                return false;   //Timeout
            else
                return true;
        }
        public void Clear()
        {
            SendChunk(this.streamSocket, "<script> parent.Clear();</script>");
        }
        public void Write(string s)
        {
            s = s.Replace("\\", "\\\\");
            s = s.Replace("\r", "");
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("\n", "<br/>");
            SendChunk(this.streamSocket, "<script> parent.PostToLog(\"" + s + "\");</script>");
        }
        public void WriteLine()
        {
            WriteLine("");
        }
        public void WriteLine(string s)
        {
            Write(s + "\n");
        }
        public void SendChunk(Socket s, string text)
        {
            string msg = text;

            byte[] b_chunk = Encoding.ASCII.GetBytes(
                (msg.Length + 2).ToString("X") + "\r\n" +
                msg + "\r\n" + 
                "\r\n"
            );
            NetworkStream ns = new NetworkStream(s);
            BinaryWriter bw = new BinaryWriter(ns);
            bw.Write(b_chunk);
            bw.Flush();
            bw.Flush();
        }
        public void Close()
        {
            streamSocket.Shutdown(SocketShutdown.Both);
            streamSocket.Close();
            return;
        }
        public static void GenerateStreamPage(RequestHeader reqH, ResponseHeader respH, System.Net.Sockets.Socket socket)
        {
            if (reqH.cookies.ContainsKey("wrcon_sid") && wRConsoleSessions.sessions.ContainsKey(reqH.cookies["wrcon_sid"]))
            {
                //Session exists
                socket.Send(
                    respH
                        .statusCode(StatusCode.Okay)
                        .transferEncoding(TransferEncoding.Chunked)
                        .cacheControl(CacheControl.noCache)
                        .contentType(ContentType.multipartReplace)
                        .date(DateTime.Now)
                        .finalize()
                        .toBytes()
                );
                wRConsole rConsole = wRConsoleSessions.sessions[reqH.cookies["wrcon_sid"]]; //Link ourselves to the real rConsole object
                rConsole.streamSocket = socket;

                while (rConsole.sessionAlive)
                    System.Threading.Thread.Sleep(1);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            else
            {
                socket.Send(
                    respH
                        .statusCode(StatusCode.NotFound)
                        .transferEncoding(TransferEncoding.Whole)
                        .cacheControl(CacheControl.noCache)
                        .contentType(ContentType.textHtml)
                        .date(DateTime.Now)
                        .finalize()
                        .toBytes()
                );
                socket.Send(
                    "Stop snooping".toBytes()
                );
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        bool readingLine = false;
        public void EnableReadLine()
        {
            if (!this.readingLine || DateTime.Now.Millisecond > 900)
            {
                Console.WriteLine("enablereadline");
                this.readingLine = true;
                this.lineRead = false;
                this.readLine = "123";
                SendChunk(this.streamSocket, "<script>parent.ReadLine();</script>");
            }
        }
        public bool HasNextLine()
        {
            if(lineRead)
                Console.WriteLine("HasNextLine?: {0}", lineRead);
            return lineRead;
        }
        public bool lineRead;
        public string readLine;
        public string ReadLine()
        {
            EnableReadLine();
            while (!HasNextLine()) System.Threading.Thread.Sleep(10);
            lineRead = false;

            string result = readLine;
            readLine = "345";
            readingLine = false;
            return result;
        }
        public string ReadLinePassword()
        {
            lineRead = false;
            SendChunk(this.streamSocket, "<script>parent.ReadLinePassword();</script>");

            while (!lineRead) System.Threading.Thread.Sleep(10);
            return readLine;
        }
        public void Redirect(string to)
        {
            SendChunk(this.streamSocket, "<script>parent.location='"+to+"';</script>");
        }
        public static void ManageSendPage(RequestHeader reqH, ResponseHeader respH, System.Net.Sockets.Socket socket)
        {
            if (reqH.cookies.ContainsKey("wrcon_sid") && wRConsoleSessions.sessions.ContainsKey(reqH.cookies["wrcon_sid"]) && reqH._GET.ContainsKey("r"))
            {
                //Session exists
                socket.Send(
                    respH
                        .statusCode(StatusCode.Okay)
                        .transferEncoding(TransferEncoding.Whole)
                        .cacheControl(CacheControl.noCache)
                        .contentType(ContentType.textHtml)
                        .date(DateTime.Now)
                        .finalize()
                        .toBytes()
                );
                wRConsole rConsole = wRConsoleSessions.sessions[reqH.cookies["wrcon_sid"]]; //Link ourselves to the real rConsole object
                lock (rConsole)
                {
                    rConsole.lineRead = true;
                    //Strip html
                    string finalLine = System.Text.RegularExpressions.Regex.Replace(reqH._GET["r"], "<.*?>", string.Empty);
                    finalLine = System.Net.WebUtility.HtmlDecode(finalLine);
                    finalLine = finalLine.Replace("%20", " ");
                    rConsole.readLine = finalLine;
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            else
            {
                socket.Send(
                    respH
                        .statusCode(StatusCode.NotFound)
                        .transferEncoding(TransferEncoding.Whole)
                        .cacheControl(CacheControl.noCache)
                        .contentType(ContentType.textHtml)
                        .date(DateTime.Now)
                        .finalize()
                        .toBytes()
                );
                socket.Send(
                    "Stop snooping".toBytes()
                );
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
