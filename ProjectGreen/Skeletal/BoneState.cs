using System;

namespace ProjectGreen.Skeletal
{
    class BoneState
    {
        public float RelativeAngle { get; set; }

        public BoneState(float relativeAngle)
        {
            this.RelativeAngle = relativeAngle;
        }

        public static BoneState Interpolate(BoneState stateA, BoneState stateB, float factor)
        {
            const float DoublePi = 2 * (float)Math.PI;

            float angleDiff;
            float angleDiffUp = stateB.RelativeAngle - stateA.RelativeAngle;
            float angleDiffDown = angleDiffUp - (DoublePi * Math.Sign(angleDiffUp));

            if (Math.Abs(angleDiffUp) < Math.Abs(angleDiffDown))
            { angleDiff = angleDiffUp; }
            else
            { angleDiff = angleDiffDown; }

            float relAngle = (stateA.RelativeAngle + angleDiff * factor) % DoublePi;

            return new BoneState(relAngle);
        }
    }
}
