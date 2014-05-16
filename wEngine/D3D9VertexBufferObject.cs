using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime;
using SlimDX.Direct3D9;

using System.IO;

namespace ItzWarty.wEngine
{
    public class D3D9VertexBufferObject<E>:IVertexBufferObject
        where E:IVertex, E[]:IVertex[]
    {
        private E[] vertices = null;
        private VertexBuffer vb = null;
        private IVertex dummyInstance = null;
        private Device d3dDevice = null;

        public D3D9VertexBufferObject() { }
        public D3D9VertexBufferObject(Device d3dDevice, int length)
        {
            this.d3dDevice = d3dDevice;
            dummyInstance = (IVertex)Activator.CreateInstance(typeof(E));
            vertices = new E[length];
            vb = CreateVertexBuffer(d3dDevice, length);
        }
        private VertexBuffer CreateVertexBuffer(Device d3dDevice, int length)
        {
            return new VertexBuffer(d3dDevice, dummyInstance.Stride * length, Usage.Dynamic, VertexFormat.Position, Pool.Default);
        }
        public VertexBuffer GetVertexBuffer()
        {
            return null;
            SlimDX.DataStream ds = vb.Lock(0, dummyInstance.Stride * vertices.Length, LockFlags.None);
            ds.Seek(0, SeekOrigin.Begin);
            StreamWriter sw = new StreamWriter(ds);
            for (int i = 0; i < vertices.Length; i++)
            {
            }
            vb.Unlock();
            return vb;
        }
        public int GetVerticesStride()
        {
            return dummyInstance.Stride;
        }
        public int NumElements
        {
            get
            {
                return -1;
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
        public void PreDeviceReset() 
        {
            vb.Dispose();
        }
        public void PostDeviceReset() 
        {
            vb = CreateVertexBuffer(d3dDevice, this.vertices.Length);
        }
        public void Render() { }
    }
}
