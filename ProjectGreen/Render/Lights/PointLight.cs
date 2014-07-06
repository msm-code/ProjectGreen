using OpenTK;
using OpenTK.Graphics;

namespace ProjectGreen.Render.Lights
{
    class PointLight
    {
        public PointLight(Vector2 position, Color4 color, QuadraticCurve attenuation)
        {
            this.Position = position;
            this.Color = color;
            this.Attenuation = attenuation;
            this.PositionZ = 1.5f;
        }

        public Vector2 Position { get; set; }
        public Color4 Color { get; set; }
        public QuadraticCurve Attenuation { get; set; }
        public float PositionZ { get; set; }
    }
}
