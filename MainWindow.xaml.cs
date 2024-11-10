using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessGame
{
    public partial class MainWindow : Window
    {
        private Button[,] buttonArray = new Button[8, 8];
        private ChessLogic chessLogic;
        private Button selectedSquare = null;
        private List<Button> highlightedSquares = new List<Button>();
        private Player currentPlayer = Player.White;
        private Stack<Move> moveHistory = new Stack<Move>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeChessBoard();
            InitializePieces();
            chessLogic = new ChessLogic(buttonArray);
        }

        private void InitializeChessBoard()
        {
            // Create buttons for each square
            for (int row = 0; row < 8; row++)
            {
                ChessBoardGrid.RowDefinitions.Add(new RowDefinition());
                ChessBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button btn = new Button
                    {
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                        Background = (row + col) % 2 == 0 ? Brushes.White : Brushes.Gray
                    };
                    btn.Click += Square_Click;

                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, col);
                    ChessBoardGrid.Children.Add(btn);
                    buttonArray[row, col] = btn;
                }
            }
        }

        private void InitializePieces()
        {
            // Initialize White pieces
            // Pawns
            for (int col = 0; col < 8; col++)
            {
                Pawn whitePawn = new Pawn(Player.White, (6, col));
                buttonArray[6, col].Tag = whitePawn;
                buttonArray[6, col].Content = GetImageForPiece(whitePawn);
            }

            // Rooks
            Rook whiteRook1 = new Rook(Player.White, (7, 0));
            buttonArray[7, 0].Tag = whiteRook1;
            buttonArray[7, 0].Content = GetImageForPiece(whiteRook1);

            Rook whiteRook2 = new Rook(Player.White, (7, 7));
            buttonArray[7, 7].Tag = whiteRook2;
            buttonArray[7, 7].Content = GetImageForPiece(whiteRook2);

            // Knights
            Knight whiteKnight1 = new Knight(Player.White, (7, 1));
            buttonArray[7, 1].Tag = whiteKnight1;
            buttonArray[7, 1].Content = GetImageForPiece(whiteKnight1);

            Knight whiteKnight2 = new Knight(Player.White, (7, 6));
            buttonArray[7, 6].Tag = whiteKnight2;
            buttonArray[7, 6].Content = GetImageForPiece(whiteKnight2);

            // Bishops
            Bishop whiteBishop1 = new Bishop(Player.White, (7, 2));
            buttonArray[7, 2].Tag = whiteBishop1;
            buttonArray[7, 2].Content = GetImageForPiece(whiteBishop1);

            Bishop whiteBishop2 = new Bishop(Player.White, (7, 5));
            buttonArray[7, 5].Tag = whiteBishop2;
            buttonArray[7, 5].Content = GetImageForPiece(whiteBishop2);

            // Queen
            Queen whiteQueen = new Queen(Player.White, (7, 3));
            buttonArray[7, 3].Tag = whiteQueen;
            buttonArray[7, 3].Content = GetImageForPiece(whiteQueen);

            // King
            King whiteKing = new King(Player.White, (7, 4));
            buttonArray[7, 4].Tag = whiteKing;
            buttonArray[7, 4].Content = GetImageForPiece(whiteKing);

            // Initialize Black pieces
            // Pawns
            for (int col = 0; col < 8; col++)
            {
                Pawn blackPawn = new Pawn(Player.Black, (1, col));
                buttonArray[1, col].Tag = blackPawn;
                buttonArray[1, col].Content = GetImageForPiece(blackPawn);
            }

            // Rooks
            Rook blackRook1 = new Rook(Player.Black, (0, 0));
            buttonArray[0, 0].Tag = blackRook1;
            buttonArray[0, 0].Content = GetImageForPiece(blackRook1);

            Rook blackRook2 = new Rook(Player.Black, (0, 7));
            buttonArray[0, 7].Tag = blackRook2;
            buttonArray[0, 7].Content = GetImageForPiece(blackRook2);

            // Knights
            Knight blackKnight1 = new Knight(Player.Black, (0, 1));
            buttonArray[0, 1].Tag = blackKnight1;
            buttonArray[0, 1].Content = GetImageForPiece(blackKnight1);

            Knight blackKnight2 = new Knight(Player.Black, (0, 6));
            buttonArray[0, 6].Tag = blackKnight2;
            buttonArray[0, 6].Content = GetImageForPiece(blackKnight2);

            // Bishops
            Bishop blackBishop1 = new Bishop(Player.Black, (0, 2));
            buttonArray[0, 2].Tag = blackBishop1;
            buttonArray[0, 2].Content = GetImageForPiece(blackBishop1);

            Bishop blackBishop2 = new Bishop(Player.Black, (0, 5));
            buttonArray[0, 5].Tag = blackBishop2;
            buttonArray[0, 5].Content = GetImageForPiece(blackBishop2);

            // Queen
            Queen blackQueen = new Queen(Player.Black, (0, 3));
            buttonArray[0, 3].Tag = blackQueen;
            buttonArray[0, 3].Content = GetImageForPiece(blackQueen);

            // King
            King blackKing = new King(Player.Black, (0, 4));
            buttonArray[0, 4].Tag = blackKing;
            buttonArray[0, 4].Content = GetImageForPiece(blackKing);
        }

        private Image GetImageForPiece(Piece piece)
        {
            string color = piece.Owner == Player.White ? "white" : "black";
            string pieceName = piece.GetType().Name.ToLower();
            string imagePath = $"/Images/{color}_{pieceName}.png";

            Image img = new Image
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Relative)),
                Stretch = Stretch.Uniform
            };
            return img;
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            Button clickedSquare = sender as Button;

            // Clear all outlines before applying a new one
            ClearOutline();

            if (selectedSquare == null)
            {
                // First click: Select the square if it contains a piece of the current player
                if (clickedSquare.Tag is Piece selectedPiece && selectedPiece.Owner == currentPlayer)
                {
                    selectedSquare = clickedSquare;
                    HighlightSquare(selectedSquare, true);

                    // Highlight valid moves for the selected piece
                    var validMoves = GetLegalMoves(selectedPiece);
                    foreach (var move in validMoves)
                    {
                        Button targetSquare = buttonArray[move.Row, move.Col];
                        targetSquare.Background = Brushes.LightGreen;
                        highlightedSquares.Add(targetSquare);
                    }
                }
            }
            else
            {
                // Second click: Attempt the move
                if (highlightedSquares.Contains(clickedSquare))
                {
                    // Perform the move
                    bool moveSuccessful = MovePiece(selectedSquare, clickedSquare);

                    // Clear all highlights and outlines after the move
                    ClearHighlights();
                    ClearOutline();

                    if (moveSuccessful)
                    {
                        // Check for check or checkmate after the move
                        Player opponent = currentPlayer == Player.White ? Player.Black : Player.White;
                        bool isCheck = chessLogic.IsInCheck(opponent);
                        bool isCheckmate = chessLogic.IsCheckmate(opponent);
                        bool isStalemate = chessLogic.IsStalemate(opponent);

                        if (isCheckmate)
                        {
                            MessageBox.Show($"{currentPlayer} wins by checkmate!");
                            DisableAllSquares();
                        }
                        else if (isStalemate)
                        {
                            MessageBox.Show($"Stalemate! It's a draw.");
                            DisableAllSquares();
                        }
                        else if (isCheck)
                        {
                            MessageBox.Show($"{opponent} is in check!");
                        }

                        // Switch turns after a successful move
                        currentPlayer = opponent;
                    }
                }
                else
                {
                    ClearHighlights();
                }

                selectedSquare = null;
            }
        }

        private List<(int Row, int Col)> GetLegalMoves(Piece piece)
        {
            List<(int Row, int Col)> legalMoves = new List<(int Row, int Col)>();
            var validMoves = piece.GetValidMoves(buttonArray, chessLogic);

            foreach (var move in validMoves)
            {
                if (chessLogic.IsMoveLegal(piece, move))
                {
                    legalMoves.Add(move);
                }
            }

            return legalMoves;
        }

        private bool MovePiece(Button fromSquare, Button toSquare)
        {
            if (fromSquare.Tag is Piece movingPiece)
            {
                int fromRow = movingPiece.Position.Row;
                int fromCol = movingPiece.Position.Col;
                int toRow = Grid.GetRow(toSquare);
                int toCol = Grid.GetColumn(toSquare);

                // Ensure the move is legal
                if (!chessLogic.IsMoveLegal(movingPiece, (toRow, toCol)))
                {
                    return false;
                }

                // Prepare for special moves
                bool isCapture = false;
                bool isEnPassant = false;
                bool isCastling = false;
                string promotionPiece = null;
                Piece capturedPiece = null;

                // Clear en passant flags for all pawns
                foreach (Button btn in buttonArray)
                {
                    if (btn.Tag is Pawn pawnToReset)
                    {
                        pawnToReset.HasJustMovedTwoSquares = false;
                    }
                }

                // Handle en passant capture
                if (movingPiece is Pawn movingPawn)
                {
                    // En passant capture
                    if (fromCol != toCol && toSquare.Tag == null)
                    {
                        int capturedPawnRow = currentPlayer == Player.White ? toRow + 1 : toRow - 1;
                        int capturedPawnCol = toCol;
                        Button capturedPawnSquare = buttonArray[capturedPawnRow, capturedPawnCol];

                        if (capturedPawnSquare.Tag is Pawn capturedPawn && capturedPawn.Owner != movingPawn.Owner && capturedPawn.HasJustMovedTwoSquares)
                        {
                            capturedPiece = capturedPawn;
                            capturedPawnSquare.Content = null;
                            capturedPawnSquare.Tag = null;

                            isEnPassant = true;
                            isCapture = true;
                        }
                    }

                    // Check if the pawn moved two squares forward
                    if (Math.Abs(toRow - fromRow) == 2)
                    {
                        movingPawn.HasJustMovedTwoSquares = true;
                    }

                    // Capture detection for normal pawn captures
                    if (toSquare.Tag is Piece targetPiece && targetPiece.Owner != movingPiece.Owner)
                    {
                        isCapture = true;
                        capturedPiece = targetPiece;
                    }
                }
                else
                {
                    // For other pieces, a capture occurs if the target square has a piece
                    if (toSquare.Tag is Piece targetPiece && targetPiece.Owner != movingPiece.Owner)
                    {
                        isCapture = true;
                        capturedPiece = targetPiece;
                    }
                }

                // Handle castling
                if (movingPiece is King kingMove && Math.Abs(toCol - fromCol) == 2)
                {
                    isCastling = true;
                    bool isKingside = toCol > fromCol;
                    int rookStartCol = isKingside ? 7 : 0;
                    int rookEndCol = isKingside ? toCol - 1 : toCol + 1;

                    Button rookSquare = buttonArray[toRow, rookStartCol];
                    Button newRookSquare = buttonArray[toRow, rookEndCol];

                    if (rookSquare.Tag is Rook rook && rook.Owner == movingPiece.Owner)
                    {
                        // Move the rook
                        newRookSquare.Content = GetImageForPiece(rook);
                        newRookSquare.Tag = rook;
                        rook.Position = (toRow, rookEndCol);
                        rook.MarkAsMoved();

                        // Clear the original rook square
                        rookSquare.Content = null;
                        rookSquare.Tag = null;
                    }
                }

                // Move the piece
                toSquare.Content = GetImageForPiece(movingPiece);
                toSquare.Tag = movingPiece;

                movingPiece.Position = (toRow, toCol);
                movingPiece.MarkAsMoved();

                // Update last moved piece
                chessLogic.LastMovedPiece = movingPiece;

                // Handle promotion
                if (movingPiece is Pawn pawn && (toRow == 0 || toRow == 7))
                {
                    // Prompt the player for promotion choice (Queen, Rook, Bishop, Knight)
                    PromotionWindow promotionWindow = new PromotionWindow(movingPiece.Owner)
                    {
                        Owner = this // Sets the owner to center the dialog over the main window
                    };
                    if (promotionWindow.ShowDialog() == true)
                    {
                        string selectedPiece = promotionWindow.SelectedPiece;
                        promotionPiece = selectedPiece;
                        Piece promotedPiece = null;
                        switch (selectedPiece)
                        {
                            case "Q":
                                promotedPiece = new Queen(pawn.Owner, (toRow, toCol));
                                break;
                            case "R":
                                promotedPiece = new Rook(pawn.Owner, (toRow, toCol));
                                break;
                            case "B":
                                promotedPiece = new Bishop(pawn.Owner, (toRow, toCol));
                                break;
                            case "N":
                                promotedPiece = new Knight(pawn.Owner, (toRow, toCol));
                                break;
                            default:
                                promotedPiece = new Queen(pawn.Owner, (toRow, toCol));
                                break;
                        }

                        toSquare.Tag = promotedPiece;
                        toSquare.Content = GetImageForPiece(promotedPiece);
                    }
                    else
                    {
                        // Handle the case where the player closes the promotion window without selecting a piece
                        // For example, default to Queen
                        Piece defaultPromotedPiece = new Queen(pawn.Owner, (toRow, toCol));
                        toSquare.Tag = defaultPromotedPiece;
                        toSquare.Content = GetImageForPiece(defaultPromotedPiece);
                    }
                }

                // Record the move
                Move move = new Move(fromSquare, toSquare, movingPiece, capturedPiece, isEnPassant, isCastling, promotionPiece);
                moveHistory.Push(move);

                // Clear the original square
                fromSquare.Content = null;
                fromSquare.Tag = null;

                // Log the move
                bool isCheck = chessLogic.IsInCheck(currentPlayer == Player.White ? Player.Black : Player.White);
                string moveNotation = GetMoveNotation(fromSquare, toSquare, movingPiece, isCapture, isCheck, isEnPassant, promotionPiece);
                LogMove(moveNotation);

                return true;
            }

            return false;
        }

        private void HighlightSquare(Button square, bool highlight)
        {
            square.BorderBrush = highlight ? Brushes.Yellow : Brushes.Black;
            square.BorderThickness = highlight ? new Thickness(3) : new Thickness(1);
        }

        private void ClearOutline()
        {
            for (int row = 0; row < buttonArray.GetLength(0); row++)
            {
                for (int col = 0; col < buttonArray.GetLength(1); col++)
                {
                    Button square = buttonArray[row, col];
                    square.BorderBrush = Brushes.Black;
                    square.BorderThickness = new Thickness(1);
                }
            }
        }

        private void ClearHighlights()
        {
            foreach (var square in highlightedSquares)
            {
                int row = Grid.GetRow(square);
                int col = Grid.GetColumn(square);
                square.Background = (row + col) % 2 == 0 ? Brushes.White : Brushes.Gray;
            }
            highlightedSquares.Clear();
        }

        private void LogMove(string move)
        {
            if (currentPlayer == Player.White)
            {
                WhiteMoveListBox.Items.Add(move); // Add to White’s list
            }
            else
            {
                BlackMoveListBox.Items.Add(move); // Add to Black’s list
            }
        }

        private string GetMoveNotation(Button fromSquare, Button toSquare, Piece movingPiece, bool isCapture = false, bool isCheck = false, bool isEnPassant = false, string promotionPiece = null)
        {
            string moveNotation = "";

            // Handle castling
            if (movingPiece is King && Math.Abs(Grid.GetColumn(toSquare) - Grid.GetColumn(fromSquare)) == 2)
            {
                return Grid.GetColumn(toSquare) > Grid.GetColumn(fromSquare) ? "O-O" : "O-O-O";
            }

            // Handle pawn moves
            if (movingPiece is Pawn)
            {
                if (isCapture)
                {
                    string fromFile = ((char)('a' + Grid.GetColumn(fromSquare))).ToString();
                    moveNotation += $"{fromFile}x";
                }
            }
            else
            {
                // Handle other pieces
                string pieceNotation = movingPiece switch
                {
                    King => "K",
                    Queen => "Q",
                    Rook => "R",
                    Bishop => "B",
                    Knight => "N",
                    _ => ""
                };
                moveNotation += pieceNotation;
                if (isCapture) moveNotation += "x";
            }

            // Add destination square
            string destFile = ((char)('a' + Grid.GetColumn(toSquare))).ToString();
            string destRank = (8 - Grid.GetRow(toSquare)).ToString();
            moveNotation += $"{destFile}{destRank}";

            // Append " e.p." if en passant
            if (isEnPassant)
            {
                moveNotation += " e.p.";
            }

            // Append "=PromotionPiece" if promotion
            if (promotionPiece != null)
            {
                moveNotation += $"={PromotionPieceLetter(promotionPiece)}";
            }

            // Append "+" if check
            if (isCheck)
            {
                moveNotation += "+";
            }

            return moveNotation;
        }

        private string PromotionPieceLetter(string promotionPiece)
        {
            return promotionPiece switch
            {
                "Q" => "Q",
                "R" => "R",
                "B" => "B",
                "N" => "N",
                _ => "Q", // Default to Queen if undefined
            };
        }

        private void ResetGame()
        {
            // Clear all highlights, outlines, and move history
            ClearHighlights();
            ClearOutline();

            // Clear the move lists for both White and Black
            WhiteMoveListBox.Items.Clear();
            BlackMoveListBox.Items.Clear();

            // Clear the move history stack
            moveHistory.Clear();

            foreach (var button in buttonArray)
            {
                button.Content = null;
                button.Tag = null;
                button.IsEnabled = true;
            }

            // Reset player to start with White
            currentPlayer = Player.White;

            // Initialize pieces again
            InitializePieces();
        }

        private void ResetGame_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        // Optional: Implement Undo functionality
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            UndoMove();
        }

        private void UndoMove()
        {
            if (moveHistory.Count == 0)
            {
                MessageBox.Show("No moves to undo.");
                return;
            }

            Move lastMove = moveHistory.Pop();

            // Move the piece back to the original square
            lastMove.FromSquare.Content = GetImageForPiece(lastMove.MovedPiece);
            lastMove.FromSquare.Tag = lastMove.MovedPiece;
            lastMove.MovedPiece.Position = (Grid.GetRow(lastMove.FromSquare), Grid.GetColumn(lastMove.FromSquare));

            // Restore the captured piece, if any
            if (lastMove.CapturedPiece != null)
            {
                lastMove.ToSquare.Content = GetImageForPiece(lastMove.CapturedPiece);
                lastMove.ToSquare.Tag = lastMove.CapturedPiece;
            }
            else
            {
                // Handle en passant capture
                if (lastMove.IsEnPassant && lastMove.MovedPiece is Pawn)
                {
                    int capturedPawnRow = currentPlayer == Player.White ? Grid.GetRow(lastMove.FromSquare) + 1 : Grid.GetRow(lastMove.FromSquare) - 1;
                    int capturedPawnCol = Grid.GetColumn(lastMove.ToSquare);
                    Button capturedPawnSquare = buttonArray[capturedPawnRow, capturedPawnCol];
                    Pawn capturedPawn = new Pawn(currentPlayer == Player.White ? Player.Black : Player.White, (capturedPawnRow, capturedPawnCol));
                    capturedPawn.HasJustMovedTwoSquares = false;
                    capturedPawnSquare.Content = GetImageForPiece(capturedPawn);
                    capturedPawnSquare.Tag = capturedPawn;
                }
                else
                {
                    lastMove.ToSquare.Content = null;
                    lastMove.ToSquare.Tag = null;
                }
            }

            // Handle castling undo
            if (lastMove.IsCastling && lastMove.MovedPiece is King)
            {
                bool isKingside = Grid.GetColumn(lastMove.ToSquare) > Grid.GetColumn(lastMove.FromSquare);
                int rookFromCol = isKingside ? 7 : 0;
                int rookEndCol = isKingside ? 5 : 3;
                Button rookToSquare = buttonArray[Grid.GetRow(lastMove.FromSquare), rookEndCol];
                Button rookFromSquare = buttonArray[Grid.GetRow(lastMove.FromSquare), rookFromCol];
                if (rookToSquare.Tag is Rook rook)
                {
                    rookFromSquare.Content = GetImageForPiece(rook);
                    rookFromSquare.Tag = rook;
                    rookToSquare.Content = null;
                    rookToSquare.Tag = null;
                    rook.Position = (Grid.GetRow(rookFromSquare), rookFromCol);
                }
            }

            // Switch back the current player
            currentPlayer = currentPlayer == Player.White ? Player.Black : Player.White;

            // Remove the last move from the move history UI
            if (currentPlayer == Player.White)
            {
                // Undo Black's move
                if (BlackMoveListBox.Items.Count > 0)
                {
                    BlackMoveListBox.Items.RemoveAt(BlackMoveListBox.Items.Count - 1);
                }
            }
            else
            {
                // Undo White's move
                if (WhiteMoveListBox.Items.Count > 0)
                {
                    WhiteMoveListBox.Items.RemoveAt(WhiteMoveListBox.Items.Count - 1);
                }
            }

            // Re-enable the squares if they were disabled due to checkmate or stalemate
            foreach (Button btn in buttonArray)
            {
                btn.IsEnabled = true;
            }
        }

        private void DisableAllSquares()
        {
            foreach (Button btn in buttonArray)
            {
                btn.IsEnabled = false;
            }
        }
    }
}
