using Chess.Framework;
using Chess.Representation.Pieces;

namespace Chess.Representation
{
    /// <summary>
    /// Interface describing a cell of a chess board.
    /// </summary>
    public interface ICell : IReadOnlyCell
    {
        /// <summary>
        /// Gets or sets the position of this cell.
        /// </summary>
        new Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the current chess piece occupying this cell. Null if empty.
        /// </summary>
        new IPiece Occupant { get; set; }

        /// <summary>
        /// Gets or sets the relative weight of this cell.
        /// </summary>
        new float Weight { get; set; }
    }

    /// <summary>
    /// Immutable interface describing a cell of a chess board.
    /// </summary>
    public interface IReadOnlyCell
    {
        /// <summary>
        /// Gets the position of this cell. 
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the current chess piece occupying this cell. Null if empty.
        /// </summary>
        IReadOnlyPiece Occupant { get; }

        /// <summary>
        /// Gets the relative weight of this cell.
        /// </summary>
        float Weight { get; }
    }
}
