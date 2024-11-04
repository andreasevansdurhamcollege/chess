using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    public class ChessLogic
    {
        private Button[,] board;
        public Piece LastMovedPiece { get; set; }

        public ChessLogic(Button[,] board)
        {
            this.board = board;
        }

        public bool IsSquareThreatened((int Row, int Col) square, Player kingOwner)
        {
            // Logic to check if a square is threatened
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    Button button = board[row, col];
                    if (button.Tag is Piece piece && piece.Owner != kingOwner)
                    {
                        if (piece.CanAttackSquare(square, board))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsInCheck(Player player)
        {
            (int Row, int Col)? kingPosition = null;

            // Locate the king
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col].Tag is King king && king.Owner == player)
                    {
                        kingPosition = (row, col);
                        break;
                    }
                }
            }

            // Check if the king's position is under threat
            return kingPosition.HasValue && IsSquareThreatened(kingPosition.Value, player);
        }

        public bool IsCheckmate(Player player)
        {
            if (!IsInCheck(player)) return false;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button button = board[row, col];
                    if (button.Tag is Piece piece && piece.Owner == player)
                    {
                        // Pass 'this' to GetValidMoves as ChessLogic instance
                        if (piece.GetValidMoves(board, this).Count > 0)
                        {
                            return false; // Player can still make a valid move
                        }
                    }
                }
            }
            return true; // No valid moves available
        }
    }
}
