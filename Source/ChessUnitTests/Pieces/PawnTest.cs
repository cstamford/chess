using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ChessUnitTests.Pieces
{
    /// <summary>
    /// A series of unit tests for a pawn.
    /// </summary>
    [TestClass]
    public class PawnTest
    {
        /// <summary>
        /// Ensures that a fresh board is correctly populated with pawns.
        /// </summary>
        [TestMethod]
        public void PawnStartingPositions()
        {
            Board board = new Board();

            for (int x = 1; x <= 8; ++x)
            {
                IReadOnlyPiece blackPawn = board[x, 7].Occupant;
                Assert.IsTrue(blackPawn.Type == PieceType.Pawn && blackPawn.Team == Team.Black);
            }

            for (int x = 1; x <= 8; ++x)
            {
                IReadOnlyPiece whitePawn = board[x, 2].Occupant;
                Assert.IsTrue(whitePawn.Type == PieceType.Pawn && whitePawn.Team == Team.White);
            }
        }

        /// <summary>
        /// Ensures that a black pawn produces a valid movement set.
        /// </summary>
        [TestMethod]
        public void PawnBlackValidMoves()
        {
            IPiece pawn = new Pawn(Team.Black);
            Vector2 pawnPos = new Vector2(4, 4);
            IReadOnlyCollection<Move> pawnMoves = pawn.ValidMoves(pawnPos);

            Assert.IsTrue(pawnMoves.Count == 2);

            Assert.IsFalse(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 4));
            Assert.IsTrue(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 3));
            Assert.IsTrue(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 2));

            Vector2 newPos = new Vector2(4, 2);
            pawn.Moves.Add(new Move(pawnPos, newPos));
            pawnMoves = pawn.ValidMoves(newPos);

            Assert.IsTrue(pawnMoves.Count == 1);

            Assert.IsFalse(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 2));
            Assert.IsTrue(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 1));
        }

        /// <summary>
        /// Ensures that a white pawn produces a valid movement set.
        /// </summary>
        [TestMethod]
        public void PawnWhiteValidMoves()
        {
            IPiece pawn = new Pawn(Team.White);
            Vector2 pawnPos = new Vector2(4, 4);
            IReadOnlyCollection<Move> pawnMoves = pawn.ValidMoves(pawnPos);

            Assert.IsTrue(pawnMoves.Count == 2);

            Assert.IsFalse(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 4));
            Assert.IsTrue(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 5));
            Assert.IsTrue(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 6));

            Vector2 newPos = new Vector2(4, 6);
            pawn.Moves.Add(new Move(pawnPos, newPos));
            pawnMoves = pawn.ValidMoves(newPos);

            Assert.IsTrue(pawnMoves.Count == 1);

            Assert.IsFalse(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 6));
            Assert.IsTrue(pawnMoves.Any(vec => vec.To.X == 4 && vec.To.Y == 7));
        }
    }
}
