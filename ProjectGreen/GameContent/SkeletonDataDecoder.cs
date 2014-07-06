using System.IO;
using System.Xml.Linq;
using ProjectGreen.Skeletal;
using System.Globalization;
using System.Linq;
using ProjectGreen.GameContent.Model;
using System.Collections.Generic;

namespace ProjectGreen.GameContent
{
    class SkeletonDataDecoder : IContentDecoder
    {
        public object Decode(StreamReader document)
        {
            XDocument doc = XDocument.Load(document);
            XElement root = doc.Element("skeleton").Element("joint");

            JointModel joint = new JointModel(root);

            return ConvertFromModel(joint);
        }

        Bone ConvertFromModel(JointModel model)
        {
            return new Bone(model.Id, model.Length, model.Childs.Select((x) => ConvertFromModel(x)).ToArray());
        }
    }

    namespace Model
    {
        class JointModel
        {
            XElement element;

            public JointModel(XElement element)
            { this.element = element; }

            public string Id
            { get { return element.Attribute("name").Value; } }

            public float Length
            { get { return float.Parse(element.Attribute("length").Value, CultureInfo.InvariantCulture); } }

            public IEnumerable<JointModel> Childs
            { get { return element.Elements("joint").Select((x) => new JointModel(x)); } }
        }
    }
}
