using Chess.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Representation.Pieces
{
    /// <summary>
    /// A king is a piece that is capable of moving two squares in one direction, and then 
    /// one square left and right from that direction.
    /// </summary>
    public class Knight : Piece
    {
        /// <summary>
        /// Gets the textual representation of this knight.
        /// </summary>
        public override string AsText { get { return "Kt"; } }

        /// <summary>
        /// Constructs a knight with the relative value of 3.2 as per Hans Berliner's system.
        /// </summary>
        /// <param name="team">The requested team.</param>
        public Knight(Team team = Team.Invalid) : base(PieceType.Knight, team, 3.2f)
        {
        }

        /// <summary>
        /// Returns an immutable collection of possible moves for this knight.
        /// </summary>
        /// <param name="currentPosition">The position of the knight.</param>
        /// <returns>An immutable collection containing valid moves for this knight.</returns>
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

            // Up moves.
            if (currentPosition.Y + 2 <= Board.BoardHeight)
            {
                // Left.
                if (currentPosition.X - 1 > 0)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X - 1, currentPosition.Y + 2)));
                }

                // Right.
                if (currentPosition.X + 1 <= Board.BoardWidth)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X + 1, currentPosition.Y + 2)));
                }
            }

            // Right moves.
            if (currentPosition.X + 2 <= Board.BoardWidth)
            {
                // Up.
                if (currentPosition.Y + 1 <= Board.BoardHeight)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X + 2, currentPosition.Y + 1)));
                }

                // Down.
                if (currentPosition.Y - 1 > 0)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X + 2, currentPosition.Y - 1)));
                }
            }

            // Left moves.
            if (currentPosition.X - 2 > 0)
            {
                // Up.
                if (currentPosition.Y + 1 <= Board.BoardHeight)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X - 2, currentPosition.Y + 1)));
                }

                // Down.
                if (currentPosition.Y - 1 > 0)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X - 2, currentPosition.Y - 1)));
                }
            }

            // Down moves.
            if (currentPosition.Y - 2 > 0)
            {
                // Left.
                if (currentPosition.X - 1 > 0)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X - 1, currentPosition.Y - 2)));
                }

                // Right.
                if (currentPosition.X + 1 <= Board.BoardWidth)
                {
                    moves.Add(new Move(currentPosition, new Vector2(currentPosition.X + 1, currentPosition.Y - 2)));
                }
            }

            _cachedMoves = moves.ToArray();
            return _cachedMoves;
        }
    }
}