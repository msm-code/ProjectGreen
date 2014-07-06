using System.Collections.Generic;

namespace ProjectGreen.Skeletal
{
    class SkeletalAnimationData
    {
        public List<Keyframe> Keyframes { get; private set; }
        public string Name { get; private set; }

        public SkeletalAnimationData(List<Keyframe> keyframes, string name)
        {
            this.Keyframes = keyframes;
            this.Name = name;
        }
    }
}
