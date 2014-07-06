using OpenTK;
using OpenTK.Graphics;

namespace ProjectGreen
{
    static class ColorExt
    {
        public static Vector3 ToVector3(this Color4 color)
        {
            return new Vector3(color.R, color.G, color.B);
        }
    }
}
