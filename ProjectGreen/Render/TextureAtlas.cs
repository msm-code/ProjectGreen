using OpenTK;
using System.Collections.Generic;
namespace ProjectGreen
{
    class TextureAtlas
    {
        Dictionary<string, Box2> sprites;
        Texture texture;

        public TextureAtlas(Texture texture)
        {
            this.sprites = new Dictionary<string, Box2>();
            this.texture = texture;
        }

        public void RegisterSprite(string id, Box2 bounds) { sprites.Add(id, bounds); }

        public Sprite GetSprite(string id) { return new Sprite(texture, sprites[id]); }

        public Sprite this[string s]
        { get { return GetSprite(s); } }
    }
}
