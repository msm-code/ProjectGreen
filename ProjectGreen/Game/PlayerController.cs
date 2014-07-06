using OpenTK;
using OpenTK.Input;
using System.Linq;
using Box2CS;
using ProjectGreen.Skeletal;
using ProjectGreen.Game;
using System;

namespace ProjectGreen
{
    enum Direction
    {
        Left = -1,
        None = 0,
        Right = 1
    }

    class PlayerController
    {
        bool leftPressed;
        bool rightPressed;
        ContactCountingSensor feetSensor;

        public PlayerController(Body body, Skeleton skeleton)
        {
            this.Body = body;
            this.Skeleton = skeleton;

            Box2 bounds = body.Shapes.First().ComputeWorldBounds();
            this.feetSensor = new ContactCountingSensor();
            this.feetSensor.AttachTo(body,
                new Vector2(0, bounds.Height / 2),
                new Vector2(bounds.Width - 0.001f, 0.001f));

            this.State = Standing;
            this.Skeleton.Animation = new SkeletalAnimation(PlayerAnimation.Standing);
        }

        public Body Body { get; private set; }
        public Skeleton Skeleton { get; private set; }
        public IPlayerState State { get; private set; }
        public SkeletalAnimation Animation { get { return Skeleton.Animation; } }
        public Direction CurrentDirection
        {
            get
            {
                if (leftPressed == rightPressed) { return Direction.None; }
                if (leftPressed) { return Direction.Left; }
                return Direction.Right;
            }
        }

        public void ChangeState(IPlayerState newState, params SkeletalAnimationData[] newAnimation)
        {
            if (newAnimation.Length == 0) { throw new InvalidOperationException(); }

            this.State = newState;
            this.Animation.ChangeTo(newAnimation[0], 0.2f);

            for (int i = 1; i < newAnimation.Length; i++)
            {
                this.Animation.EnqueueAnimation(newAnimation[i]);
            }
        }

        public void Update(double delta)
        {
            State.Update(this, delta);
            Skeleton.Update(delta);
        }

        public void KeyDown(Key key) 
        {
            Direction oldDirection = CurrentDirection;

            if (key == Key.Up) { State.Jump(this); }
            else if (key == Key.Left) { leftPressed = true; }
            else if (key == Key.Right) { rightPressed = true; }

            Direction current = CurrentDirection;
            if (current != oldDirection)
            { State.ChangeDirection(this); }
        }

        public void KeyUp(Key key) 
        {
            Direction oldDirection = CurrentDirection;

            if (key == Key.Left) { leftPressed = false; }
            if (key == Key.Right) { rightPressed = false; }

            Direction current = CurrentDirection;
            if (current != oldDirection)
            { State.ChangeDirection(this); }
        }

        public bool IsStandingOnGround
        {
            get { return feetSensor.AnyContact; }
        }

        public static readonly StandingState Standing = new StandingState();
        public static readonly WalkingState Walking = new WalkingState();
        public static readonly JumpingState Jumping = new JumpingState();
    }

    interface IPlayerState
    {
        void Update(PlayerController context, double delta);
        void Jump(PlayerController context);
        void ChangeDirection(PlayerController context);
    }

    class StandingState : IPlayerState
    {
        public void Update(PlayerController context, double delta) 
        {
            float desiredSpeed = 0;

            float speedDiff = desiredSpeed - context.Body.LinearVelocity.X;

            Vector2 force = new Vector2(speedDiff * (float)delta * WalkingState.Acceleration, 0);
            context.Body.ApplyForce(force, context.Body.WorldCenter);

            if (!context.IsStandingOnGround)
            {
                context.ChangeState(PlayerController.Jumping, PlayerAnimation.Falling);
            }
        }

        public void Jump(PlayerController context)
        {
            if (context.IsStandingOnGround)
            {
                context.Body.ApplyLinearImpulse(new Vector2(0, JumpingState.JumpForce), context.Body.WorldCenter);
                context.ChangeState(PlayerController.Jumping, PlayerAnimation.Jumping);
            }
        }

        public void ChangeDirection(PlayerController context)
        {
            context.Animation.TransformMatrix = Matrix2.Scale((int)context.CurrentDirection, 1); 
            context.ChangeState(PlayerController.Walking, PlayerAnimation.Walking);
        }
    }

    class WalkingState : IPlayerState
    {
        public const float WalkSpeed = 25f;
        public const float Acceleration = 1000;

        public void Update(PlayerController context, double delta)
        {
            float desiredSpeed = (int)context.CurrentDirection * WalkSpeed;

            float speedDiff = desiredSpeed - context.Body.LinearVelocity.X;

            Vector2 force = new Vector2(speedDiff * (float)delta * Acceleration, 0);
            context.Body.ApplyForce(force, context.Body.WorldCenter);

            if (!context.IsStandingOnGround)
            {
                context.ChangeState(PlayerController.Jumping, PlayerAnimation.Falling);
            }
        }

        public void Jump(PlayerController context)
        {
            if (context.IsStandingOnGround)
            {
                context.Body.ApplyLinearImpulse(new Vector2(0, JumpingState.JumpForce), context.Body.WorldCenter);
                context.ChangeState(PlayerController.Jumping, PlayerAnimation.Jumping);
            }
        }

        public void ChangeDirection(PlayerController context)
        { context.ChangeState(PlayerController.Standing, PlayerAnimation.Braking, PlayerAnimation.Standing); }
    }

    class JumpingState : IPlayerState
    {
        public const float JumpForce = -135f;

        public void Update(PlayerController context, double delta)
        {
            float desiredSpeed = (int)context.CurrentDirection * WalkingState.WalkSpeed;

            float speedDiff = desiredSpeed - context.Body.LinearVelocity.X;

            Vector2 force = new Vector2(speedDiff * (float)delta * WalkingState.Acceleration, 0);
            context.Body.ApplyForce(force, context.Body.WorldCenter);

            if (context.IsStandingOnGround && context.Body.LinearVelocity.Y <= 0.01)
            {
                if (context.CurrentDirection == Direction.None)
                { context.ChangeState(PlayerController.Standing, PlayerAnimation.Braking, PlayerAnimation.Standing); }
                else
                { context.ChangeState(PlayerController.Walking, PlayerAnimation.Walking); }
            }
        }

        public void ChangeDirection(PlayerController context)
        {
            if ((int)context.CurrentDirection != 0)
            { context.Animation.TransformMatrix = Matrix2.Scale((int)context.CurrentDirection, 1); }
        }
        public void Jump(PlayerController context) { }
    }

    static class PlayerAnimation
    {
        public static readonly SkeletalAnimationData Standing = Content.Load<SkeletalAnimationData>("skeleton.standing");
        public static readonly SkeletalAnimationData Jumping = Content.Load<SkeletalAnimationData>("skeleton.jumping");
        public static readonly SkeletalAnimationData Falling = Content.Load<SkeletalAnimationData>("skeleton.falling");
        public static readonly SkeletalAnimationData Walking = Content.Load<SkeletalAnimationData>("skeleton.walking");
        public static readonly SkeletalAnimationData Braking = Content.Load<SkeletalAnimationData>("skeleton.braking");
    }
}
