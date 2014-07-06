using OpenTK.Input;
using System;
namespace ProjectGreen
{
    interface IGameState
    {
        void Load();
        void Render(RenderContext rc);
        void Update(double delta);
        void KeyDown(Key key);
        void KeyUp(Key key);
        event EventHandler<StateChangedEventArgs> StateChanged;
    }
}
