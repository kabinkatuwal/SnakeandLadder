using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SnakeandLadder.GameEvents;

namespace SnakeandLadder
{
    internal class Game
    {
        private readonly Board board = new Board(); // (cite: 63)
        private readonly List<Player> players; // (cite: 64)
        private int currentPlayerIndex = 0; // (cite: 65)
        private bool isRunning = false; // (cite: 66)

        // Events the UI will subscribe to (cite: 56, 57)
        public event PlayerMoveHandler OnPlayerMove;
        public event GameNotificationHandler OnGameWin;
        public event GameNotificationHandler OnGameMessage;

        public Game(List<Player> initialPlayers)
        {
            players = initialPlayers;
        }

        public void StartGame() // (cite: 67)
        {
            isRunning = true;
            OnGameMessage?.Invoke("Game started! First turn: " + players[currentPlayerIndex].Name);
        }

        public void NextTurn() // (cite: 68)
        {
            if (!isRunning) return;

            Player currentPlayer = players[currentPlayerIndex];

            // 1. Roll the dice
            int steps = currentPlayer.RollDice();
            OnGameMessage?.Invoke($"{currentPlayer.Name} rolled a {steps}.");

            int oldPosition = currentPlayer.Position;
            int newPosition = oldPosition + steps;

            // 2. Apply the exact 100 landing rule (cite: 27)
            if (newPosition > 100)
            {
                newPosition = oldPosition;
            }

            // 3. Update the player's position (basic move)
            currentPlayer.Position = newPosition;

            // 4. Check for a snake or ladder landing
            int finalPosition = board.GetNewPosition(currentPlayer.Position);
            bool hitSpecial = finalPosition != newPosition;

            // 5. Notify the UI about the move (DELEGATE/EVENT)
            OnPlayerMove?.Invoke(currentPlayer, steps, oldPosition, finalPosition, hitSpecial);

            // 6. Finalize player position after snake/ladder check
            currentPlayer.Position = finalPosition;

            // 7. Check for a win (cite: 69)
            if (CheckWin(currentPlayer))
            {
                isRunning = false;
                OnGameWin?.Invoke($"🎉 CONGRATULATIONS! {currentPlayer.Name} wins the game!");
                return;
            }

            // 8. Advance turn
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            // If the next player is AI, trigger their turn automatically
            if (players[currentPlayerIndex].IsAI)
            {
                // Note: In a real WinForms app, you'd use a Timer here to allow the UI to update
                // before the AI moves, but we'll call NextTurn() directly for now.
                NextTurn();
            }
        }

        public bool CheckWin(Player player) // (cite: 69)
        {
            return player.Position == 100;
        }
    }
}
