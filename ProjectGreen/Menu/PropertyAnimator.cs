using System;
using System.Collections.Generic;
namespace ProjectGreen.Menu
{
    class AnimatedProperty
    {
        readonly object target;
        readonly double maxTime;
        readonly Action<object, double> animation;
        double elsaped;

        public AnimatedProperty(object target, double maxTime, Action<object, double> animation)
        {
            this.target = target;
            this.maxTime = maxTime;
            this.animation = animation;
        }

        public void Update(double delta)
        {
            elsaped += delta;
            animation(target, TimeFactor);
        }

        double TimeFactor
        { get { return Math.Min(elsaped / maxTime, 1); } }

        public bool IsDone
        { get { return elsaped > maxTime; } }
    }

    class PropertyAnimator
    {
        List<AnimatedProperty> animated;

        public PropertyAnimator()
        {
            animated = new List<AnimatedProperty>();
        }

        public void Update(double delta)
        {
            foreach (var animation in animated)
            { animation.Update(delta); }

            animated.Sweep((x) => !x.IsDone);
        }

        public void SetAnimation(object target, double maxTime, Action<object, double> animation)
        {
            animated.Add(new AnimatedProperty(target, maxTime, animation));
        }
    }
}
