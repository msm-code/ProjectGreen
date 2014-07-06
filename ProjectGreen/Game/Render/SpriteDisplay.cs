using OpenTK;
using ProjectGreen.Game.Render;

namespace ProjectGreen
{
    class BumpedSpriteDisplay : IEnvironmentObject
    {
        public BumpedSpriteDisplay(Box2 bounds, Sprite colors, Sprite normals)
        {
            this.Bounds = bounds;
            this.Colors = colors;
            this.Normals = normals;
        }

        public Box2 Bounds { get; private set; }
        public Sprite Colors { get; private set; }
        public Sprite Normals { get; private set; }

        public void DrawColor(RenderContext rc)
        { rc.DrawSprite(Colors, Bounds); }

        public void DrawNormals(RenderContext rc)
        { rc.DrawSprite(Normals, Bounds); }
    }
}
