using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class AIPlayer : Player
    {
        public int DifficultyLevel { get; set; }
        public AIPlayer(string name, string color, int difficulty = 1) : base(name, color)
        {
            this.IsAI = true;
            this.DifficultyLevel = difficulty;
        }
        public void TakeAITurn()
        {
          
        }
    }
}