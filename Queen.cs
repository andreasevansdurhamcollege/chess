using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    public class Queen : Piece
    {
        public Queen(Player owner, (int Row, int Col) position) : base(owner, position)
        {
        }

        public override List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic, bool ignoreKingSafety = false)
        {
            List<(int Row, int Col)> moves = new List<(int Row, int Col)>();

            // Directions: all eight directions
            int[] dRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dCols = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int direction = 0; direction < 8; direction++)
            {
                int newRow = Position.Row;
                int newCol = Position.Col;

                while (true)
                {
                    newRow += dRows[direction];
                    newCol += dCols[direction];

                    if (newRow < 0 || newRow >= 8 || newCol < 0 || newCol >= 8)
                        break;

                    Button targetSquare = board[newRow, newCol];

                    if (targetSquare.Tag == null)
                    {
                        moves.Add((newRow, newCol));
                    }
                    else
                    {
                        Piece targetPiece = (Piece)targetSquare.Tag;
                        if (targetPiece.Owner != Owner)
                        {
                            moves.Add((newRow, newCol));
                        }
                        break;
                    }
                }
            }

            return moves;
        }
    }
}
