using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ProjectGreen.Render.Shaders;
using System.Linq;

namespace ProjectGreen.Render.Lights
{
    class HardShadowRenderer : IShadowRenderer
    {
        private const float MaxShadow = 1000;
        ShaderProgram textureProgram;

        public HardShadowRenderer()
        {
            textureProgram = ShaderRepository.Get(ShaderRepository.TexturedDraw);
        }  

        public void Render(PointLight light, List<Shape> shadowCasters, RenderContext rc)
        {
            textureProgram.Use(rc);

            foreach (var caster in shadowCasters)
            {
                if (!caster.UserData.CastsShadow) { continue; }

                RenderShadow(light, caster, rc);
            }
        }

        private void RenderShadow(PointLight light, Shape caster, RenderContext rc)
        {
            List<Vector2> verticles = caster.WorldVerticles.ToList();
            List<Vector2> normals = caster.Normals.ToList();
            int vCount = verticles.Count;
            for (int currVertexNdx = 0; currVertexNdx < vCount; currVertexNdx++)
            {
                int nextVertexNdx = currVertexNdx + 1;
                if (nextVertexNdx >= vCount) { nextVertexNdx = 0; }

                Vector2 currVertex = verticles[currVertexNdx];
                Vector2 nextVertex = verticles[nextVertexNdx];

                Vector2 lightToCurrDir = currVertex - light.Position;
                Vector2 lightToNextDir = nextVertex - light.Position;

                 if (Vector2.Dot(lightToCurrDir, normals[currVertexNdx]) > 0)
                 { continue; }

                lightToCurrDir.Normalize();
                lightToNextDir.Normalize();

                Vector2 currVertexFar = currVertex + lightToCurrDir * MaxShadow;
                Vector2 nextVertexFar = nextVertex + lightToNextDir * MaxShadow;

                DrawQuad(currVertex, nextVertex, nextVertexFar, currVertexFar, rc);
            }
        }

        private void DrawQuad(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, RenderContext rc)
        {
            Primitives quad = new Primitives();
            quad.SetAsPolygon(v1, v2, v3, v4);
            quad.Draw(Texture.Black, rc);
        }
    }
}
