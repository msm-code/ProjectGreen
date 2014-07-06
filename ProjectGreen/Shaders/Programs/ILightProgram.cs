using OpenTK;
using OpenTK.Graphics;
using ProjectGreen.Render.Lights;
using OpenTK.Graphics.OpenGL;

namespace ProjectGreen.Shaders.Programs
{
    interface ILightProgram
    {
        PointLight Light { get; set; }
        TextureUnit ColorMap { get; set; }
        TextureUnit NormalMap { get; set; }
        void Use(RenderContext rc);
    }
}
