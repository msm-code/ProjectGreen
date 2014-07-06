using System.Collections.Generic;
using System.Linq;
using OpenTK;
using System;

namespace ProjectGreen
{
    class World
    {
        public const float Epsilon = 0.00001f;
        const float TimeStep = 1.0f / 60.0f;
        float elsaped;
        readonly Box2CS.World world;

        public World(Vector2 gravity)
        {
            this.world = new Box2CS.World(gravity.ToBox2(), true);
            this.world.ContactListener = new ContactListener();
        }

        public void Update(double delta)
        {
            elsaped += (float)delta;
            while (elsaped >= TimeStep)
            {
                world.Step(TimeStep, 6, 2);
                elsaped -= TimeStep;
            }
        }

        public Body AddBody(BodyDefinition def)
        {
            var body = world.CreateBody(def.Box2dDef);
            return new Body(body);
        }

        public void CreateJoint(RevoluteJointDefinition jointDef)
        {
            world.CreateJoint(jointDef.RevoluteJointDef);
        }

        public IEnumerable<Shape> QueryAABBs(Box2 box)
        {
            var shapes = new List<Shape>();
            Box2CS.World.QueryCallbackDelegate callback = (fixture) =>
            {
                shapes.Add(new Shape(fixture));
                return true;
            };

            world.QueryAABB(callback, box.ToBox2());

            return shapes;
        }

        public List<Shape> Shapes
        {
            get { return world.Bodies.SelectMany((x) => x.Fixtures).Select((x) => new Shape(x)).ToList(); }
        }

        public List<Body> Bodies
        {
            get { return world.Bodies.Select((x) => new Body(x)).ToList(); }
        }
    }
}
