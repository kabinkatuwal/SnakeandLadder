using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace SnakeandLadder
{
    public partial class Form1 : Form
    {
        private Game game;
        private const int GRID_SIZE = 10;
        private Image snakeImg;
        private Image ladderImg;
        private Dictionary<string, Image> playerTokens = new Dictionary<string, Image>();

        private int targetPosition;
        private Player animatingPlayer;
        private System.Windows.Forms.Timer moveTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake Slide";

            groupBox1.Dock = DockStyle.Right;
            groupBox1.Width = 280;
            boardPanel.Dock = DockStyle.Fill;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer, true);

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, boardPanel, new object[] { true });

            LoadVisualAssets();
            SetupNewGame();

            moveTimer.Interval = 150;
            moveTimer.Tick += moveTimer_Tick;

            rollButton.Click += rollButton_Click;
            btnSaveGame.Click += btnSaveGame_Click;
            btnLoadGame.Click += btnLoadGame_Click;
            btnNewGame.Click += (sender, e) => SetupNewGame();

            boardPanel.Paint += boardPanel_Paint;
            this.Resize += (s, e) => boardPanel.Invalidate();
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
                    if (File.Exists(path))
                        playerTokens[color] = Image.FromFile(path);
                }
            }
            catch { }
        }

        private void SetupNewGame()
        {
            if (game != null)
                UnsubscribeFromGameEvents();

            string countInput = Interaction.InputBox("How many humans? (1 or 2)", "Setup", "1");
            if (!int.TryParse(countInput, out int humanCount) || humanCount < 1)
                humanCount = 1;

            DialogResult addAI = MessageBox.Show("Add an AI Bot?", "Add AI?", MessageBoxButtons.YesNo);

            List<Player> initialPlayers = new List<Player>();
            string[] colors = { "Red", "Blue", "Green" };

            for (int i = 0; i < humanCount; i++)
            {
                string name = Interaction.InputBox($"Enter name for Player {i + 1}:", "Names", $"Player {i + 1}");
                initialPlayers.Add(new Player(string.IsNullOrWhiteSpace(name) ? $"Player {i + 1}" : name, colors[i]));
            }

            if (addAI == DialogResult.Yes)
                initialPlayers.Add(new AIPlayer("AI Bot", colors[initialPlayers.Count]));

            game = new Game(initialPlayers);
            SubscribeToGameEvents();
            game.StartGame();
            UpdateTurnUI();
            boardPanel.Invalidate();
        }

        private void SubscribeToGameEvents()
        {
            game.OnGameWin += (message) =>
            {
                rollButton.Enabled = false;
                aiTimer.Stop();
                moveTimer.Stop();
                MessageBox.Show(message, "Winner!");
            };

            game.OnPlayerMove += HandlePlayerMove;
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
                moveTimer.Stop();
            }
        }

        private void HandlePlayerMove(Player player, int steps, int fromPos, int toPos, bool hitSpecial)
        {
            UpdateDiceVisual(steps);
            animatingPlayer = player;
            animatingPlayer.Position = fromPos;
            targetPosition = toPos;
            rollButton.Enabled = false;
            moveTimer.Start();
        }

        private void moveTimer_Tick(object sender, EventArgs e)
        {
            if (targetPosition < animatingPlayer.Position)
            {
                animatingPlayer.Position = targetPosition;
                moveTimer.Stop();
                UpdateTurnUI();
            }
            else if (animatingPlayer.Position < targetPosition)
            {
                animatingPlayer.Position++;
            }
            else
            {
                moveTimer.Stop();
                UpdateTurnUI();
            }
            boardPanel.Invalidate();
            boardPanel.Update();
        }

        private void UpdateTurnUI()
        {
            if (game == null || !game.IsRunning || moveTimer.Enabled)
                return;

            lblPlayerInfo.Text = $"Current: {game.CurrentPlayer.Name}";
            rollButton.Text = $"Roll ({game.CurrentPlayer.Name})";

            if (game.CurrentPlayer.IsAI)
            {
                rollButton.Enabled = false;
                aiTimer.Interval = 2000;
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
                game.NextTurn();
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
            catch { }
        }

        private void rollButton_Click(object sender, EventArgs e)
        {
            if (game != null && game.IsRunning && !game.CurrentPlayer.IsAI && !moveTimer.Enabled)
                game.NextTurn();
        }

        private void btnSaveGame_Click(object sender, EventArgs e)
        {
            if (game == null) return;

            SaveFileDialog sfd = new SaveFileDialog { Filter = "JSON|*.json" };
            if (sfd.ShowDialog() == DialogResult.OK)
                game.SaveGame(sfd.FileName);
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "JSON|*.json" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    UnsubscribeFromGameEvents();
                    game = Game.LoadGame(ofd.FileName);
                    SubscribeToGameEvents();
                    UpdateTurnUI();
                    boardPanel.Invalidate();
                }
                catch { }
            }
        }

        private Point GetCellCenterCoordinates(int cellIndex, int cellSize)
        {
            int row = (cellIndex - 1) / GRID_SIZE;
            int col = (cellIndex - 1) % GRID_SIZE;

            if (row % 2 != 0)
                col = (GRID_SIZE - 1) - col;

            return new Point(
                col * cellSize + (cellSize / 2),
                (GRID_SIZE - 1 - row) * cellSize + (cellSize / 2)
            );
        }

        private void boardPanel_Paint(object sender, PaintEventArgs e)
        {
            if (game == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int cellSize = Math.Min(boardPanel.Width, boardPanel.Height) / GRID_SIZE;

            for (int i = 1; i <= 100; i++)
            {
                Point center = GetCellCenterCoordinates(i, cellSize);
                Rectangle rect = new Rectangle(center.X - cellSize / 2, center.Y - cellSize / 2, cellSize, cellSize);
                Brush cellBrush = (i % 2 == 0) ? Brushes.WhiteSmoke : Brushes.White;

                g.FillRectangle(cellBrush, rect);
                g.DrawRectangle(Pens.Gray, rect);
                g.DrawString(i.ToString(), Font, Brushes.Black, rect.X + 2, rect.Y + 2);
            }

            foreach (var l in game.Board.Ladders)
            {
                DrawCorrectedImage(g,
                    GetCellCenterCoordinates(l.Bottom, cellSize),
                    GetCellCenterCoordinates(l.Top, cellSize),
                    ladderImg, (int)(cellSize * 1.2));
            }

            foreach (var s in game.Board.Snakes)
            {
                DrawCorrectedImage(g,
                    GetCellCenterCoordinates(s.Tail, cellSize),
                    GetCellCenterCoordinates(s.Head, cellSize),
                    snakeImg, (int)(cellSize * 1.5));
            }

            int offset = 0;
            foreach (var p in game.Players)
            {
                Point pos = GetCellCenterCoordinates(p.Position, cellSize);
                int tSize = (int)(cellSize * 0.65);
                RectangleF tokenRect = new RectangleF(pos.X - tSize / 2 + offset, pos.Y - tSize / 2 + offset, tSize, tSize);

                if (playerTokens.ContainsKey(p.TokenColor))
                    g.DrawImage(playerTokens[p.TokenColor], tokenRect);
                else
                    g.FillEllipse(new SolidBrush(Color.FromName(p.TokenColor)), tokenRect);

                offset += 4;
            }
        }

        private void DrawCorrectedImage(Graphics g, Point start, Point end, Image img, int thickness)
        {
            if (img == null)
            {
                g.DrawLine(new Pen(Color.Gray, 3), start, end);
                return;
            }

            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            float angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);

            GraphicsState state = g.Save();
            g.TranslateTransform(start.X, start.Y);
            g.RotateTransform(angle);
            g.DrawImage(img, new RectangleF(0, -thickness / 2, distance, thickness));
            g.Restore(state);
        }
    }
}