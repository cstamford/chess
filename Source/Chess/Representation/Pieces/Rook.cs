using Chess.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Representation.Pieces
{
    /// <summary>
    /// A rook is a piece that is capable of moving horiztonally or vertically.
    /// </summary>
    public class Rook : Piece
    {
        /// <summary>
        /// Gets the textual representation of this rook.
        /// </summary>
        public override string AsText { get { return "R"; } }

        /// <summary>
        /// Constructs a rook with the relative value of 5.1 as per Hans Berliner's system.
        /// </summary>
        /// <param name="team">The requested team.</param>
        public Rook(Team team = Team.Invalid) : base(PieceType.Rook, team, 5.1f)
        {
        }

        /// <summary>
        /// Returns an immutable collection of possible moves for this rook.
        /// </summary>
        /// <param name="currentPosition">The position of the rook.</param>
        /// <returns>An immutable collection containing valid moves for this rook.</returns>
        public override IReadOnlyCollection<Move> ValidMoves(Vector2 currentPosition)
        {
            Debug.Assert(currentPosition.X > 0 && currentPosition.X <= Board.BoardWidth &&
                         currentPosition.Y > 0 && currentPosition.Y <= Board.BoardHeight);

            if (_cachedMoves != null && currentPosition.Equals(_cachedPosition))
            {
                return _cachedMoves;
            }

            _cachedPosition = currentPosition;
            List<Move> moves = new List<Move>();

            // Generate the horizontal moves.
            for (int x = 1; x <= Board.BoardWidth; ++x)
            {
                // Our current position can never be valid.
                if (x == currentPosition.X)
                {
                    continue;
                }

                moves.Add(new Move(currentPosition, new Vector2(x, currentPosition.Y)));
            }

            // Generate the vertical moves.
            for (int y = 1; y <= Board.BoardHeight; ++y)
            {
                // Our current position can never be valid.
                if (y == currentPosition.Y)
                {
                    continue;
                }

                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X, y)));
            }

            _cachedMoves = moves.ToArray();
            return _cachedMoves;
        }
    }
}