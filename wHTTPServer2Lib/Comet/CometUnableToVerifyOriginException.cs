using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib.Comet
{
    public class CometUnableToVerifyOriginException:Exception
    {
        public CometUnableToVerifyOriginException():base("wHTTPServer2Lib.Comet unable to verify remote endpoint to link Comet Sessions")
        {
        }
    }
}
