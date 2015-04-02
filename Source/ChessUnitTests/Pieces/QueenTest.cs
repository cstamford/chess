using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ChessUnitTests.Pieces
{
    /// <summary>
    /// A series of unit tests for a queen.
    /// </summary>
    [TestClass]
    public class QueenTest
    {
        /// <summary>
        /// Ensures that a fresh board is correctly populated with queens.
        /// </summary>
        [TestMethod]
        public void QueenStartingPositions()
        {
            Board board = new Board();

            IReadOnlyPiece blackQueen = board[4, 8].Occupant;
            Assert.IsTrue(blackQueen.Type == PieceType.Queen && blackQueen.Team == Team.Black);

            IReadOnlyPiece whiteQueen = board[4, 1].Occupant;
            Assert.IsTrue(whiteQueen.Type == PieceType.Queen && whiteQueen.Team == Team.White);
        }

        /// <summary>
        /// Ensures that a queen produces a valid movement set.
        /// </summary>
        [TestMethod]
        public void QueenValidMoves()
        {
            IPiece queen = new Queen();
            Vector2 queenPos = new Vector2(4, 4);
            IReadOnlyCollection<Move> queenMoves = queen.ValidMoves(queenPos);

            Assert.IsTrue(queenMoves.Count == 27);

            // Our current position shouldn't be a valid move.
            Assert.IsFalse(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 4));

            // Check the horizontal moves.
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 1 && vec.To.Y == 4));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 4));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 4));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 4));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 4));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 7 && vec.To.Y == 4));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 8 && vec.To.Y == 4));

            // Check the vertical moves.
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 1));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 2));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 3));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 5));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 6));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 7));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 8));

            // Check the diagonal centre up to the right.
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 5));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 6));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 7 && vec.To.Y == 7));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 8 && vec.To.Y == 8));

            // Check the diagonal centre down to the right.
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 3));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 2));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 7 && vec.To.Y == 1));

            // Check the diagonal centre down to the left.
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 3));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 2));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 1 && vec.To.Y == 1));

            // Check the diagonal centre up to the left.
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 5));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 6));
            Assert.IsTrue(queenMoves.Any(vec => vec.To.X == 1 && vec.To.Y == 7));
        }
    }
}
