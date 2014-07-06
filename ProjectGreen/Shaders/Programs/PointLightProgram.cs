using ProjectGreen.Render.Shaders;
using OpenTK;
using OpenTK.Graphics;
using ProjectGreen.Render.Lights;
using ProjectGreen.Shaders.Sources;
using OpenTK.Graphics.OpenGL;

namespace ProjectGreen.Shaders.Programs
{
    class PointLightProgram : CameredProgram, ILightProgram
    {
        readonly Uniform lightPosition;
        readonly Uniform lightColor;
        readonly Uniform lightAttenuation;
        readonly Uniform colorMap;

        public PointLightProgram()
        {
            string vert = ShaderSources.vert_dynamic_light;
            string frag = ShaderSources.frag_dynamic_light;
            base.Compile(vert, frag);

            lightPosition = new Uniform(this, "lightPosition");
            lightColor = new Uniform(this, "lightColor");
            lightAttenuation = new Uniform(this, "lightAttenuation");
            colorMap = new Uniform(this, "colorMap");

            base.InitCamera();
        }

        protected override void OnUse(RenderContext context)
        {
            lightPosition.Set2f(Light.Position);
            lightColor.Set3f(Light.Color.ToVector3());
            lightAttenuation.Set3f(Light.Attenuation.Vector);
            colorMap.SetI(ColorMap - TextureUnit.Texture0);

            base.OnUse(context);
        }

        public PointLight Light { get; set; }
        public TextureUnit ColorMap { get; set; }

        public TextureUnit NormalMap { get; set; } // Ignored
    }
}
