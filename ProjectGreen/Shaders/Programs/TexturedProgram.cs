using ProjectGreen.Render.Shaders;
using ProjectGreen.Shaders.Sources;

namespace ProjectGreen.Shaders.Programs
{
    class TexturedProgram : CameredProgram
    {
        public TexturedProgram()
            : base()
        {
            string vert = ShaderSources.vert_camera2d;
            string frag = ShaderSources.frag_textured;
            base.Compile(vert, frag);

            base.InitCamera();
        }
    }
}
