using OpenTK;
using System.Collections.Generic;
using Poly2Tri;
using System.Linq;

namespace ProjectGreen.Algorithms
{
    class PolygonTriangulator
    {
        static PolygonTriangulator instance = new PolygonTriangulator();
        private PolygonTriangulator() { }
        public static PolygonTriangulator Instance { get { return instance; } }

        public List<Vector2[]> Triangulate(Vector2[] input)
        {
            var points= input.Select((x)=>new PolygonPoint(x.X, x.Y));

            Polygon polygon = new Polygon(points);
            P2T.Triangulate(polygon);
            var triangles = polygon.Triangles;

            var trianglePoints = triangles.Select((x) =>
            {
                return x.Points.Select((y) => new Vector2((float)y.X, (float)y.Y)).ToArray();
            });

            return trianglePoints.ToList();
        }
    }
}
