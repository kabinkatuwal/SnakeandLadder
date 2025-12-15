// Game.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SnakeandLadder;
using static SnakeandLadder.GameEvents;

namespace SnakeandLadder
{
    public class Game
    {
        private Board board;
        private readonly List<Player> players;
        private int currentPlayerIndex = 0;
        private bool isRunning = false;

        public event PlayerMoveHandler OnPlayerMove;
        public event GameNotificationHandler OnGameWin;
        public event GameNotificationHandler OnGameMessage;

        // Public properties for UI access
        public Player CurrentPlayer => players[currentPlayerIndex];
        public List<Player> Players => players;
        public Board Board => board;
        public bool IsRunning => isRunning;

        public Game(List<Player> initialPlayers)
        {
            players = initialPlayers;
            board = new Board(); // Generates random board on creation
        }

        // Constructor used for loading saved games
        private Game(List<Player> loadedPlayers, Board loadedBoard, int currentIndex, bool running)
        {
            players = loadedPlayers;
            board = loadedBoard;
            currentPlayerIndex = currentIndex;
            isRunning = running;
        }

        public void StartGame()
        {
            isRunning = true;
            // Reset player positions for a new game
            foreach (var p in players) p.Position = 1;

            OnGameMessage?.Invoke("Game started! First turn: " + CurrentPlayer.Name);
        }

        public void NextTurn(int? forcedRoll = null)
        {
            if (!isRunning) return;

            Player currentPlayer = CurrentPlayer;

            int steps = forcedRoll ?? currentPlayer.RollDice(); // Use forced roll or dice roll

            int oldPosition = currentPlayer.Position;
            int newPosition = oldPosition + steps;

            if (newPosition > 100)
            {
                newPosition = oldPosition;
                OnGameMessage?.Invoke($"{currentPlayer.Name} rolled a {steps}. Needs exact roll.");
            }
            else
            {
                OnGameMessage?.Invoke($"{currentPlayer.Name} rolled a {steps}.");
            }

            // 1. Update the player's position (basic move)
            currentPlayer.Position = newPosition;

            // 2. Check for a snake or ladder landing
            int finalPosition = board.GetNewPosition(currentPlayer.Position);
            bool hitSpecial = finalPosition != newPosition;

            // 3. Notify the UI about the move (DELEGATE/EVENT)
            OnPlayerMove?.Invoke(currentPlayer, steps, oldPosition, finalPosition, hitSpecial);

            // 4. Finalize player position after snake/ladder check
            currentPlayer.Position = finalPosition;

            // 5. Check for win
            if (CheckWin(currentPlayer))
            {
                isRunning = false;
                OnGameWin?.Invoke($"🎉 CONGRATULATIONS! {currentPlayer.Name} wins the game!");
                return;
            }

            // 6. Advance turn
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }

        public bool CheckWin(Player player)
        {
            return player.Position == 100;
        }

        // --- SAVE/LOAD ---

        public void SaveGame(string path)
        {
            var state = new GameState
            {
                CurrentPlayerIndex = this.currentPlayerIndex,
                IsRunning = this.isRunning,

                Players = this.players.Select(p => new PlayerState
                {
                    Name = p.Name,
                    Position = p.Position,
                    TokenColor = p.TokenColor,
                    IsAI = p.IsAI
                }).ToList(),

                Snakes = this.board.Snakes.ToList(),
                Ladders = this.board.Ladders.ToList()
            };

            string jsonString = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, jsonString);
            OnGameMessage?.Invoke($"Game saved successfully to {path}");
        }

        public static Game LoadGame(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Save file not found at {path}");
            }

            string jsonString = File.ReadAllText(path);
            var state = JsonSerializer.Deserialize<GameState>(jsonString);

            var loadedPlayers = state.Players.Select(ps =>
                ps.IsAI ? (Player)new AIPlayer(ps.Name, ps.TokenColor) : new Player(ps.Name, ps.TokenColor)
            ).ToList();

            for (int i = 0; i < loadedPlayers.Count; i++)
            {
                loadedPlayers[i].Position = state.Players[i].Position;
            }

            var loadedBoard = new Board(state.Snakes, state.Ladders);

            var loadedGame = new Game(loadedPlayers, loadedBoard, state.CurrentPlayerIndex, state.IsRunning);

            return loadedGame;
        }
    }
}
