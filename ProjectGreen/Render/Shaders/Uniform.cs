using OpenTK.Graphics.OpenGL;
using OpenTK;
using ProjectGreen.Render.Shaders;

namespace ProjectGreen.Render.Shaders
{
    class Uniform
    {
        int location;

        public Uniform(ShaderProgram program, string id)
        {
            this.location = GL.GetUniformLocation(program, id);
        }

        public void SetI(int i)
        { GL.Uniform1(location, i); }

        public void Set1f(float f)
        { GL.Uniform1(location, f); }

        public void Set2f(float x, float y)
        { GL.Uniform2(location, x, y); }

        public void Set2f(Vector2 vec)
        { GL.Uniform2(location, vec); }

        public void Set3f(Vector3 vector3)
        { GL.Uniform3(location, vector3); }
    }
}
