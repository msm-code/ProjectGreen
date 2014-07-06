using OpenTK;
using System;

namespace ProjectGreen
{
    class Camera
    {
        Vector2 center;
        Vector2 size;

        public Camera(Vector2 center, Vector2 size)
        {
            this.Center = center;
            this.Size = size;
        }

        public Vector2 Center
        {
            get { return center; }
            set { center = value; OnChanged(); }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; OnChanged(); }
        }

        public Box2 VisibleRegion
        {
            get { return BoxExt.FromCenterAndSize(Center, Size); }
            set { center = value.Center(); size = value.Size(); OnChanged(); }
        }

        public Vector2 WorldToScreenCoords(Vector2 world)
        {
            Vector2 cameraView = new Vector2(Size.X / 2, Size.Y / -2);
            world -= Center;

            Vector2 screen = new Vector2(world.X / cameraView.X, world.Y / cameraView.Y);

            return screen;
        }

        void OnChanged()
        {
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        public event EventHandler Changed;
    }
}
