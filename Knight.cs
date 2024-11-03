using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    internal class Knight : Piece
    {
        public Knight(Player owner, (int Row, int Col) position) : base(owner, position)
        {
        }

        // Method to get all valid moves for the knight
        public override List<(int Row, int Col)> GetValidMoves(Button[,] board)
        {
            List<(int Row, int Col)> validMoves = new List<(int Row, int Col)>();

            // Define all possible L-shaped moves for the knight
            int[,] moves = {
                { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
                { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
            };

            for (int i = 0; i < moves.GetLength(0); i++)
            {
                int newRow = Position.Row + moves[i, 0];
                int newCol = Position.Col + moves[i, 1];

                // Ensure the move is within bounds
                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                {
                    Button targetSquare = board[newRow, newCol];

                    // Check if the square is empty or occupied by an opponent
                    if (targetSquare.Content == null || (targetSquare.Tag is Piece piece && piece.Owner != Owner))
                    {
                        validMoves.Add((newRow, newCol));
                    }
                }
            }

            return validMoves;
        }

        // Method to check if this knight can attack a specific square (used for threat detection)
        public override bool CanAttackSquare((int Row, int Col) square, Button[,] board)
        {
            // Define all possible L-shaped moves for the knight
            int[,] moves = {
                { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
                { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
            };

            // Check if the target square matches any of the knight's possible moves
            for (int i = 0; i < moves.GetLength(0); i++)
            {
                int targetRow = Position.Row + moves[i, 0];
                int targetCol = Position.Col + moves[i, 1];

                if (square == (targetRow, targetCol))
                {
                    return true;
                }
            }

            return false; // The knight cannot attack the target square
        }
    }
}
