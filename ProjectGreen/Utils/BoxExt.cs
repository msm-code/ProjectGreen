using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace ProjectGreen
{
    public static class BoxExt
    {
        public static Box2 FromCenterAndSize(Vector2 center, Vector2 size)
        {
            return new Box2(center.X - size.X / 2, center.Y - size.Y / 2,
                center.X + size.X / 2, center.Y + size.Y / 2);
        }

        public static bool Intersects(Box2 box1, Box2 box2)
        {
            return !(box1.Left > box2.Right ||
                    box1.Right < box2.Left ||
                    box1.Top > box2.Bottom ||
                    box1.Bottom < box2.Top);
        }

        public static Vector2 Intersection(this Box2 box1, Box2 box2)
        {
            Vector2 box1Center = box1.Center();
            Vector2 box2Center = box2.Center();
            Vector2 distance = box1Center - box2Center;

            float xIntersection = (box1.Width + box2.Width) / 2 - Math.Abs(distance.X);
            float yIntersection = (box1.Height + box2.Height) / 2 - Math.Abs(distance.Y);

            if (xIntersection < 0) { return Vector2.Zero; }
            if (yIntersection < 0) { return Vector2.Zero; }

            if (xIntersection < yIntersection)
            { return new Vector2(xIntersection * Math.Sign(distance.X), 0); }
            else
            { return new Vector2(0, yIntersection * Math.Sign(distance.Y)); }
        }

        public static Vector2 TopLeft(this Box2 box)
        {
            return new Vector2(box.Left, box.Top);
        }

        public static Vector2 TopRight(this Box2 box)
        {
            return new Vector2(box.Right, box.Top);
        }

        public static Vector2 BottomLeft(this Box2 box)
        {
            return new Vector2(box.Left, box.Bottom);
        }

        public static Vector2 BottomRight(this Box2 box)
        {
            return new Vector2(box.Right, box.Bottom);
        }

        public static Vector2 Center(this Box2 box)
        {
            return new Vector2(box.Left + box.Width / 2,
                box.Top + box.Height / 2);
        }

        public static Vector2 Size(this Box2 box)
        {
            return new Vector2(box.Width, box.Height);
        }

        public static Box2 FromAnyPoints(Vector2 point1, Vector2 point2)
        {
            float left = Math.Min(point1.X, point2.X);
            float top = Math.Min(point1.Y, point2.Y);
            float right = Math.Max(point1.X, point2.X);
            float bottom = Math.Max(point1.Y, point2.Y);
            return new Box2(left, top, right, bottom);
        }

        public static Box2 FromStartAndSize(Vector2 start, Vector2 size)
        {
            return new Box2(start.X, start.Y, start.X + size.X, start.Y + size.Y);
        }

        public static Box2 FromStartAndSize(float x, float y, float width, float height)
        {
            return new Box2(x, y, x + width, y + height);
        }

        public static bool Contains(this Box2 box, Vector2 point)
        {
            return box.Left < point.X && box.Right > point.X &&
                box.Top < point.Y && box.Bottom > point.Y;
        }

        public static bool IsZero(this Box2 box)
        {
            return box.Left == 0 && box.Right == 0 && box.Top == 0 && box.Bottom == 0;
        }
    }
}
