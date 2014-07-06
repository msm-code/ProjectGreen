using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectGreen
{
    static class ListExt
    {
        public static void Sweep<T>(this List<T> bodies, Func<T, bool> isValid)
        {
            if (bodies.Count == 0) { return; }

            int lastAlive = bodies.Count - 1;
            while (lastAlive >= 0 && !isValid(bodies[lastAlive])) { lastAlive--; }

            for (int i = 0; i <= lastAlive; i++)
            {
                if (!isValid(bodies[i]))
                {
                    bodies[i] = bodies[lastAlive--];
                }
            }
            bodies.RemoveRange(lastAlive + 1, bodies.Count - (lastAlive + 1));
        }
    }
}
