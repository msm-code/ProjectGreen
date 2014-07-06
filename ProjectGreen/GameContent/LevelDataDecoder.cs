using System.IO;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using Box2CS;
using ProjectGreen.LevelModel;
using ProjectGreen.Algorithms;

namespace ProjectGreen
{
    class LevelDataDecoder : IContentDecoder
    {
        public object Decode(StreamReader stream)
        {
            var document = XDocument.Load(stream);
            var data = new LevelData();

            LevelDataModel model = new LevelDataModel(document);

            data.NextLevel = model.NextLevel;
            data.World = new World(model.Gravity);

            AddPlayer(data, model.Player);
            AddExit(data, model.Exit);
            foreach (var body in model.Bodies) { AddBody(data, body); }
            foreach (var sprite in model.Sprites) { AddSprite(data, sprite); }

            return data;
        }

        void AddPlayer(LevelData level, PlayerData data)
        {
            var size = new Vector2(1.9f, 3.5f);

            var bodyDef = new BodyDefinition
            {
                Type = BodyType.Dynamic,
                Position = data.Position,
                FixedRotation = true
            };

            var body = level.World.AddBody(bodyDef);

            var polygon = PolygonData.Create(
                  new Vector2(-size.X / 2, -(size.Y - size.X) / 2),
                  new Vector2(0, -size.Y / 2),
                  new Vector2(size.X / 2, -(size.Y - size.X) / 2),
                  new Vector2(size.X / 2, size.Y / 2),
                  new Vector2(-size.X / 2, size.Y / 2));

            var shapeDef = new ShapeDefinition
            {
                Polygon = polygon,
                Density = 1,
                Restitution = 0.01f,
                Friction = 0.1f,
            };

            Shape shape = body.AddShape(shapeDef);
            shape.UserData.CastsShadow = false;

            level.PlayerBody = body;
        }

        void AddExit(LevelData level, ExitData data)
        {
            var bodyDef = new BodyDefinition
            {
                Type = BodyType.Static,
                Position = data.Bounds.Center()
            };

            var body = level.World.AddBody(bodyDef);

            var shapeDef = new ShapeDefinition
            {
                Polygon = PolygonData.CreateBox(data.Bounds.Width / 2, data.Bounds.Height / 2),
                IsSensor = true
            };

            Shape shape = body.AddShape(shapeDef);
            shape.UserData.CastsShadow = false;
            level.ExitBody = body;
        }

        void AddBody(LevelData level, BodyData data)
        {
            var absolutePoints = data.Verticles.Select((x) => x + data.Center);
            var absoluteTriangles = PolygonTriangulator.Instance.Triangulate(absolutePoints.ToArray());
            var triangles = absoluteTriangles.Select((t) => t.Select((x) => x - data.Center));

            foreach (var triangle in triangles)
            {
                var bodyDef = new BodyDefinition
                {
                    Type = BodyType.Static,
                    Position = data.Center
                };

                var shape = new ShapeDefinition
                {
                    Polygon = PolygonData.Create(triangle.ToArray()),
                };

                var body = level.World.AddBody(bodyDef);
                body.AddShape(shape);

                // level.Bodies.Add(new BodyVisualiser(body, null));
            }
        }

        void AddSprite(LevelData level, SpriteData data)
        {
            var texture = Content.Load<Texture>(data.Texture);
            var image = new Sprite(texture);

            Sprite s = new Sprite(Content.Load<Texture>("level1.a"));
            level.Images.Add(new BumpedSpriteDisplay(data.Bounds, image, s));
        }
    }

    namespace LevelModel
    {
        class LevelDataModel : BaseData
        {
            public LevelDataModel(XDocument document)
                : base(document.Element("map"))
            {
            }

            public PlayerData Player
            { get { return new PlayerData(Data.Element("player")); } }

            public ExitData Exit
            { get { return new ExitData(Data.Element("exit")); } }

            public IEnumerable<BodyData> Bodies
            { get { return Data.Elements("body").Select((x) => new BodyData(x)); } }

            public IEnumerable<SpriteData> Sprites
            { get { return Data.Elements("sprite").Select((x) => new SpriteData(x)); } }

            public string NextLevel
            { get { return ReadString("next-level"); } }

            public Vector2 Gravity
            { get { return ReadVector("gravity"); } }
        }

        class BaseData
        {
            protected XElement Data { get; private set; }
            public BaseData(XElement data) { this.Data = data; }

            protected string ReadString(string attribName)
            { return Data.Attribute(attribName).Value; }

            protected Vector2 ReadVector(string attribName)
            {
                return VectorExt.Parse(ReadString(attribName));
            }

            protected IEnumerable<Vector2> ReadVectorList(string attribName)
            {
                string[] vectors = ReadString(attribName).Split(',');
                return vectors.Select((x) => VectorExt.Parse(x));
            }

            protected Box2 ReadBox(string centerAttrib, string sizeAttrib)
            {
                return BoxExt.FromCenterAndSize(ReadVector(centerAttrib), ReadVector(sizeAttrib));
            }
        }

        class PlayerData : BaseData
        {
            public PlayerData(XElement element) : base(element) { }

            public Vector2 Position
            { get { return ReadVector("position"); } }
        }

        class ExitData : BaseData
        {
            public ExitData(XElement element) : base(element) { }

            public Box2 Bounds
            { get { return ReadBox("position", "size"); } }
        }

        class BodyData : BaseData
        {
            public BodyData(XElement element) : base(element) { }

            public Vector2 Center
            { get { return ReadVector("center"); } }

            public IEnumerable<Vector2> Verticles
            { get { return ReadVectorList("verticles"); } }
        }

        class SpriteData : BaseData
        {
            public SpriteData(XElement element) : base(element) { }

            public Box2 Bounds
            { get { return ReadBox("position", "size"); } }

            public string Texture
            { get { return ReadString("texture"); } }
        }
    }
}
