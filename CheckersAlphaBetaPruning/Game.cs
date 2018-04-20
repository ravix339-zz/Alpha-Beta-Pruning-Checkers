using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersAlphaBetaPruning
{
    class Game
    {
        private Map<Tuple<int, int>, Button> CoordinateButtonMap; //forward is Map[Tuple<int,int>] = Button... reverse is Map[Button] = Tuple<int,int>

        private List<List<GamePiece>> board;
        private int BoardSize;
        /*
         *       Board's Coordinate System
         *  
         *  5
         *  4
         *  3
         *  2
         *  1
         *  0 
         *  y/x 5 4 3 2 1 0
         */

        //Possible Sentinents of Pieces
        private const int PLAYER = 1;
        private const int NEUTRAL = 0;
        private const int AI = -1;

        //Recursion Depth for Alpha-Beta Pruning
        private const int RECURSION_DEPTH_FIRST = 20; //Depth if Player is first
        private const int RECURSION_DEPTH_SECOND = 19; //Depth is Player is second

        
        private bool playersTurn;
        private Label AI_Status; //Label that tells the user the status of the game
        private DataGridView AI_Stats; //Table that displays specific stats
        private bool gameOver = false;
        private List<Tuple<int, int>> move; //List of two coordinates that stores coordinates to move (StartPosition of piece, EndPosition of piece)
        private Tuple<int, int> dummyCoordinate = new Tuple<int, int>(-1, -1); //False coordinate to detect for invalid moves
        private AlphaBetaPruning ComputerPlayer; //Alpha-Beta Pruning Object

        //Public Bool to let CheckerBoard.cs if the game is over.
        public bool GameOver 
        {
            get { return gameOver; }
        }

        //Constructor for a game.
        public Game(List<Button> buttons, bool playFirst, Label label, DataGridView table)
        {
            //Initialize Variables
            AI_Status = label;
            AI_Stats = table;
            CoordinateButtonMap = new Map<Tuple<int, int>, Button>();
            board = new List<List<GamePiece>>();
            move = new List<Tuple<int, int>>(2);
            ComputerPlayer = new AlphaBetaPruning(playFirst ? RECURSION_DEPTH_FIRST : RECURSION_DEPTH_SECOND); //set Recursion depth depending on if player is first.
            move.Add(dummyCoordinate);
            move.Add(dummyCoordinate); //make entire move invalid.
            BoardSize = Convert.ToInt32(Math.Sqrt(buttons.Count)); //get Size of board
            playersTurn = playFirst;

            int xCoordinate = BoardSize; //start at one row below the bottom of the board
            int yCoordinate = -1;

            foreach (Button button in buttons)
            {
                if (xCoordinate == BoardSize) //go to the next row and start at the beginning of the row (right)
                {
                    xCoordinate = 0;
                    yCoordinate += 1;
                    board.Add(new List<GamePiece>()); //add a new list
                }
                button.Enabled = false; //disable button
                button.Text = ""; //remove text from the UI
                //button.Text = "(" + xCoordinate.ToString() + "," + yCoordinate.ToString() + ")";
                button.BackColor = System.Drawing.Color.FloralWhite; //set color to the NEUTRAL color
                button.ForeColor = System.Drawing.Color.FloralWhite;
                board[yCoordinate].Add(new GamePiece(NEUTRAL, xCoordinate, yCoordinate)); //Add the GamePiece to the list

                CoordinateButtonMap.Add(new Tuple<int, int>(xCoordinate, yCoordinate), button); //Map the GamePiece to the Button
                xCoordinate++;
            }
            AI_Status.Text = "Start the Game!";

            for(int y = 0; y < BoardSize; y++)
            {
                for(int x = 0; x < BoardSize; x++)
                {
                    Button button = CoordinateButtonMap.Forward[new Tuple<int, int>(x, y)]; //get the button at the specific coordinate
                    if (x % 2 == 1 && y == 0) //if its in the first row (bottom) & at an odd xCoordinate
                    {
                        board[y][x].Sentiment = PLAYER;
                        button.BackColor = System.Drawing.Color.Black;
                        button.ForeColor = System.Drawing.Color.Black;
                    }
                    if (x % 2 == 1 && y == BoardSize - 2) //if its in second to last row (top) & at an odd xCoordinate
                    {
                        board[y][x].Sentiment = AI;
                        button.BackColor = System.Drawing.Color.Red;
                        button.ForeColor = System.Drawing.Color.Red;
                    }
                    if (x % 2 == 0 && y == 1) //if its in the second row (bottom) & at an even xCoordinate
                    {
                        board[y][x].Sentiment = PLAYER;
                        button.BackColor = System.Drawing.Color.Black;
                        button.ForeColor = System.Drawing.Color.Black;
                    }
                    if (x % 2 == 0 && y == BoardSize - 1) //if its in last to last row (top) & at an even xCoordinate
                    {
                        board[y][x].Sentiment = AI;
                        button.BackColor = System.Drawing.Color.Red;
                        button.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        //Function that starts the game
        public void StartGame()
        {
            if (playersTurn) { AI_Status.Text = "Your turn!"; }
            FixButtons();
        }

        //Function that fixes the colors of the buttons and lets the next player go.
        private void FixButtons()
        {
            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++) //go through each button
                {
                    Tuple<int, int> location = new Tuple<int, int>(x, y); //get the current location
                    Button button = CoordinateButtonMap.Forward[location]; //and associated button
                    int sentiment = board[y][x].Sentiment;
                    /*
                     * Based on the sentiment of the piece, set the piece's color. 
                     * Player pieces being enabled is based upon if the piece has a valid move.
                     */
                    switch (sentiment)
                    {
                        case PLAYER:
                            button.Enabled = false;
                            button.BackColor = System.Drawing.Color.Black;
                            button.ForeColor = System.Drawing.Color.Black;
                            break;
                        case AI:
                            button.Enabled = false;
                            button.BackColor = System.Drawing.Color.Red;
                            button.ForeColor = System.Drawing.Color.Red;
                            break;
                        case NEUTRAL:
                            button.Enabled = false;
                            button.BackColor = System.Drawing.Color.FloralWhite;
                            button.ForeColor = System.Drawing.Color.FloralWhite;
                            break;
                        default:
                            break;
                    }
                    button.Invalidate(); //force an update on the button.
                    button.Update();
                }
            }
            Tuple<bool, int> results = Terminal_Test(board); //get results of termination test (result is described later)
            if (results.Item1 == true) //if Game is Over
            {
                gameOver = true;
                if (results.Item2 == NEUTRAL) //check who won
                {
                    AI_Status.Text = "ITS A TIE!";
                }
                else if (results.Item2 == AI) { AI_Status.Text = "I won!"; }
                else { AI_Status.Text = "You win!"; }
            }
            if (results.Item1 == false) //Otherwise
            {
                if (ValidMoves(board, PLAYER).Count == 0) //If no valid moves for player
                {
                    AI_Status.Text = "You have no available moves! It's my turn!"; //Let the AI play
                    AI_Status.Invalidate();
                    AI_Status.Update(); //Force update on UI
                    Thread.Sleep(1000); //Wait a second
                    CPU_Turn(); //Allow the CPU to go.
                }
                else if (!playersTurn) //if its not the players turn regardless... let the CPU go.
                {
                    CPU_Turn();
                }
                else { enablePlayerButtons(); } //Allow certain buttons to be selected by the player.
            }

        }

        //Function to enable the buttons corresponding with the first part of the moves for the player
        private void enablePlayerButtons()
        {
            if (gameOver) { return; }
            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> movement in ValidMoves(board, PLAYER)) //Get the valid moves currently.
            {
                CoordinateButtonMap.Forward[movement.Item1].Enabled = true; //set the button for the first part of the move to enabled
                CoordinateButtonMap.Forward[movement.Item1].Invalidate();
                CoordinateButtonMap.Forward[movement.Item1].Update(); //force UI updated
            }
        }

        //Function that keeps track of the buttons selected for a move by the player.
        public void SelectPiece(Button selectedButton)
        {
            if (gameOver) { return; }
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> moves = ValidMoves(board, PLAYER); //Get all valid moves
            if (move[0] == dummyCoordinate) //If the button selected is the first button in the move
            {
                move[0] = CoordinateButtonMap.Reverse[selectedButton]; //get the game piece associated with the button selected
                foreach(Tuple<Tuple<int,int>, Tuple<int,int>> validMove in moves) //go through all of the moves
                {
                    Tuple<int, int> startPos = validMove.Item1;
                    if(startPos.Item1 == move[0].Item1 && startPos.Item2 == move[0].Item2) //If the starting position for the move is equal to the selected button
                    {
                        Button button = CoordinateButtonMap.Forward[validMove.Item2]; //get the button for the ending position of the move
                        button.ForeColor = System.Drawing.Color.Yellow; //set it to yellow
                        button.BackColor = System.Drawing.Color.Yellow; 
                    }
                }
                for(int y = 0; y < BoardSize; y++)
                {
                    for(int x = 0; x < BoardSize; x++)
                    {
                        Tuple<int, int> location = new Tuple<int, int>(x, y);
                        if ((location.Item1 != move[0].Item1 || location.Item2 != move[0].Item2) && CoordinateButtonMap.Forward[location].ForeColor != System.Drawing.Color.Yellow)
                            //Disable all buttons that are not yellow OR are not the original piece (to cancel a move)
                        {
                            CoordinateButtonMap.Forward[location].Enabled = false;
                        }
                        else
                        {
                            CoordinateButtonMap.Forward[location].Enabled = true;
                        }
                    }
                }
            }
            else //Player selected second part of the move.
            {
                move[1] = CoordinateButtonMap.Reverse[selectedButton]; //Assign ending position in move to Coordinate associated with button
                if (move[0] == move[1]) //If player selected the same button
                {
                    move[0] = dummyCoordinate;
                    move[1] = dummyCoordinate; //reset move
                    FixButtons(); //Fix buttons and redoes turn.
                }
                else //we completed selecting a move
                {
                    board = ApplyMove(board, new Tuple<Tuple<int, int>, Tuple<int, int>>(move[0], move[1])); //apply move to the board
                    move[0] = dummyCoordinate;
                    move[1] = dummyCoordinate; //reset move
                    if(ValidMoves(board, AI).Count != 0) { playersTurn = false; } //if there are moves for the AI, set the turn to the AI
                    FixButtons(); //Fix buttons to allow AI to go.
                }
            }
        }

        //Function that performs the AI's turn.
        private void CPU_Turn()
        {
            if (gameOver) { return; }

            AI_Status.Text = "I'm thinking...";
            AI_Status.Invalidate();
            AI_Status.Update(); //Force Update UI to alert Player

            Tuple<Tuple<int, int>, Tuple<int, int>> move = ComputerPlayer.determineNextMove(board); //Determine the move the AI will use (AlphaBetaPruning.cs)

            Tuple<int, int> startPos = move.Item1; //Get starting position from move
            Tuple<int, int> endPos = move.Item2; //Get ending position from move

            DataTable statsTable = new DataTable(); //Create a datatable that will store statistics to output to player.
            statsTable.Columns.Add(new DataColumn("MaxDepth")); //Maximum depth reached
            statsTable.Columns.Add(new DataColumn("nodesGenerated")); //Number of nodes generated
            statsTable.Columns.Add(new DataColumn("MaxPruned")); //Number of times Max-Value pruned
            statsTable.Columns.Add(new DataColumn("MinPruned")); //Number of times Min-Value pruned
            DataRow row = statsTable.NewRow(); //Create new row in the table
            
            //Add statistics to the table
            row["MaxDepth"] = ComputerPlayer.Statistics[0];
            row["nodesGenerated"] = ComputerPlayer.Statistics[1];
            row["MaxPruned"] = ComputerPlayer.Statistics[2];
            row["MinPruned"] = ComputerPlayer.Statistics[3];
            statsTable.Rows.Add(row);

            //Put all of the information onto the UI table
            AI_Stats.Columns[0].DataPropertyName = "MaxDepth";
            AI_Stats.Columns[1].DataPropertyName = "nodesGenerated";
            AI_Stats.Columns[2].DataPropertyName = "MaxPruned";
            AI_Stats.Columns[3].DataPropertyName = "MinPruned";
            AI_Stats.DataSource = statsTable;
            AI_Stats.Invalidate();
            AI_Stats.Update(); //Force Update the UI

            if (startPos.Item1 != -1  && endPos.Item1 != -1) //Precautionary check for invalid moves
            {
                board = Game.ApplyMove(board, move); //Apply move to the board
            }
            playersTurn = true; //set the players turn to true
            FixButtons(); //Fix buttons (if it will be the CPU's turn again, the function will determine that)
            if (!gameOver) //Once the CPU's turn is done (including when it runs multiple moves
            {
                AI_Status.Text = "Your turn!"; //Allow player to move.
                AI_Status.Invalidate();
                AI_Status.Update();
            }
        }

        /*
         * 
         *  Static Methods 
         * 
         */
        
        //Function to create a new copy of the board to avoid rewriting the board (necessary for AlphaBetaPruning.cs)
        public static List<List<GamePiece>> CopyBoard(List<List<GamePiece>> board)
        {
            List<List<GamePiece>> ret = new List<List<GamePiece>>();
            for (int y = 0; y < board.Count; y++)
            {
                ret.Add(new List<GamePiece>());
                for (int x = 0; x < board.Count; x++)
                {
                    ret[y].Add(new GamePiece(board[y][x].Sentiment, x, y)); //Create a new piece thats identical to the original.
                }
            }
            return ret;
        }

        //Function to give the valid moves for a particular player(AI, PLAYER)
        public static List<Tuple<Tuple<int, int>, Tuple<int, int>>> ValidMoves(List<List<GamePiece>> board, int sentiment)
        {
            //Moves that allow for killing opponent
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> killMoves = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
            //Moves that are just normal movements.
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> nonKillMoves = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
            int boardSize = board.Count; //since its static, we must calculate the board size internally.

            if (sentiment == AI) //If AI
            {
                for (int y = 1; y < boardSize; y++) //If y == 0, we are at the bottom of the board and there are no valid moves
                {
                    for (int x = 0; x < boardSize; x++)
                    {
                        if (board[y][x].Sentiment != AI) { continue; } //Only check for moves if the current piece is an AI
                        Tuple<int, int> currentPosition = new Tuple<int, int>(x, y);
                        if (x > 0 && board[y - 1][x - 1].Sentiment == NEUTRAL)   //Check if possible to move down right to NEUTRAL space
                        {
                            nonKillMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x - 1, y - 1))); //add move to the nonKillMoves
                        }
                        if (x > 1 && y > 1 && board[y - 1][x - 1].Sentiment == PLAYER && board[y - 2][x - 2].Sentiment == NEUTRAL)  //Check if possible to move down right killing PLAYER
                        {
                            killMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x - 2, y - 2))); //add move to the killMoves
                        }
                        if (x < boardSize - 1 && board[y - 1][x + 1].Sentiment == NEUTRAL)   //Check if possible to move down left to NEUTRAL space
                        {
                            nonKillMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x + 1, y - 1))); //add move to the nonKillMoves
                        }
                        if (x < boardSize - 2 && y > 1 && board[y - 1][x + 1].Sentiment == PLAYER && board[y - 2][x + 2].Sentiment == NEUTRAL) //Check if possible to move down left killing PLAYER
                        {
                            killMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x + 2, y - 2))); //add move to the killMoves
                        }
                    }
                }
            }
            if (sentiment == PLAYER)
            {
                for (int y = 0; y < boardSize - 1; y++) //Dont check y = boardSize-1 since if a PLAYER piece is at the row (top), there are no moves
                {
                    for (int x = 0; x < boardSize; x++)
                    {
                        if (board[y][x].Sentiment != PLAYER) { continue; } //Only want to check if the piece is the player
                        Tuple<int, int> currentPosition = new Tuple<int, int>(x, y);
                        if (x > 0 && board[y + 1][x - 1].Sentiment == NEUTRAL)  //Check if possible to move up right to NEUTRAL space
                        {
                            nonKillMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x - 1, y + 1))); //add move to the nonKillMoves
                        }
                        if (x > 1 && y < boardSize - 2 && board[y + 1][x - 1].Sentiment == AI && board[y + 2][x - 2].Sentiment == NEUTRAL) //Check if possible to move up right killing AI
                        {
                            killMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x - 2, y + 2))); //add move to the killMoves
                        }
                        if (x < boardSize - 1 && board[y + 1][x + 1].Sentiment == NEUTRAL) //Check if possible to move up left to NEUTRAL space
                        {
                            nonKillMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x + 1, y + 1))); //add move to the nonKillMoves
                        }
                        if (x < boardSize - 2 && y < boardSize - 2 && board[y + 1][x + 1].Sentiment == AI && board[y + 2][x + 2].Sentiment == NEUTRAL) //Check if possible to move up left killing AI
                        {
                            killMoves.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(currentPosition, new Tuple<int, int>(x + 2, y + 2))); //add move to the killMoves
                        }
                    }
                }
            }
            if(killMoves.Count != 0) { return killMoves; } //If the number of killMoves is non-zero force the player (sentiment) to perform a kill move
            return nonKillMoves; //otherwise kill the non-kill moves
        }

        //Function to return a new board when a move is made
        public static List<List<GamePiece>> ApplyMove(List<List<GamePiece>> board, Tuple<Tuple<int, int>, Tuple<int, int>> move)
        {
            List<List<GamePiece>> newBoard = Game.CopyBoard(board); //Create local copy of the board
            Tuple<int, int> startPos = move.Item1; //Get starting position of the move
            Tuple<int, int> endPos = move.Item2; //Get ending position of the move
            newBoard[endPos.Item2][endPos.Item1].Sentiment = newBoard[startPos.Item2][startPos.Item1].Sentiment; //move piece
            newBoard[startPos.Item2][startPos.Item1].Sentiment = NEUTRAL; //set initial position to NEUTRAL
            if (Math.Abs(startPos.Item1 - endPos.Item1) == 2 && Math.Abs(startPos.Item2 - endPos.Item2) == 2) //If the movement was 2 units then it was a kill move. therefore, we must remove the killed piece
            {
                int xDiff = (endPos.Item1 - startPos.Item1) / 2; 
                int yDiff = (endPos.Item2 - startPos.Item2) / 2;
                newBoard[startPos.Item2 + yDiff][startPos.Item1 + xDiff].Sentiment = NEUTRAL; //Get the middle position & make it NEUTRAL
            }
            return newBoard;
        }

        //Function to determine if the game is over.
        //Returns a Tuple<bool, int>. Bool is if the game is over (true if over, false if not). Int is who wins (AI, PLAYER, NEUTRAL)
        public static Tuple<bool, int> Terminal_Test(List<List<GamePiece>> board)
        {
            int boardSize = board.Count; //Static function so we need to recalculate the size of the board

            int AI_Squares = 0; //Number of pieces on the board that are AI
            int PLAYER_Squares = 0; //Number of pieces on the board that are PLAYER

            int numberOfMoves = 0; //Number of possible moves available this turn.

            int AI_maxY = 0; //Highest y-value for the AI pieces
            int PLAYER_minY = boardSize; //Lowest y-value for the PLAYER pieces

            //Count number of AI & PLAYER pieces and also assign values for PLAYER_minY & AI_maxY
            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    if (board[y][x].Sentiment == AI) {
                        AI_Squares++;
                        if(AI_maxY < y) { AI_maxY = y; }
                    }
                    if (board[y][x].Sentiment == PLAYER) {
                        PLAYER_Squares++;
                        if (PLAYER_minY > y) { PLAYER_minY = y; }
                    }
                }
            }
            //If no pieces of one of the players exist then the other is the victor
            if (PLAYER_Squares == 0) { return new Tuple<bool, int>(true, AI); }
            if (AI_Squares == 0) { return new Tuple<bool, int>(true, PLAYER); }

            //otherwise we check how many moves are available in the game.
            numberOfMoves = Game.ValidMoves(board, AI).Count + Game.ValidMoves(board, PLAYER).Count;

            //If the number of moves is zero we can say that the game is over
            //OR we can say the game is over if every PLAYER piece is above every AI piece (there can be no more kill moves)
            if (numberOfMoves == 0 || PLAYER_minY >= AI_maxY)
            {
                //Check who has more pieces and say that they are the victor
                if (PLAYER_Squares > AI_Squares) { return new Tuple<bool, int>(true, PLAYER); }
                if (AI_Squares > PLAYER_Squares) { return new Tuple<bool, int>(true, AI); }
                return new Tuple<bool, int>(true, NEUTRAL);
            }
            
            //If none of the previous cases are true, then the game isn't over.
            return new Tuple<bool, int>(false, NEUTRAL);

        }
    }
    class GamePiece //Game pieces that are stored on the board.
    {
        private int sentiment;
        private int xPosition;
        private int yPosition;
        public GamePiece(int p_sentiment, int p_xPosition, int p_yPosition)
        {
            sentiment = p_sentiment;
            xPosition = p_xPosition;
            yPosition = p_yPosition;
        }
        public int Sentiment
        {
            get { return sentiment; }
            set { sentiment = value; }
        }
        public int xCoordinate
        {
            get { return xPosition; }
        }
        public int yCoordinate
        {
            get { return yPosition; }
        }
    }
}
