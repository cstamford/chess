using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ChessUnitTests.Pieces
{
    /// <summary>
    /// A series of unit tests for a bishop.
    /// </summary>
    [TestClass]
    public class BishopTest
    {
        /// <summary>
        /// Ensures that a fresh board is correctly populated with bishops.
        /// </summary>
        [TestMethod]
        public void BishopStartingPositions()
        {
            Board board = new Board();

            IReadOnlyPiece blackLeftBishop = board[3, 8].Occupant;
            Assert.IsTrue(blackLeftBishop.Type == PieceType.Bishop && blackLeftBishop.Team == Team.Black);

            IReadOnlyPiece blackRightBishop = board[6, 8].Occupant;
            Assert.IsTrue(blackRightBishop.Type == PieceType.Bishop && blackRightBishop.Team == Team.Black);

            IReadOnlyPiece whiteLeftBishop = board[3, 1].Occupant;
            Assert.IsTrue(whiteLeftBishop.Type == PieceType.Bishop && whiteLeftBishop.Team == Team.White);

            IReadOnlyPiece whiteRightBishop = board[6, 1].Occupant;
            Assert.IsTrue(whiteRightBishop.Type == PieceType.Bishop && whiteRightBishop.Team == Team.White);
        }

        /// <summary>
        /// Ensures that a bishop produces a valid movement set.
        /// </summary>
        [TestMethod]
        public void BishopValidMoves()
        {
            IPiece bishop = new Bishop();
            Vector2 bishopPos = new Vector2(4, 4);
            IReadOnlyCollection<Move> bishopMoves = bishop.ValidMoves(bishopPos);

            Assert.IsTrue(bishopMoves.Count == 13);

            // Our current position shouldn't be a valid move.
            Assert.IsFalse(bishopMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 4));

            // Check the diagonal centre up to the right.
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 5));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 6));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 7 && vec.To.Y == 7));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 8 && vec.To.Y == 8));

            // Check the diagonal centre down to the right.
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 5 && vec.To.Y == 3));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 6 && vec.To.Y == 2));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 7 && vec.To.Y == 1));

            // Check the diagonal centre down to the left.
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 3));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 2));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 1 && vec.To.Y == 1));

            // Check the diagonal centre up to the left.
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 3 && vec.To.Y == 5));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 2 && vec.To.Y == 6));
            Assert.IsTrue(bishopMoves.Any(vec => vec.To.X == 1 && vec.To.Y == 7));
        }
    }
}