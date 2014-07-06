using System.IO;
using System.Drawing;
namespace ProjectGreen
{
    class TextureDecoder : IContentDecoder
    {
        public object Decode(StreamReader data)
        {
            var bitmap = new Bitmap(data.BaseStream);
            var texture = Texture.FromBitmap(bitmap);
            return texture;
        }
    }
}
