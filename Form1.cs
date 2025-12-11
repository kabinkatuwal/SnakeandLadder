
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;



namespace SnakeandLadder
{
    public partial class Form1 : Form
    {
        private Game game; 
        private Dice dice = new Dice();
        public Form1()
        {
            InitializeComponent();
            this.Text = "Snake Slide Game"; // Set the window title

            // 1. Initialize Players (Example: 2 Human + 1 AI)
            List<Player> initialPlayers = new List<Player>
        {
            new Player("Kabin", "Red"),
            new Player("Baburam", "Blue"),
            new AIPlayer("AI Bot", "Green")
        };

            // 2. Initialize the Game Engine
            game = new Game(initialPlayers);

            // 3. Subscribe to Game Events (using Lambda Expressions for clean code!)
            game.OnGameMessage += (message) =>
            {
                // Simple display of non-critical messages
                lblStatus.Text = message;
            };

            game.OnGameWin += (message) =>
            {
                lblStatus.Text = message;
                rollButton.Enabled = false; // Stop the game
                MessageBox.Show(message, "Game Over!");
            };

            game.OnPlayerMove += HandlePlayerMove;

            // 4. Start the game
            game.StartGame();
        }
        // Form1.cs (Inside the Form1 class)
        private void HandlePlayerMove(Player player, int steps, int fromPos, int toPos, bool hitSpecial)
        {
            // Update the player's position on the board panel
            boardPanel.Invalidate(); // Triggers the Paint event to redraw the board

            if (hitSpecial)
            {
                // Display a special message for snakes or ladders
                if (toPos > fromPos)
                {
                    lblStatus.Text = $"{player.Name} rolled {steps}, landed on a 🪜 ladder and moved UP to {toPos}!";
                }
                else
                {
                    lblStatus.Text = $"{player.Name} rolled {steps}, landed on a 🐍 snake and slid DOWN to {toPos}!";
                }
            }
            else
            {
                lblStatus.Text = $"{player.Name} rolled {steps}. New Position: {toPos}.";
            }

            // Update the button text to show whose turn is next
            if (game.IsRunning)
            {
                rollButton.Text = $"Roll ({game.CurrentPlayer.Name})";
            }
            if (game.CurrentPlayer.IsAI)
            {
                rollButton.Enabled = false; // Disable button while AI is thinking
                aiTimer.Start(); // Start the timer to trigger the AI's turn after a delay
            }
            else
            {
                rollButton.Enabled = true; // Enable button for human player
            }
        }
       
        private void rollButton_Click(object sender, EventArgs e)
        {
            // The button simply tells the Game engine to run the next turn.
            game.NextTurn();
        }
        // Form1.cs (Add this method)
        private const int GRID_SIZE = 10;
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

            // Y is calculated from the bottom up (9 - row)
            int x = col * cellSize + (cellSize / 2);
            int y = (GRID_SIZE - 1 - row) * cellSize + (cellSize / 2);

            return new Point(x, y);
        }
       
        private void boardPanel_Paint(object sender, PaintEventArgs e)
        {
            // Safety check: ensure the game engine is initialized
            if (game == null || game.Board == null) return;

            Graphics g = e.Graphics;
            int cellSize = boardPanel.Width / GRID_SIZE; // GRID_SIZE is 10

            // --- Setup Drawing Resources (Pens and Brushes) ---
            // Note: In production code, these would often be class members for efficiency.
            Pen borderPen = new Pen(Color.Gray, 1);
            SolidBrush textBrush = new SolidBrush(Color.Black);
            Font cellFont = new Font("Arial", 8, FontStyle.Bold);

            // 1. Draw Grid and Numbers
            for (int i = 1; i <= 100; i++)
            {
                Point center = GetCellCenterCoordinates(i);

                // Calculate the top-left corner of the cell
                int cellX = center.X - (cellSize / 2);
                int cellY = center.Y - (cellSize / 2);
                Rectangle cellRect = new Rectangle(cellX, cellY, cellSize, cellSize);

                // Draw the cell background (Alternating colors for visual appeal)
                Color bgColor = i % 2 == 0 ? Color.FromArgb(240, 240, 240) : Color.White;
                g.FillRectangle(new SolidBrush(bgColor), cellRect);

                // Draw the cell border
                g.DrawRectangle(borderPen, cellRect);

                // Draw the cell number (centered or cornered)
                string cellNumber = i.ToString();
                g.DrawString(cellNumber, cellFont, textBrush, cellX + 2, cellY + 2);
            }

            // 2. Draw Snakes and Ladders (Uses data from the Game.Board object)

            // Ladders (Green lines)
            Pen ladderPen = new Pen(Color.Green, 3) { StartCap = LineCap.Round, EndCap = LineCap.ArrowAnchor };
            foreach (var ladder in game.Board.Ladders)
            {
                Point start = GetCellCenterCoordinates(ladder.Bottom);
                Point end = GetCellCenterCoordinates(ladder.Top);
                // Draw the line from the center of the bottom cell to the center of the top cell
                g.DrawLine(ladderPen, start, end);
            }

            // Snakes (Red, wavy/thick lines)
            Pen snakePen = new Pen(Color.Red, 5) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            foreach (var snake in game.Board.Snakes)
            {
                Point start = GetCellCenterCoordinates(snake.Head);
                Point end = GetCellCenterCoordinates(snake.Tail);

                // MVP: Draw a simple straight line. 
                // Improvement: Use GraphicsPath to draw a curved/wavy snake path.
                g.DrawLine(snakePen, start, end);
            }

            // 3. Draw Player Tokens (cite: 24)
            int tokenSize = cellSize / 4;
            int playerOffset = 0; // Offset multiple players on the same square

            foreach (Player player in game.Players)
            {
                Point tokenCenter = GetCellCenterCoordinates(player.Position);

                // Convert the string color to a Brush
                // Using System.Drawing.ColorConverter is safer for dynamic colors
                Color playerColor = Color.FromName(player.TokenColor);
                Brush playerBrush = new SolidBrush(playerColor);

                // Draw a circle for the token
                g.FillEllipse(playerBrush,
                              tokenCenter.X - (tokenSize / 2) + playerOffset,
                              tokenCenter.Y - (tokenSize / 2) + playerOffset,
                              tokenSize,
                              tokenSize);

                // Draw a small outline (optional)
                g.DrawEllipse(Pens.Black,
                              tokenCenter.X - (tokenSize / 2) + playerOffset,
                              tokenCenter.Y - (tokenSize / 2) + playerOffset,
                              tokenSize,
                              tokenSize);

                // Increment offset so tokens don't perfectly overlap
                playerOffset += 5;
            }

        }
        // Form1.cs (Inside the aiTimer_Tick event handler)
        private void aiTimer_Tick(object sender, EventArgs e)
        {
            // 1. Stop the timer immediately so it only fires once
            aiTimer.Stop();

            // 2. Disable the button (since the turn is automatic)
            rollButton.Enabled = false;

            // 3. Trigger the AI's turn
            game.NextTurn();

            // 4. Re-enable the button if the current player is NOT AI
            if (!game.CurrentPlayer.IsAI && game.IsRunning)
            {
                rollButton.Enabled = true;
            }
        }
    }
    }

