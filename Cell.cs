using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class Cell
    {
        public int Index { get; set; } // The cell number (1 to 100) (cite: 92)
        public bool HasSnake { get; set; } // Flag if the cell is a snake head (cite: 92)
        public bool HasLadder { get; set; }
    }
}
