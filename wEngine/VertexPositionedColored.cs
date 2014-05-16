using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System.IO;

namespace ItzWarty.wEngine
{
    public class VertexPositionedColored:IVertex
    {
        public VertexPositionedColored() { }
        public VertexPositionedColored(Vector3 pos, Color4 c)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
            Color = c.ToArgb();
        }
        public VertexPositionedColored(float x, float y, float z, Color4 c)
        {
            X = x;
            Y = y;
            Z = z;
            Color = c.ToArgb();
        }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int Color { get; set; }

        public int Stride
        {
            get
            {
                return sizeof(float) * 3 + sizeof(int);
            }
        }
        public int PositionOffset
        {
            get
            {
                return 0;
            }
        }

        public bool HasColor
        {
            get
            {
                return true;
            }
        }
        public int ColorOffset
        {
            get
            {
                return sizeof(int) * 3;
            }
        }

        public byte[] GetBufferData(IVertex[] elements)
        {
            byte[] result = new byte[elements.Length * this.Stride];
            MemoryStream ms = new MemoryStream(result);
            ms.Seek(0, SeekOrigin.Begin);
            BinaryWriter bw = new BinaryWriter(ms);
            for (int i = 0; i < elements.Length; i++)
            {
                VertexPositionedColored v = (VertexPositionedColored)elements[i];
                bw.Write(v.X);
                bw.Write(v.Y);
                bw.Write(v.Z);
                bw.Write(v.Color);
            }
            bw.Flush();
            ms.Close();
            return result;
        }
    }
}
