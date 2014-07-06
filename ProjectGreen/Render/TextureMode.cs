using System.Collections.Generic;
namespace ProjectGreen.Render
{
    enum TextureLocation
    {
        Texture0 = 0,
        Texture1,
        Texture2,
        Texture3,
    }

    class TextureMode
    {
        Dictionary<TextureLocation, Texture> textures;

        public TextureMode()
        {
            textures = new Dictionary<TextureLocation, Texture>();
        }

        public TextureMode(Texture texture)
            :this(TextureLocation.Texture0, texture){}

        public TextureMode(TextureLocation location, Texture texture)
            : this()
        {
            textures.Add(location, texture);
        }

        public Texture this[TextureLocation location]
        {
            get
            {
                if (textures.ContainsKey(location)) { return textures[location]; }
                else { return null; }
            }
            set { textures[location] = value; }
        }

        public IEnumerable<KeyValuePair<TextureLocation, Texture>> Entries
        { get { return textures; } }
    }
}
