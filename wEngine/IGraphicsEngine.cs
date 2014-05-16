using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace ItzWarty.wEngine
{
    public delegate void OnLoadHandler();
    public delegate void OnRenderFrameHandler();
    public interface IGraphicsEngine
    {
        void Reset();
        void Run();
        void Dispose();

        void Clear(Color color);
        void BeginScene();
        void EndScene();
        void Present();

        IVertexBufferObject CreateVertexBuffer<E>(int length) where E:IVertex;
        void SetStreamSource<E>(int n, IVertexBufferObject vbo) where E:IVertex;

        event OnLoadHandler Load;
        event OnRenderFrameHandler RenderFrame;
    }
}
