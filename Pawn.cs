using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    public class Pawn : Piece
    {
        public bool HasJustMovedTwoSquares { get; set; }

        public Pawn(Player owner, (int Row, int Col) position) : base(owner, position)
        {
            HasJustMovedTwoSquares = false;
        }

        public override List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic, bool ignoreKingSafety = false)
        {
            List<(int Row, int Col)> moves = new List<(int Row, int Col)>();

            int direction = Owner == Player.White ? -1 : 1;
            int startRow = Owner == Player.White ? 6 : 1;

            int newRow = Position.Row + direction;
            int col = Position.Col;

            // Forward move
            if (newRow >= 0 && newRow < 8)
            {
                if (board[newRow, col].Tag == null)
                {
                    moves.Add((newRow, col));

                    // Double move from starting position
                    if (Position.Row == startRow)
                    {
                        int doubleNewRow = newRow + direction;
                        if (board[doubleNewRow, col].Tag == null)
                        {
                            moves.Add((doubleNewRow, col));
                        }
                    }
                }

                // Captures
                for (int dCol = -1; dCol <= 1; dCol += 2)
                {
                    int newCol = col + dCol;
                    if (newCol >= 0 && newCol < 8)
                    {
                        Button targetSquare = board[newRow, newCol];

                        if (targetSquare.Tag is Piece targetPiece)
                        {
                            if (targetPiece.Owner != Owner)
                            {
                                moves.Add((newRow, newCol));
                            }
                        }
                        else if (!ignoreKingSafety)
                        {
                            // En passant
                            int capturedPawnRow = Position.Row;
                            int capturedPawnCol = newCol;
                            Button capturedPawnSquare = board[capturedPawnRow, capturedPawnCol];
                            if (capturedPawnSquare.Tag is Pawn capturedPawn && capturedPawn.Owner != Owner && capturedPawn.HasJustMovedTwoSquares)
                            {
                                moves.Add((newRow, newCol));
                            }
                        }
                    }
                }
            }

            return moves;
        }
    }
}
