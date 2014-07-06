using ProjectGreen.Render.Shaders;
using OpenTK;
namespace ProjectGreen.Shaders.Programs
{
    abstract class CameredProgram : ShaderProgram
    {
        Uniform cameraPosition;
        Uniform cameraSize;

        protected void InitCamera()
        {
            cameraPosition = new Uniform(this, "camPosition");
            cameraSize = new Uniform(this, "camSize");
        }

        protected override void OnUse(RenderContext context)
        {
            Camera camera = context.Camera;
            camera.Changed += (sender, e) => SetCamera((Camera)sender);
            SetCamera(camera);
        }

        void SetCamera(Camera camera)
        {
            Vector2 center = camera.Center;
            Vector2 size = camera.Size;

            cameraPosition.Set2f(center);
            cameraSize.Set2f(size);
        }
    }
}
