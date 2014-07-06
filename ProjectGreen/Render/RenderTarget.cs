using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace ProjectGreen.Render
{
    class RenderTarget
    {
        static ContextStack<RenderTarget> renderTargets = new ContextStack<RenderTarget>((r1, r2) => r1.FboId == r2.FboId);

        int? fboId;
        Texture texture;

        private RenderTarget() // Screen render target
        { fboId = 0; }

        public RenderTarget(int width, int height)
            : this(Texture.Create(width, height))
        { }

        public RenderTarget(Texture texture)
        {
            this.texture = texture;
            this.fboId = null; // framebuffer not initialised
        }

        public void Use(RenderContext rc) 
        {
            EnsureFramebufferInit(rc);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FboId);
            renderTargets.Push(this);
        }

        public void Unuse(RenderContext rc) 
        {
            renderTargets.Pop(this);
            if (renderTargets.Empty)
            { GL.BindFramebuffer(FramebufferTarget.Framebuffer, RenderTarget.Default.FboId); }
            else 
            { GL.BindFramebuffer(FramebufferTarget.Framebuffer, renderTargets.Top.FboId); }
        }

        public void EnsureFramebufferInit(RenderContext rc)
        {
            if (fboId != null) { return; }

            int fbo;
            GL.GenFramebuffers(1, out fbo);
            this.fboId = fbo;

            this.Use(rc);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, 
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D, 
                texture,
                0);
            this.Unuse(rc);
        }

        public Texture Texture
        { get { return texture; } }

        public static void EnsureCreated(ref RenderTarget target)
        {
            if (target == null)
            {
                Vector2 size = Screen.Instance.Size;
                target = new RenderTarget((int)size.X, (int)size.Y);
            }
        }

        public static readonly RenderTarget Default = new RenderTarget();

        int FboId
        { get { return fboId.Value; } }
    }
}
