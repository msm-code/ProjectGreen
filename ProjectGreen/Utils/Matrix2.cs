using System;
using OpenTK;

namespace ProjectGreen
{
    struct Matrix2
    {
        float M0, M1, M2, M3;

        public Matrix2(float m0, float m1, float m2, float m3)
        {
            this.M0 = m0;
            this.M1 = m1;
            this.M2 = m2;
            this.M3 = m3;
        }

        public static Matrix2 Rotation(float angle)
        {
            return new Matrix2(
                (float)Math.Cos(angle), -(float)Math.Sin(angle),
                (float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        public static Matrix2 Scale(float scaleX, float scaleY)
        {
            return new Matrix2(
                scaleX, 0,
                0, scaleY);
        }

        public static Matrix2 Identity
        { get { return new Matrix2(1, 0, 0, 1); } }

        public static Vector2 operator *(Matrix2 matrix, Vector2 vec)
        {
            return new Vector2(
                matrix.M0 * vec.X + matrix.M1 * vec.Y,
                matrix.M2 * vec.X + matrix.M3 * vec.Y);
        }

        void SetIdentity()
        {
            M0 = M3 = 1;
            M1 = M2 = 0;
        }
    }
}
