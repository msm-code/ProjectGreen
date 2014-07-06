namespace ProjectGreen
{
    class ShapeDefinition
    {
        readonly Box2CS.FixtureDef fixtureDef;

        public ShapeDefinition()
        {
            fixtureDef = new Box2CS.FixtureDef();
        }

        public float Density
        {
            get { return fixtureDef.Density; }
            set { fixtureDef.Density = value; }
        }

        public float Friction
        {
            get { return fixtureDef.Friction; }
            set { fixtureDef.Friction = value; }
        }

        public float Restitution
        {
            get { return fixtureDef.Restitution; }
            set { fixtureDef.Restitution = value; }
        }

        public bool IsSensor
        {
            get { return fixtureDef.IsSensor; }
            set { fixtureDef.IsSensor = value; }
        }

        public PolygonData Polygon
        {
            get { return new PolygonData(fixtureDef.Shape); }
            set { fixtureDef.Shape = value.Shape; }
        }

        internal Box2CS.FixtureDef Fixture
        { get { return fixtureDef; } }
    }
}
