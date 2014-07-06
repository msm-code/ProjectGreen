using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectGreen
{
    class Shape
    {
        readonly Box2CS.Body body;
        readonly Box2CS.Fixture fixture;
        readonly Box2CS.PolygonShape shape;
        readonly ShapeUserData userData;

        public Shape(Box2CS.Fixture box2dFixture)
        {
            this.fixture = box2dFixture;
            this.body = fixture.Body;
            this.shape = (Box2CS.PolygonShape)box2dFixture.Shape;

            this.userData = (ShapeUserData)fixture.UserData;
            if (this.userData == null)
            { fixture.UserData = this.userData = new ShapeUserData(); }
        }

        public void Destroy()
        {
            this.fixture.Body.DestroyFixture(this.fixture);
        }

        public Body Body
        { get { return new Body(fixture.Body); } }

        public IEnumerable<Vector2> WorldVerticles
        {
            get{ return shape.Vertices.Select((x) => body.GetWorldPoint(x).ToGl()).ToList();}
        }

        public Vector2 WorldCenteroid
        {
            get { return body.GetWorldPoint(shape.Centroid).ToGl(); }
        }

        public IEnumerable<Vector2> Normals
        { get { return shape.Normals.Select((x) => x.ToGl()); } }

        public Box2 ComputeWorldBounds()
        {
            Box2CS.AABB aabb;
            shape.ComputeAABB(out aabb, Box2CS.Transform.Identity);
            aabb.LowerBound = body.GetWorldPoint(aabb.LowerBound);
            aabb.UpperBound = body.GetWorldPoint(aabb.UpperBound);
            return aabb.ToGl();
        }

        public event EventHandler<BodyContactEventArgs> BeginContact
        {
            add { userData.BeginContact += value; }
            remove { userData.BeginContact -= value; }
        }

        public event EventHandler<BodyContactEventArgs> EndContact
        {
            add { userData.EndContact += value; }
            remove { userData.EndContact -= value; }
        }

        public ShapeUserData UserData { get { return userData; } }
    }
}
