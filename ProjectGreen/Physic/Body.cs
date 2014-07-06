using System.Collections.Generic;
using System.Linq;
using OpenTK;
using System;

namespace ProjectGreen
{
    class Body : IEquatable<Body>
    {
        readonly Box2CS.Body body;
        readonly BodyUserData userData;

        public Body(Box2CS.Body box2dBody)
        {
            this.body = box2dBody;

            this.userData = (BodyUserData)body.UserData;
            if (userData == null)
            { body.UserData = this.userData = new BodyUserData(); }
        }

        #region Wrappers
        public void ApplyForce(Vector2 force, Vector2 center)
        {
            body.ApplyForce(force.ToBox2(), center.ToBox2());
        }

        public void ApplyLinearImpulse(Vector2 force, Vector2 point)
        {
            body.ApplyLinearImpulse(force.ToBox2(), point.ToBox2());
        }

        public Vector2 LinearVelocity
        { get { return body.LinearVelocity.ToGl(); } }

        public Vector2 Position
        { get { return body.Position.ToGl(); } }

        public Vector2 WorldCenter
        { get { return body.Position.ToGl(); } }

        public IEnumerable<Shape> Shapes
        { get { return body.Fixtures.Select((x) => new Shape(x)); } }
        #endregion

        public Shape AddShape(ShapeDefinition shape)
        {
            Box2CS.Fixture fixture = body.CreateFixture(shape.Fixture);
            return new Shape(fixture);
        }

        public BodyUserData UserData { get { return userData; } }

        internal Box2CS.Body BodyData
        { get { return body; } }

        #region Check Equality
        public bool Equals(Body other)
        { return this.body == other.body; }

        public override bool Equals(object obj)
        { return obj is Body && ((Body)obj).body == body; }

        public static bool operator ==(Body b1, Body b2)
        {
            if (Object.ReferenceEquals(b1, b2))
            { return true; }

            if ((object)b1 == null || (object)b2 == null)
            { return false; }

            return b1.body == b2.body;
        }

        public static bool operator !=(Body b1, Body b2)
        { return !(b1 == b2); }

        public override int GetHashCode()
        { return body.GetHashCode(); }
        #endregion
    }
}
