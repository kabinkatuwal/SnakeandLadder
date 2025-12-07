using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    internal class AIPlayer
    {
        public int DifficultyLevel { get; set; } // Specific property for AI (cite: 84)

        public AIPlayer(string name, string color, int difficulty = 1) : base(name, color)
        {
            IsAI = true; // Set the AI flag
            DifficultyLevel = difficulty;
        }

        // The Game class will call this method during the AI's turn (cite: 85)
        public void TakeAITurn()
        {
            // In the full game, this will trigger the move sequence.
        }
    }
}
