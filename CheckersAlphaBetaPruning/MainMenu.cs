using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersAlphaBetaPruning
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var nextStep = new CheckerBoard(true);
            this.Hide();
            nextStep.StartPosition = FormStartPosition.CenterParent;
            nextStep.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var nextStep = new CheckerBoard(false);
            this.Hide();
            nextStep.StartPosition = FormStartPosition.CenterParent;
            nextStep.ShowDialog();
            this.Close();
        }
    }
}
