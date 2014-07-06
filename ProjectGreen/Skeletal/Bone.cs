using OpenTK;
using System.Collections.Generic;

namespace ProjectGreen.Skeletal
{
    class Bone
    {
        public string Id { get; private set; }
        public float Length { get; set; }
        public Vector2 CurrentVector { get; private set; }

        public Bone(string id, float length, params Bone[] childs)
        {
            this.Id = id;
            this.Length = length;
            this.Childs = new List<Bone>(childs);
        }

        public SkeletalAnimation AnimationOverride { get; set; }
        public List<Bone> Childs { get; private set; }

        public void Add(Bone child)
        {
            this.Childs.Add(child);
        }

        public void Animate(SkeletalAnimation baseAnimation, float baseRotation)
        {
            SkeletalAnimation animation = baseAnimation ?? AnimationOverride;

            Vector2 untransformedBone = this.CurrentVector;
            float rotation = untransformedBone.Rotation();

            if (animation != null)
            {
                BoneState myState = animation.Get(this.Id);
                rotation = baseRotation + myState.RelativeAngle;

                untransformedBone = VectorExt.FromRotationAndLength(rotation, Length);
                this.CurrentVector = animation.Transform(untransformedBone);
            }

            foreach (var bone in Childs)
            {
                bone.Animate(animation, rotation);
            }
        }
    }
}
