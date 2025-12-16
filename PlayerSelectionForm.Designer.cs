namespace SnakeandLadder
{
    partial class PlayerSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            numPlayers = new NumericUpDown();
            btnStart = new Button();
            ((System.ComponentModel.ISupportInitialize)numPlayers).BeginInit();
            SuspendLayout();
            // 
            // numPlayers
            // 
            numPlayers.Location = new Point(284, 44);
            numPlayers.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            numPlayers.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPlayers.Name = "numPlayers";
            numPlayers.Size = new Size(150, 27);
            numPlayers.TabIndex = 0;
            numPlayers.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnStart
            // 
            btnStart.Location = new Point(135, 44);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start Game";
            btnStart.UseVisualStyleBackColor = true;
            // 
            // PlayerSelectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStart);
            Controls.Add(numPlayers);
            Name = "PlayerSelectionForm";
            Text = "PlayerSelectionForm";
            ((System.ComponentModel.ISupportInitialize)numPlayers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private NumericUpDown numPlayers;
        private Button btnStart;
    }
}