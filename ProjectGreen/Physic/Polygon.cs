using OpenTK;
using System.Linq;

namespace ProjectGreen
{
    class PolygonData
    {
        readonly Box2CS.Shape shape;

        internal PolygonData(Box2CS.Shape shape)
        {
            this.shape = shape;
        }

        public static PolygonData CreateBox(float halfWidth, float halfHeight)
        { return CreateBox(halfWidth, halfHeight, 0, Vector2.Zero); }

        public static PolygonData CreateBox(float halfWidth, float halfHeight, Vector2 centeroid)
        { return CreateBox(halfWidth, halfHeight, 0, centeroid); }

        public static PolygonData CreateBox(float halfWidth, float halfHeight, float angle)
        { return CreateBox(halfWidth, halfHeight, angle, Vector2.Zero); }

        public static PolygonData CreateBox(float halfWidth, float halfHeight, float angle, Vector2 centeroid)
        {
            var shape = new Box2CS.PolygonShape(halfWidth, halfHeight, centeroid.ToBox2(), angle);
            return new PolygonData(shape);
        }

        public static PolygonData Create(params Vector2[] verticles)
        {
            var shape = new Box2CS.PolygonShape(verticles.Select(x => x.ToBox2()).ToArray());
            return new PolygonData(shape);
        }

        internal Box2CS.Shape Shape
        { get { return shape; } }
    }
}
