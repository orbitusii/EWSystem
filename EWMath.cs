using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSystem
{
    public static class EWMath
    {
        public static float InverseSquare (Vector64 pos1, Vector64 pos2, float Power)
        {
            double sqrMag = (pos2 - pos1).SquareMagnitude;
            return Power / (float)sqrMag;
        }
    }
}
