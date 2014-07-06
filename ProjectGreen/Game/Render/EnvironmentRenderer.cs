using System.Collections.Generic;
using ProjectGreen.Render;
using OpenTK;
using OpenTK.Graphics;
using ProjectGreen.Render.Lights;
using ProjectGreen.Render.Shaders;

namespace ProjectGreen.Game.Render
{
    class EnvironmentRenderer
    {
        List<IEnvironmentObject> environment;
        DynamicLightBlender blender;
        World world;
        RenderTarget colorMap;
        RenderTarget normalMap;
        ShaderProgram texturedDraw;

        public EnvironmentRenderer(World world)
        {
            this.environment = new List<IEnvironmentObject>();
            this.world = world;
            this.blender = new DynamicLightBlender();
            this.texturedDraw = ShaderRepository.Get(ShaderRepository.TexturedDraw);
        }

        public void Add(IEnvironmentObject obj)
        {
            environment.Add(obj);
        }

        public void AddRange(IEnumerable<IEnvironmentObject> objs)
        {
            environment.AddRange(objs);
        }

        public void AddLight(PointLight light)
        {
            this.blender.Add(light);
        }

        public void Render(RenderContext rc)
        {
            RenderTarget.EnsureCreated(ref normalMap);
            RenderTarget.EnsureCreated(ref colorMap);

            texturedDraw.Use(rc);
            colorMap.Use(rc);
            rc.Clear(1, 1, 1, 1);
            environment.ForEach((x) => x.DrawColor(rc));
            colorMap.Unuse(rc);

            normalMap.Use(rc);
            rc.Clear(0.5f, 0.5f, 1, 1);
            environment.ForEach((x) => x.DrawNormals(rc));
            normalMap.Unuse(rc);

            if (Settings.UserData.GetBool("display", "dynamic-shadows"))
            { blender.Render(colorMap.Texture, normalMap.Texture, world, rc); }
            else
            { rc.DrawSprite(new Sprite(colorMap.Texture), rc.Camera.VisibleRegion); }
        }
    }
}
