using OpenTK;
using System;
using OpenTK.Input;
using ProjectGreen.Render;
using ProjectGreen.Shaders.Programs;

namespace ProjectGreen
{
    class MainWindow : GameWindow
    {
        IGameState state;
        double totalTime;

        public MainWindow()
        {
            this.InitSize();
        }

        void InitSize()
        {
            bool fullscreened = Settings.UserData.GetBool("display", "fullscreened");
            if (fullscreened)
            {
                base.Width = DisplayDevice.Default.Width;
                base.Width = DisplayDevice.Default.Height;
                base.WindowState = WindowState.Fullscreen;
            }
            else
            {
                base.Width = 640;
                base.Height = 480;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Keyboard.KeyDown += (sender, ev) => KeyDown(ev.Key);
            Keyboard.KeyUp += (sender, ev) => KeyUp(ev.Key);

            Screen.Instance.BindToWindow(this);
            ShaderRepository.Add(ShaderRepository.TexturedDraw, new TexturedProgram());
            this.InitState();

            base.OnLoad(e);
        }

        void KeyUp(Key key)
        {
            state.KeyUp(key);
        }

        void KeyDown(Key key)
        {
            if (key == Key.Escape)
            { this.SetState(ExitState.Instance); }

            state.KeyDown(key);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            totalTime += e.Time;
            state.Update(e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var context = CreateRenderContext();
            context.BeginScene();

            state.Render(context);

            context.EndScene();
            base.SwapBuffers();

            this.Title = this.RenderTime.ToString();
        }

        void SetState(IGameState newState)
        {
            if (newState == ExitState.Instance)
            { this.Exit(); }

            if (state != null) { state.StateChanged -= SetStateHandler; }
            newState.StateChanged += SetStateHandler;

            newState.Load();
            GC.Collect();

            this.state = newState;
        }

        void SetStateHandler(object sender, StateChangedEventArgs e)
        {
            SetState(e.NewState);
        }

        RenderContext CreateRenderContext()
        {
            var context = new RenderContext();
            context.ApplyCamera(new Camera(Vector2.Zero, new Vector2(2, 2)));
            context.SetViewport(0, 0, this.Width, this.Height);
            context.TotalTime = totalTime;
            return context;
        }

        void InitState()
        {
            this.SetState(MenuState.CreateMainMenu());
        }
    }
}
