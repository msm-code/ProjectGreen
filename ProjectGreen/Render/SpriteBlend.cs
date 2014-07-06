using OpenTK;
using OpenTK.Graphics;

namespace ProjectGreen.Render
{
    class SpriteBlend : IVisual
    {
        public SpriteBlend(Sprite spriteA, Sprite spriteB, float factor)
        {
            this.SpriteA = spriteA;
            this.SpriteB = spriteB;
            this.Factor = factor;
        }

        public Sprite SpriteA {get; set;}
        public Sprite SpriteB { get; set; }
        public float Factor { get; set; }

        public void Render(Box2 bounds, RenderContext rc)
        {
            Color4 color = Color4.White;
            color.A = Factor;
            rc.DrawSprite(SpriteA, bounds, color);

            color.A = 1 - Factor;
            rc.DrawSprite(SpriteB, bounds, color);
        }
    }
}
