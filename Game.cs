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

        public Player CurrentPlayer => players[currentPlayerIndex];
        public List<Player> Players => players;
        public Board Board => board;
        public bool IsRunning => isRunning;

        public Game(List<Player> initialPlayers)
        {
            players = initialPlayers;
            board = new Board();
        }

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
            foreach (var p in players)
            {
                p.Position = 1;
            }
        }

        public void NextTurn(int? forcedRoll = null)
        {
            if (!isRunning) return;

            Player currentPlayer = CurrentPlayer;
            int steps = forcedRoll ?? currentPlayer.RollDice();

            int oldPosition = currentPlayer.Position;
            int newPosition = oldPosition + steps;

            if (newPosition > 100)
            {
                newPosition = oldPosition;
            }

            currentPlayer.Position = newPosition;
            int finalPosition = board.GetNewPosition(currentPlayer.Position);
            bool hitSpecial = finalPosition != newPosition;
            currentPlayer.Position = finalPosition;

            if (CheckWin(currentPlayer))
            {
                isRunning = false;
                OnPlayerMove?.Invoke(currentPlayer, steps, oldPosition, finalPosition, hitSpecial);
                OnGameWin?.Invoke($"{currentPlayer.Name} wins the game!");
                return;
            }

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            OnPlayerMove?.Invoke(currentPlayer, steps, oldPosition, finalPosition, hitSpecial);
        }

        public bool CheckWin(Player player)
        {
            return player.Position == 100;
        }

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
        }

        public static Game LoadGame(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
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

            var loadedBoard = new Board();
            return new Game(loadedPlayers, loadedBoard, state.CurrentPlayerIndex, state.IsRunning);
        }
    }
}