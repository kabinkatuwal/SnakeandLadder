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
            btnStartGame = new Button();
            btnSettings = new Button();
            groupBox1 = new GroupBox();
            btnNewGame = new Button();
            btnLoadGame = new Button();
            btnSaveGame = new Button();
            aiTimer = new System.Windows.Forms.Timer(components);
            pbDice = new PictureBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbDice).BeginInit();
            SuspendLayout();
            // 
            // boardPanel
            // 
            boardPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            boardPanel.BackColor = Color.White;
            boardPanel.ImeMode = ImeMode.Disable;
            boardPanel.Location = new Point(21, 12);
            boardPanel.Name = "boardPanel";
            boardPanel.Size = new Size(500, 426);
            boardPanel.TabIndex = 0;
            boardPanel.Paint += boardPanel_Paint;
            // 
            // rollButton
            // 
            rollButton.Location = new Point(70, 148);
            rollButton.Name = "rollButton";
            rollButton.Size = new Size(94, 29);
            rollButton.TabIndex = 1;
            rollButton.Text = "Roll Dice";
            rollButton.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(41, 33);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(149, 20);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Welcome ! Click Start";
            // 
            // lblPlayerInfo
            // 
            lblPlayerInfo.AutoSize = true;
            lblPlayerInfo.Location = new Point(70, 110);
            lblPlayerInfo.Name = "lblPlayerInfo";
            lblPlayerInfo.Size = new Size(101, 20);
            lblPlayerInfo.TabIndex = 3;
            lblPlayerInfo.Text = "Current Player";
            // 
            // btnStartGame
            // 
            btnStartGame.Location = new Point(70, 67);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(94, 29);
            btnStartGame.TabIndex = 4;
            btnStartGame.Text = "Start Game";
            btnStartGame.UseVisualStyleBackColor = true;
            // 
            // btnSettings
            // 
            btnSettings.Location = new Point(70, 183);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(94, 29);
            btnSettings.TabIndex = 5;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox1.Controls.Add(pbDice);
            groupBox1.Controls.Add(btnNewGame);
            groupBox1.Controls.Add(btnLoadGame);
            groupBox1.Controls.Add(btnSaveGame);
            groupBox1.Controls.Add(lblStatus);
            groupBox1.Controls.Add(btnSettings);
            groupBox1.Controls.Add(btnStartGame);
            groupBox1.Controls.Add(lblPlayerInfo);
            groupBox1.Controls.Add(rollButton);
            groupBox1.Location = new Point(538, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 431);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // btnNewGame
            // 
            btnNewGame.Location = new Point(70, 253);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(94, 29);
            btnNewGame.TabIndex = 8;
            btnNewGame.Text = "New Game";
            btnNewGame.UseVisualStyleBackColor = true;
            // 
            // btnLoadGame
            // 
            btnLoadGame.Location = new Point(150, 218);
            btnLoadGame.Name = "btnLoadGame";
            btnLoadGame.Size = new Size(94, 29);
            btnLoadGame.TabIndex = 7;
            btnLoadGame.Text = "Load Game";
            btnLoadGame.UseVisualStyleBackColor = true;
            // 
            // btnSaveGame
            // 
            btnSaveGame.Location = new Point(6, 218);
            btnSaveGame.Name = "btnSaveGame";
            btnSaveGame.Size = new Size(94, 29);
            btnSaveGame.TabIndex = 6;
            btnSaveGame.Text = "Save";
            btnSaveGame.UseVisualStyleBackColor = true;
            // 
            // aiTimer
            // 
            aiTimer.Interval = 500;
            //aiTimer.Tick += aiTimer_Tick;
            // 
            // pbDice
            // 
            pbDice.Location = new Point(30, 288);
            pbDice.Name = "pbDice";
            pbDice.Size = new Size(183, 137);
            pbDice.SizeMode = PictureBoxSizeMode.StretchImage;
            pbDice.TabIndex = 9;
            pbDice.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
        private Button btnStartGame;
        private Button btnSettings;
        private GroupBox groupBox1;
        private System.Windows.Forms.Timer aiTimer;
        private Button btnSaveGame;
        private Button btnNewGame;
        private Button btnLoadGame;
        private PictureBox pbDice;
    }
}
