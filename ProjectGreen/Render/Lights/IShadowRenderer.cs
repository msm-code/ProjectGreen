using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGreen.Render.Lights
{
    interface IShadowRenderer
    {
        void Render(PointLight light, List<Shape> shadowCasters, RenderContext rc);
    }
}
