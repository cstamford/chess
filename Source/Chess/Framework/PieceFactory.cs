using Chess.Representation;
using Chess.Representation.Pieces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.Framework
{
    /// <summary>
    /// Extends the <see cref="ObjectFactory"/> class to facilitate easy
    /// resetting of chess pieces.
    /// </summary>
    public class PieceFactory : ObjectFactory<IPiece>
    {
        /// <summary>
        /// Constructs the factory and populates it with the appropriate amount of pieces
        /// for a traditionally sized chess board.
        /// </summary>
        public PieceFactory() : base(GeneratePieces())
        {
        }

        /// <summary>
        /// Resets an IPiece to its default state.
        /// </summary>
        /// <param name="cacheEntry">The entry to reset.</param>
        protected override void ResetEntry(ObjectFactoryCacheEntry cacheEntry)
        {
            Debug.Assert(cacheEntry != null);

            cacheEntry.Used = false;
            cacheEntry.CachedObject.Team = Team.Invalid;
            cacheEntry.CachedObject.Moves.Clear();
        }

        /// <summary>
        /// Prepares a cache entry to be released to a caller.
        /// </summary>
        /// <param name="cacheEntry">The entry to release.</param>
        protected override void PrepareEntry(ObjectFactoryCacheEntry cacheEntry)
        {
            Debug.Assert(cacheEntry != null);
            cacheEntry.Used = true;
        }

        /// <summary>
        /// Generates an appropriately sized collection of chess pieces.
        /// Contains a large number of queens to account for piece promotion rules.
        /// </summary>
        /// <returns>An array of chess pieces.</returns>
        private static IPiece[] GeneratePieces()
        {
            List<IPiece> pieces = new List<IPiece>();
            pieces.AddRange(Enumerable.Range(0, 6).Select(obj => new Bishop()));
            pieces.AddRange(Enumerable.Range(0, 2).Select(obj => new King()));
            pieces.AddRange(Enumerable.Range(0, 6).Select(obj => new Knight()));
            pieces.AddRange(Enumerable.Range(0, 16).Select(obj => new Pawn()));
            pieces.AddRange(Enumerable.Range(0, 18).Select(obj => new Queen()));
            pieces.AddRange(Enumerable.Range(0, 6).Select(obj => new Rook()));
            return pieces.ToArray();
        }
    }
}
