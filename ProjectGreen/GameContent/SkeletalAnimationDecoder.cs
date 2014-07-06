using System.Xml.Linq;
using System.Linq;
using ProjectGreen.Model;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using ProjectGreen.Skeletal;

namespace ProjectGreen
{
    class SkeletalAnimationDecoder : IContentDecoder
    {
        public object Decode(StreamReader content)
        {
            XDocument document = XDocument.Load(content);
            XElement root = document.Element("animation");

            AnimationModel model = new AnimationModel(root);

            List<Keyframe> keyframes = new List<Keyframe>();

            foreach (var keyframe in model.Keyframes)
            {
                var states = new Dictionary<string, BoneState>();

                foreach (var bone in keyframe.Joints)
                {
                    states.Add(bone.Id, new BoneState(bone.RelativeAngle));
                }

                keyframes.Add(new Keyframe(keyframe.FrameTime, keyframe.Translation, states));
            }

            return new SkeletalAnimationData(keyframes, model.Name);
        }
    }

    namespace Model
    {
        class AnimationModel
        {
            XElement element;

            public AnimationModel(XElement element)
            { this.element = element; }

            public string Name
            { get { return element.Attribute("name").Value; } }

            public IEnumerable<KeyframeModel> Keyframes
            { get { return element.Elements("keyframe").Select((x) => new KeyframeModel(x)); } }
        }

        class KeyframeModel
        {
            XElement element;

            public KeyframeModel(XElement element)
            { this.element = element; }

            public float FrameTime
            { get { return float.Parse(element.Attribute("time").Value, CultureInfo.InvariantCulture); } }

            public float Translation
            { get { return float.Parse(element.Attribute("translation").Value, CultureInfo.InvariantCulture); } }

            public IEnumerable<JointStateModel> Joints
            { get { return element.Elements("joint").Select((x) => new JointStateModel(x)); } }
        }

        class JointStateModel
        {
            XElement element;

            public JointStateModel(XElement element)
            { this.element = element; }

            public string Id
            { get { return element.Attribute("id").Value; } }

            public float RelativeAngle
            { get { return float.Parse(element.Attribute("rel-angle").Value, CultureInfo.InvariantCulture); } }
        }
    }
}
