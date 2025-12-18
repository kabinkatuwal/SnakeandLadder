using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class Cell
    {
        public int Index { get; set; }
        public bool HasSnake { get; set; }
        public bool HasLadder { get; set; }
    }
}
