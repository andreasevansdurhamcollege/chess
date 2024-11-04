using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    internal class Bishop : Piece
    {
        public Bishop(Player owner, (int Row, int Col) position) : base(owner, position)
        {
        }

        // Updated GetValidMoves to include ChessLogic parameter
        public override List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic)
        {
            List<(int Row, int Col)> validMoves = new List<(int Row, int Col)>();

            // Diagonal moves in all four directions
            AddMovesInDirection(board, validMoves, -1, -1); // Top-left
            AddMovesInDirection(board, validMoves, -1, 1);  // Top-right
            AddMovesInDirection(board, validMoves, 1, -1);  // Bottom-left
            AddMovesInDirection(board, validMoves, 1, 1);   // Bottom-right

            return validMoves;
        }

        // Helper method to add moves in a specific diagonal direction until an obstacle is encountered
        private void AddMovesInDirection(Button[,] board, List<(int Row, int Col)> validMoves, int rowOffset, int colOffset)
        {
            int currentRow = Position.Row;
            int currentCol = Position.Col;

            while (true)
            {
                currentRow += rowOffset;
                currentCol += colOffset;

                // Stop if we move out of bounds
                if (currentRow < 0 || currentRow >= 8 || currentCol < 0 || currentCol >= 8)
                    break;

                // Check the contents of the square
                if (board[currentRow, currentCol].Content == null)
                {
                    // Empty square
                    validMoves.Add((currentRow, currentCol));
                }
                else if (board[currentRow, currentCol].Tag is Piece piece && piece.Owner != Owner)
                {
                    // Square occupied by opponent's piece
                    validMoves.Add((currentRow, currentCol));
                    break; // Block further moves in this direction
                }
                else
                {
                    // Square occupied by player's own piece
                    break; // Block further moves in this direction
                }
            }
        }

        // Method to check if this bishop can attack a specific square (used for threat detection)
        public override bool CanAttackSquare((int Row, int Col) square, Button[,] board)
        {
            int targetRow = square.Row;
            int targetCol = square.Col;

            // Check if the target square is on a diagonal
            if (System.Math.Abs(targetRow - Position.Row) == System.Math.Abs(targetCol - Position.Col))
            {
                // Determine the direction of movement
                int rowOffset = targetRow > Position.Row ? 1 : -1;
                int colOffset = targetCol > Position.Col ? 1 : -1;

                // Traverse in the direction until we reach the target square or encounter a piece
                int currentRow = Position.Row + rowOffset;
                int currentCol = Position.Col + colOffset;

                while (currentRow != targetRow || currentCol != targetCol)
                {
                    // Stop if there's an obstacle in the path
                    if (board[currentRow, currentCol].Tag is Piece)
                        return false;

                    currentRow += rowOffset;
                    currentCol += colOffset;
                }
                return true; // The bishop can attack the target square
            }
            return false; // The square is not in line with the bishop's movement
        }
    }
}
