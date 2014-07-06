using OpenTK;

namespace ProjectGreen
{
    interface IVisual
    {
        void Render(Box2 bounds, RenderContext rc);
    }
}
