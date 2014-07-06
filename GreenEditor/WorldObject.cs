using System.Xml.Linq;
using System.Windows;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GreenEditor
{
    public class WorldObject
    {
        public Geometry Shape { get; set; }
        public XElement Data { get; set; }
        public Brush DisplayBrush { get; set; }

        public WorldObject(string name)
        {
            Data = new XElement(name);
            DisplayBrush = Brushes.Black;
        }

        public void SetShapeFromRect(Rect rect)
        {
            this.Shape = new RectangleGeometry(rect);
        }

        public void SetShapeFromPoly(List<Point> poly)
        {
            this.Shape = WorldDisplay.CreatePolygon(poly);
        }

        public void SetDataFromRect(Rect rect)
        {
            var centerX = rect.Left + rect.Width / 2;
            var centerY = rect.Top + rect.Height / 2;

            Data.Add(new XAttribute("position", FormatVector(centerX, centerY)));
            Data.Add(new XAttribute("size", FormatVector(rect.Width, rect.Height)));

            this.SetShapeFromRect(rect);
        }

        public void SetDataFromPoly(List<Point> poly)
        {
            StringBuilder data = new StringBuilder();

            double avgX = poly.Sum((x) => x.X) / poly.Count;
            double avgY = poly.Sum((x) => x.Y) / poly.Count;

            foreach (var pt in poly)
            {
                data.Append(FormatVector(pt.X - avgX, pt.Y - avgY) + ",");
            }
            data.Remove(data.Length - 1, 1); // remove last comma

            Data.Add(new XAttribute("verticles", data.ToString()));
            Data.Add(new XAttribute("center", FormatVector(avgX, avgY)));

            this.SetShapeFromPoly(poly);
        }

        public void SetShapeFromData()
        {
            var pos = Data.Attribute("position");
            var size = Data.Attribute("size");

            if (size == null && pos == null) { return; }
            if (size == null && pos != null)
            {
                Point posp = ParseVector(pos.Value);
                SetShapeFromRect(new Rect(posp.X - 0.1, posp.Y - 0.1, 0.2, 0.2));
            }
            if (size != null && pos != null)
            {
                Point posp = ParseVector(pos.Value);
                Point sizep = ParseVector(size.Value);
                SetShapeFromRect(new Rect(posp.X - sizep.X / 2, posp.Y - sizep.Y / 2, sizep.X, sizep.Y));
            }
        }

        public static string FormatVector(double x, double y)
        {
            return string.Format("{0} {1}",
                x.ToString(CultureInfo.InvariantCulture),
                y.ToString(CultureInfo.InvariantCulture));
        }

        public static Point ParseVector(string vec)
        {
            var commaNdx = vec.IndexOf(" ");
            var x = float.Parse(vec.Substring(0, commaNdx), CultureInfo.InvariantCulture);
            var y = float.Parse(vec.Substring(commaNdx + 1), CultureInfo.InvariantCulture);

            return new Point(x, y);
        }
    }
}
