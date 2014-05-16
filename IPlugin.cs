using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.Web
{
    public interface IPlugin
    {
        object InitPlugin();

        string pluginGUID
        {
            get;
        }
        bool isThreadSafe
        {
            get;
        }
    }
}
