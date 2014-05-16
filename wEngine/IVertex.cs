using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wEngine
{
    public interface IVertex
    {
        //It's given that we have a position
        int PositionOffset { get; }
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }

        bool HasColor { get; }
        int Color { get; }
        int Stride { get; }

        byte[] GetBufferData(IVertex[] elements);
    }
}
