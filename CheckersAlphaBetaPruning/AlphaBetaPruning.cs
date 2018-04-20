using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersAlphaBetaPruning
{
    class AlphaBetaPruning
    {
        //Possible Sentinents of Pieces
        private const int AI = -1;
        private const int NEUTRAL = 0;
        private const int PLAYER = 1;

        private readonly int MAX_DEPTH; //Max Recursion Depth
        private Tuple<Tuple<int, int>, Tuple<int, int>> returnedMove; //Move that is returned to Game.cs via determineNextMove()

        //Statistics
        private int _nodesGenerated = 1;  //Nodes generated (initialized to 1 because of root)
        private int _maxDepthReached = 0; //Max depth reached
        private int _maxValuePruning = 0; //Number of times Max_Value pruned
        private int _minValuePruning = 0; //Number of times Min_Value pruned

        //Public List of the statistics for display in table (Game.cs)
        public List<int> Statistics
        {
            get
            {
                List<int> ret = new List<int>();
                ret.Add(_maxDepthReached);
                ret.Add(_nodesGenerated);
                ret.Add(_maxValuePruning);
                ret.Add(_minValuePruning);
                return ret;
            }
        }

        //Constructor for class. Initializes the max depth allowed.
        public AlphaBetaPruning(int depth)
        {
            MAX_DEPTH = depth;
        }

        //Function that generates statistics as well as determines the next move to be returned to player
        public Tuple<Tuple<int,int>, Tuple<int, int>> determineNextMove(List<List<GamePiece>> board)
        {
            //reset statistics
            _nodesGenerated = 1;
            _maxDepthReached = 0;
            _maxValuePruning = 0;
            _minValuePruning = 0;

            //copy the board
            List<List<GamePiece>> tempBoard = Game.CopyBoard(board);
            
            //set returnedMove to  dummy moves
            returnedMove = new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(-1, -1), new Tuple<int, int>(-1, -1));

            //if no available moves... return
            if (Game.ValidMoves(tempBoard,AI).Count == 0) { return returnedMove; }
            
            //otherwise find the best option
            Min_Value(tempBoard, AI, PLAYER, MAX_DEPTH, true);
            return returnedMove;
        }
        
        //Determines the best move for the PLAYER and returns the best value
        private int Max_Value(List<List<GamePiece>> board, int p_alpha, int p_beta, int levelNumber)
        {
            _maxDepthReached = (_maxDepthReached < MAX_DEPTH - levelNumber ? MAX_DEPTH - levelNumber : _maxDepthReached);  //update max depth reached if current depth is lower
            if(levelNumber == 0) //if we've reached the max depth
            {
                return CutOff_Function(board); //return value of cutoff function
            }

            //create local copies of the passed in alpha & beta
            int alpha = p_alpha;
            int beta = p_beta;

            //set value to minimum value (AI wins)
            int v = AI;

            Tuple<bool, int> results = Game.Terminal_Test(board); //check if terminal
            if (results.Item1)
            {
                return results.Item2;
            }

            List<List<GamePiece>> tempBoard = Game.CopyBoard(board); //create local copy of the board
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> validMoves = Game.ValidMoves(board, PLAYER); //get all of the valid moves for the PLAYER
            foreach(Tuple<Tuple<int,int>, Tuple<int,int>> move in validMoves)
            {
                tempBoard = Game.ApplyMove(board, move); //apply the move
                v = Math.Max(v, Min_Value(tempBoard, alpha, beta, levelNumber-1)); //get the value from Min_Value function
                _nodesGenerated++; //increment the number of nodes generated (this move counts as a node)
                if(v >= beta) { //when you break here that means that we've pruned
                    _maxValuePruning++;
                    return v;
                }
                alpha = Math.Max(alpha, v); //set alpha to the new value
            }
            if(validMoves.Count == 0) //if no moves are available then we should run the original board with the AI making moves
            {
                v = Math.Max(v, Min_Value(tempBoard, alpha, beta, levelNumber - 1));
            }
            return v;
        }

        //Determines the best move for the AI and returns the best value
        private int Min_Value(List<List<GamePiece>> board, int p_alpha, int p_beta, int levelNumber, bool updateMove = false)
        {
            _maxDepthReached = (_maxDepthReached < MAX_DEPTH - levelNumber ? MAX_DEPTH - levelNumber : _maxDepthReached); //update maximum depth reached
            if (levelNumber == 0) //if recursion depth is reached, return the cutoff function's value
            {
                return CutOff_Function(board);
            }
            //initialize alpha and beta to the values that are passed in
            int alpha = p_alpha;
            int beta = p_beta;
            //assign v to the maximum value in the game universe (PLAYER wins)
            int v = PLAYER;

            Tuple<Tuple<int, int>, Tuple<int, int>> MinUtilityMove = new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(-1,1), new Tuple<int, int>(-1,-1)); //variable that stores the best move for returning to Game.cs
            Tuple<bool, int> results = Game.Terminal_Test(board); //check if the game is over
            if (results.Item1) //if so then return the results
            {
                return results.Item2;
            }
            List<List<GamePiece>> tempBoard = Game.CopyBoard(board); //make a local copy of the game board
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> validMoves = Game.ValidMoves(board, AI);
            foreach (Tuple<Tuple<int, int>, Tuple<int, int>> move in validMoves) //for each of the valid moves...
            {
                tempBoard = Game.ApplyMove(board, move); //apply the move
                int resultOfMax = Max_Value(tempBoard, alpha, beta, levelNumber - 1); //Perform the Max_Value function on the new board
                if (v > resultOfMax)
                {
                    v = resultOfMax; //Set the new value of max
                    MinUtilityMove = move; //Set the best move to the current one
                }
                _nodesGenerated++; //increase the number of nodes generated
                if (v <= alpha) {
                    _minValuePruning++; //if the program prunes, increment the counter
                    break;
                }

                beta = Math.Min(beta, v); //set a new value of beta
            }
            if(validMoves.Count == 0) //if there are no available moves, set the min utility move to the dummy move
            {
                v = Math.Min(v, Max_Value(board, alpha, beta, levelNumber - 1));
                Tuple<int, int> dummyCoordinate = new Tuple<int, int>(-1, -1);
                MinUtilityMove = new Tuple<Tuple<int, int>, Tuple<int, int>>(dummyCoordinate, dummyCoordinate);
            }
            else
            {
                if (v == PLAYER) { //if the best result from any of the moves (from the computer's standpoint) is a loss
                    Random rnd = new Random();
                    MinUtilityMove = validMoves[rnd.Next(0, validMoves.Count)]; //pick a random move thats possible since it doesn't matter which is chosen
                }
            }
            if (updateMove) //If we are to return a move (highest level call of Min_Value)
            {
                returnedMove = MinUtilityMove; //then set the returnedMove to the best move.
            }
            return v;
        }

        //Determines a value for a given board. Used for when the recursion depth is met.
        private int CutOff_Function(List<List<GamePiece>> board)
        {
            int boardSize = board.Count; //Size of board
            int AI_Squares = 0; //Number of AI pieces
            int PLAYER_Squares = 0; //Number of PLAYER pieces

            //Count how many AI & PLAYER pieces are there
            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    if (board[y][x].Sentiment == AI) { AI_Squares++; }
                    if (board[y][x].Sentiment == PLAYER) { PLAYER_Squares++; }
                }
            }

            //return the victor is whoever has more squares or a tie
            if (PLAYER_Squares > AI_Squares) { return PLAYER; }
            if (AI_Squares > PLAYER_Squares) { return AI; }
            else { return NEUTRAL; }

        }       
    }
}
