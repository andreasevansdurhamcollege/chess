using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    internal class King : Piece
    {
        public King(Player owner, (int Row, int Col) position) : base(owner, position)
        {
        }

        // Updated GetValidMoves to include ChessLogic parameter
        public override List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic)
        {
            List<(int Row, int Col)> validMoves = new List<(int Row, int Col)>();

            // Normal one-square moves
            int[,] moves = {
                { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 },
                { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
            };

            for (int i = 0; i < moves.GetLength(0); i++)
            {
                int newRow = Position.Row + moves[i, 0];
                int newCol = Position.Col + moves[i, 1];

                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                {
                    Button targetSquare = board[newRow, newCol];
                    Piece targetPiece = targetSquare.Tag as Piece;

                    // Allow move if square is empty or contains an opponent's piece
                    if (targetPiece == null || targetPiece.Owner != Owner)
                    {
                        // Temporarily move the king to the new position to check for safety
                        var originalPosition = Position;
                        var originalTarget = targetSquare.Tag;
                        Position = (newRow, newCol);
                        targetSquare.Tag = this;

                        bool isSafe = !chessLogic.IsSquareThreatened((newRow, newCol), Owner);

                        // Restore the original position and state
                        Position = originalPosition;
                        targetSquare.Tag = originalTarget;

                        // Add move if safe, or if capturing an unprotected piece
                        if (isSafe || (targetPiece != null && targetPiece.Owner != Owner && !chessLogic.IsSquareThreatened((newRow, newCol), targetPiece.Owner)))
                        {
                            validMoves.Add((newRow, newCol));
                        }
                    }
                }
            }

            // Castling
            if (!HasMoved && !chessLogic.IsInCheck(Owner))
            {
                // Kingside castling (short)
                if (CanCastle(board, chessLogic, true))
                {
                    validMoves.Add((Position.Row, Position.Col + 2));
                }
                // Queenside castling (long)
                if (CanCastle(board, chessLogic, false))
                {
                    validMoves.Add((Position.Row, Position.Col - 2));
                }
            }

            return validMoves;
        }

        // Castling validation method
        private bool CanCastle(Button[,] board, ChessLogic chessLogic, bool isKingside)
        {
            int row = Position.Row;
            int rookCol = isKingside ? 7 : 0;   // Rook starting column
            int direction = isKingside ? 1 : -1; // Direction of castling (right for kingside, left for queenside)

            // Check if the rook hasn't moved and is still at its starting position
            if (board[row, rookCol].Tag is Rook rook && !rook.HasMoved)
            {
                // Check if all squares between king and rook are empty
                for (int col = Position.Col + direction; col != rookCol; col += direction)
                {
                    if (board[row, col].Content != null) return false; // A piece is blocking castling
                }

                // Check if each square the king would pass through is not threatened
                for (int col = Position.Col; col != Position.Col + 2 * direction; col += direction)
                {
                    if (chessLogic.IsSquareThreatened((row, col), Owner)) return false; // Square is threatened
                }

                // Also check the destination square for the king
                int destinationCol = Position.Col + 2 * direction;
                if (chessLogic.IsSquareThreatened((row, destinationCol), Owner)) return false;

                return true; // All conditions for castling are met
            }

            return false; // Castling is not allowed
        }

        // CanAttackSquare method for check detection
        public override bool CanAttackSquare((int Row, int Col) square, Button[,] board)
        {
            int[,] moves = {
                { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 },
                { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }
            };

            for (int i = 0; i < moves.GetLength(0); i++)
            {
                int targetRow = Position.Row + moves[i, 0];
                int targetCol = Position.Col + moves[i, 1];

                if (square == (targetRow, targetCol))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
