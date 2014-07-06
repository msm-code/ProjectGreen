using OpenTK;
namespace ProjectGreen
{
    class Sprite : IVisual
    {
        public Sprite(Texture texture, Box2 texCoords)
        {
            this.Texture = texture;
            this.TexCoords = texCoords;
        }

        public Sprite(Texture texture)
            : this(texture, new Box2(0, 0, 1, 1))
        { }

        public Texture Texture { get; private set; }
        public Box2 TexCoords { get; private set; }

        public void Render(Box2 bounds, RenderContext rc)
        {
            rc.DrawSprite(this, bounds);
        }
    }
}
