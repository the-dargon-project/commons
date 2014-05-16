using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace ItzWarty.Web.HTTP.Headers
{
    public class RequestHeader
    {
        public string method = "";
        //The redirected/resolved location
        public string location = "";
        //The actual location requested by browser
        public string requestedLocation = "";
        public string protocol = "";
        public Dictionary<string, string> cookies = new Dictionary<string, string>();
        public string request = "";
        public string fileName = "";
        public string fileExt = "";
        public string directory = "";
        public Dictionary<string, string> _GET = new Dictionary<string, string>();

        public string endpointIP;
        public RequestHeader(string requestHeader, Socket conn)
        {
            string ipPort = conn.RemoteEndPoint.ToString(); //123.456.789.123:123
            endpointIP = ipPort.Substring(0, ipPort.IndexOf(":"));

            request = requestHeader;

            string[] lines = requestHeader.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (parts[0].ToUpper() == "GET")
                {
                    this.method = "GET";
                    string locHttp = line.Substring(line.IndexOf(" "));//Slice off the get part to get location http/1.x
                    string pttHcol = locHttp.reverse();//LOL
                    string loc = pttHcol.Substring(pttHcol.IndexOf(" ")).reverse().Trim(); //Location like: asdf.w?asdf=s

                    this.location = loc.Trim().Split(new string[] { "?" }, StringSplitOptions.None)[0]; ; //blargh/asdf.w
                    this.protocol = parts[parts.Length - 1].Trim();


                    //Now we take GETs out...
                    string paramString = loc.Substring(loc.IndexOf("?") + 1);
                    string[] paramParts = paramString.Split(new string[] { "&" }, StringSplitOptions.None); //In the future this should be a qASS

                    for (int j = 0; j < paramParts.Length; j++)
                    {
                        string param = paramParts[j];
                        string name = param.Split(new string[] { "=" }, StringSplitOptions.None)[0];
                        string value = param.Substring(param.IndexOf("=") + 1);

                        _GET.Add(name, value);
                    }

                    //Now we find out the filename by itself
                    this.fileName = this.location.Substring(this.location.LastIndexOf("/")+1);
                    if(fileName.IndexOf(".") != -1)
                        this.fileExt = this.fileName.Substring(this.fileName.LastIndexOf("."));
                    this.directory = this.location.Substring(0, this.location.LastIndexOf("/")+1);
                }
                else if (parts[0].ToUpper() == "COOKIE:")
                {
                    string cookiesString = line.Substring(line.IndexOf(" ")).Trim(); //Cookie=123;asdf=123
                    string[] cookies = cookiesString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < cookies.Length; j++)
                    {
                        string[] keyValue = cookies[j].SplitAtIndex(cookies[j].IndexOf("=")); //Split by first =
                        this.cookies.Add(keyValue[0].Trim(), keyValue[1]);
                    }
                }
            }
            this.requestedLocation = this.location;
        }
    }
}
