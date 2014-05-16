using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItzWarty.Web.HTTP.Headers;

namespace ItzWarty.Web
{
    public static class SessionManager
    {
        static readonly TimeSpan sessionTimeout = TimeSpan.FromMinutes(60 * 1); //You can AFK for 1 hours before a new session starts


        static Dictionary<string /*Cookie:sid*/, Session /*Session object*/> sessions = new Dictionary<string, Session>();

        /*ssid*/
        public static string serverSessionID = Guid.NewGuid().ToString();

        public static Session GetSession(RequestHeader reqH, ResponseHeader respH)
        {
            if (reqH.cookies.ContainsKey("ssid") &&             /*Check the cookies has a server session id */
                reqH.cookies["ssid"] == serverSessionID &&      /*Make sure that serv session ID is from this server instance*/
                reqH.cookies.ContainsKey("sid") &&              /*Check if the reqH has the session ID*/
                sessions.ContainsKey(reqH.cookies["sid"])               /*Check that we know about the session ID*/
               )
            {
                return sessions[reqH.cookies["sid"]];
            }else{
                return CreateNewSession(reqH, respH);
            }
        }
        private static Session CreateNewSession(RequestHeader reqH, ResponseHeader respH)
        {
            Session newSession = new Session();                 /*Create new session object*/
            string sID = DateTime.Now.ToFileTimeUtc().ToString() + 
                         Guid.NewGuid().ToString();             /*And get a new guid for it... To prevent collision we prepend time*/

            newSession.sID = sID;

            respH.setCookie("ssid", serverSessionID, DateTime.Now + sessionTimeout);
            respH.setCookie("sid", sID, DateTime.Now + sessionTimeout);

            return newSession;
        }
    }
}
