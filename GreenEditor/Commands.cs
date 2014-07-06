using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System;
using System.IO;

namespace GreenEditor
{
    class ExitCommand : IEditorCommand
    {
        public string GetDescription() { return "Add Exit"; }

        public void Execute(WorldDisplay display)
        {
            WorldObject obj = new WorldObject("exit");
            display.SelectRectangle((rect) =>
            {
                obj.SetDataFromRect(rect);
                obj.DisplayBrush = Brushes.Green.WithAlphaSetTo(128);

                display.AddObject(obj);
            });
        }
    }

    class PlayerCommand : IEditorCommand
    {
        public string GetDescription()
        { return "Set Player"; }

        public void Execute(WorldDisplay display)
        {
            WorldObject obj = new WorldObject("player");
            display.SelectPoint((pt) =>
            {
                const double sizeX = 1.5;
                const double sizeY = 1.5;

                obj.SetShapeFromRect(new Rect(pt.X - sizeX / 2, pt.Y - sizeY / 2, sizeX, sizeY));
                obj.Data.Add(new XAttribute("position", WorldObject.FormatVector(pt.X, pt.Y)));
                obj.DisplayBrush = Brushes.Red.WithAlphaSetTo(128);

                display.AddObject(obj);
            });
        }
    }

    class WallCommand : IEditorCommand
    {
        public string GetDescription()
        { return "Add Static Body"; }

        public void Execute(WorldDisplay display)
        {
            WorldObject obj = new WorldObject("body");
            display.SelectPolygon((poly) =>
            {
                if (poly.Count < 3) { return; }

                obj.SetDataFromPoly(poly);
                obj.DisplayBrush = Brushes.Cyan.WithAlphaSetTo(128);

                display.AddObject(obj);
            });
        }
    }

    class SpriteCommand : IEditorCommand
    {
        public string GetDescription() { return "Add Bg Sprite"; }

        public void Execute(WorldDisplay display)
        {
            WorldObject obj = new WorldObject("sprite");
            display.SelectRectangle((rect) =>
            {
                obj.SetDataFromRect(rect);
                OpenFileDialog dialog = new OpenFileDialog();
                bool? result = dialog.ShowDialog();
                string file = dialog.FileName;
                if (result == null || !result.Value) { return; }
                obj.DisplayBrush = new ImageBrush(new BitmapImage(new Uri(file)));
                obj.Data.Add(new XAttribute("texture", Path.GetFileNameWithoutExtension(file)));

                display.AddObject(obj);
            });
        }
    }

    static class ExtBrush
    {
        public static SolidColorBrush WithAlphaSetTo(this SolidColorBrush b, byte alpha)
        {
            var newBrush = b.Clone();
            Color c = b.Color;
            c.A = alpha;
            newBrush.Color = c;
            return newBrush;
        }
    }
}
