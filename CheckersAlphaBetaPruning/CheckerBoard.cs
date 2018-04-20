using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CheckersAlphaBetaPruning
{
    public partial class CheckerBoard : Form
    {
        private Game game;

        public CheckerBoard(bool playerOne)
        {
            InitializeComponent();
            List<Button> allButtons = this.Controls.OfType<Button>().ToList(); //Get every button to pass to the game
            allButtons.Reverse(); //put the list in sorted order
            allButtons.Remove(button37); //remove button for StartGame
            allButtons.Remove(mainmenu); //remove button for MainMenu
            game = new Game(allButtons, playerOne, label1, AI_StatsTable); //pass in everybutton for the game, the label, and the stats table

            //Set the event for clicking a button to the same function (ButtonPress)
            button1.Click  += ButtonPress;
            button2.Click  += ButtonPress;
            button3.Click  += ButtonPress;
            button4.Click  += ButtonPress;
            button5.Click  += ButtonPress;
            button6.Click  += ButtonPress;
            button7.Click  += ButtonPress;
            button8.Click  += ButtonPress;
            button9.Click  += ButtonPress;
            button10.Click += ButtonPress;
            button11.Click += ButtonPress;
            button12.Click += ButtonPress;
            button13.Click += ButtonPress;
            button14.Click += ButtonPress;
            button15.Click += ButtonPress;
            button16.Click += ButtonPress;
            button17.Click += ButtonPress;
            button18.Click += ButtonPress;
            button19.Click += ButtonPress;
            button20.Click += ButtonPress;
            button21.Click += ButtonPress;
            button22.Click += ButtonPress;
            button23.Click += ButtonPress;
            button24.Click += ButtonPress;
            button25.Click += ButtonPress;
            button26.Click += ButtonPress;
            button27.Click += ButtonPress;
            button28.Click += ButtonPress;
            button29.Click += ButtonPress;
            button30.Click += ButtonPress;
            button31.Click += ButtonPress;
            button32.Click += ButtonPress;
            button33.Click += ButtonPress;
            button34.Click += ButtonPress;
            button35.Click += ButtonPress;
            button36.Click += ButtonPress;
            button37.Click += StartGame;
        }

        public void ButtonPress(object sender, EventArgs e)
        {
            game.SelectPiece(sender as Button); //Let the game know that the piece was selected
            if (game.GameOver) //If game is over, then allow the user to go to the main menu
            {
                mainmenu.Enabled = true;
                mainmenu.Visible = true;
                mainmenu.Invalidate();
                mainmenu.Update();
            }
        }
        public void StartGame(object sender, EventArgs e) //Start the game
        {
            //do not allow the user to go to the main menu /start game anymore
            button37.Enabled = false; 
            button37.Visible = false;
            mainmenu.Enabled = false;
            mainmenu.Visible = false;
            button37.Invalidate();
            mainmenu.Invalidate();
            button37.Update();
            mainmenu.Update();
            //tell game to start
            game.StartGame();
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            //Create a new instance of the main menu and start it
            var nextStep = new MainMenu();
            this.Hide();
            nextStep.StartPosition = FormStartPosition.CenterParent;
            nextStep.ShowDialog();
            this.Close();
        }
    }
}
