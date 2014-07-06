using System.Drawing;
using System;

namespace ProjectGreen
{
    static class TextRenderer
    {
        public static Texture Render(string text, Font font)
        {
            SizeF size = TextMeasurer.Measure(text, font);
            int width = (int)Math.Ceiling(size.Width);
            int height = (int)Math.Ceiling(size.Height);

            Bitmap bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawString(text, font, Brushes.Black, PointF.Empty);
            }

            return Texture.FromBitmap(bmp);
        }
    }

    static class TextMeasurer
    {
        // There is no way(*) to measure length of string without
        // using graphics object. Stupid. So this is what this hack
        // is for - measure length of text.
        // (*) - except (unprecise) TextMeasurer in System.Windows.Forms (!) namespace.
        static readonly Bitmap hack = new Bitmap(1, 1);
        static readonly Graphics strMeasure = Graphics.FromImage(hack);

        public static SizeF Measure(string text, Font font)
        {
            return strMeasure.MeasureString(text, font);
        }
    }
}
