using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Linq;
using ProjectGreen.Skeletal;
using ProjectGreen.GameContent;

namespace ProjectGreen
{
    static class Content
    {
        static Dictionary<Type, IContentDecoder> decoders;
        static DirectoryInfo contentRoot;

        static Content()
        {
            decoders = new Dictionary<Type, IContentDecoder>();
            contentRoot = new DirectoryInfo("Content/");

            InitDecoders();
        }

        public static T Load<T>(string id)
        {            
            return (T)LoadContent(id, typeof(T));
        }

        public static void AddDecoder(Type type, IContentDecoder decoder)
        {
            decoders.Add(type, decoder);
        }

        static object LoadContent(string id, Type type)
        {
            using (var stream = OpenResource(id))
            {
                var decoder = decoders[type];

                return decoder.Decode(stream);
            }
        }

        static StreamReader OpenResource(string id)
        {
            FileInfo file = contentRoot.GetFiles(id + ".*").SingleOrDefault();
            if (file == null) { return null; }
            return file.OpenText();
        }

        static void InitDecoders()
        {
            decoders.Add(typeof(Texture), new TextureDecoder());
            decoders.Add(typeof(LevelData), new LevelDataDecoder());
            decoders.Add(typeof(TextureAtlas), new TextureAtlasDecoder());
            decoders.Add(typeof(Settings), new SettingsDecoder());
            decoders.Add(typeof(Bone), new SkeletonDataDecoder());
            decoders.Add(typeof(SkeletalAnimationData), new SkeletalAnimationDecoder());
        }
    }
}
