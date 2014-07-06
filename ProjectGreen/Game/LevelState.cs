using OpenTK.Input;
using OpenTK;
using System;
using Box2CS;
using ProjectGreen.Render.Shaders;
using ProjectGreen.Shaders.Programs;
using ProjectGreen.Game;
using ProjectGreen.Render.Lights;
using OpenTK.Graphics;
using ProjectGreen.Game.Render;
using System.Linq;
using System.Collections.Generic;
using ProjectGreen.Render;
using ProjectGreen.Skeletal;

namespace ProjectGreen
{
    class LevelState : IGameState
    {
        readonly Campaign campaign;
        readonly string levelId;

        World world;
        EnvironmentRenderer envRender;
        PlayerController controller;
        PointLight playerLight;

        public LevelState(Campaign campaign, string levelId)
        {
            this.levelId = levelId;
            this.campaign = campaign;
        }

        public void Load()
        {
            var data = Content.Load<LevelData>(levelId);

            this.world = data.World;
            var atlas = Content.Load<TextureAtlas>("global");

            this.envRender = new EnvironmentRenderer(world);
            envRender.AddRange(data.Images.Cast<IEnvironmentObject>());

            this.playerLight = new PointLight(data.PlayerBody.Position, Color4.White, QuadraticCurve.Quadratic(0f, 0.1f, 0.3f));
            this.envRender.AddLight(playerLight);

            this.envRender.AddLight(new PointLight(new Vector2(25, 10), Color4.White, QuadraticCurve.Quadratic(0.01f, 0.05f, 0.7f)));
            data.ExitBody.Shapes.First().BeginContact 
                += (sender, e) => { if (e.Obstacle.Body == data.PlayerBody) { LevelFinished(); } };

            Bone root = Content.Load<Bone>("skeleton.skeleton");
            Skeleton playerSkeleton = new Skeleton(root);

            envRender.Add(new PlayerDisplay(data.PlayerBody, playerSkeleton));

            this.controller = new PlayerController(data.PlayerBody, playerSkeleton);
        }

        public void Render(RenderContext rc)
        {
            const int HorisontalSize = 30;
            float factor = Screen.Instance.Height / (float)Screen.Instance.Width;
            var size = new Vector2(HorisontalSize, HorisontalSize * factor);
            var center = controller.Body.Position;
            var camera = new Camera(center, size);
            rc.ApplyCamera(camera);

            envRender.Render(rc);
        }

        public void Update(double delta)
        {
            controller.Update(delta);
            playerLight.Position = controller.Body.Position;
            world.Update(delta);
        }

        public void KeyDown(Key key)
        {
            controller.KeyDown(key);
        }

        public void KeyUp(Key key)
        {
            controller.KeyUp(key);
        }

        void LevelFinished()
        {
            if (StateChanged != null)
            { StateChanged(this, new StateChangedEventArgs(campaign.MoveToNextLevel())); }
        }

        public event EventHandler<StateChangedEventArgs> StateChanged;
    }
}
