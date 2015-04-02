using Chess.Framework;
using Chess.Representation.Pieces;
using System.Diagnostics;

namespace Chess.Representation
{
    /// <summary>
    /// A representation of a chess board that comprises <see cref="BoardWidth"/> by
    /// <see cref="BoardHeight"/> cells.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The maximum width of a chess board.
        /// </summary>
        public const int BoardWidth = 8;

        /// <summary>
        /// The maximum height of a chess board.
        /// </summary>
        public const int BoardHeight = 8;

        /// <summary>
        /// Gets a read only cell of the chess board.
        /// </summary>
        /// <param name="x">
        /// The desired x coordinate; greater than 0 and less than or equal to <see cref="BoardWidth"/>.
        /// </param>
        /// <param name="y">
        /// The desired y coordinate; greater than 0 and less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <returns>
        /// The cell at the provided coordinates. 
        /// This is returned as a memberwise copy to prevent external tampering of the occupant.
        /// </returns>
        public IReadOnlyCell this[int x, int y]
        {
            get { return At(x, y); }
        }

        /// <summary>
        /// Gets a read only cell of the chess board.
        /// </summary>
        /// <param name="loc">
        /// Each coordinate should be greater than 0, the X coordinate 
        /// less than or equal to <see cref="BoardWidth"/>, and the Y 
        /// coordinate less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <returns>
        /// The cell at the provided coordinates. 
        /// This is returned as a memberwise copy to prevent external tampering of the occupant.
        /// </returns>
        public IReadOnlyCell this[Vector2 loc]
        {
            get { return At(loc); }
        }

        /// <summary>
        /// Gets the array of cells belonging to this board. This array is read-only.
        /// </summary>
        public IReadOnlyCell[,] Cells { get { return _cells; } }

        /// <summary>
        /// Constructs a chess board with the default position of pieces.
        /// </summary>
        public Board()
        {
            // Now we need to intiailise the cells of the board to their blank state.
            for (int x = 1; x <= BoardWidth; ++x)
            {
                for (int y = 1; y <= BoardHeight; ++y)
                {
                    SetAt(x, y, new Cell(new Vector2(x, y)));
                }
            }

            SetupBoard();
        }

        /// <summary>
        /// Moves a piece from the first location to the second location. If there is already
        /// a piece at the second location, that piece is discarded. 
        /// </summary>
        /// <param name="fromX">
        /// The desired x coordinate for the origin piece; greater than 0 and less than or equal to <see cref="BoardWidth"/>.
        /// </param>
        /// <param name="fromY">
        /// The desired y coordinate for the origin piece; greater than 0 and less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <param name="toX">
        /// The desired x coordinate for the destination piece; greater than 0 and less than or equal to <see cref="BoardWidth"/>.
        /// </param>
        /// <param name="toY">
        /// The desired y coordinate for the destination piece; greater than 0 and less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        public void MovePiece(int fromX, int fromY, int toX, int toY)
        {
            Debug.Assert(fromX > 0 && fromX <= BoardWidth && fromY > 0 && fromY <= BoardHeight);
            Debug.Assert(toX > 0 && toX <= BoardWidth && toY > 0 && toY <= BoardHeight);
            Debug.Assert(fromX != toX || fromY != toY);

            IPiece fromOccupant = At(fromX, fromY).Occupant;
            Debug.Assert(fromOccupant != null);

            IPiece toOccupant = At(toX, toY).Occupant;

            if (toOccupant != null)
            {
                _pieceFactory.FreeObject(toOccupant);
            }

            At(fromX, fromY).Occupant = null;
            At(toX, toY).Occupant = fromOccupant;

            fromOccupant.Moves.Add(new Move(new Vector2(fromX, fromY), new Vector2(toX, toY)));
        }

        /// <summary>
        /// Moves a piece from the first location to the second location. If there is already
        /// a piece at the second location, that piece is discarded. 
        /// </summary>
        /// <param name="from">
        /// Origin piece.
        /// Each coordinate should be greater than 0, the X coordinate 
        /// less than or equal to <see cref="BoardWidth"/>, and the Y 
        /// coordinate less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <param name="to">
        /// Destination piece.
        /// Each coordinate should be greater than 0, the X coordinate 
        /// less than or equal to <see cref="BoardWidth"/>, and the Y 
        /// coordinate less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        public void MovePiece(Vector2 from, Vector2 to)
        {
            MovePiece(from.X, from.Y, to.X, to.Y);
        }

