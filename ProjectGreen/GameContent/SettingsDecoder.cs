using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ProjectGreen
{
    class SettingsDecoder : IContentDecoder
    {
        public object Decode(StreamReader stream)
        {
            var doc = XDocument.Load(stream);
            var root = doc.Element("data-dict");
            var data = new Dictionary<string, string>();

            foreach (var elem in root.Elements("item"))
            {
                var key = elem.Attribute("key").Value;
                var value = elem.Attribute("value").Value;

                data.Add(key, value);
            }

            return new Settings(data);
        }
    }
}
