// Form1.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SnakeandLadder;

namespace SnakeandLadder
{
    public partial class Form1 : Form
    {
        private Game game;
        private const int GRID_SIZE = 10;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake Slide Project";

            // 1. Initialize Game Setup
            SetupNewGame();

            // 2. Wire up Button Click Events
            rollButton.Click += rollButton_Click;
            btnSaveGame.Click += btnSaveGame_Click;
            btnLoadGame.Click += btnLoadGame_Click;
            btnNewGame.Click += (sender, e) => SetupNewGame();

            // 3. Wire up Paint Event
            boardPanel.Paint += boardPanel_Paint;
        }

        private void SetupNewGame()
        {
            // Unsubscribe from old game events before creating a new one
            if (game != null) UnsubscribeFromGameEvents();

            // Initialize Players
            List<Player> initialPlayers = new List<Player>
        {
            new Player("Kabin", "Red"),
            new Player("Baburam", "Blue"),
            new AIPlayer("AI Bot", "Green")
        };

            game = new Game(initialPlayers);
            SubscribeToGameEvents();
            game.StartGame();
            rollButton.Enabled = true;
            aiTimer.Stop(); // Ensure timer is off
        }

        private void SubscribeToGameEvents()
        {
            game.OnGameMessage += (message) => lblStatus.Text = message;
            game.OnGameWin += (message) =>
            {
                lblStatus.Text = message;
                rollButton.Enabled = false;
                aiTimer.Stop();
            };
            game.OnPlayerMove += HandlePlayerMove;
            aiTimer.Tick += aiTimer_Tick; // Wire up AI timer
        }

        private void UnsubscribeFromGameEvents()
        {
            game.OnGameMessage -= (message) => lblStatus.Text = message;
            game.OnGameWin -= (message) => { /* ... */ };
            game.OnPlayerMove -= HandlePlayerMove;
            aiTimer.Tick -= aiTimer_Tick;
        }

        private void HandlePlayerMove(Player player, int steps, int fromPos, int toPos, bool hitSpecial)
        {
            // Redraw board to show token movement
            boardPanel.Invalidate();

            // Update status message
            if (hitSpecial)
            {
                string messageType = toPos > fromPos ? "ladder 🪜" : "snake 🐍";
                lblStatus.Text = $"{player.Name} rolled {steps}, hit a {messageType} and moved to {toPos}!";
            }

            // Handle AI turn transition
            if (game.IsRunning)
            {
                rollButton.Text = $"Roll ({game.CurrentPlayer.Name})";

                if (game.CurrentPlayer.IsAI)
                {
                    rollButton.Enabled = false;
                    aiTimer.Start();
                }
                else
                {
                    rollButton.Enabled = true;
                }
            }
        }

        private void rollButton_Click(object sender, EventArgs e)
        {
            // Only trigger turn if the current player is human
            if (!game.CurrentPlayer.IsAI)
            {
                game.NextTurn();
            }
        }

        private void aiTimer_Tick(object sender, EventArgs e)
        {
            aiTimer.Stop();
            if (game.CurrentPlayer.IsAI && game.IsRunning)
            {
                game.NextTurn();
            }
        }

        // --- SAVE/LOAD HANDLERS ---
        private void btnSaveGame_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Snake Slide Save File (*.json)|*.json";
                sfd.FileName = "SnakeSlide_Save.json";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    game.SaveGame(sfd.FileName);
                }
            }
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Snake Slide Save File (*.json)|*.json";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        UnsubscribeFromGameEvents();
                        game = Game.LoadGame(ofd.FileName);
                        SubscribeToGameEvents();
                        boardPanel.Invalidate();
                        rollButton.Enabled = !game.CurrentPlayer.IsAI && game.IsRunning;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading game: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetupNewGame(); // Start a new game if load fails
                    }
                }
            }
        }

        // --- DRAWING LOGIC ---
        private Point GetCellCenterCoordinates(int cellIndex)
        {
            if (cellIndex < 1 || cellIndex > 100) return new Point(0, 0);

            int cellSize = boardPanel.Width / GRID_SIZE;
            int row = (cellIndex - 1) / GRID_SIZE;
            int col = (cellIndex - 1) % GRID_SIZE;

            // Reverse column order for odd rows (snake pattern)
            if (row % 2 != 0)
            {
                col = (GRID_SIZE - 1) - col;
            }

            // Y is calculated from the top of the panel, so we invert the row
            int x = col * cellSize + (cellSize / 2);
            int y = (GRID_SIZE - 1 - row) * cellSize + (cellSize / 2);

            return new Point(x, y);
        }

        private void boardPanel_Paint(object sender, PaintEventArgs e)
        {
            if (game == null || game.Board == null) return;

            Graphics g = e.Graphics;
            int cellSize = boardPanel.Width / GRID_SIZE;

            Pen borderPen = new Pen(Color.Gray, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            Font cellFont = new Font("Arial", 8, FontStyle.Bold);

            // 1. Draw Grid and Numbers
            for (int i = 1; i <= 100; i++)
            {
                Point center = GetCellCenterCoordinates(i);
                int cellX = center.X - (cellSize / 2);
                int cellY = center.Y - (cellSize / 2);
                Rectangle cellRect = new Rectangle(cellX, cellY, cellSize, cellSize);

                Color bgColor = i % 2 == 0 ? Color.FromArgb(240, 240, 240) : Color.White;
                g.FillRectangle(new SolidBrush(bgColor), cellRect);
                g.DrawRectangle(borderPen, cellRect);

                g.DrawString(i.ToString(), cellFont, textBrush, cellX + 2, cellY + 2);
            }

            // 2. Draw Snakes and Ladders
            Pen ladderPen = new Pen(Color.Green, 3) { StartCap = LineCap.Round, EndCap = LineCap.ArrowAnchor };
            Pen snakePen = new Pen(Color.Red, 5) { StartCap = LineCap.Round, EndCap = LineCap.Round };

            foreach (var ladder in game.Board.Ladders)
            {
                Point start = GetCellCenterCoordinates(ladder.Bottom);
                Point end = GetCellCenterCoordinates(ladder.Top);
                g.DrawLine(ladderPen, start, end);
            }

            foreach (var snake in game.Board.Snakes)
            {
                Point start = GetCellCenterCoordinates(snake.Head);
                Point end = GetCellCenterCoordinates(snake.Tail);
                g.DrawLine(snakePen, start, end);
            }

            // 3. Draw Player Tokens
            int tokenSize = cellSize / 4;
            int playerOffset = 0;

            // Sort players to ensure consistent drawing order
            var orderedPlayers = game.Players.OrderBy(p => p.Position).ToList();

            foreach (Player player in orderedPlayers)
            {
                Point tokenCenter = GetCellCenterCoordinates(player.Position);

                Color playerColor = Color.FromName(player.TokenColor);
                Brush playerBrush = new SolidBrush(playerColor);

                g.FillEllipse(playerBrush,
                              tokenCenter.X - (tokenSize / 2) + playerOffset,
                              tokenCenter.Y - (tokenSize / 2) + playerOffset,
                              tokenSize,
                              tokenSize);

                g.DrawEllipse(Pens.Black,
                              tokenCenter.X - (tokenSize / 2) + playerOffset,
                              tokenCenter.Y - (tokenSize / 2) + playerOffset,
                              tokenSize,
                              tokenSize);

                playerOffset += 5;
            }
        }
    }
}