using System;

namespace ProjectGreen
{
    class BodyContactEventArgs : EventArgs
    {
        public BodyContactEventArgs(Shape myShape, Shape obstacle)
        {
            this.MyShape = myShape;
            this.Obstacle = obstacle;
        }

        public Shape MyShape { get; private set; }
        public Shape Obstacle { get; private set; }
    }
}
