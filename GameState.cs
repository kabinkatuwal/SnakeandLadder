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

       
        public List<PlayerState> Players { get; set; }

        public List<Snake> Snakes { get; set; }
        public List<Ladder> Ladders { get; set; }
    }
}
