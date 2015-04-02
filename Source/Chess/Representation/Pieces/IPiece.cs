using Chess.Framework;
using System.Collections.Generic;

namespace Chess.Representation.Pieces
{
    /// <summary>
    /// Interface describing a chess piece.
    /// </summary>
    public interface IPiece : IReadOnlyPiece
    {
        /// <summary>
        /// Gets or sets the team which this piece belongs to.
        /// </summary>
        new Team Team { get; set; }

        /// <summary>
        /// Gets the list of historical moves for this piece.
        /// </summary>
        new List<Move> Moves { get; }
    }

    /// <summary>
    /// Immutable interface describing a chess piece.
    /// </summary>
    public interface IReadOnlyPiece
    {
        /// <summary>
        /// Gets the team which this piece belongs to.
        /// </summary>
        Team Team { get; }

        /// <summary>
        /// Gets the type of chess piece.
        /// </summary>
        PieceType Type { get; }

        /// <summary>
        /// Gets the relative weight of this piece, as per Hans Berliner's system.
        /// </summary>
        float Value { get; }

        /// <summary>
        /// Gets the textual representation of this piece.
        /// </summary>
        string AsText { get; }

        /// <summary>
        /// Gets the immutable collection of historical moves for this piece.
        /// </summary>
        IReadOnlyCollection<Move> Moves { get; }

        /// <summary>
        /// Returns an immutable collection of possible moves for this piece.
        /// </summary>
        /// <param name="currentPosition">The position of the piece.</param>
        /// <returns>An immutable collection containing valid moves for this piece.</returns>
        IReadOnlyCollection<Move> ValidMoves(Vector2 currentPosition);
    }
}