# Alpha-Beta-Pruning-Checkers

This repository contains a simplified form of the game of Checkers in which a player plays against a computer player.
This code was written by Ravi Sinha for CS-UY 4613/6613: Artifical Intelligence at New York University's Tandon School of Engineering.

## Project Requirements
The game consists of red and black squares that represent the Computer Player's, and human player's pieces, respectively. The board used is the 6x6 board. The human player can choose to either go first or second and will choose a piece to move diagonally left or right. 

### Game Rules
There are two types of move in this form of checkers: Regular moves and Capture moves.
- Regular moves are moves that are diagonal left or right one space. 
- Capture moves are moves in which there a piece can jump (diagonally) over an enemy piece into an empty position 2 spots away. Unlike in regular checkers, no additional jumps can be made. If a capture move is possible, the player is forced to take it. 
For both of these types of moves, the direction of movement must be towards the opposite side of the board as the piece originally started on. For the human player, the pieces move towards the top of the screen and for the computer player the piece moves towards the bottom of the screen.

If there are no available moves for a player, their turn is forfeit and the opponent goes.
Unlike in real checkers, if a piece reaches the end of the board, it does not become a king. In this case, the piece can no longer have any more movement.

The winner is determined when a player's opponent has no more pieces remaining. However, in the case where both players have pieces remaining and there are no more possible moves for either player, the player with more pieces is the winner. However, if there are an equal amount of pieces for both players at the end of the game, the game results in a tie.

### Implementation Requirements
For this program, the computer player must use [Alpha-Beta Pruning](https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning) to determine the best move in a certain situation. For this specific implementation, the utility of the computer winning is -1, human player winning is 1 and a neutral game is 0. The computer player acts as the minimum player in the algorithm. 

The program must output the number of nodes generated, maximum depth of search tree reached, number of times the Max_Value function pruned, and number of times the Min_Value function pruned. In this implementation, the values are all displayed on a table below the gameboard. 

Finally, the computer player must determine the best move in 15 seconds or less. Due to the number of possible cases, a recursion depth must be set and once the recursion depth is met, a cut off function would be used to evaluate the board's state and return a utility value. In this implementation, the cutoff function is whoever has more pieces on the board.
