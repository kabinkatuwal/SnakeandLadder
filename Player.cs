using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class Player
    {
        public string Name { get; set; } 
        public int Position { get; set; }
        public string TokenColor { get; set; } 
        public bool IsAI { get; protected set; } 

        public Player(string name, string color)
        {
            Name = name;
            TokenColor = color;
            Position = 1;
            IsAI = false;
        }

        public int RollDice()
        {
            return new Dice().Roll();
        }
    }
}
