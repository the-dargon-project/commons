using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty.Web.HTTP.Headers;

namespace ItzWarty.Web
{
    public interface IPage
    {
        void Generate(RequestHeader reqH, ResponseHeader respH, System.Net.Sockets.Socket socket);
    }
}
