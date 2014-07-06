using OpenTK;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ProjectGreen.Skeletal
{
    class AnimationFinishedEventArgs : EventArgs
    {
        public string FinishedAnimationName { get; private set; }

        public AnimationFinishedEventArgs(string finishedId)
        {
            this.FinishedAnimationName = finishedId;
        }
    }

    class SkeletalAnimation
    {
        Queue<SkeletalAnimationData> animationQueue;
        float elsaped;
        int currentNdx;

        public SkeletalAnimation(SkeletalAnimationData animation)
        {
            this.animationQueue = new Queue<SkeletalAnimationData>();
            this.animationQueue.Enqueue(animation);

            this.TransformMatrix = Matrix2.Identity;
        }

        public void Update(float delta)
        {
            elsaped += delta;

            while (elsaped > CurrentKeyframe.FrameTime)
            {
                elsaped -= CurrentKeyframe.FrameTime;
                currentNdx++;

                if (currentNdx == CurrentAnimation.Keyframes.Count)
                    // move to next animation, if this animation is finished
                {
                    string finished = CurrentAnimation.Name;

                    this.currentNdx = 0;
                    if (animationQueue.Count > 1)
                    { animationQueue.Dequeue(); }

                    OnAnimationFinished(finished);
                }
            }
        }

        public void ChangeTo(SkeletalAnimationData newAnimation, float fadeTime)
        {
            var dump = DumpCurrent(fadeTime);
            var keyframes = new List<Keyframe>();

            keyframes.Add(dump);

            this.elsaped = 0;
            this.currentNdx = 0;
            animationQueue.Clear();
            animationQueue.Enqueue(new SkeletalAnimationData(keyframes, "#fadeout#"));
            animationQueue.Enqueue(newAnimation);
        }

        public void EnqueueAnimation(SkeletalAnimationData animation)
        {
            animationQueue.Enqueue(animation);
        }

        public BoneState Get(string id)
        {
            BoneState currentState = CurrentKeyframe.Get(id);
            BoneState nextState = NextKeyframe.Get(id);

            return BoneState.Interpolate(currentState, nextState, CurrentFactor);
        }

        public Vector2 Transform(Vector2 bone)
        {
            return TransformMatrix * bone;
        }

        public Matrix2 TransformMatrix
        { get; set; }

        public string Name
        { get { return CurrentAnimation.Name; } }

        public float CurrentTranslation
        { get { return CurrentKeyframe.Translation * (1 - CurrentFactor) + NextKeyframe.Translation * CurrentFactor; } }

        Keyframe DumpCurrent(float desiredFadeTime)
        {
            var keys = CurrentKeyframe.Keys.Union(NextKeyframe.Keys);
            var states = new Dictionary<string, BoneState>();

            foreach (var key in keys)
            {
                states.Add(key, Get(key));
            }

            return new Keyframe(desiredFadeTime, CurrentTranslation, states);
        }

        SkeletalAnimationData CurrentAnimation { get { return animationQueue.Peek(); } }
        SkeletalAnimationData NextAnimation
        {
            get
            {
                return animationQueue.Count > 1 ?
                    animationQueue.ElementAt(1) :
                    animationQueue.Peek();
            }
        }

        Keyframe CurrentKeyframe { get { return CurrentAnimation.Keyframes[currentNdx]; } }
        Keyframe NextKeyframe
        {
            get
            {
                return currentNdx + 1 < CurrentAnimation.Keyframes.Count ?
                    CurrentAnimation.Keyframes[currentNdx + 1] :
                    NextAnimation.Keyframes[0];
            }
        }
        float CurrentFactor { get { return elsaped / CurrentKeyframe.FrameTime; } }

        void OnAnimationFinished(string id)
        {
            if (AnimationFinished != null)
            { AnimationFinished(this, new AnimationFinishedEventArgs(id)); }
        }

        public event EventHandler<AnimationFinishedEventArgs> AnimationFinished;
    }
}
