using ProjectGreen.Render.Shaders;
using ProjectGreen.Shaders.Sources;
using OpenTK.Graphics.OpenGL;
using ProjectGreen.Render.Lights;
using OpenTK;
using OpenTK.Graphics;

namespace ProjectGreen.Shaders.Programs
{
    class BumpedPointLightProgram : CameredProgram, ILightProgram
    {
        readonly Uniform lightPosition;
        readonly Uniform lightColor;
        readonly Uniform lightAttenuation;
        readonly Uniform colorMap;
        readonly Uniform normalMap;

        public BumpedPointLightProgram()
        {
            string vert = ShaderSources.vert_bumped_dynamic_light;
            string frag = ShaderSources.frag_bumped_dynamic_light;
            base.Compile(vert, frag);

            lightPosition = new Uniform(this, "lightPosition");
            lightColor = new Uniform(this, "lightColor");
            lightAttenuation = new Uniform(this, "lightAttenuation");
            colorMap = new Uniform(this, "colorMap");
            normalMap = new Uniform(this, "normalMap");

            base.InitCamera();
        }

        protected override void OnUse(RenderContext context)
        {
            lightPosition.Set3f(new Vector3(Light.Position.X, Light.Position.Y, Light.PositionZ));
            lightColor.Set3f(Light.Color.ToVector3());
            lightAttenuation.Set3f(Light.Attenuation.Vector);
            colorMap.SetI(ColorMap - TextureUnit.Texture0);
            normalMap.SetI(NormalMap - TextureUnit.Texture0);

            base.OnUse(context);
        }

        public PointLight Light { get; set; }
        public TextureUnit ColorMap { get; set; }
        public TextureUnit NormalMap { get; set; }
    }
}
