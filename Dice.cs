using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    internal class Dice
    {
        // A single random object is shared for better performance and randomness.
        private static readonly Random random = new Random();
        public int Value { get; private set; } // Stores the result of the last roll (cite: 78)

        public int Roll() // (cite: 79)
        {
            // Generates a random integer between 1 and 6 (Next(min, max) is exclusive of max)
            Value = random.Next(1, 7);
            return Value;
        }
    }
}
