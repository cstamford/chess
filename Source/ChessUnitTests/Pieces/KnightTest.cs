using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;


namespace ChessUnitTests.Pieces
{
    /// <summary>
    /// A series of unit tests for a knight.
    /// </summary>
    [TestClass]
    public class KnightTest
    {
        /// <summary>
        /// Ensures that a fresh board is correctly populated with knights.
        /// </summary>
        [TestMethod]
        public void KnightStartingPositions()
        {
            Board board = new Board();

            IReadOnlyPiece blackLeftKnight = board[2, 8].Occupant;
            Assert.IsTrue(blackLeftKnight.Type == PieceType.Knight && blackLeftKnight.Team == Team.Black);

            IReadOnlyPiece blackRightKnight = board[7, 8].Occupant;
            Assert.IsTrue(blackRightKnight.Type == PieceType.Knight && blackRightKnight.Team == Team.Black);

            IReadOnlyPiece whiteLeftKnight = board[2, 1].Occupant;
            Assert.IsTrue(whiteLeftKnight.Type == PieceType.Knight && whiteLeftKnight.Team == Team.White);

            IReadOnlyPiece whiteRightKnight = board[7, 1].Occupant;
            Assert.IsTrue(whiteRightKnight.Type == PieceType.Knight && whiteRightKnight.Team == Team.White);
        }

        /// <summary>
        /// Ensures that a knight produces a valid movement set.
        /// </summary>
        [TestMethod]
        public void KnightValidMoves()
        {
            IPiece knight = new Knight();
            Vector2 knightPos = new Vector2(4, 4);
            IReadOnlyCollection<Move> knightMoves = knight.ValidMoves(knightPos);

            Assert.IsTrue(knightMoves.Count == 8);
            Assert.IsFalse(knightMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 4));

            // Top.
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 6));
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 6));

            // Right.
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 3));
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 5));

            // Bottom.
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 2));
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 2));

            // Left.
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 3));
            Assert.IsTrue(knightMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 5));
        }
    }
}
