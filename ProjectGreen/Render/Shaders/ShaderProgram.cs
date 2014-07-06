using OpenTK.Graphics.OpenGL;
namespace ProjectGreen.Render.Shaders
{
    abstract class ShaderProgram
    {
        int id;

        private ShaderProgram(int id)
        { this.id = id; }

        protected ShaderProgram(string vertSource, string fragSource)
        {
            this.Compile(vertSource, fragSource);
        }

        protected ShaderProgram() { }

        public void Use(RenderContext rc)
        {
            activeProgram = this;
            GL.UseProgram(this.id);
            this.OnUse(rc);
        }

        protected virtual void OnUse(RenderContext rc) { }

        #region Compilation
        protected void Compile(string vertexSrc, string fragmentSrc)
        {
            int vertShader = GL.CreateShader(ShaderType.VertexShader);
            int fragShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertShader, vertexSrc);
            GL.ShaderSource(fragShader, fragmentSrc);

            GL.CompileShader(vertShader);
            GL.CompileShader(fragShader);

            CheckCompileStatus(vertShader);
            CheckCompileStatus(fragShader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertShader);
            GL.AttachShader(program, fragShader);

            GL.LinkProgram(program);
            CheckLinkStatus(program);

            this.id = program;
        }

        void CheckCompileStatus(int shader)
        {
            int status;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
            if (status != (int)OpenTK.Graphics.OpenGL.Boolean.True)
            { throw new System.InvalidOperationException("Compile shader error"); }
        }

        void CheckLinkStatus(int program)
        {
            int status;
            GL.GetProgram(program, ProgramParameter.LinkStatus, out status);
            if (status != (int)OpenTK.Graphics.OpenGL.Boolean.True)
            { throw new System.InvalidOperationException("Link program error"); }
        }
        #endregion

        public static implicit operator int(ShaderProgram prog)
        { return prog.id; }

        #region State Saving
        static ShaderProgram activeProgram;
        static ContextStack<ShaderProgram> stack = new ContextStack<ShaderProgram>((x, y) => (int)x == (int)y);

        public static void Save()
        {
            stack.Push(activeProgram);
        }

        public static void Restore(RenderContext rc)
        {
            stack.Pop(activeProgram);
            activeProgram.Use(rc);
        }
        #endregion
    }
}
