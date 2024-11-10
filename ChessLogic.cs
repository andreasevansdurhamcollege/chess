using System;
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

        /// <summary>
        /// Checks if the specified player is in check.
        /// </summary>
        public bool IsInCheck(Player player)
        {
            // Find the player's king
            King king = null;
            foreach (var btn in board)
            {
                if (btn.Tag is King k && k.Owner == player)
                {
                    king = k;
                    break;
                }
            }

            if (king == null)
                return false; // No king found, should not happen

            return IsSquareThreatened(king.Position.Row, king.Position.Col, player);
        }

        /// <summary>
        /// Checks if the specified player is in checkmate.
        /// </summary>
        public bool IsCheckmate(Player player)
        {
            if (!IsInCheck(player))
                return false;

            // Iterate through all pieces of the player
            foreach (var btn in board)
            {
                if (btn.Tag is Piece piece && piece.Owner == player)
                {
                    var validMoves = piece.GetValidMoves(board, this);
                    foreach (var move in validMoves)
                    {
                        if (IsMoveLegal(piece, move))
                        {
                            return false; // Found a legal move, not checkmate
                        }
                    }
                }
            }

            return true; // No legal moves, checkmate
        }

        /// <summary>
        /// Checks if the specified player is in stalemate.
        /// </summary>
        public bool IsStalemate(Player player)
        {
            if (IsInCheck(player))
                return false; // Not stalemate if in check

            // Iterate through all pieces of the player
            foreach (var btn in board)
            {
                if (btn.Tag is Piece piece && piece.Owner == player)
                {
                    var validMoves = piece.GetValidMoves(board, this);
                    foreach (var move in validMoves)
                    {
                        if (IsMoveLegal(piece, move))
                        {
                            return false; // Found a legal move, not stalemate
                        }
                    }
                }
            }

            return true; // No legal moves, stalemate
        }

        /// <summary>
        /// Determines if a move is legal (does not leave the king in check).
        /// </summary>
        public bool IsMoveLegal(Piece piece, (int Row, int Col) move)
        {
            int originalRow = piece.Position.Row;
            int originalCol = piece.Position.Col;
            int moveRow = move.Row;
            int moveCol = move.Col;

            Button fromSquare = board[originalRow, originalCol];
            Button toSquare = board[moveRow, moveCol];

            // Save original state
            var originalFromContent = fromSquare.Content;
            var originalFromTag = fromSquare.Tag;
            var originalToContent = toSquare.Content;
            var originalToTag = toSquare.Tag;
            var originalPosition = piece.Position;

            // Simulate the move
            fromSquare.Content = null;
            fromSquare.Tag = null;
            toSquare.Content = originalFromContent;
            toSquare.Tag = originalFromTag;
            piece.Position = (moveRow, moveCol);

            // Check if the king is still in check after the move
            bool isKingInCheck = IsInCheck(piece.Owner);

            // Revert the move
            piece.Position = originalPosition;
            fromSquare.Content = originalFromContent;
            fromSquare.Tag = originalFromTag;
            toSquare.Content = originalToContent;
            toSquare.Tag = originalToTag;

            return !isKingInCheck;
        }

        /// <summary>
        /// Determines if a given square is threatened by the opponent.
        /// </summary>
        public bool IsSquareThreatened(int row, int col, Player player)
        {
            Player opponent = player == Player.White ? Player.Black : Player.White;

            foreach (var btn in board)
            {
                if (btn.Tag is Piece piece && piece.Owner == opponent)
                {
                    var moves = piece.GetValidMoves(board, this, ignoreKingSafety: true);
                    foreach (var move in moves)
                    {
                        if (move.Row == row && move.Col == col)
                        {
                            // For pawns, ensure they can actually capture on that square
                            if (piece is Pawn pawn)
                            {
                                int direction = opponent == Player.White ? -1 : 1;
                                int pawnRow = piece.Position.Row;
                                int pawnCol = piece.Position.Col;
                                if (pawnRow + direction == row && Math.Abs(pawnCol - col) == 1)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
