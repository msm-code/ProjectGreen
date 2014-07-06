using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace ProjectGreen.Render.Lights
{
    public struct QuadraticCurve
    {
        private Vector3 vector;

        private QuadraticCurve(float a, float b, float c)
            : this()
        {
            this.vector = new Vector3(c, b, a);
        }

        public static QuadraticCurve Const(float constCoeff)
        {
            return new QuadraticCurve(0, 0, constCoeff);
        }

        public static QuadraticCurve Linear(float linearCoeff, float constCoeff)
        {
            return new QuadraticCurve(0, linearCoeff, constCoeff);
        }

        public static QuadraticCurve Quadratic(float quadraticCoeff, float linearCoeff, float constCoeff)
        {
            return new QuadraticCurve(quadraticCoeff, linearCoeff, constCoeff);
        }

        public Vector3 Vector
        { get { return vector; } }
    }
}
