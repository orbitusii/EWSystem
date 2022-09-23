using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSystem
{
    public static class EWMath
    {
        public static float InverseSquare (Vector64 from, Vector64 to, float Power)
        {
            double sqrMag = (to - from).SquareMagnitude;
            return Power / (float)sqrMag;
        }
    }
}
