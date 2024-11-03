using ChessGame;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;

internal class Pawn : Piece
{
    public Pawn(Player owner, (int Row, int Col) position) : base(owner, position)
    {
    }

    public override List<(int Row, int Col)> GetValidMoves(Button[,] board)
    {
        List<(int Row, int Col)> validMoves = new List<(int Row, int Col)>();
        int direction = Owner == Player.White ? -1 : 1;

        // Standard one-square move forward
        int newRow = Position.Row + direction;
        if (newRow >= 0 && newRow < 8 && board[newRow, Position.Col].Content == null)
        {
            validMoves.Add((newRow, Position.Col));

            // Two-square move from starting position only if it hasn't moved before
            if (!HasMoved && board[newRow + direction, Position.Col].Content == null)
            {
                validMoves.Add((newRow + direction, Position.Col));
            }
        }

        // Capture moves
        int[] captureCols = { Position.Col - 1, Position.Col + 1 };
        foreach (int newCol in captureCols)
        {
            if (newCol >= 0 && newCol < 8)
            {
                Button targetSquare = board[newRow, newCol];
                if (targetSquare.Tag is Piece targetPiece && targetPiece.Owner != Owner)
                {
                    validMoves.Add((newRow, newCol));
                }
            }
        }

        return validMoves;
    }

    // Override MarkAsMoved to set HasMoved to true after the first move
    public override void MarkAsMoved()
    {
        base.MarkAsMoved();
    }
}
