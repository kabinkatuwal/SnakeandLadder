using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic; // Required for Interaction.InputBox

namespace SnakeandLadder
{
    public partial class Form1 : Form
    {
        private Game game;
        private const int GRID_SIZE = 10;
        private Image snakeImg;
        private Image ladderImg;
        private Dictionary<string, Image> playerTokens = new Dictionary<string, Image>();

        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake Slide Project - AI Logic Final";

            // Enable Double Buffering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer, true);

            LoadVisualAssets();
            SetupNewGame();

            // Wire up UI Events
            rollButton.Click += rollButton_Click;
            btnSaveGame.Click += btnSaveGame_Click;
            btnLoadGame.Click += btnLoadGame_Click;
            btnNewGame.Click += (sender, e) => SetupNewGame();
            boardPanel.Paint += boardPanel_Paint;
            boardPanel.Resize += (s, e) => boardPanel.Invalidate();
        }

        private void LoadVisualAssets()
        {
            try
            {
                string resPath = Path.Combine(Application.StartupPath, "Resources");
                snakeImg = Image.FromFile(Path.Combine(resPath, "snake.png"));
                ladderImg = Image.FromFile(Path.Combine(resPath, "ladder.png"));

                string[] colors = { "Red", "Blue", "Green" };
                foreach (var color in colors)
                {
                    string path = Path.Combine(resPath, $"{color}Token.png");
                    if (File.Exists(path)) playerTokens[color] = Image.FromFile(path);
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Status: Visual assets missing.";
            }
        }

        private void SetupNewGame()
        {
            if (game != null) UnsubscribeFromGameEvents();

            // 1. Get Human Player Count
            string countInput = Interaction.InputBox("How many humans? (1 or 2)", "Setup", "1");
            if (!int.TryParse(countInput, out int humanCount) || humanCount < 1) humanCount = 1;

            // 2. Ask for AI
            DialogResult addAI = MessageBox.Show("Add an AI Bot?", "Add AI?", MessageBoxButtons.YesNo);

            List<Player> initialPlayers = new List<Player>();
            string[] colors = { "Red", "Blue", "Green" };

            for (int i = 0; i < humanCount; i++)
            {
                string name = Interaction.InputBox($"Enter name for Player {i + 1}:", "Names", $"Player {i + 1}");
                initialPlayers.Add(new Player(string.IsNullOrWhiteSpace(name) ? $"Player {i + 1}" : name, colors[i]));
            }

            if (addAI == DialogResult.Yes)
            {
                // Ensure this is using the AIPlayer class
                initialPlayers.Add(new AIPlayer("AI Bot", colors[initialPlayers.Count]));
            }

            game = new Game(initialPlayers);
            SubscribeToGameEvents();
            game.StartGame();

            // FORCE UI UPDATE
            UpdateTurnUI();
            boardPanel.Invalidate();
        }

        private void SubscribeToGameEvents()
        {
            game.OnGameMessage += (message) => lblStatus.Text = message;
            game.OnGameWin += (message) =>
            {
                lblStatus.Text = message;
                rollButton.Enabled = false;
                aiTimer.Stop();
                MessageBox.Show(message, "Winner!");
            };

            game.OnPlayerMove += HandlePlayerMove;

            // Connect AI timer
            aiTimer.Tick -= aiTimer_Tick;
            aiTimer.Tick += aiTimer_Tick;
        }

        private void UnsubscribeFromGameEvents()
        {
            if (game != null)
            {
                game.OnPlayerMove -= HandlePlayerMove;
                aiTimer.Tick -= aiTimer_Tick;
                aiTimer.Stop();
            }
        }

        private void HandlePlayerMove(Player player, int steps, int fromPos, int toPos, bool hitSpecial)
        {
            UpdateDiceVisual(steps);
            boardPanel.Invalidate();

            // Advance UI and check for AI turn
            UpdateTurnUI();
        }

        private void UpdateTurnUI()
        {
            if (game == null || !game.IsRunning) return;

            // Update Label Text
            lblPlayerInfo.Text = $"Current: {game.CurrentPlayer.Name}";
            rollButton.Text = $"Roll ({game.CurrentPlayer.Name})";

            // If the current player is an AI, start the timer
            if (game.CurrentPlayer.IsAI)
            {
                rollButton.Enabled = false;
                aiTimer.Interval = 500; // Snappy 0.5s delay
                aiTimer.Start();
            }
            else
            {
                rollButton.Enabled = true;
                aiTimer.Stop();
            }
        }

        private void aiTimer_Tick(object sender, EventArgs e)
        {
            aiTimer.Stop();

            if (game != null && game.IsRunning && game.CurrentPlayer.IsAI)
            {
                game.NextTurn(); // Computer rolls
            }
        }

        private void UpdateDiceVisual(int value)
        {
            try
            {
                string resPath = Path.Combine(Application.StartupPath, "Resources");
                string dicePath = Path.Combine(resPath, $"die{value}.png");

                if (File.Exists(dicePath))
                {
                    using (FileStream fs = new FileStream(dicePath, FileMode.Open, FileAccess.Read))
                    {
                        if (pbDice.Image != null) pbDice.Image.Dispose();
                        pbDice.Image = Image.FromStream(fs);
                    }
                }
            }
            catch (Exception) { }
        }

        private void rollButton_Click(object sender, EventArgs e)
        {
            if (game != null && game.IsRunning && !game.CurrentPlayer.IsAI)
            {
                game.NextTurn();
            }
        }

        private Point GetCellCenterCoordinates(int cellIndex, int cellSize)
        {
            int row = (cellIndex - 1) / GRID_SIZE;
            int col = (cellIndex - 1) % GRID_SIZE;
            if (row % 2 != 0) col = (GRID_SIZE - 1) - col;
            int x = col * cellSize + (cellSize / 2);
            int y = (GRID_SIZE - 1 - row) * cellSize + (cellSize / 2);
            return new Point(x, y);
        }

        private void boardPanel_Paint(object sender, PaintEventArgs e)
        {
            if (game == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int smallestDim = Math.Min(boardPanel.Width, boardPanel.Height);
            int cellSize = smallestDim / GRID_SIZE;

            for (int i = 1; i <= 100; i++)
            {
                Point center = GetCellCenterCoordinates(i, cellSize);
                Rectangle rect = new Rectangle(center.X - cellSize / 2, center.Y - cellSize / 2, cellSize, cellSize);
                g.FillRectangle((i % 2 == 0) ? Brushes.WhiteSmoke : Brushes.White, rect);
                g.DrawRectangle(Pens.Gray, rect);
                g.DrawString(i.ToString(), Font, Brushes.Black, rect.X + 2, rect.Y + 2);
            }

            foreach (var ladder in game.Board.Ladders)
            {
                Point start = GetCellCenterCoordinates(ladder.Bottom, cellSize);
                Point end = GetCellCenterCoordinates(ladder.Top, cellSize);
                DrawCorrectedImage(g, start, end, ladderImg, (int)(cellSize * 1.2));
            }

            foreach (var snake in game.Board.Snakes)
            {
                Point head = GetCellCenterCoordinates(snake.Head, cellSize);
                Point tail = GetCellCenterCoordinates(snake.Tail, cellSize);
                DrawCorrectedImage(g, head, tail, snakeImg, (int)(cellSize * 1.5));
            }

            int playerOffset = 0;
            foreach (var player in game.Players)
            {
                Point pos = GetCellCenterCoordinates(player.Position, cellSize);
                int tSize = (int)(cellSize * 0.65);
                RectangleF tokenRect = new RectangleF(pos.X - tSize / 2 + playerOffset, pos.Y - tSize / 2 + playerOffset, tSize, tSize);

                if (playerTokens.ContainsKey(player.TokenColor))
                    g.DrawImage(playerTokens[player.TokenColor], tokenRect);
                else
                    g.FillEllipse(new SolidBrush(Color.FromName(player.TokenColor)), tokenRect);

                playerOffset += 4;
            }
        }

        private void DrawCorrectedImage(Graphics g, Point start, Point end, Image img, int thickness)
        {
            if (img == null) { g.DrawLine(new Pen(Color.Gray, 3), start, end); return; }
            float dx = end.X - start.X, dy = end.Y - start.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            float angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
            GraphicsState state = g.Save();
            g.TranslateTransform(start.X, start.Y);
            g.RotateTransform(angle);
            g.DrawImage(img, new RectangleF(0, -thickness / 2, distance, thickness));
            g.Restore(state);
        }

        private void btnSaveGame_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "JSON|*.json" };
            if (sfd.ShowDialog() == DialogResult.OK) game.SaveGame(sfd.FileName);
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "JSON|*.json" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                UnsubscribeFromGameEvents();
                game = Game.LoadGame(ofd.FileName);
                SubscribeToGameEvents();
                UpdateTurnUI();
                boardPanel.Invalidate();
            }
        }
    }
}