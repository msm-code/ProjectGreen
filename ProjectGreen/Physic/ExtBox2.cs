using OpenTK;
using Box2CS;

namespace ProjectGreen
{
    static class ExtBox2Vector
    {
        public static Vector2 ToGl(this Vec2 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static Vec2 ToBox2(this Vector2 vec)
        {
            return new Vec2(vec.X, vec.Y);
        }

        public static Box2 ToGl(this AABB box)
        {
            return new Box2(box.LowerBound.X, box.LowerBound.Y, box.UpperBound.X, box.UpperBound.Y);
        }

        public static AABB ToBox2(this Box2 box)
        {
            return new AABB(new Vec2(box.Left, box.Top), new Vec2(box.Right, box.Bottom));
        }
    }
}
