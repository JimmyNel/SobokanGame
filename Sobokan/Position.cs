using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sobokan
{
    // création d'une classe Position permettant de déterminer la position de chaque objet
    public class Position
    {
        public int x;
        public int y;

        // construction pour initialiser la position
        public Position(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
