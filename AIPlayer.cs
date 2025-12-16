using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class AIPlayer : Player
    {
        // Property for future expansions (e.g., smarter dice rolls)
        public int DifficultyLevel { get; set; }

        // Constructor matches the 'new AIPlayer(name, color)' call in Form1
        public AIPlayer(string name, string color, int difficulty = 1) : base(name, color)
        {
            this.IsAI = true; // Essential for Form1 to detect this as an AI
            this.DifficultyLevel = difficulty;
        }

        // This remains for future use if you want to add custom AI logic 
        // beyond just an automated dice roll.
        public void TakeAITurn()
        {
            // The automated roll is currently handled by the aiTimer in Form1.cs
        }
    }
}