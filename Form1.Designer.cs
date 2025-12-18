namespace SnakeandLadder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            boardPanel = new Panel();
            rollButton = new Button();
            lblStatus = new Label();
            lblPlayerInfo = new Label();
            groupBox1 = new GroupBox();
            pbDice = new PictureBox();
            btnNewGame = new Button();
            btnLoadGame = new Button();
            btnSaveGame = new Button();
            aiTimer = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbDice).BeginInit();
            SuspendLayout();
            // 
            // boardPanel
            // 
            boardPanel.BackColor = Color.White;
            boardPanel.Dock = DockStyle.Fill;
            boardPanel.ImeMode = ImeMode.Disable;
            boardPanel.Location = new Point(0, 0);
            boardPanel.Name = "boardPanel";
            boardPanel.Size = new Size(1772, 774);
            boardPanel.TabIndex = 0;
            boardPanel.Paint += boardPanel_Paint;
            // 
            // rollButton
            // 
            rollButton.Location = new Point(88, 351);
            rollButton.Name = "rollButton";
            rollButton.Size = new Size(94, 49);
            rollButton.TabIndex = 1;
            rollButton.Text = "Roll Dice";
            rollButton.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(36, 39);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(149, 20);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Welcome ! Click Start";
            // 
            // lblPlayerInfo
            // 
            lblPlayerInfo.AutoSize = true;
            lblPlayerInfo.Location = new Point(36, 122);
            lblPlayerInfo.Name = "lblPlayerInfo";
            lblPlayerInfo.Size = new Size(101, 20);
            lblPlayerInfo.TabIndex = 3;
            lblPlayerInfo.Text = "Current Player";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(pbDice);
            groupBox1.Controls.Add(btnNewGame);
            groupBox1.Controls.Add(btnLoadGame);
            groupBox1.Controls.Add(btnSaveGame);
            groupBox1.Controls.Add(lblStatus);
            groupBox1.Controls.Add(lblPlayerInfo);
            groupBox1.Controls.Add(rollButton);
            groupBox1.Dock = DockStyle.Right;
            groupBox1.Location = new Point(1196, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(576, 774);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            // 
            // pbDice
            // 
            pbDice.Location = new Point(47, 419);
            pbDice.Name = "pbDice";
            pbDice.Size = new Size(213, 196);
            pbDice.SizeMode = PictureBoxSizeMode.StretchImage;
            pbDice.TabIndex = 9;
            pbDice.TabStop = false;
            // 
            // btnNewGame
            // 
            btnNewGame.Location = new Point(88, 193);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(94, 46);
            btnNewGame.TabIndex = 8;
            btnNewGame.Text = "New Game";
            btnNewGame.UseVisualStyleBackColor = true;
            // 
            // btnLoadGame
            // 
            btnLoadGame.Location = new Point(147, 266);
            btnLoadGame.Name = "btnLoadGame";
            btnLoadGame.Size = new Size(94, 45);
            btnLoadGame.TabIndex = 7;
            btnLoadGame.Text = "Load Game";
            btnLoadGame.UseVisualStyleBackColor = true;
            // 
            // btnSaveGame
            // 
            btnSaveGame.Location = new Point(17, 266);
            btnSaveGame.Name = "btnSaveGame";
            btnSaveGame.Size = new Size(94, 45);
            btnSaveGame.TabIndex = 6;
            btnSaveGame.Text = "Save";
            btnSaveGame.UseVisualStyleBackColor = true;
            // 
            // aiTimer
            // 
            aiTimer.Interval = 500;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1772, 774);
            Controls.Add(groupBox1);
            Controls.Add(boardPanel);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbDice).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel boardPanel;
        private Button rollButton;
        private Label lblStatus;
        private Label lblPlayerInfo;
        private GroupBox groupBox1;
        private System.Windows.Forms.Timer aiTimer;
        private Button btnSaveGame;
        private Button btnNewGame;
        private Button btnLoadGame;
        private PictureBox pbDice;
    }
}
