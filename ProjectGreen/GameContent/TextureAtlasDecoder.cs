using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ProjectGreen
{
    class TextureAtlasDecoder : IContentDecoder
    {
        public object Decode(StreamReader stream)
        {
            var xmlData = XDocument.Load(stream);
            var root = xmlData.Element("atlas");

            var textureId = root.Attribute("texture").Value;
            var texture = Content.Load<Texture>(textureId);
            var atlas = new TextureAtlas(texture);

            foreach (var entry in root.Elements("sprite"))
            {
                var id = entry.Attribute("id").Value;
                var start = VectorExt.Parse(entry.Attribute("start").Value);
                var size = VectorExt.Parse(entry.Attribute("size").Value);
                var bounds = BoxExt.FromStartAndSize(start, size);

                atlas.RegisterSprite(id, bounds);
            }

            /*  REMOVED
             * foreach (var entry in root.Elements("animation"))
            {
                var id = entry.Attribute("id").Value;
                var frameTime = (double)entry.Attribute("frame-time");
                var frames = entry.Attribute("frames").Value.Split(',');

                var frameList = new List<Sprite>();
                foreach (var frame in frames)
                {
                    frameList.Add(atlas.GetSprite(frame.Trim()));
                }
                var animation = new SpriteAnimation(frameList, frameTime);

                atlas.RegisterAnimation(id, animation);
            }*/

            return atlas;
        }
    }
}
