using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wEngine
{
    public interface IVertexBufferObject
    {
        int NumElements { get; }
        IVertex this[int i] { get; set; }

        void PreDeviceReset();
        void PostDeviceReset();

        void Render();
    }
}
