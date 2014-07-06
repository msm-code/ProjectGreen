using System.Collections.Generic;
using ProjectGreen.Shaders.Programs;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using ProjectGreen.Render.Shaders;
using OpenTK.Graphics;
using OpenTK;

namespace ProjectGreen.Render.Lights
{
    class DynamicLightBlender
    {
        ILightProgram lightProgram;
        ShaderProgram textureProgram;
        RenderTarget helperSurface = null;
        IShadowRenderer shadowRenderer;
        List<PointLight> lights;

        public DynamicLightBlender()
        {
            shadowRenderer = new HardShadowRenderer();
            textureProgram = ShaderRepository.Get(ShaderRepository.TexturedDraw);
            lights = new List<PointLight>();

            float ambient = (float)Settings.UserData.GetDouble("display", "ambient-light");
            AmbientLight = new Color4(ambient, ambient, ambient, 1);

            if (Settings.UserData.GetBool("display", "bump-map"))
            { lightProgram = new BumpedPointLightProgram(); }
            else
            { lightProgram = new PointLightProgram(); }
        }

        public Color4 AmbientLight { get; private set; }

        public void Add(PointLight light)
        {
            this.lights.Add(light);
        }

        public void Render(Texture colorMap, Texture normalMap, World world, RenderContext rc)
        {
            RenderTarget.EnsureCreated(ref helperSurface);

            rc.DrawSprite(new Sprite(colorMap), rc.Camera.VisibleRegion, AmbientLight);
            foreach (var light in lights)
            {
                var shadowCasters = world.Shapes;

                helperSurface.Use(rc);
                RenderLightSource(light, colorMap, normalMap, rc);
                DrawShadowmap(light, shadowCasters, rc);
                helperSurface.Unuse(rc);

                BlendLightToSurface(helperSurface.Texture, rc);
            }
        }

        void BlendLightToSurface(Texture texture, RenderContext rc)
        {
            BlendMode oldBlend = rc.BlendMode;
            rc.BlendMode = new BlendMode(BlendEquation.Add, BlendSrc.One, BlendDst.One);

            TextureMode texMode = new TextureMode(TextureLocation.Texture0, texture);
            rc.TextureMode = texMode;

            textureProgram.Use(rc);
            GL.Color4(1f, 1f, 1f, 1f);
            rc.DrawQuad(new Box2(0, 0, 1, 1), rc.Camera.VisibleRegion);

            rc.BlendMode = oldBlend;
        }

        void DrawShadowmap(PointLight light, List<Shape> shadowCasters, RenderContext rc)
        {
            BlendMode oldBlend = rc.BlendMode;
            rc.BlendMode = new BlendMode(BlendEquation.Add, BlendSrc.SrcAlpha, BlendDst.OneMinusSrcAlpha);

            shadowRenderer.Render(light, shadowCasters, rc);

            rc.BlendMode = oldBlend;
        }

        void RenderLightSource(PointLight light, Texture colorMap, Texture normalMap, RenderContext rc)
        {
            BlendMode oldBlend = rc.BlendMode;
            rc.BlendMode = new BlendMode(BlendEquation.Add, BlendSrc.SrcAlpha, BlendDst.OneMinusSrcAlpha);

            lightProgram.Light = light;
            lightProgram.ColorMap = TextureUnit.Texture0;
            lightProgram.NormalMap = TextureUnit.Texture1;

            lightProgram.Use(rc);

            TextureMode texMode = new TextureMode();
            texMode[TextureLocation.Texture0] = colorMap;
            texMode[TextureLocation.Texture1] = normalMap;
            rc.TextureMode = texMode;

            rc.DrawQuad(new Box2(0, 0, 1, 1), rc.Camera.VisibleRegion);

            rc.BlendMode = oldBlend;
        }
    }
}
