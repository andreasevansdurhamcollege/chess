using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class Move
    {
        public Piece PieceMoved { get; set; }
        public (int Row, int Col) StartPosition { get; set; }
        public (int Row, int Col) EndPosition { get; set; }

        public Move(Piece piece, (int Row, int Col) startPosition, (int Row, int Col) endPosition)
        {
            PieceMoved = piece;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}
