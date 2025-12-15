using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    public class GameEvents
    {
        

        // Delegate for simple messages (e.g., win/loss)
        public delegate void GameNotificationHandler(string message);

        // Delegate to notify the UI exactly how a player moved
        public delegate void PlayerMoveHandler(Player player, int steps, int fromPos, int toPos, bool hitSpecial);
    }
}
