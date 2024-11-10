using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    public class Knight : Piece
    {
        public Knight(Player owner, (int Row, int Col) position) : base(owner, position)
        {
        }

        public override List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic, bool ignoreKingSafety = false)
        {
            List<(int Row, int Col)> moves = new List<(int Row, int Col)>();

            int[] dRows = { -2, -1, 1, 2, 2, 1, -1, -2 };
            int[] dCols = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = Position.Row + dRows[i];
                int newCol = Position.Col + dCols[i];

                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                {
                    Button targetSquare = board[newRow, newCol];

                    if (targetSquare.Tag == null || ((Piece)targetSquare.Tag).Owner != Owner)
                    {
                        moves.Add((newRow, newCol));
                    }
                }
            }

            return moves;
        }
    }
}
