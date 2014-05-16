using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib.Comet
{
    public static class CometSessionManager
    {
        private static Dictionary<string, CometSession> cometSessions = new Dictionary<string, CometSession>();
        public static void RegisterSession(CometSession cometSession, string cometSessionGuid)
        {
            CometSessionManager.cometSessions.Add(cometSessionGuid, cometSession);
        }
        /// <summary>
        /// 
        /// throws CometUnableToVerifyOriginException
        /// </summary>
        /// <param name="requestInfo"></param>
        /// <param name="sessionGuid"></param>
        /// <returns></returns>
        public static CometSession GetCometSession(HttpSession session, string cometSessionGuid)
        {
            if (cometSessions.ContainsKey(cometSessionGuid))
            {
                CometSession theSession = cometSessions[cometSessionGuid];

                //Ensure that we have the same origin, so this isn't a hacker
                if (theSession.RequestHasSameOrigin(session))
                {
                    Console.WriteLine("Got same request origin!");
                    return theSession;
                }
                else
                {
                    Console.WriteLine("Unable to verify origin exception");
                    return null; // throw new CometUnableToVerifyOriginException();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
