using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ChessUnitTests.Pieces
{
    /// <summary>
    /// A series of unit tests for a king.
    /// </summary>
    [TestClass]
    public class KingTest
    {
        /// <summary>
        /// Ensures that a fresh board is correctly populated with kings.
        /// </summary>
        [TestMethod]
        public void KingStartingPosition()
        {
            Board board = new Board();

            IReadOnlyPiece blackKing = board[5, 8].Occupant;
            Assert.IsTrue(blackKing.Type == PieceType.King && blackKing.Team == Team.Black);

            IReadOnlyPiece whiteKing = board[5, 1].Occupant;
            Assert.IsTrue(whiteKing.Type == PieceType.King && whiteKing.Team == Team.White);
        }

        /// <summary>
        /// Ensures that a king produces a valid movement set.
        /// </summary>
        [TestMethod]
        public void KingValidMoves()
        {
            IPiece king = new King();
            Vector2 kingPos = new Vector2(4, 4);
            IReadOnlyCollection<Move> kingMoves = king.ValidMoves(kingPos);

            Assert.IsTrue(kingMoves.Count == 8);
            Assert.IsFalse(kingMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 4));

            // Top.
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 5));

            // Right side.
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 5));
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 4));
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 3));

            // Bottom.
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 3));

            // Left side.
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 3));
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 4));
            Assert.IsTrue(kingMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 5));
        }
    }
}
