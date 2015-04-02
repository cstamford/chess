using Chess.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Representation.Pieces
{
    /// <summary>
    /// A pawn is a piece that can move vertically, up for white or down for black.
    /// </summary>
    public class Pawn : Piece
    {
        /// <summary>
        /// Gets the textual representation of this pawn.
        /// </summary>
        public override string AsText { get { return "P"; } }

        /// <summary>
        /// Constructs a pawn with the relative value of 1.0 as per Hans Berliner's system.
        /// </summary>
        /// <param name="team">The requested team.</param>
        public Pawn(Team team = Team.Invalid) : base(PieceType.Pawn, team, 1.0f)
        {
        }

        /// <summary>
        /// Returns an immutable collection of possible moves for this pawn.
        /// </summary>
        /// <param name="currentPosition">The position of the pawn.</param>
        /// <returns>An immutable collection containing valid moves for this pawn.</returns>
        public override IReadOnlyCollection<Move> ValidMoves(Vector2 currentPosition)
        {
            Debug.Assert(_team != Team.Invalid);
            Debug.Assert(currentPosition.X > 0 && currentPosition.X <= Board.BoardWidth &&
                         currentPosition.Y > 0 && currentPosition.Y <= Board.BoardHeight);

            if (_cachedMoves != null && currentPosition.Equals(_cachedPosition))
            {
                return _cachedMoves;
            }

            _cachedPosition = currentPosition;

            int validMoves = 1;

            // If we haven't yet moved, we have the option of moving two steps forwards.
            if (_moves.Count == 0)
            {
                validMoves = 2;
            }

            List<Move> moves = new List<Move>();

            for (int i = 1; i <= validMoves; ++i)
            {
                // If we're black, we have to move down. Otherwise we move up.
                int offset = _team == Team.Black ? -i : i;
                moves.Add(new Move(currentPosition, new Vector2(currentPosition.X, currentPosition.Y + offset)));
            }

            _cachedMoves = moves.ToArray();
            return _cachedMoves;
        }
    }
}