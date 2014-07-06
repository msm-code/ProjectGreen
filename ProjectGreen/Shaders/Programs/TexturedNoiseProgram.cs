using ProjectGreen.Render.Shaders;
using ProjectGreen.Shaders.Sources;

namespace ProjectGreen.Shaders.Programs
{
    class TexturedNoiseProgram : ShaderProgram
    {
        readonly Uniform cameraPosition;
        readonly Uniform cameraSize;
        readonly Uniform totalTime;
        readonly Uniform noiseStr;

        public TexturedNoiseProgram()
        {
            string vert = ShaderSources.vert_camera2d;
            string frag = ShaderSources.frag_noised_textured;
            base.Compile(vert, frag);

            cameraPosition = new Uniform(this, "camPosition");
            cameraSize = new Uniform(this, "camSize");
            totalTime = new Uniform(this, "totalTime");
            noiseStr = new Uniform(this, "noiseStr");
            this.NoiseStrength = 0.5f;
        }

        protected override void OnUse(RenderContext context)
        {
            cameraPosition.Set2f(context.Camera.Center);
            cameraSize.Set2f(context.Camera.Size);
            totalTime.Set1f((float)context.TotalTime);
            noiseStr.Set1f(NoiseStrength);
        }

        public float NoiseStrength { get; set; }
    }
}
