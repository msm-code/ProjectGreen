using OpenTK.Input;
using System;
using System.Collections.Generic;
using OpenTK;
using ProjectGreen.Render;
using ProjectGreen.Menu;
using ProjectGreen.Game;
using ProjectGreen.Render.Shaders;
using ProjectGreen.Shaders.Programs;

namespace ProjectGreen
{
    class MenuState : IGameState
    {
        List<MenuItem> items;
        TextureAtlas atlas;
        PropertyAnimator animator;
        ShaderProgram shader;
        int currentNdx;

        public MenuState(List<MenuItem> items, TextureAtlas atlas)
        {
            this.items = items;
            this.atlas = atlas;
            this.animator = new PropertyAnimator();
            this.shader = ShaderRepository.Get(ShaderRepository.TexturedDraw);

            for(int i = 0; i < items.Count; i++)
            {
                var normalSprite = atlas[items[i].TextureId];
                var selectedSprite = atlas[items[i].SelectedTextureId];
                items[i].CurrentVisual = new SpriteBlend(normalSprite, selectedSprite, i == currentNdx ? 0 : 1);
            }
        }

        public void Load() { }

        public void Render(RenderContext rc)
        {
            rc.Clear(1, 1, 1, 1);

            shader.Use(rc);

            PositionButtons(rc.Camera.VisibleRegion);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].CurrentVisual.Render(items[i].Bounds, rc);
            }
        }

        public void Update(double delta) 
        {
            animator.Update(delta);
        }

        public void KeyDown(Key key)
        {
            if (key != Key.Down && key != Key.Up && key != Key.Enter) { return; }

            animator.SetAnimation(items[currentNdx].CurrentVisual, 0.3, (x, d) => ((SpriteBlend)x).Factor = (float)d);

            if (key == Key.Down)
            { currentNdx = (currentNdx + 1) % items.Count; }
            if (key == Key.Up)
            { currentNdx = (currentNdx - 1 + items.Count) % items.Count; }
            if (key == Key.Enter)
            { items[currentNdx].Action(this); }

            animator.SetAnimation(items[currentNdx].CurrentVisual, 0.3, (x, d) => ((SpriteBlend)x).Factor = 1 - (float)d);
        }

        public void KeyUp(Key key) { }

        public event EventHandler<StateChangedEventArgs> StateChanged;

        void PositionButtons(Box2 space)
        {
            int buttonsCount = items.Count;
            int marginsCount = buttonsCount + 1;
            int units = buttonsCount * 2 + marginsCount;
            float unit = space.Height / units;
            float halfWidth = unit * 6;

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Bounds = BoxExt.FromCenterAndSize(
                    new Vector2(0, (2 * i) * unit + space.Top / 2),
                    new Vector2(halfWidth, unit * 2));
            }
        }

        void SetState(IGameState newState)
        {
            if (this.StateChanged != null)
            {
                StateChanged(this, new StateChangedEventArgs(newState));
            }
        }

        #region Menus
        public static MenuState CreateMainMenu()
        {
            TextureAtlas atlas = Content.Load<TextureAtlas>("menu-atlas");

            List<MenuItem> items = new List<MenuItem>();
            items.Add(new MenuItem("new-game", "new-game-selected", (s) => s.SetState(Campaign.Main.Begin(s))));
            items.Add(new MenuItem("continue-game", "continue-game-selected", (s) => s.SetState(Campaign.Main.Continue(s))));
            items.Add(new MenuItem("end-game", "end-game-selected", (s) => { s.SetState(ExitState.Instance); }));

            return new MenuState(items, atlas);
        }
        #endregion
    }
}
