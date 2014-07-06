using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace ProjectGreen.Render
{
    class Primitives
    {
        BeginMode drawMode;
        int vertexCount;
        Vector2[] verticles;

        public void SetAsPolygon(params Vector2[] verticles)
        {
            this.drawMode = BeginMode.Polygon;
            this.vertexCount = verticles.Length;
            this.verticles = verticles;            
        }

        public void Draw(Texture tex, RenderContext rc)
        { Draw(tex, rc, Color4.White); }

        public void Draw(Texture tex, RenderContext rc, Color4 blendColor)
        {
            rc.TextureMode = new TextureMode(tex);
            GL.Color4(blendColor);
            GL.Begin(drawMode);
            for (int i = 0; i < vertexCount; i++)
            {
                GL.Vertex2(verticles[i]);
            }
            GL.End();
        }
    }
}
