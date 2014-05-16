using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ItzWarty.wEngine
{
    public unsafe class OpenGLVertexBufferObject<E>:IVertexBufferObject
        where E:IVertex
    {
        private E[] vertices = null;

        private uint vboID = 0;
        private uint eboID = 0;
        private int numElements = 0;

        private IVertex dummyInstance = null;

        public OpenGLVertexBufferObject(int length)
        {
            dummyInstance = (IVertex)Activator.CreateInstance(typeof(E));
            vertices = new E[length];
        }
        public object GetVerticesData()
        {
            return null;
        }
        public object GetVerticesStride()
        {
            return null;
        }
        public int NumElements
        {
            get
            {
                return numElements;
            }
        }
        public IVertex this[int index]
        {
            get
            {
                return this.vertices[index];
            }
            set
            {
                this.vertices[index] = (E)value;
            }
        }
        public uint VboID
        {
            get
            {
                return this.vboID;
            }
        }

        public void PreDeviceReset() { }
        public void PostDeviceReset() { }
  
        public void Render()
        {
            if (dummyInstance is VertexPositionedColored)
            {
                //GL.PushClientAttrib(ClientAttribMask.ClientAllAttribBits);
                GL.EnableClientState(ArrayCap.ColorArray);
                GL.EnableClientState(ArrayCap.VertexArray);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
                GL.BufferData(BufferTarget.ArrayBuffer, 
                    new IntPtr(vertices.Length * BlittableValueType.StrideOf(new int[4])),
                    dummyInstance.GetBufferData(
                        (IVertex[])(vertices)
                    ),
                    BufferUsageHint.StaticDraw);
//                GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboID);
                //GL.VertexPointer(3, VertexPointerType.Float, dummyInstance.Stride, 0);
//                GL.ColorPointer(4, ColorPointerType.Byte, dummyInstance.Stride, 3*sizeof(float));

                //GL.DrawElements(BeginMode.Triangles, 1, DrawElementsType.UnsignedShort, IntPtr.Zero);
                //GL.PopClientAttrib();
            }
        }
    }
}
