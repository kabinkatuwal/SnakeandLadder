using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class GameEvents
    {
       
        public delegate void GameNotificationHandler(string message);

        public delegate void PlayerMoveHandler(Player player, int steps, int fromPos, int toPos, bool hitSpecial);
    }
}
