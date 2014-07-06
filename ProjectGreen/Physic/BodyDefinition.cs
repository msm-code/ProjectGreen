using System;
using OpenTK;
namespace ProjectGreen
{
    enum BodyType
    {
        Static = Box2CS.BodyType.Static,
        Dynamic = Box2CS.BodyType.Dynamic
    }

    class BodyDefinition
    {
        Box2CS.BodyDef box2dDef;

        public BodyDefinition()
        {
            box2dDef = new Box2CS.BodyDef();
        }

        public BodyType Type
        {
            get { return (BodyType)box2dDef.BodyType; }
            set { box2dDef.BodyType = (Box2CS.BodyType)value; }
        }

        public Vector2 Position
        {
            get { return box2dDef.Position.ToGl(); }
            set { box2dDef.Position = value.ToBox2(); }
        }

        public bool FixedRotation
        {
            get { return box2dDef.FixedRotation; }
            set { box2dDef.FixedRotation = value; }
        }

        public Box2CS.BodyDef Box2dDef
        { get { return box2dDef; } }
    }
}
