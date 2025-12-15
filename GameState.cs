using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class GameState
    {
        public int CurrentPlayerIndex { get; set; }
        public bool IsRunning { get; set; }

        // Capture Player state (Need to save all players)
        public List<PlayerState> Players { get; set; }

        // Capture Board state (Need to know where the Snakes/Ladders are)
        public List<Snake> Snakes { get; set; }
        public List<Ladder> Ladders { get; set; }
    }
}
