using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.Game
{
    /// <summary>
    /// The delegate that notifies subscribers when a turn has been made.
    /// </summary>
    /// <param name="team">The team that made the turn.</param>
    /// <param name="move">The move made this turn.</param>
    public delegate void TurnMadeDelegate(Team team, Move move);

    /// <summary>
    /// The delegate that notifies subscribers when a king is in check.
    /// </summary>
    /// <param name="king">The cell containing the king in check.</param>
    public delegate void CheckDelegate(IReadOnlyCell king);

    /// <summary>
    /// The delegate that notifies subscribers when a king is in checkmate or has been destroyed.
    /// </summary>
    /// <param name="king">The cell containing the king in checkmate.</param>
    public delegate void CheckmateDelegate(IReadOnlyCell king);

    /// <summary>
    /// The delegate that notifies subscribers when a piece has been promoted.
    /// </summary>
    /// <param name="cell">The cell of the promoted piece.</param>
    /// <param name="piece">The promoted piece.</param>
    /// <returns>The type of piece to promote to.</returns>
    public delegate PieceType PiecePromotionDelegate(IReadOnlyCell cell, IReadOnlyPiece piece);

    /// <summary>
    /// Exposes various methods that can be employed to play a game of chess.
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// Gets the representation of a board upon which the game of chess is played.
        /// </summary>
        public Board Board { get { return _board; } }

        /// <summary>
        /// Notifies subscribers when a turn has been made.
        /// </summary>
        public event TurnMadeDelegate OnTurnMade
        {
            add { _onTurnMade += value; }
            remove { _onTurnMade -= value; }
        }

        /// <summary>
        /// Notifies subscribers when a king is in check.
        /// </summary>
        public event CheckDelegate OnCheck
        {
            add { _onCheck += value; }
            remove { _onCheck -= value; }
        }

        /// <summary>
        /// Notifies subscribers when a king has left check.
        /// </summary>
        public event CheckDelegate OnCheckCleared
        {
            add { _onCheckCleared += value; }
            remove { _onCheckCleared -= value; }
        }

        /// <summary>
        /// Notifies subscribers when a king is in checkmate or has been destroyed.
        /// </summary>
        public event CheckmateDelegate OnCheckmate
        {
            add { _onCheckmate += value; }
            remove { _onCheckmate -= value; }
        }

        /// <summary>
        /// Notifies subscribers when a piece has been promoted.
        /// </summary>
        public PiecePromotionDelegate PiecePromotionHandler
        {
            get { return _piecePromotionHandler; }
            set { _piecePromotionHandler = value; }
        }

        /// <summary>
        /// Resets the underlying board.
        /// </summary>
        public void ResetGame()
        {
            _board.ResetBoard();
            _activeTurn = Team.Black;
        }

        /// <summary>
        /// Handles 
        /// </summary>
        /// <param name="cell"></param>
        public void OnCellDown(IReadOnlyCell cell)
        {
            Debug.Assert(cell != null);
        }

        /// <summary>
        /// Handles catching user input from <see cref="GameWindow"/> and moving a piece in
        /// response to this input.
        /// </summary>
        /// <param name="from">The cell that the piece was selected in.</param>
        /// <param name="to">The cell that the piece was deselected in.</param>
        public void OnCellUp(IReadOnlyCell from, IReadOnlyCell to)
        {
            Debug.Assert(from != null && to != null);

            // TODO: Check for valid move.
            if (from.Occupant != null && from != to)
            {
                Move theMove = new Move(from.Position, to.Position);

                if (GetValidMoves(from).Contains(theMove))
                {
                    // If the destination holds a king, it's checkmate.
                    if (to.Occupant != null && to.Occupant.Type == PieceType.King)
                    {
                        if (_onCheckmate != null)
                        {
                            _onCheckmate(to);
                        }
                        else
                        {
                            ResetGame();
                        }

                        return;
                    }

                    // If we're in check and moving the checked cell, update the cell along with this move.
                    if (_cellInCheck != null && _cellInCheck.Position.Equals(from.Position))
                    {
                        _cellInCheck = to;
                    }

                    HandleCastling(from, to);

                    Board.MovePiece(from.Position.X, from.Position.Y, to.Position.X, to.Position.Y);

                    HandlePiecePromotion(to);
                    HandleCheck();

                    if (_onTurnMade != null)
                    {
                        _onTurnMade(_activeTurn, theMove);
                    }

                    _activeTurn = _activeTurn == Team.Black ? Team.White : Team.Black;
                }
            }
        }

        /// <summary>
        /// Returns a list of valid moves for the piece in the given cell.
        /// This provides a more accurate list that <see cref="IPiece"/> because it
        /// takes into account special movement rules that vary with the context
        /// of the board.
        /// </summary>
        /// <param name="cell">The cell containing the piece.</param>
        /// <param name="filterTeam">
        /// If true, only returns a list when it is the current unit's turn to move.
        /// </param>
        /// <returns>A list of valid moves for the piece in the given cell.</returns>
        public List<Move> GetValidMoves(IReadOnlyCell cell, bool filterTeam = true)
        {
            Debug.Assert(cell.Occupant != null);

            List<Move> moves = new List<Move>();

            // Only generate moves if it is this piece's turn.
            if (!filterTeam || cell.Occupant.Team == _activeTurn)
            {
                IReadOnlyCollection<Move> validMoves = cell.Occupant.ValidMoves(cell.Position);
                Debug.Assert(validMoves != null);

                moves.AddRange(validMoves);

                if (moves.Count > 0)
                {
                    if (cell.Occupant.Type == PieceType.Queen ||
                        cell.Occupant.Type == PieceType.Rook)
                    {
                        TrimHorizontalMoves(moves);
                    }

                    if (cell.Occupant.Type == PieceType.Queen ||
                        cell.Occupant.Type == PieceType.Pawn ||
                        cell.Occupant.Type == PieceType.Rook)
                    {
                        TrimVerticalMoves(moves);
                    }

                    if (cell.Occupant.Type == PieceType.Bishop ||
                        cell.Occupant.Type == PieceType.Queen)
                    {
                        TrimDiagonalMoves(moves);
                    }

                    TrimFriendlyMoves(moves);
                }

                moves.AddRange(GenerateExceptionalMoves(cell));

                if (moves.Count > 0)
                {
                    TrimExceptionalMoves(moves);
                }

            }

            return moves;
        }

        /// <summary>
        /// The representation of a board upon which the game of chess is played.
        /// </summary>
        private Board _board = new Board();

        /// <summary>
        /// Notifies subscribers when a turn has been made.
        /// </summary>
        private event TurnMadeDelegate _onTurnMade;

        /// <summary>
        /// Notifies subscribers when a king is in check. 
        /// </summary>
        private event CheckDelegate _onCheck;

        /// <summary>
        /// Notifies subscribers when a king has left check./// 
        /// </summary>
        private event CheckDelegate _onCheckCleared;

        /// <summary>
        /// Notifies subscribers when a king is in checkmate or has been destroyed. 
        /// </summary>
        private event CheckmateDelegate _onCheckmate;

        /// <summary>
        /// Notifies subscribers when a piece has been promoted. 
        /// </summary>
        private PiecePromotionDelegate _piecePromotionHandler;

        /// <summary>
        /// Tracks which team's turn it currently is.
        /// </summary>
        private Team _activeTurn = Team.Black;

        /// <summary>
        /// Keeps a track of the cell currently in check.
        /// </summary>
        private IReadOnlyCell _cellInCheck = null;

        /// <summary>
        /// Returns true if the provided grid is contested by the provided team,
        /// or false otherwise.
        /// </summary>
        /// <param name="grid">The grid coordinates.</param>
        /// <param name="by">The team.</param>
        /// <returns>
        /// True if the provided grid is contested by the provided team,
        /// or false otherwise.
        /// </returns>
        private bool GridContested(Vector2 grid, Team by)
        {
            foreach (IReadOnlyCell cell in _board.Cells)
            {
                if (cell.Occupant != null && cell.Occupant.Team == by)
                {
                    List<Move> validMoves = GetValidMoves(cell, false);

                    if (validMoves.Any(move => move.To.Equals(grid)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Handles moving the rook when a special castling move has been made.
        /// </summary>
        /// <param name="from">The piece was moved from here.</param>
        /// <param name="to">The piece was moved to here.</param>
        private void HandleCastling(IReadOnlyCell from, IReadOnlyCell to)
        {
            Debug.Assert(from != null && to != null);
                               
            if (from.Occupant.Type == PieceType.King)
            {
                // If this king has moved more than one grid, we know he's castling.
                // We can skip checking the rules because the move would only be generated
                // if valid.
                int dx = from.Position.X - to.Position.X;

                if (dx > 1)
                {
                    // He moved left.
                    Debug.Assert(Board[1, from.Position.Y].Occupant != null);
                    Board.MovePiece(1, from.Position.Y, to.Position.X + 1, from.Position.Y);

                }
                else if (dx < -1)
                {
                    // He moved right.
                    Debug.Assert(Board[8, from.Position.Y].Occupant != null);
                    Board.MovePiece(8, from.Position.Y, to.Position.X - 1, from.Position.Y);
                }
            }
        }

        /// <summary>
        /// Handles promoting a pawn when it has reached the other team's rear line.
        /// </summary>
        /// <param name="cell">The cell containing the piece.</param>
        private void HandlePiecePromotion(IReadOnlyCell cell)
        {
            Debug.Assert(cell != null);

            // Special pawn promotion rules.
            if (cell.Occupant.Type == PieceType.Pawn)
            {
                if ((cell.Occupant.Team == Team.Black && cell.Position.Y == 1) ||
                    (cell.Occupant.Team == Team.White && cell.Position.Y == Board.BoardHeight))
                {
                    if (_piecePromotionHandler != null)
                    {
                        Board.SetPieceAt(cell.Position, _piecePromotionHandler(cell, cell.Occupant));
                    }
                    else
                    {
                        Board.SetPieceAt(cell.Position, PieceType.Queen);
                    }
                }
            }
        }

        /// <summary>
        /// Handles toggling and clearing check status.
        /// </summary>
        private void HandleCheck()
        {
            if (_cellInCheck != null)
            {
                Debug.Assert(_cellInCheck.Occupant != null);

                Team otherTeam = _cellInCheck.Occupant.Team == Team.Black ? Team.White : Team.Black;

                if (!GridContested(_cellInCheck.Position, otherTeam))
                {
                    // The check has now been lifted. Handle it.
                    if (_onCheckCleared != null)
                    {
                        _onCheckCleared(_cellInCheck);
                    }

                    _cellInCheck = null;
                }
            }
            else
            {
                foreach (IReadOnlyCell boardCell in _board.Cells)
                {
                    if (boardCell.Occupant == null || boardCell.Occupant.Type != PieceType.King)
                    {
                        continue;
                    }

                    // We're a king.
                    Team otherTeam = boardCell.Occupant.Team == Team.Black ? Team.White : Team.Black;

                    if (GridContested(boardCell.Position, otherTeam))
                    {
                        // We're now in check.
                        _cellInCheck = boardCell;

                        if (_onCheck != null)
                        {
                            _onCheck(_cellInCheck);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Given a list of moves, removes any moves from that list which travel through
        /// other pieces horizontally.
        /// </summary>
        /// <param name="moves">The list of moves to mutate.</param>
        private void TrimHorizontalMoves(List<Move> moves)
        {
            Debug.Assert(moves != null);

            if (moves.Count == 0)
            {
                return;
            }

            // Every move should have the same origin. Just use the first one.
            Vector2 origin = moves[0].From;

            // Check left.
            for (int x = origin.X - 1; x > 0; --x)
            {
                if (!moves.Any(move => move.To.X == x && move.To.Y == origin.Y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[x, origin.Y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X < x && move.To.Y == origin.Y);
                }
            }

            // Check right.
            for (int x = origin.X + 1; x <= Board.BoardWidth; ++x)
            {
                if (!moves.Any(move => move.To.X == x && move.To.Y == origin.Y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[x, origin.Y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X > x && move.To.Y == origin.Y);
                }
            }
        }

        /// <summary>
        /// Given a list of moves, removes any moves from that list which travel through
        /// other pieces vertically.
        /// </summary>
        /// <param name="moves">The list of moves to mutate.</param>
        private void TrimVerticalMoves(List<Move> moves)
        {
            Debug.Assert(moves != null);

            if (moves.Count == 0)
            {
                return;
            }

            // Every move should have the same origin. Just use the first one.
            Vector2 origin = moves[0].From;

            // Check up.
            for (int y = origin.Y + 1; y <= Board.BoardHeight; ++y)
            {
                if (!moves.Any(move => move.To.X == origin.X && move.To.Y == y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[origin.X, y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X == origin.X && move.To.Y > y);
                }
            }

            // Check down.
            for (int y = origin.Y - 1; y > 0; --y)
            {
                if (!moves.Any(move => move.To.X == origin.X && move.To.Y == y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[origin.X, y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X == origin.X && move.To.Y < y);
                }
            }
        }

        /// <summary>
        /// Given a list of moves, removes any moves from that list which travel through
        /// other pieces diagonally.
        /// </summary>
        /// <param name="moves">The list of moves to mutate.</param>
        private void TrimDiagonalMoves(List<Move> moves)
        {
            Debug.Assert(moves != null);

            if (moves.Count == 0)
            {
                return;
            }

            // Every move should have the same origin. Just use the first one.
            Vector2 origin = moves[0].From;

            // Check up-right.
            for (int x = origin.X + 1, y = origin.Y + 1;
                 x <= Board.BoardWidth && y <= Board.BoardHeight;
                 ++x, ++y)
            {
                if (!moves.Any(move => move.To.X == x && move.To.Y == y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[x, y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X > x && move.To.Y > y);
                }
            }

            // Check down-right.
            for (int x = origin.X + 1, y = origin.Y - 1;
                 x <= Board.BoardWidth && y > 0;
                 ++x, --y)
            {
                if (!moves.Any(move => move.To.X == x && move.To.Y == y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[x, y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X > x && move.To.Y < y);
                }
            }

            // Check down-left.
            for (int x = origin.X - 1, y = origin.Y - 1;
                 x > 0 && y > 0;
                 --x, --y)
            {
                if (!moves.Any(move => move.To.X == x && move.To.Y == y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[x, y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X < x && move.To.Y < y);
                }
            }

            // Check up-left.
            for (int x = origin.X - 1, y = origin.Y + 1;
                 x > 0 && y <= Board.BoardHeight;
                 --x, ++y)
            {
                if (!moves.Any(move => move.To.X == x && move.To.Y == y))
                {
                    // No more moves along this line.
                    break;
                }

                IReadOnlyPiece piece = _board[x, y].Occupant;

                if (piece != null)
                {
                    IReadOnlyPiece originPiece = Board[origin].Occupant;
                    Debug.Assert(originPiece != null);
                    moves.RemoveAll(move => move.To.X < x && move.To.Y > y);
                }
            }
        }

        /// <summary>
        /// Given a list of moves, removes any moves from that list which touch friendly units.
        /// </summary>
        /// <param name="moves">The list of moves to mutate.</param>
        private void TrimFriendlyMoves(List<Move> moves)
        {
            Debug.Assert(moves != null);

            if (moves.Count == 0)
            {
                return;
            }

            // Every move should have the same origin. Just use the first one.
            Vector2 origin = moves[0].From;
            moves.RemoveAll(move => _board[move.To].Occupant != null &&
                                    _board[move.To].Occupant.Team == _board[move.From].Occupant.Team);
        }

        /// <summary>
        /// Geneates a list of exceptional moves (castling or pawn capturing).
        /// </summary>
        /// <param name="cell">
        /// The cell containing the piece to generate the list of moves for.
        /// </param>
        /// <returns>A list of exceptional moves.</returns>
        private List<Move> GenerateExceptionalMoves(IReadOnlyCell cell)
        {
            Debug.Assert(cell.Occupant != null);

            List<Move> moves = new List<Move>();

            switch (cell.Occupant.Type)
            {
                case PieceType.Bishop: break;
                case PieceType.King:

                    // Castling!
                    // If the king has moved, we can't castle.
                    if (cell.Occupant.Moves.Count != 0)
                    {
                        break;
                    }

                    // Check left.
                    bool clearLeft = true;

                    for (int x = cell.Position.X - 1; x > 2; --x)
                    {
                        if (_board[x, cell.Position.Y].Occupant != null)
                        {
                            clearLeft = false;
                            break;
                        }
                    }

                    if (clearLeft)
                    {
                        // Check the left rook.
                        IReadOnlyPiece rook = Board[1, cell.Position.Y].Occupant;

                        if (rook != null && rook.Moves.Count == 0)
                        {
                            moves.Add(new Move(cell.Position, new Vector2(cell.Position.X - 2, cell.Position.Y)));
                        }
                    }

                    bool clearRight = true;

                    // Check right.
                    for (int x = cell.Position.X + 1; x < Board.BoardWidth - 1; ++x)
                    {
                        if (_board[x, cell.Position.Y].Occupant != null)
                        {
                            clearRight = false;
                            break;
                        }
                    }

                    if (clearRight)
                    {
                        // Check the right rook.
                        IReadOnlyPiece rook = Board[8, cell.Position.Y].Occupant;

                        if (rook != null && rook.Moves.Count == 0)
                        {
                            moves.Add(new Move(cell.Position, new Vector2(cell.Position.X + 2, cell.Position.Y)));
                        }
                    }

                    break;

                case PieceType.Knight: break;
                case PieceType.Pawn:

                    // Black moves down.
                    int dy = cell.Occupant.Team == Team.Black ? -1 : 1;

                    // dy-left.
                    if (cell.Position.X - 1 > 0 && cell.Position.Y + dy <= Board.BoardHeight)
                    {
                        IReadOnlyPiece piece = Board[cell.Position.X - 1, cell.Position.Y + dy].Occupant;

                        if (piece != null && piece.Team != cell.Occupant.Team)
                        {
                            moves.Add(new Move(cell.Position, new Vector2(cell.Position.X - 1, cell.Position.Y + dy)));
                        }
                    }

                    // dy-right.
                    if (cell.Position.X + 1 <= Board.BoardWidth && cell.Position.Y + dy <= Board.BoardHeight)
                    {
                        IReadOnlyPiece piece = Board[cell.Position.X + 1, cell.Position.Y + dy].Occupant;

                        if (piece != null && piece.Team != cell.Occupant.Team)
                        {
                            moves.Add(new Move(cell.Position, new Vector2(cell.Position.X + 1, cell.Position.Y + dy)));
                        }
                    }

                    break;

                case PieceType.Queen: break;
                case PieceType.Rook: break;
            }

            return moves;
        }

        /// <summary>
        /// Given a list of moves, removes any moves from that list which disallowed for
        /// individual pieces, such as pawn capturing by moving straight up or down.
        /// </summary>
        /// <param name="moves">The list of moves to mutate.</param>
        private void TrimExceptionalMoves(List<Move> moves)
        {
            Debug.Assert(moves != null);

            if (moves.Count == 0)
            {
                return;
            }

            // Every move should have the same origin. Just use the first one.
            Vector2 origin = moves[0].From;

            switch (_board[origin].Occupant.Type)
            {
                case PieceType.Bishop: break;
                case PieceType.King: break;
                case PieceType.Knight: break;
                case PieceType.Pawn:

                    // Pawns can only capture units when moving diagonally.
                    moves.RemoveAll(move => move.To.X == move.From.X &&_board[move.To].Occupant != null);
                    break;

                case PieceType.Queen: break;
                case PieceType.Rook: break;
            }
        }
    }
}