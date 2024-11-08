﻿using ChessGame;
using System.Collections.Generic;
using System.Windows.Controls;

public class Piece
{
    public Player Owner { get; }
    public (int Row, int Col) Position { get; set; }
    public bool HasMoved { get; protected set; } = false;

    public Piece(Player owner, (int Row, int Col) position)
    {
        Owner = owner;
        Position = position;
    }

    // Updated GetValidMoves to include ChessLogic parameter
    public virtual List<(int Row, int Col)> GetValidMoves(Button[,] board, ChessLogic chessLogic)
    {
        return new List<(int Row, int Col)>();
    }

    public virtual void MarkAsMoved()
    {
        HasMoved = true;
    }

    public virtual bool CanAttackSquare((int Row, int Col) square, Button[,] board)
    {
        return false;
    }
}
