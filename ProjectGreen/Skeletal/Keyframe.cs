using System.Collections.Generic;

namespace ProjectGreen.Skeletal
{
    class Keyframe
    {
        Dictionary<string, BoneState> states;

        public Keyframe(float frameTime, float translation, Dictionary<string, BoneState> states)
        {
            this.FrameTime = frameTime;
            this.Translation = translation;
            this.states = states;
        }

        public BoneState Get(string id)
        {
            return states[id];
        }

        public IEnumerable<string> Keys { get { return states.Keys; } }

        public float FrameTime { get; private set; }

        public float Translation { get; private set; }
    }
}
