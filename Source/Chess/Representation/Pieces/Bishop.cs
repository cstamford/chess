using Chess.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Representation.Pieces
{
    /// <summary>
    /// A bishop is a piece that is capable of moving diagonally.
    /// </summary>
    public class Bishop : Piece
    {
        /// <summary>
        /// Gets the textual representation of this bishop.
        /// </summary>
        public override string AsText { get { return "B"; } }

        /// <summary>
        /// Constructs a bishop with the relative value of 3.33 as per Hans Berliner's system.
        /// </summary>
        /// <param name="team">The requested team.</param>
        public Bishop(Team team = Team.Invalid) : base(PieceType.Bishop, team, 3.33f)
        {
        }

        /// <summary>
        /// Returns an immutable collection of possible moves for this bishop.
        /// </summary>
        /// <param name="currentPosition">The position of the bishop.</param>
        /// <returns>An immutable collection containing valid moves for this bishop.</returns>
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

            // Add the diagonal centre up to the right.
            for (int x = currentPosition.X + 1, y = currentPosition.Y + 1;
                 x <= Board.BoardWidth && y <= Board.BoardHeight; ++x, ++y)
            {
                moves.Add(new Move(currentPosition, new Vector2(x, y)));
            }

            // Add the diagonal centre down to the right.
            for (int x = currentPosition.X + 1, y = currentPosition.Y - 1;
                 x <= Board.BoardWidth && y > 0; ++x, --y)
            {
                moves.Add(new Move(currentPosition, new Vector2(x, y)));
            }

            // Add the diagonal centre down to the left.
            for (int x = currentPosition.X - 1, y = currentPosition.Y - 1;
                 x > 0 && y > 0; --x, --y)
            {
                moves.Add(new Move(currentPosition, new Vector2(x, y)));
            }

            // Add the diagonal centre up to the left.
            for (int x = currentPosition.X - 1, y = currentPosition.Y + 1;
                 x > 0 && y <= Board.BoardHeight; --x, ++y)
            {
                moves.Add(new Move(currentPosition, new Vector2(x, y)));
            }

            _cachedMoves = moves.ToArray();
            return _cachedMoves;
        }
    }
}