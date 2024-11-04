using ChessGame;
using System.Windows.Controls;
using System.Collections.Generic;

internal class Pawn : Piece
{
    public bool HasJustMovedTwoSquares { get; set; }
    public Pawn(Player owner, (int Row, int Col) position) : base(owner, position)
    {
    }

    // Updated GetValidMoves to include ChessLogic parameter
    public override List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic)
    {
        List<(int Row, int Col)> validMoves = new List<(int Row, int Col)>();
        int direction = Owner == Player.White ? -1 : 1;

        int currentRow = Position.Row;
        int currentCol = Position.Col;

        // Standard one-square move forward
        int newRow = currentRow + direction;
        if (IsWithinBounds(newRow, currentCol) && board[newRow, currentCol].Tag == null)
        {
            validMoves.Add((newRow, currentCol));

            // Two-square move from starting position
            if (!HasMoved)
            {
                int twoRow = currentRow + 2 * direction;
                if (IsWithinBounds(twoRow, currentCol) && board[twoRow, currentCol].Tag == null)
                {
                    validMoves.Add((twoRow, currentCol));
                }
            }
        }

        // Capture moves including en passant
        int[] captureCols = { currentCol - 1, currentCol + 1 };
        foreach (int col in captureCols)
        {
            if (IsWithinBounds(newRow, col))
            {
                Button targetSquare = board[newRow, col];

                // Normal capture
                if (targetSquare.Tag is Piece targetPiece && targetPiece.Owner != Owner)
                {
                    validMoves.Add((newRow, col));
                }
                // En passant capture
                else if (board[currentRow, col].Tag is Pawn adjacentPawn &&
                         adjacentPawn.Owner != Owner &&
                         adjacentPawn == chessLogic.LastMovedPiece &&
                         adjacentPawn.HasJustMovedTwoSquares)
                {
                    validMoves.Add((newRow, col));
                }
            }
        }

        return validMoves;
    }

    private bool IsWithinBounds(int row, int col)
    {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }






    // Override MarkAsMoved to set HasMoved to true after the first move
    public override void MarkAsMoved()
    {
        base.MarkAsMoved();
    }
}
