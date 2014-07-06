using OpenTK;
namespace ProjectGreen.Game
{
    class ContactCountingSensor
    {
        int contactCount;

        public void AttachTo(Body body, Vector2 offset, Vector2 size)
        {
            ShapeDefinition sensorDef = new ShapeDefinition
            {
                Polygon = PolygonData.CreateBox(size.X / 2, size.Y / 2, offset),
                IsSensor = true
            };

            Shape shape = body.AddShape(sensorDef);
            shape.UserData.CastsShadow = false;

            shape.BeginContact += (sender, e) => { contactCount++; };
            shape.EndContact += (sender, e) => { contactCount--; };
        }

        public int ContactCount
        { get { return contactCount; } }

        public bool AnyContact
        { get { return contactCount > 0; } }
    }
}
