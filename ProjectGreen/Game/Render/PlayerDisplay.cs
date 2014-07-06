using System.Collections.Generic;
using OpenTK;
using ProjectGreen.Render;
using OpenTK.Graphics;
using System.Linq;
using ProjectGreen.Skeletal;

namespace ProjectGreen.Game.Render
{
    class PlayerDisplay : IEnvironmentObject
    {
        Body playerBody;
        Skeleton skeleton;
        SkeletonRenderer skeletonRender;

        public PlayerDisplay(Body playerBody, Skeleton skeleton)
        {
            this.playerBody = playerBody;
            this.skeleton = skeleton;
            this.skeletonRender = new SkeletonRenderer(skeleton);
        }

        public void DrawColor(RenderContext rc)
        {
            skeletonRender.Render(Color4.Black, playerBody.WorldCenter, rc);
        }

        public void DrawNormals(RenderContext rc)
        {
            skeletonRender.Render(new Color4(127, 127, 255, 255), playerBody.WorldCenter, rc);
        }

        void DrawShape(Shape shape, RenderContext rc, Color4 color)
        {
            var verticles = shape.WorldVerticles.ToArray();

            Primitives primitives = new Primitives();
            primitives.SetAsPolygon(verticles);
            primitives.Draw(Texture.White, rc, color);
        }
    }
}
