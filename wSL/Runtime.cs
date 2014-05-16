using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wSL
{
    public class Runtime
    {
        private List<Thread> threads    = null;
        private Thread mainThread       = null;
        public Runtime()
        {
            this.Initialize();
        }
        private void Initialize()
        {
            this.threads = new List<Thread>();
        }
    }
}