        /// <summary>
        /// Clears the piece at the provided location. Does nothing if the location is
        /// already empty.
        /// </summary>
        /// <param name="x">
        /// The desired x coordinate; greater than 0 and less than or equal to <see cref="BoardWidth"/>.
        /// </param>
        /// <param name="y">
        /// The desired y coordinate; greater than 0 and less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        public void ClearPieceAt(int x, int y)
        {
            Debug.Assert(x > 0 && x <= BoardWidth && y > 0 && y <= BoardHeight);

            IPiece occupant = At(x, y).Occupant;

            if (occupant != null)
            {
                _pieceFactory.FreeObject(occupant);
                At(x, y).Occupant = null;
            }
        }

        /// <summary>
        /// Clears the piece at the provided location. Does nothing if the location is
        /// already empty.
        /// </summary>
        /// <param name="loc">
        /// Each coordinate should be greater than 0, the X coordinate 
        /// less than or equal to <see cref="BoardWidth"/>, and the Y 
        /// coordinate less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        public void ClearPieceAt(Vector2 loc)
        {
            ClearPieceAt(loc.X, loc.Y);
        }

        /// <summary>
        /// Converts an existing piece from its current type to the provided type.
        /// There should be a valid piece at the provided location.
        /// The two pieces should not be the same type. 
        /// </summary>
        /// <param name="x">
        /// The desired x coordinate; greater than 0 and less than or equal to <see cref="BoardWidth"/>.
        /// </param>
        /// <param name="y">
        /// The desired y coordinate; greater than 0 and less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <param name="type"></param>
        public void SetPieceAt(int x, int y, PieceType type)
        {
            Debug.Assert(x > 0 && x <= BoardWidth && y > 0 && y <= BoardHeight);

            IPiece curPiece = At(x, y).Occupant;
            Debug.Assert(curPiece != null);
            Debug.Assert(curPiece.Type != type);

            IPiece newPiece;

            switch (type)
            {
                case PieceType.Bishop: newPiece = _pieceFactory.GetObject<Bishop>(); break;
                case PieceType.King:   newPiece = _pieceFactory.GetObject<King>();   break;
                case PieceType.Knight: newPiece = _pieceFactory.GetObject<Knight>(); break;
                case PieceType.Pawn:   newPiece = _pieceFactory.GetObject<Pawn>();   break;
                case PieceType.Queen:  newPiece = _pieceFactory.GetObject<Queen>();  break;
                case PieceType.Rook:   newPiece = _pieceFactory.GetObject<Rook>();   break;
                default:               newPiece = null;                              break;
            }

            Debug.Assert(newPiece != null);
            newPiece.Team = curPiece.Team;
            At(x, y).Occupant = newPiece;
            _pieceFactory.FreeObject(curPiece);
        }

        /// <summary>
        /// Converts an existing piece from its current type to the provided type.
        /// There should be a valid piece at the provided location.
        /// The two pieces should not be the same type. 
        /// </summary>
        /// <param name="loc">
        /// Each coordinate should be greater than 0, the X coordinate 
        /// less than or equal to <see cref="BoardWidth"/>, and the Y 
        /// coordinate less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <param name="type">The type of chess piece.</param>
        public void SetPieceAt(Vector2 loc, PieceType type)
        {
            SetPieceAt(loc.X, loc.Y, type);
        }

        /// <summary>
        /// Resets the chess board to its default state.
        /// </summary>
        public void ResetBoard()
        {
            // Iterate over the board and release every non-null piece.
            for (int x = 1; x <= BoardWidth; ++x)
            {
                for (int y = 1; y <= BoardHeight; ++y)
                {
                    ClearPieceAt(x, y);
                }
            }

            SetupBoard();
        }

        /// <summary>
        /// The multi-dimensional array which holds each cell of the board.
        /// </summary>
        private readonly ICell[,] _cells = new Cell[BoardWidth, BoardHeight];

        /// <summary>
        /// An object factory containing the amount of pieces necessary to run a board.
        /// This should be used when fetching or releasing pieces on the board.
        /// </summary>
        private readonly IObjectFactory<IPiece> _pieceFactory = new PieceFactory();

