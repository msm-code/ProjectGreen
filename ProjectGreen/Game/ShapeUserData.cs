using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGreen
{
    class ShapeUserData
    {
        public bool CastsShadow { get; set; }

        public ShapeUserData()
        {
            this.CastsShadow = true;
        }

        public void RaiseEndContact(Shape myShape, Shape obstacle)
        {
            if (EndContact != null)
            { EndContact(myShape.Body, new BodyContactEventArgs(myShape, obstacle)); }
        }

        public void RaiseBeginContact(Shape myShape, Shape obstacle)
        {
            if (BeginContact != null)
            { BeginContact(myShape.Body, new BodyContactEventArgs(myShape, obstacle)); }
        }

        public event EventHandler<BodyContactEventArgs> BeginContact;

        public event EventHandler<BodyContactEventArgs> EndContact;
    }
}
