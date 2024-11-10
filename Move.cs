using System.Windows.Controls;

namespace ChessGame
{
    public class Move
    {
        public Button FromSquare { get; set; }
        public Button ToSquare { get; set; }
        public Piece MovedPiece { get; set; }
        public Piece CapturedPiece { get; set; }
        public bool IsEnPassant { get; set; }
        public bool IsCastling { get; set; }
        public string PromotionPiece { get; set; }

        public Move(Button from, Button to, Piece moved, Piece captured = null, bool enPassant = false, bool castling = false, string promotion = null)
        {
            FromSquare = from;
            ToSquare = to;
            MovedPiece = moved;
            CapturedPiece = captured;
            IsEnPassant = enPassant;
            IsCastling = castling;
            PromotionPiece = promotion;
        }
    }
}
