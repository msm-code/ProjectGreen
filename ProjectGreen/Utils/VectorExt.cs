using OpenTK;
using System.Globalization;
using System;

namespace ProjectGreen
{
    static class VectorExt
    {
        public static bool Equal(this Vector2 vec, Vector2 other)
        {
            return !(vec - other).IsZero();
        }

        public static bool IsZero(this Vector2 vec)
        {
            const float epsilon = 0.000001f;

            return !(vec.X > epsilon ||
                   vec.X < -epsilon ||
                   vec.Y > epsilon ||
                   vec.Y < -epsilon);
        }

        public static Vector2 Parse(string vec)
        {
            vec = vec.Trim();
            var spaceNdx = vec.IndexOf(" ");
            var x = float.Parse(vec.Substring(0, spaceNdx), CultureInfo.InvariantCulture);
            var y = float.Parse(vec.Substring(spaceNdx + 1), CultureInfo.InvariantCulture);

            return new Vector2(x, y);
        }

        public static Vector2 Rotate(Vector2 vector, float rotation)
        {
            return new Vector2(
                vector.X * (float)Math.Cos(rotation) - vector.Y * (float)Math.Sin(rotation),
                vector.Y * (float)Math.Cos(rotation) + vector.X * (float)Math.Sin(rotation));
        }

        public static float Rotation(this Vector2 vec)
        {
            float angle = (float)Math.Atan2(vec.Y, vec.X);

            if (angle < 0) { angle += 2 * (float)Math.PI; }

            return angle;
        }

        public static Vector2 FromRotationAndLength(float angle, float length)
        {
            return new Vector2(
                (float)Math.Cos(angle) * length,
                (float)Math.Sin(angle) * length);
        }

        public static Vector2 Normalized(this Vector2 vec)
        {
            vec.Normalize();
            return vec;
        }

        public static Vector2 NormalizedTo(this Vector2 vec, float length)
        {
            vec.Normalize();
            return vec * length;
        }
    }
}
