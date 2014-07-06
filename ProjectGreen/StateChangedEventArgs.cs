using System;

namespace ProjectGreen
{
    class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(IGameState newState)
        {
            this.NewState = newState;
        }

        public IGameState NewState { get; private set; }
    }
}
