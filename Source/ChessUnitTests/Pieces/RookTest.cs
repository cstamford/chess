using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ChessUnitTests.Pieces
{
    /// <summary>
    /// A series of unit tests for a rook.
    /// </summary>
    [TestClass]
    public class RookTest
    {
        /// <summary>
        /// Ensures that a fresh board is correctly populated with rooks.
        /// </summary>
        [TestMethod]
        public void RookStartingPositions()
        {
            Board board = new Board();

            IReadOnlyPiece blackLeftRook = board[1, 8].Occupant;
            Assert.IsTrue(blackLeftRook.Type == PieceType.Rook && blackLeftRook.Team == Team.Black);

            IReadOnlyPiece blackRightRook = board[8, 8].Occupant;
            Assert.IsTrue(blackRightRook.Type == PieceType.Rook && blackRightRook.Team == Team.Black);

            IReadOnlyPiece whiteLeftRook = board[1, 1].Occupant;
            Assert.IsTrue(whiteLeftRook.Type == PieceType.Rook && whiteLeftRook.Team == Team.White);

            IReadOnlyPiece whiteRightRook = board[8, 1].Occupant;
            Assert.IsTrue(whiteRightRook.Type == PieceType.Rook && whiteRightRook.Team == Team.White);
        }

        /// <summary>
        /// Ensures that a rook produces a valid movement set.
        /// </summary>
        [TestMethod]
        public void RookValidMoves()
        {
            IPiece rook = new Rook();
            Vector2 rookPos = new Vector2(4, 4);
            IReadOnlyCollection<Move> rookMoves = rook.ValidMoves(rookPos);

            Assert.IsTrue(rookMoves.Count == 14);

            // Our current position shouldn't be a valid move.
            Assert.IsFalse(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 4));

            // Check the horizontal moves.
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 1 && vec.To.Y == 4));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 4));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 4));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 4));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 4));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 7 && vec.To.Y == 4));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 8 && vec.To.Y == 4));

            // Check the vertical moves.
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 1));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 2));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 3));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 5));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 6));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 7));
            Assert.IsTrue(rookMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 8));
        }
    }
}
