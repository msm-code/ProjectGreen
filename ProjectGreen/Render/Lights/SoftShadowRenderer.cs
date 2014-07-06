using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ProjectGreen.Render.Shaders;
using System.Linq;

namespace ProjectGreen.Render.Lights
{
    class SoftShadowRenderer : IShadowRenderer
    {
        private const float MaxShadow = 1000;

        ShaderProgram textureProgram;
        //Texture penumbra;

        public SoftShadowRenderer()
        {
            textureProgram = ShaderRepository.Get(ShaderRepository.TexturedDraw);
        }

        public void Render(PointLight light, List<Shape> shadowCasters, RenderContext rc)
        {
            textureProgram.Use(rc);

            foreach (var caster in shadowCasters)
            {
                if (!caster.UserData.CastsShadow) { continue; }

                throw new System.Exception();
            }
        }


        private void DrawQuad(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, RenderContext rc)
        {
            Primitives quad = new Primitives();
            quad.SetAsPolygon(v1, v2, v3, v4 );
            quad.Draw(Texture.Black, rc);
        }

        private void DrawSoftShadow(Vector2 vStart, Vector2 vLight, Vector2 vDark, RenderContext rc)
        {
            //softShadowProgram.Use(rc);

            //rc.TextureMode = new TextureMode(penumbra);

            GL.Begin(BeginMode.Triangles);
            GL.TexCoord2(0f, 1f);
            GL.Vertex2(vStart);

            GL.TexCoord2(1f, 0f);
            GL.Vertex2(vLight);

            GL.TexCoord2(0f, 0f);
            GL.Vertex2(vDark);
            GL.End();

            //Primitives quad = new Primitives();
            //quad.SetAsPolygon(new List<Vector2>() { vStart, vLight, vDark });
            //quad.Draw(Texture.Black, rc);
        }
    }
}
