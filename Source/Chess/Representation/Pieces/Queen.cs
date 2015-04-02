using Chess.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Representation.Pieces
{
    /// <summary>
    /// A queen is a piece that can move horizontally, vertically, and diagonally.
    /// </summary>
    public class Queen : Piece
    {
        /// <summary>
        /// Gets the textual representation of this queen.
        /// </summary>
        public override string AsText { get { return "Q"; } }

        /// <summary>
        /// Constructs a queen with the relative value of 8.8 as per Hans Berliner's system.
        /// </summary>
        /// <param name="team">The requested team.</param>
        public Queen(Team team = Team.Invalid) : base(PieceType.Queen, team, 8.8f)
        {
        }

        /// <summary>
        /// Returns an immutable collection of possible moves for this queen.
        /// </summary>
        /// <param name="currentPosition">The position of the queen.</param>
        /// <returns>An immutable collection containing valid moves for this queen.</returns>
        public override IReadOnlyCollection<Move> ValidMoves(Vector2 currentPosition)
        {
            Debug.Assert(currentPosition.X > 0 && currentPosition.X <= Board.BoardWidth &&
                         currentPosition.Y > 0 && currentPosition.Y <= Board.BoardHeight);

            if (_cachedMoves != null && currentPosition.Equals(_cachedPosition))
            {
                return _cachedMoves;
            }

            _cachedPosition = currentPosition;

            // Horizontal/vertical movement.
            IReadOnlyCollection<Move> rookMovement = new Rook().ValidMoves(currentPosition);

            // Diagonal movement.
            IReadOnlyCollection<Move> bishopMovement = new Bishop().ValidMoves(currentPosition);

            // Because a queen has the movement set of a rook and a bishop combined,
            // we can simply combine both of those movement sets.
            _cachedMoves = rookMovement.Concat(bishopMovement).ToArray();
            return _cachedMoves;
        }
    }
}