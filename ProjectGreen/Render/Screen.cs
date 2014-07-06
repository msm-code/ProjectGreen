using OpenTK;
using System;
namespace ProjectGreen.Render
{
    class Screen
    {
        static Screen instance = new Screen();
        public static Screen Instance
        { get { return instance; } }

        private Screen() { }

        public Vector2 Size { get; private set; }
        public int Width { get { return (int)Size.X; } }
        public int Height { get { return (int)Size.Y; } }

        public void BindToWindow(GameWindow window)
        {
            window.Resize += (sender, e) =>
            {
                var wnd = (GameWindow)sender;
                OnResize(wnd.Width, wnd.Height);
            };

            OnResize(window.Width, window.Height);
        }

        void OnResize(int width, int height)
        {
            Size = new Vector2(width, height);

            if (Resized != null)
            { Resized(this, EventArgs.Empty); }
        }

        public event EventHandler Resized;
    }
}
