using OpenTK;
using OpenTK.Graphics.OpenGL;
using ProjectGreen.Render.Shaders;
using OpenTK.Graphics;
using ProjectGreen.Render;
using ProjectGreen.Shaders.Programs;

namespace ProjectGreen
{
    class RenderContext
    {
        static RenderTarget flipYHack = null;

        public void BeginScene()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            this.BlendMode = new BlendMode(BlendEquation.Add, BlendSrc.SrcAlpha, BlendDst.OneMinusSrcAlpha);
            //GL.BlendEquation(BlendEquationMode.FuncAdd);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            RenderTarget.EnsureCreated(ref flipYHack);
            flipYHack.Use(this);
        }

        public void EndScene()
        {
            flipYHack.Unuse(this);

            ShaderRepository.Get(ShaderRepository.TexturedDraw).Use(this);

            Box2 reversed = Camera.VisibleRegion;
            {
                float tmp = reversed.Bottom;
                reversed.Bottom = reversed.Top;
                reversed.Top = tmp;
            };

            this.DrawSprite(new Sprite(flipYHack.Texture), reversed);
        }

        public Camera Camera
        { get; private set; }

        public Box2 Viewport
        { get; private set; }

        public double TotalTime
        { get; set; }

        public void ApplyCamera(Camera camera)
        {
            this.Camera = camera;

            /*GL.LoadIdentity();
            GL.Scale(2f / camera.Size.X, -2f / camera.Size.Y, 1);
            GL.Translate(-camera.Center.X, -camera.Center.Y, 0);*/
        }

        BlendMode blend;
        public BlendMode BlendMode
        {
            get { return blend; }
            set
            {
                this.blend = value;
                GL.BlendEquation((BlendEquationMode)blend.Equation);
                GL.BlendFunc((BlendingFactorSrc)blend.Source, (BlendingFactorDest)blend.Destination);
            }
        }

        TextureMode textureMode;
        public TextureMode TextureMode
        {
            get { return textureMode; }
            set
            {
                this.textureMode = value;

                foreach (var entry in textureMode.Entries)
                {
                    GL.ActiveTexture(TextureUnit.Texture0 + (int)entry.Key);
                    GL.BindTexture(TextureTarget.Texture2D, entry.Value);
                }
            }
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            this.Viewport = BoxExt.FromStartAndSize(x, y, width, height);
            GL.Viewport(x, y, width, height);
        }

        public void DrawSprite(Sprite sprite, Box2 box)
        { DrawSprite(sprite, box, Color4.White); }

        public void DrawSprite(Sprite sprite, Box2 box, Color4 blendColor)
        {
            TextureMode = new TextureMode(sprite.Texture);

            GL.Color4(blendColor);
            var tc = sprite.TexCoords;
            DrawQuad(tc, box);
        }

        public void DrawQuad(Box2 tc, Box2 box)
        {
            GL.Begin(BeginMode.Quads);
            { GL.TexCoord2(tc.TopLeft()); GL.Vertex2(box.TopLeft()); }
            { GL.TexCoord2(tc.TopRight()); GL.Vertex2(box.TopRight()); }
            { GL.TexCoord2(tc.BottomRight()); GL.Vertex2(box.BottomRight()); }
            { GL.TexCoord2(tc.BottomLeft()); GL.Vertex2(box.BottomLeft()); }
            GL.End();
        }

        public void Clear(float r, float g, float b, float a)
        {
            GL.ClearColor(r, g, b, a);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
    }
}
