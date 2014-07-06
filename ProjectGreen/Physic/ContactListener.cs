namespace ProjectGreen
{
    class ContactListener : Box2CS.ContactListener
    {
        public override void BeginContact(Box2CS.Contact contact)
        {
            var shape1 = new Shape(contact.FixtureA);
            var shape2 = new Shape(contact.FixtureB);

            shape1.UserData.RaiseBeginContact(shape1, shape2);
            shape2.UserData.RaiseBeginContact(shape2, shape1);
        }

        public override void EndContact(Box2CS.Contact contact)
        {
            var shape1 = new Shape(contact.FixtureA);
            var shape2 = new Shape(contact.FixtureB);

            shape1.UserData.RaiseEndContact(shape1, shape2);
            shape2.UserData.RaiseEndContact(shape2, shape1);
        }

        public override void PostSolve(Box2CS.Contact contact, Box2CS.ContactImpulse impulse) { }

        public override void PreSolve(Box2CS.Contact contact, Box2CS.Manifold oldManifold) { }
    }
}
