using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess.Representation;
using Chess.Representation.Pieces;

namespace ChessUnitTests
{
    /// <summary>
    /// A series of unit tests for a chess board.
    /// </summary>
    [TestClass]
    public class BoardTest
    {
        /// <summary>
        /// Tests the board's cell access by index.
        /// </summary>
        [TestMethod]
        public void BoardIndexAccess()
        {
            Board board = new Board();

            Assert.IsTrue(board[1, 1].Occupant.Type == PieceType.Rook);
            Assert.IsTrue(board[8, 1].Occupant.Type == PieceType.Rook);
        }

        /// <summary>
        /// Tests the board's <see cref="Board.MovePiece(int, int, int, int)"/> functionality.
        /// </summary>
        [TestMethod]
        public void BoardMovePiece()
        {
            Board board = new Board();

            IReadOnlyPiece rook = board[1, 1].Occupant;
            IReadOnlyPiece pawn = board[1, 2].Occupant;

            Assert.IsTrue(rook.Moves.Count == 0 && rook.Type == PieceType.Rook && rook.Team == Team.White);
            Assert.IsTrue(pawn.Moves.Count == 0 && pawn.Type == PieceType.Pawn && pawn.Team == Team.White);

            board.MovePiece(1, 2, 1, 1);

            IReadOnlyPiece movedPawn = board[1, 1].Occupant;
            IReadOnlyPiece empty = board[1, 2].Occupant;

            Assert.IsTrue(movedPawn.Moves.Count == 1 && movedPawn.Type == PieceType.Pawn && movedPawn.Team == Team.White);
            Assert.IsTrue(empty == null);
        }

        /// <summary>
        /// Tests the board's <see cref="Board.ClearPieceAt(int, int)"/> functionality.
        /// </summary>
        [TestMethod]
        public void BoardClearPieceAt()
        {
            Board board = new Board();

            Assert.IsTrue(board[1, 1].Occupant != null);

            board.ClearPieceAt(1, 1);
            Assert.IsTrue(board[1, 1].Occupant == null);
        }

        /// <summary>
        /// Tests the board's <see cref="Board.SetPieceAt(int, int, PieceType)"/> functionality.
        /// Note that the pawn and king types have not been tested, because there are no spares of
        /// either type in the factory.
        /// </summary>
        [TestMethod]
        public void BoardSetPieceAt()
        {
            Board board = new Board();

            IReadOnlyCell cell = board[1, 1];
            Assert.IsTrue(cell.Occupant.Type == PieceType.Rook);

            board.SetPieceAt(1, 1, PieceType.Bishop);
            Assert.IsTrue(cell.Occupant.Type == PieceType.Bishop && cell.Occupant.Team == Team.White);

            board.SetPieceAt(1, 1, PieceType.Knight);
            Assert.IsTrue(cell.Occupant.Type == PieceType.Knight && cell.Occupant.Team == Team.White);

            board.SetPieceAt(1, 1, PieceType.Queen);
            Assert.IsTrue(cell.Occupant.Type == PieceType.Queen && cell.Occupant.Team == Team.White);

            board.SetPieceAt(1, 1, PieceType.Rook);
            Assert.IsTrue(cell.Occupant.Type == PieceType.Rook && cell.Occupant.Team == Team.White);
        }

        /// <summary>
        /// Tests the board's <see cref="Board.ResetBoard"/> functionality.
        /// </summary>
        [TestMethod]
        public void BoardResetBoard()
        {
            Board board = new Board();

            Assert.IsTrue(board[1, 1].Occupant != null);

            board.MovePiece(1, 2, 1, 1);

            IReadOnlyPiece movedPawn = board[1, 1].Occupant;
            Assert.IsTrue(movedPawn.Moves.Count == 1 && movedPawn.Type == PieceType.Pawn);

            board.ResetBoard();

            IReadOnlyPiece resetRook = board[1, 1].Occupant;
            Assert.IsTrue(resetRook.Moves.Count == 0 && resetRook.Type == PieceType.Rook);

            IReadOnlyPiece originalPawn = board[1, 2].Occupant;
            Assert.IsTrue(originalPawn.Moves.Count == 0 && originalPawn.Type == PieceType.Pawn);
        }
    }
}