using Chess.Framework;
using Chess.Representation.Pieces;

namespace Chess.Representation
{
    /// <summary>
    /// Represents a cell on a chess board. Implements ICell.
    /// </summary>
    public class Cell : ICell
    {
        /// <summary>
        /// Gets or sets the position of this cell.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the current chess piece occupying this cell. Null if empty.
        /// </summary>
        public IPiece Occupant
        {
            get { return _occupant; }
            set { _occupant = value; }
        }

        /// <summary>
        /// Gets the current chess piece occupying this cell. Null if empty.
        /// </summary>
        IReadOnlyPiece IReadOnlyCell.Occupant
        {
            get { return _occupant; }
        }

        /// <summary>
        /// Gets the relative weight of this cell.
        /// </summary>
        public float Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /// <summary>
        /// Constructs a cell with the given occupant and weight.
        /// </summary>
        /// <param name="position">The position of this cell.</param>
        /// <param name="occupant">The provided occupant. Null if empty.</param>
        /// <param name="weight">The provided weight.</param>
        public Cell(Vector2 position, IPiece occupant = null, float weight = 1.0f)
        {
            _position = position;
            _occupant = occupant;
            _weight = weight;
        }

        /// <summary>
        /// The position of this cell.
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The current chess piece occupying this cell. Null if empty.
        /// </summary>
        private IPiece _occupant;

        /// <summary>
        /// The relative weight of this cell.
        /// </summary>
        private float _weight;
    }
}
