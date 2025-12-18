using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SnakeandLadder
{
    public partial class PlayerSelectionForm : Form
    {
        public List<Player> SelectedPlayers { get; private set; }

        public PlayerSelectionForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            SelectedPlayers = new List<Player>();
            int count = (int)numPlayers.Value;
            string[] colors = { "Red", "Blue", "Green", "Yellow" };
            for (int i = 0; i < count; i++)
            {
                SelectedPlayers.Add(new Player($"Player {i + 1}", colors[i]));
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}