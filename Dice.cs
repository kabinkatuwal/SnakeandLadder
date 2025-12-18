using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class Dice
    {
        private static readonly Random random = new Random();
        public int Value { get; private set; }

        public int Roll()
        {
           
            Value = random.Next(1, 7);
            return Value;
        }
    }
}
