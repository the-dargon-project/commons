using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib
{
    public interface IPage
    {
        /// <summary>
        /// Handle the client, as in, generate the page.
        /// </summary>
        /// <param name="session">the http session</param>
        void HandleClient(HttpSession session, string filePath);

        /// <summary>
        /// This is called when the page has reached its
        /// maximum execution time.  1 second after
        /// abort is called, wHTTPServer will kill the
        /// thread regardless.
        /// </summary>
        void Abort();

        /// <summary>
        /// The maximum execution time of the IPage.
        /// If the page runs independent, then this is
        /// ignored.
        /// </summary>
        TimeSpan TimeoutInterval
        {
            get;
        }

        /// <summary>
        /// When wHTTPServer sees a page with this set to true,
        /// the server will create a new thread for the page,
        /// to simply call the page's generate method.  wHttpServ
        /// will then simply move onwards, and the socket will be
        /// left on its own.  The page MUST end the socket by itself,
        /// as well as return eventually.  wHTTP will not do this.
        /// 
        /// If this is set, then TimeoutInterval does nothing
        /// </summary>
        bool RunsIndependently
        {
            get;
        }
    }
}
