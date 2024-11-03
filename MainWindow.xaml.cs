﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessGame
{
    public partial class MainWindow : Window
    {
        private Button[,] buttonArray = new Button[8, 8];
        private Button selectedSquare = null;
        private List<Button> highlightedSquares = new List<Button>();
        private Player currentPlayer = Player.White;
        private List<string> moveHistory = new List<string>();
        public Piece LastMovedPiece {  get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CreateChessBoard();
            InitializePieces();
    
        }

        private void CreateChessBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button square = new Button
                    {
                        Background = (row + col) % 2 == 0 ? Brushes.White : Brushes.Gray
                    };

                    Grid.SetRow(square, row);
                    Grid.SetColumn(square, col);
                    ChessBoardGrid.Children.Add(square);

                    buttonArray[row, col] = square;
                    square.Click += Square_Click;
                }
            }
        }

        private void InitializePieces()
        {
            // Place black pieces
            PlacePiece(buttonArray[0, 0], new Rook(Player.Black, (0, 0)));
            PlacePiece(buttonArray[0, 1], new Knight(Player.Black, (0, 1)));
            PlacePiece(buttonArray[0, 2], new Bishop(Player.Black, (0, 2)));
            PlacePiece(buttonArray[0, 3], new Queen(Player.Black, (0, 3)));
            PlacePiece(buttonArray[0, 4], new King(Player.Black, (0, 4)));
            PlacePiece(buttonArray[0, 5], new Bishop(Player.Black, (0, 5)));
            PlacePiece(buttonArray[0, 6], new Knight(Player.Black, (0, 6)));
            PlacePiece(buttonArray[0, 7], new Rook(Player.Black, (0, 7)));

            for (int i = 0; i < 8; i++)
            {
                PlacePiece(buttonArray[1, i], new Pawn(Player.Black, (1, i)));
            }

            // Place white pieces
            PlacePiece(buttonArray[7, 0], new Rook(Player.White, (7, 0)));
            PlacePiece(buttonArray[7, 1], new Knight(Player.White, (7, 1)));
            PlacePiece(buttonArray[7, 2], new Bishop(Player.White, (7, 2)));
            PlacePiece(buttonArray[7, 3], new Queen(Player.White, (7, 3)));
            PlacePiece(buttonArray[7, 4], new King(Player.White, (7, 4)));
            PlacePiece(buttonArray[7, 5], new Bishop(Player.White, (7, 5)));
            PlacePiece(buttonArray[7, 6], new Knight(Player.White, (7, 6)));
            PlacePiece(buttonArray[7, 7], new Rook(Player.White, (7, 7)));

            for (int i = 0; i < 8; i++)
            {
                PlacePiece(buttonArray[6, i], new Pawn(Player.White, (6, i)));
            }
        }

        private void PlacePiece(Button square, Piece piece)
        {
            Image pieceImageControl = new Image
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/Images/{piece.Owner.ToString().ToLower()}_{piece.GetType().Name.ToLower()}.png"))
            };

            square.Content = pieceImageControl;
            square.Tag = piece;
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
                    var safeMoves = IsInCheck(currentPlayer) ? GetSafeMoves(selectedPiece) : selectedPiece.GetValidMoves(buttonArray);
                    foreach (var move in safeMoves)
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
                    bool isCapture = clickedSquare.Tag is Piece;
                    MovePiece(selectedSquare, clickedSquare);

                    // Clear all highlights and outlines after the move
                    ClearHighlights();
                    ClearOutline();

                    // Check for check or checkmate after the move
                    bool isCheck = IsInCheck(currentPlayer == Player.White ? Player.Black : Player.White);
                    if (IsCheckmate(currentPlayer == Player.White ? Player.Black : Player.White))
                    {
                        MessageBox.Show($"{currentPlayer} wins by checkmate!");
                    }
                    else if (isCheck)
                    {
                        MessageBox.Show($"{(currentPlayer == Player.White ? "Black" : "White")} is in check!");
                    }

                    // Get and log the move notation
                    string moveNotation = GetMoveNotation(selectedSquare, clickedSquare, (Piece)clickedSquare.Tag, isCapture, isCheck);
                    LogMove(moveNotation);

                    // Switch turns after a successful move
                    currentPlayer = currentPlayer == Player.White ? Player.Black : Player.White;
                    
                }
                else
                {
                    ClearHighlights();
                }

                selectedSquare = null;
            }
        }

        private void MovePiece(Button fromSquare, Button toSquare)
        {
            toSquare.Content = fromSquare.Content;
            toSquare.Tag = fromSquare.Tag;

            if (fromSquare.Tag is Piece movingPiece)
            {
                movingPiece.Position = (Grid.GetRow(toSquare), Grid.GetColumn(toSquare));

                // Mark the king or rook as having moved if this is their first move
                if (movingPiece is King || movingPiece is Rook)
                {
                    movingPiece.MarkAsMoved();
                }

                // Handle castling if the king moves two squares
                if (movingPiece is King king && Math.Abs(Grid.GetColumn(toSquare) - Grid.GetColumn(fromSquare)) == 2)
                {
                    bool isKingside = Grid.GetColumn(toSquare) > Grid.GetColumn(fromSquare);
                    int rookStartCol = isKingside ? 7 : 0;
                    int rookEndCol = isKingside ? 5 : 3;

                    Button rookSquare = buttonArray[king.Position.Row, rookStartCol];
                    Button newRookSquare = buttonArray[king.Position.Row, rookEndCol];
                    newRookSquare.Content = rookSquare.Content;
                    newRookSquare.Tag = rookSquare.Tag;

                    if (rookSquare.Tag is Rook rook)
                    {
                        rook.Position = (king.Position.Row, rookEndCol);
                        rook.MarkAsMoved();
                    }

                    rookSquare.Content = null;
                    rookSquare.Tag = null;
                }

                if (movingPiece is Pawn pawn && !pawn.HasMoved)
                {
                    pawn.MarkAsMoved();
                }
            }

            fromSquare.Content = null;
            fromSquare.Tag = null;
        }

        public List<(int Row, int Col)> GetSafeMoves(Piece piece)
        {
            List<(int Row, int Col)> safeMoves = new List<(int Row, int Col)>();
            var validMoves = piece.GetValidMoves(buttonArray);

            foreach (var move in validMoves)
            {
                int originalRow = piece.Position.Row;
                int originalCol = piece.Position.Col;
                Button targetSquare = buttonArray[move.Row, move.Col];
                var originalTargetContent = targetSquare.Content;
                var originalTargetTag = targetSquare.Tag;

                buttonArray[originalRow, originalCol].Tag = null;
                targetSquare.Tag = piece;
                piece.Position = move;

                if (!IsInCheck(piece.Owner))
                {
                    safeMoves.Add(move);
                }

                targetSquare.Tag = originalTargetTag;
                targetSquare.Content = originalTargetContent;
                piece.Position = (originalRow, originalCol);
                buttonArray[originalRow, originalCol].Tag = piece;
            }

            return safeMoves;
        }

        public bool IsSquareThreatened((int Row, int Col) square, Player kingOwner)
        {
            for (int row = 0; row < buttonArray.GetLength(0); row++)
            {
                for (int col = 0; col < buttonArray.GetLength(1); col++)
                {
                    Button button = buttonArray[row, col];

                    if (button.Tag is Piece piece && piece.Owner != kingOwner)
                    {
                        if (piece.CanAttackSquare(square, buttonArray))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsInCheck(Player player)
        {
            (int Row, int Col)? kingPosition = null;

            for (int row = 0; row < buttonArray.GetLength(0); row++)
            {
                for (int col = 0; col < buttonArray.GetLength(1); col++)
                {
                    Button button = buttonArray[row, col];
                    if (button.Tag is King king && king.Owner == player)
                    {
                        kingPosition = (row, col);
                        break;
                    }
                }
            }

            if (kingPosition.HasValue)
            {
                return IsSquareThreatened(kingPosition.Value, player);
            }

            return false;
        }

        public bool IsCheckmate(Player player)
        {
            if (!IsInCheck(player)) return false;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button button = buttonArray[row, col];
                    if (button.Tag is Piece piece && piece.Owner == player)
                    {
                        if (GetSafeMoves(piece).Count > 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
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
                square.Background = (Grid.GetRow(square) + Grid.GetColumn(square)) % 2 == 0 ? Brushes.White : Brushes.Gray;
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

        private string GetMoveNotation(Button fromSquare, Button toSquare, Piece movingPiece, bool isCapture = false, bool isCheck = false)
        {
            string moveNotation = "";

            if (movingPiece is King && Math.Abs(Grid.GetColumn(toSquare) - Grid.GetColumn(fromSquare)) == 2)
            {
                return Grid.GetColumn(toSquare) > Grid.GetColumn(fromSquare) ? "O-O" : "O-O-O";
            }

            if (movingPiece is Pawn)
            {
                if (isCapture)
                {
                    moveNotation += $"{(char)('a' + Grid.GetColumn(fromSquare))}x";
                }
            }
            else
            {
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

            moveNotation += $"{(char)('a' + Grid.GetColumn(toSquare))}{8 - Grid.GetRow(toSquare)}";
            if (isCheck) moveNotation += "+";

            return moveNotation;
        }

        private void ResetGame()
        {
            // Clear all highlights, outlines, and move history
            ClearHighlights();
            ClearOutline();

            // Clear the move lists for both White and Black
            WhiteMoveListBox.Items.Clear();
            BlackMoveListBox.Items.Clear();

            foreach (var button in buttonArray)
            {
                button.Content = null;
                button.Tag = null;
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
    }

    public enum Player
    {
        White,
        Black
    }
}
