using OpenTK;
namespace ProjectGreen
{
    class RevoluteJointDefinition
    {
        Box2CS.RevoluteJointDef b2dDef;

        public RevoluteJointDefinition()
        {
            this.b2dDef = new Box2CS.RevoluteJointDef();
        }

        public Body BodyA
        {
            get { return new Body(b2dDef.BodyA); }
            set { b2dDef.BodyA = value.BodyData; }
        }

        public Body BodyB
        {
            get { return new Body(b2dDef.BodyB); }
            set { b2dDef.BodyB = value.BodyData; }
        }

        public bool CollideConnected
        {
            get { return b2dDef.CollideConnected; }
            set { b2dDef.CollideConnected = value; }
        }

        public Vector2 LocalAnchorA
        {
            get { return b2dDef.LocalAnchorA.ToGl(); }
            set { b2dDef.LocalAnchorA = value.ToBox2(); }
        }

        public Vector2 LocalAnchorB
        {
            get { return b2dDef.LocalAnchorB.ToGl(); }
            set { b2dDef.LocalAnchorB = value.ToBox2(); }
        }

        public Box2CS.RevoluteJointDef RevoluteJointDef
        { get { return b2dDef; } }
    }
}
