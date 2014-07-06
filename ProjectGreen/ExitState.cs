namespace ProjectGreen
{
    class ExitState : IGameState
    {
        private ExitState() { }
        static readonly ExitState instance = new ExitState();
        public static ExitState Instance { get { return instance; } }

        #region
        public void Load() { }
        public void Render(RenderContext rc) { }
        public void Update(double delta) { }
        public void KeyDown(OpenTK.Input.Key key) { }
        public void KeyUp(OpenTK.Input.Key key) { }
        public event System.EventHandler<StateChangedEventArgs> StateChanged { add { } remove { } }
        #endregion
    }
}
