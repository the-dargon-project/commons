using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib
{
    public class ClientSession
    {
        private static List<ClientSession> sessions = new List<ClientSession>();
        /// <summary>
        /// Opens the ClientSession from the given HttpSession.  If one doesn't
        /// already exist, a new one is created.  Also updates the HttpSession's
        /// cookies with our ClientSession, so that this session can be found in
        /// future usage.
        /// 
        /// Note that if Session.Open isn't called, the client's cookies aren't
        /// refreshed, so this session could be forgotten.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static ClientSession Open(HttpSession session)
        {
            ClientSession finalSession = null;
            if (session.Request.fCookies["wsid"] != "")
            {
                string expectedWsid = session.Request.fCookies["wsid"];
                for(int i = 0; i < sessions.Count && finalSession == null; i++)
                {
                    if (sessions[i].MatchesRequest(session.Request))
                        finalSession = sessions[i];
                }
            }//otherwise, we're going to make a new one

            if (finalSession == null)
            {
                finalSession = new ClientSession(session.Request);
            }
            session.Response.outCookies["wsid"] = finalSession.SessionID;
            return finalSession;
        }

        string ownerUserAgent = "";
        string wSessionId     = "";
        Dictionary<string, object> keyValues = new Dictionary<string, object>();

        private ClientSession(HttpRequest request)
        {
            this.ownerUserAgent = request.fUserAgent;
            this.wSessionId     = Guid.NewGuid().ToString("N");

            sessions.Add(this);
        }
        /// <param name="request"></param>
        /// <returns>Whether or not requests have the same origin</returns>
        public bool MatchesRequest(HttpRequest request)
        {
            /*
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(request.fUserAgent);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(ownerUserAgent);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(request.fCookies["wsid"]);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(wSessionId);
            */
            Console.WriteLine("  ua1: " + request.fUserAgent);
            Console.WriteLine("  ua2: " + ownerUserAgent);
            //Console.WriteLine("wsid1: " + request.fCookies["wsid"]);
            //Console.WriteLine("wsid2: " + wSessionId);
            return request.fUserAgent.Equals(ownerUserAgent);// &&
                   //request.fCookies["wsid"].Equals(wSessionId);
        }
        /// <summary>
        /// Gets the given session variable
        /// </summary>
        /// <param name="key">Key to lookup</param>
        public object this[string key]
        {
            get
            {
                return this.keyValues[key];
            }
            set
            {
                this.keyValues[key] = value;
            }
        }

        public T Get<T>(string key)
        {
            return (T)this[key];
        }
        public void Set(string key, object value)
        {
            this[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return this.keyValues.ContainsKey(key);
        }
        public string SessionID { get { return this.wSessionId; } }
    }
}
