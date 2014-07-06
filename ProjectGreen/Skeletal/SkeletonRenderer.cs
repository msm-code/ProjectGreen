using OpenTK;
using OpenTK.Graphics;
using ProjectGreen.Render;
using OpenTK.Graphics.OpenGL;

namespace ProjectGreen.Skeletal
{
    class SkeletonRenderer
    {
        Skeleton skeleton;

        public SkeletonRenderer(Skeleton skeleton)
        {
            this.skeleton = skeleton;
        }

        public void Render(Color4 color, Vector2 center, RenderContext rc)
        {
            DrawBone(center + TranslationVector, skeleton.Root, color, rc);
        }

        Vector2 TranslationVector
        {
            get
            {
                float angle = skeleton.Animation.Get(skeleton.Root.Id).RelativeAngle;
                float length = skeleton.Animation.CurrentTranslation;
                return VectorExt.FromRotationAndLength(angle, length);
            }
        }

        void DrawBone(Vector2 startPoint, Bone bone, Color4 color, RenderContext rc)
        {
            Vector2 endPoint = startPoint + bone.CurrentVector;

            DrawLine(startPoint, endPoint, 0.2f, color,rc);

            foreach (var child in bone.Childs)
            {
               DrawBone(endPoint, child, color, rc);
            }
        }

        void DrawLine(Vector2 startPoint, Vector2 endPoint, float width, Color4 color, RenderContext rc)
        {
            Vector2 direction = endPoint - startPoint;

            Vector2 vWidth = direction.PerpendicularRight.NormalizedTo(width);

            Vector2 vLength = direction.NormalizedTo(width);

            Primitives primitives = new Primitives();
            primitives.SetAsPolygon(startPoint - vLength + vWidth,
                startPoint - vLength - vWidth,
                endPoint + vLength - vWidth,
                endPoint + vLength + vWidth);
            primitives.Draw(Texture.White, rc, color);
        }
    }
}
