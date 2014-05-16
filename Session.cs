using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.Web
{
    public class Session
    {
        public Dictionary<string, object> sessionVariables = new Dictionary<string,object>();
        public string sID;
    }
}
