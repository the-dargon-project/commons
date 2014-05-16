using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wHTTPServer2Lib
{
    public class MutatedFrozenObjectException:Exception
    {
        public MutatedFrozenObjectException() : base("Attempted to mutate an object which was frozen.") { }
    }
}