        /// <summary>
        /// Returns a cell of the chess board.
        /// </summary>
        /// <param name="x">
        /// The desired x coordinate; greater than 0 and less than or equal to <see cref="BoardWidth"/>.
        /// </param>
        /// <param name="y">
        /// The desired y coordinate; greater than 0 and less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <returns>The cell at the provided coordinates.</returns>
        private ICell At(int x, int y)
        {
            Debug.Assert(x > 0 && x <= BoardWidth && y > 0 && y <= BoardHeight);
            return _cells[x - 1, y - 1];
        }

        /// <summary>
        /// Returns a cell of the chess board.
        /// </summary>
        /// <param name="loc">
        /// Each coordinate should be greater than 0, the X coordinate 
        /// less than or equal to <see cref="BoardWidth"/>, and the Y 
        /// coordinate less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <returns>The cell at the provided coordinates.</returns>
        private ICell At(Vector2 loc)
        {
            return At(loc.X, loc.Y);
        }

        /// <summary>
        /// Sets a cell of the chess board.
        /// </summary>
        /// <param name="x">
        /// The desired x coordinate; greater than 0 and less than or equal to <see cref="BoardWidth"/>.
        /// </param>
        /// <param name="y">
        /// The desired y coordinate; greater than 0 and less than or equal to <see cref="BoardHeight"/>.
        /// <param name="cell">The new cell.</param>
        /// <returns>The cell at the provided coordinates.</returns>
        private void SetAt(int x, int y, Cell cell)
        {
            Debug.Assert(x > 0 && x <= BoardWidth && y > 0 && y <= BoardHeight);
            _cells[x - 1, y - 1] = cell;
        }

        /// <summary>
        /// Sets a cell of the chess board.
        /// </summary>
        /// <param name="loc">
        /// Each coordinate should be greater than 0, the X coordinate 
        /// less than or equal to <see cref="BoardWidth"/>, and the Y 
        /// coordinate less than or equal to <see cref="BoardHeight"/>.
        /// </param>
        /// <param name="cell">The new cell.</param>
        /// <returns>The cell at the provided coordinates.</returns>
        private void SetAt(Vector2 loc, Cell cell)
        {
            SetAt(loc.X, loc.Y, cell);
        }

        /// <summary>
        /// Populates each cell with the relevant starting pieces.
        /// </summary>
        private void SetupBoard()
        {
            SetupBackLine(Team.Black);
            SetupPawnLine(Team.Black);
            SetupBackLine(Team.White);
            SetupPawnLine(Team.White);

            // The remaining cells need to be empty.
            for (int x = 1; x <= BoardWidth; ++x)
            {
                for (int y = 3; y <= BoardHeight - 2; ++y)
                {
                    At(x, y).Occupant = null;
                }
            }
        }

        /// <summary>
        /// Sets up a back line in the tradition chess order, which is:
        /// Rook -> Knight -> Bishop -> Queen -> King -> Bishop -> Knight -> Rook
        /// </summary>
        /// <param name="team">The provided team. Should not be <see cref="Team.Invalid"/>.</param>
        private void SetupBackLine(Team team)
        {
            Debug.Assert(team != Team.Invalid);
            int rank = team == Team.White ? 1 : BoardHeight;

            At(1, rank).Occupant = _pieceFactory.GetObject<Rook>();
            At(2, rank).Occupant = _pieceFactory.GetObject<Knight>();
            At(3, rank).Occupant = _pieceFactory.GetObject<Bishop>();
            At(4, rank).Occupant = _pieceFactory.GetObject<Queen>();
            At(5, rank).Occupant = _pieceFactory.GetObject<King>();
            At(6, rank).Occupant = _pieceFactory.GetObject<Bishop>();
            At(7, rank).Occupant = _pieceFactory.GetObject<Knight>();
            At(8, rank).Occupant = _pieceFactory.GetObject<Rook>();

            // Set each piece to the correct team.
            for (int x = 1; x <= 8; ++x)
            {
                At(x, rank).Occupant.Team = team;
            }
        }

        /// <summary>
        /// Sets up a pawn line, filling the entire rank with pawns.
        /// </summary>
        /// <param name="team">The provided team. Should not be <see cref="Team.Invalid"/>.</param>
        private void SetupPawnLine(Team team)
        {
            Debug.Assert(team != Team.Invalid);
            int rank = team == Team.White ? 2 : BoardHeight - 1;

            for (int x = 1; x <= BoardWidth; ++x)
            {
                At(x, rank).Occupant = _pieceFactory.GetObject<Pawn>();
                At(x, rank).Occupant.Team = team;
            }
        }
    }
}