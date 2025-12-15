using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class Player
    {
        public string Name { get; set; } // Player name (cite: 72)
        public int Position { get; set; } // Current cell number (1-100) (cite: 72)
        public string TokenColor { get; set; } // Color for the token on the board (cite: 73)
        public bool IsAI { get; protected set; } // Flag for AI control (cite: 74)

        // Constructor to initialize a player
        public Player(string name, string color)
        {
            Name = name;
            TokenColor = color;
            Position = 1; // All players start at cell 1
            IsAI = false;
        }

        // Method to roll the dice (cite: 75)
        public int RollDice()
        {
            // Creates a new Dice object just to perform the roll
            return new Dice().Roll();
        }
    }
}
