using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessGame
{
    public enum Player
    {
        White,
        Black
    }

    public abstract class Piece
    {
        public Player Owner { get; set; }
        public (int Row, int Col) Position { get; set; }
        public bool HasMoved { get; private set; }

        public Piece(Player owner, (int Row, int Col) position)
        {
            Owner = owner;
            Position = position;
            HasMoved = false;
        }

        /// <summary>
        /// Marks the piece as having moved.
        /// </summary>
        public virtual void MarkAsMoved()
        {
            HasMoved = true;
        }

        /// <summary>
        /// Gets the list of valid moves for the piece.
        /// </summary>
        public abstract List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic, bool ignoreKingSafety = false);
    }
}
