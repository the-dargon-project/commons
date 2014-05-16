using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib
{
    public interface IPlugin: IDisposable
    {
        object Query(object message, object a, object b);
        string PluginName
        {
            get;
        }
    }
}
