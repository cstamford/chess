using Chess.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Representation.Pieces
{
    /// <summary>
    /// A king is a piece that is capable of moving one square in any direction.
    /// </summary>
    public class King : Piece
    {
        /// <summary>
        /// Gets the textual representation of this king.
        /// </summary>
        public override string AsText { get { return "K"; } }

        /// <summary>
        /// Constructs a king. The king has no value under Hans Berliner's system.
        /// </summary>
        /// <param name="team">The requested team.</param>
        public King(Team team = Team.Invalid) : base(PieceType.King, team, 0.0f)
        {
        }

        /// <summary>
        /// Returns an immutable collection of possible moves for this king.
        /// </summary>
        /// <param name="currentPosition">The position of the king.</param>
        /// <returns>An immutable collection containing valid moves for this king.</returns>
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
            
            // Up.
            if (currentPosition.Y + 1 <= Board.BoardHeight)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X, currentPosition.Y + 1)));
            }

            // Up-right.
            if (currentPosition.X + 1 <= Board.BoardWidth && currentPosition.Y + 1 <= Board.BoardHeight)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X + 1, currentPosition.Y + 1)));
            }

            // Right.
            if (currentPosition.X + 1 <= Board.BoardWidth)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X + 1, currentPosition.Y)));
            }

            // Down-right.
            if (currentPosition.X + 1 <= Board.BoardWidth && currentPosition.Y - 1 > 0)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X + 1, currentPosition.Y - 1)));
            }

            // Down.
            if (currentPosition.Y - 1 > 0)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X, currentPosition.Y - 1)));
            }

            // Down-left.
            if (currentPosition.X - 1 > 0 && currentPosition.Y - 1 > 0)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X - 1, currentPosition.Y - 1)));
            }

            // Left.
            if (currentPosition.X - 1 > 0)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X - 1, currentPosition.Y)));
            }

            // Up-left.
            if (currentPosition.X - 1 > 0 && currentPosition.Y + 1 <= Board.BoardHeight)
            {
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X - 1, currentPosition.Y + 1)));
            }

            _cachedMoves = moves.ToArray();
            return _cachedMoves;
        }
    }
}