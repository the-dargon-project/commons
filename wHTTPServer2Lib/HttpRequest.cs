using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty;

namespace ItzWarty.wHTTPServer2Lib
{
    public class HttpRequest
    {
        public string requestMethod             = "";
        public string requestURL                = "";
        public string requestProtocol           = "";

        public string fAccept                   = "";
        public string fAcceptCharset            = "";
        //accept encoding
        //accept language
        //accept ranges
        //authorization
        public string fCacheControl             = "";
        public string fConnection               = "";
        public StringCollection fCookies        = new StringCollection();
        public string fContentLength            = "";
        public string fContentType              = "";
        public string fDate                     = "";
        //expect
        public string fFrom                     = "";  //Email of user
        public string fHost                     = "";  //Host, ie: subdomain.itzwarty.com or itzwarty.com or www.itzwarty.com
        //ifwhatever
        //maxforwards
        //pragma
        //proxy
        //requests only a part of an entity
        //referer
        //transfer encoding
        //upgrade
        public string fUserAgent                = "";
        //via
        //warning

        public StringCollection GET             = new StringCollection();

        public static HttpRequest Parse(string s)
        {
            HttpRequest result = new HttpRequest();

            Console.WriteLine("Parsing:" + s);
            string[] lines = s.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].QASS(' ');
                string contentOfHeader = String.Join(" ", tokens.SubArray(1, tokens.Length-1));
                switch (tokens[0].ToLower())
                {
                    case "get":
                        result.requestMethod    = "GET";

                        string[] urlParts       = (tokens[1].IndexOf("?") == -1)?new string[]{tokens[1]}:tokens[1].SplitAtIndex(tokens[1].IndexOf("?"));

                        result.requestURL       = urlParts[0];
                        result.requestProtocol  = tokens[2].Trim();

                        if (urlParts.Length > 1)
                        {
                            string[] nameEqualsValues = urlParts[1].Split("&");
                            foreach (string nev in nameEqualsValues)
                            {
                                string[] nameValue = nev.SplitAtIndex(nev.IndexOf('='));
                                result.GET[nameValue[0]] = nameValue[1];
                            }
                        }
                        break;
                    case "accept:":
                        result.fAccept          = contentOfHeader;
                        break;
                    case "accept-charset:":
                        result.fAcceptCharset   = contentOfHeader;
                        break;
                    case "cache-control:":
                        result.fCacheControl    = contentOfHeader;
                        break;
                    case "connection:":
                        result.fConnection      = contentOfHeader;
                        break;
                    case "cookie:":
                        result.fCookies         = new StringCollection(contentOfHeader, ";");
                        break;
                    case "content-length:":
                        result.fContentLength   = contentOfHeader;
                        break;
                    case "content-type:":
                        result.fContentType     = contentOfHeader;
                        break;
                    case "date:":
                        result.fDate            = contentOfHeader;
                        break;
                    case "from:":
                        result.fFrom            = contentOfHeader;
                        break;
                    case "host:": //ie: www.ItzWarty.com
                        string host = contentOfHeader;
                        if (host.ToLower() == g.domain.ToLower() || host == "127.0.0.1")
                            host = "www." + g.domain; //itzwarty.com => www.itzwarty.com, 127.0.0.1 -> www.it...

                        result.fHost            = host;
                        break;
                    case "user-agent:":
                        result.fUserAgent       = contentOfHeader.Trim();
                        break;
                    default:
                        //Console.WriteLine("Line:" + lines[i]);
                        //foreach (string token in tokens)
                        //    Console.WriteLine(" token:" + token);
                        break;
                }
            }
            result.GET.Freeze();
            result.fCookies.Freeze();
            return result;
        }
    }
}
