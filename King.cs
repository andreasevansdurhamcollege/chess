using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    public class King : Piece
    {
        public King(Player owner, (int Row, int Col) position) : base(owner, position)
        {
        }

        /// <summary>
        /// Gets the list of valid moves for the king.
        /// </summary>
        public override List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic, bool ignoreKingSafety = false)
        {
            List<(int Row, int Col)> moves = new List<(int Row, int Col)>();

            // Define all possible king moves
            int[] dRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dCols = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = Position.Row + dRows[i];
                int newCol = Position.Col + dCols[i];

                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                {
                    Button targetSquare = board[newRow, newCol];

                    if (targetSquare.Tag == null || ((Piece)targetSquare.Tag).Owner != Owner)
                    {
                        // Check if the square is not threatened
                        if (ignoreKingSafety || !chessLogic.IsSquareThreatened(newRow, newCol, Owner))
                        {
                            moves.Add((newRow, newCol));
                        }
                    }
                }
            }

            // Castling logic (if not ignoring king safety)
            if (!ignoreKingSafety && !HasMoved && !chessLogic.IsInCheck(Owner))
            {
                // Kingside castling
                if (CanCastleKingside(board, chessLogic))
                {
                    moves.Add((Position.Row, Position.Col + 2));
                }

                // Queenside castling
                if (CanCastleQueenside(board, chessLogic))
                {
                    moves.Add((Position.Row, Position.Col - 2));
                }
            }

            return moves;
        }

        private bool CanCastleKingside(Button[,] board, ChessLogic chessLogic)
        {
            int row = Position.Row;
            // Ensure rook is present and hasn't moved
            if (board[row, 7].Tag is Rook rook && !rook.HasMoved && rook.Owner == Owner)
            {
                // Check if squares between king and rook are empty
                if (board[row, 5].Tag == null && board[row, 6].Tag == null)
                {
                    // Ensure squares the king moves through are not threatened
                    if (!chessLogic.IsSquareThreatened(row, 5, Owner) &&
                        !chessLogic.IsSquareThreatened(row, 6, Owner))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CanCastleQueenside(Button[,] board, ChessLogic chessLogic)
        {
            int row = Position.Row;
            // Ensure rook is present and hasn't moved
            if (board[row, 0].Tag is Rook rook && !rook.HasMoved && rook.Owner == Owner)
            {
                // Check if squares between king and rook are empty
                if (board[row, 1].Tag == null && board[row, 2].Tag == null && board[row, 3].Tag == null)
                {
                    // Ensure squares the king moves through are not threatened
                    if (!chessLogic.IsSquareThreatened(row, 2, Owner) &&
                        !chessLogic.IsSquareThreatened(row, 3, Owner))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Marks the king as moved.
        /// </summary>
        public override void MarkAsMoved()
        {
            base.MarkAsMoved();
            // Additional logic if needed
        }
    }
}
