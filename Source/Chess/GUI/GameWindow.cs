using Chess.Framework;
using Chess.Representation;
using Chess.Representation.Pieces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace Chess.GUI
{
    /// <summary>
    /// The delegate that notifies subscribers when a cell has been selected.
    /// </summary>
    /// <param name="cell">The cell that the piece was selected in.</param>
    public delegate void CellDownDelegate(IReadOnlyCell cell);

    /// <summary>
    /// The delegate that notifies subscribers when a cell has been deselected.
    /// </summary>
    /// <param name="from">The cell that the piece was selected in.</param>
    /// <param name="to">The cell that the piece was deselected in.</param>
    public delegate void CellUpDelegate(IReadOnlyCell from, IReadOnlyCell to);

    /// <summary>
    /// The delegate that is used to fetch a list of valid moves to render.
    /// </summary>
    /// </summary>
    /// <param name="cell">The cell containing the piece.</param>
    /// <param name="filterTeam">
    /// If true, only returns a list when it is the current unit's turn to move.
    /// </param>
    /// <returns>A list of valid moves for the piece in the given cell.</returns>
    public delegate IReadOnlyCollection<Move> ValidMoveDelegate(IReadOnlyCell cell, bool filterTeam = true);

    /// <summary>
    /// A custom control which handles drawing the chess board.
    /// Exposes a series of events.
    /// </summary>
    public class GameWindow : UserControl
    {
        /// <summary>
        /// A list of cells comprising the chess board.
        /// Should be set in order to render.
        /// </summary>
        public IReadOnlyCell[,] CellList
        {
            get { return _cellList; }
            set { _cellList = value; Invalidate(); }
        }

        /// <summary>
        /// Notifies subscribers when a cell has been selected.
        /// </summary>
        public event CellDownDelegate OnCellDown
        {
            add { _onCellDown += value; }
            remove { _onCellDown -= value; }
        }

        /// <summary>
        /// Notifies subscribers when a cell has been deselected.
        /// </summary>
        public event CellUpDelegate OnCellUp
        {
            add { _onCellUp += value; }
            remove { _onCellUp -= value; }
        }

        /// <summary>
        /// Provides a list of valid moves to render.
        /// </summary>
        public ValidMoveDelegate ValidMoveHandler
        {
            set { _validMoveHandler = value; }
        }

        /// <summary>
        /// Constructs a GameWindow and loads the assets.
        /// </summary>
        public GameWindow()
        {
            // Set up double buffering and make this control selectable
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.Selectable, true);

            try
            {
                LoadAssets();
            }
            catch
            {
                // Intentionally left empty. 
                // If loading the assets fail, the textual representation will be drawn instead.
                // We can recover from this exception with no handling.
            }
        }

        /// <summary>
        /// The default size of this control.
        /// </summary>
        protected override Size DefaultSize
        {
            get { return new Size(CellRenderWidth * Board.BoardWidth, CellRenderHeight * Board.BoardHeight); }
        }

        /// <summary>
        /// Handles repainting this control.
        /// Draws the background, foreground possible moves, and then the pieces.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_cellList == null)
            {
                return;
            }

            Debug.Assert(_cellList.Length == Board.BoardWidth * Board.BoardHeight);

            e.Graphics.Clear(SystemColors.ControlDark);
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            e.Graphics.SmoothingMode = SmoothingMode.None;

            bool lightBg = true;

            // Draw the background; alternating between two colours.
            for (int y = 0; y < Board.BoardHeight; ++y)
            {
                for (int x = 0; x < Board.BoardWidth; ++x)
                {
                    int renderX = x * CellRenderWidth;
                    int renderY = y * CellRenderHeight;
                    Rectangle renderRect = new Rectangle(renderX, renderY, CellRenderWidth, CellRenderHeight);

                    if (lightBg)
                    {
                        e.Graphics.FillRectangle(Brushes.SandyBrown, renderRect);
                        lightBg = false;
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.SaddleBrown, renderRect);
                        lightBg = true;
                    }
                }

                lightBg = !lightBg;
            }

            // Draw the overview of valid moves.
            if (_selectedCell != null && _selectedCell.Occupant != null)
            {
                IReadOnlyCollection<Move> validMoves;

                if (_validMoveHandler != null)
                {
                    validMoves = _validMoveHandler(_selectedCell);
                }
                else
                {
                    validMoves = _selectedCell.Occupant.ValidMoves(_selectedCell.Position);
                }

                Debug.Assert(validMoves != null);

                foreach (Move move in validMoves)
                {
                    Vector2 renderCoords = GridSpaceToRenderSpace(move.To);
                    Rectangle renderRect = new Rectangle(renderCoords.X, renderCoords.Y, CellRenderWidth, CellRenderHeight);
                    e.Graphics.FillRectangle(Brushes.Orange, renderRect);
                }
            }

            // Draw each cell.
            foreach (IReadOnlyCell cell in _cellList)
            {
                IReadOnlyPiece piece = cell.Occupant;

                if (piece != null)
                {
                    Vector2 renderCoords = GridSpaceToRenderSpace(cell.Position);
                    Rectangle renderRect = new Rectangle(renderCoords.X, renderCoords.Y, CellRenderWidth, CellRenderHeight);

                    Debug.Assert(piece.Team != Team.Invalid);
                    Dictionary<PieceType, Bitmap> graphics = piece.Team == Team.Black ? _blackGraphics : _whiteGraphics;

                    if (graphics.ContainsKey(piece.Type) && graphics[piece.Type] != null)
                    {
                        // Grab the asset and draw it.
                        e.Graphics.DrawImage(graphics[piece.Type], renderRect);
                    }
                    else
                    {
                        // No asset found -- draw the text representation instead.
                        StringFormat format = new StringFormat();
                        format.LineAlignment = StringAlignment.Center;
                        format.Alignment = StringAlignment.Center;
                        e.Graphics.DrawString(piece.AsText, Font, Brushes.Red, renderRect, format);
                    }
                }
            }
        }

        /// <summary>
        /// Handles responding to the MouseDown event.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Focus();

            if (_cellList == null)
            {
                return;
            }

            Vector2 loc = RenderSpaceToGridSpace(new Vector2(e.X, e.Y));
            _selectedCell = _cellList[loc.X, loc.Y];

            if (_onCellDown != null)
            {
                _onCellDown(_selectedCell);
            }

            Invalidate();
        }

        /// <summary>
        /// Handles responding to the MouseUp event.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Focus();

            IReadOnlyCell other = null;

            if (_cellList != null)
            {
                Vector2 loc = RenderSpaceToGridSpace(new Vector2(e.X, e.Y));
                other = _cellList[loc.X, loc.Y];
            }

            if (_onCellUp != null)
            {
                _onCellUp(_selectedCell, other);
            }

            _selectedCell = null;

            Invalidate();
        }

        /// <summary>
        /// The width to render each cell of the chess board.
        /// </summary>
        private const int CellRenderWidth = 64;

        /// <summary>
        /// The height to render each cell of the chess board.
        /// </summary>
        private const int CellRenderHeight = 64;

        /// <summary>
        /// A list of cells comprising the chess board.
        /// </summary>
        private IReadOnlyCell[,] _cellList;

        /// <summary>
        /// Notifies subscribers when a cell has been selected.
        /// </summary>
        private event CellDownDelegate _onCellDown;

        /// <summary>
        /// Notifies subscribers when a cell has been deselected.
        /// </summary>
        private event CellUpDelegate _onCellUp;

        /// <summary>
        /// Provides a list of valid moves to render.
        /// </summary>
        private ValidMoveDelegate _validMoveHandler;

        /// <summary>
        /// A map containing each bitmap file for the white team.
        /// </summary>
        private Dictionary<PieceType, Bitmap> _whiteGraphics = new Dictionary<PieceType, Bitmap>();

        /// <summary>
        /// A map containing each bitmap file for the black team.
        /// </summary>
        private Dictionary<PieceType, Bitmap> _blackGraphics = new Dictionary<PieceType, Bitmap>();

        /// <summary>
        /// The currently selected cell.
        /// </summary>
        private IReadOnlyCell _selectedCell = null;

        /// <summary>
        /// Loads all of the assets into the relevant maps.
        /// Note that the resources should be embedded into the assembly.
        /// </summary>
        private void LoadAssets()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            // Black pieces.
            _blackGraphics[PieceType.Bishop] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.black_bishop.png"));
            _blackGraphics[PieceType.King] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.black_king.png"));
            _blackGraphics[PieceType.Knight] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.black_knight.png"));
            _blackGraphics[PieceType.Pawn] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.black_pawn.png"));
            _blackGraphics[PieceType.Queen] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.black_queen.png"));
            _blackGraphics[PieceType.Rook] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.black_rook.png"));

            // White pieces.
            _whiteGraphics[PieceType.Bishop] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.white_bishop.png"));
            _whiteGraphics[PieceType.King] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.white_king.png"));
            _whiteGraphics[PieceType.Knight] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.white_knight.png"));
            _whiteGraphics[PieceType.Pawn] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.white_pawn.png"));
            _whiteGraphics[PieceType.Queen] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.white_queen.png"));
            _whiteGraphics[PieceType.Rook] = new Bitmap(thisAssembly.GetManifestResourceStream("Chess.Assets.white_rook.png"));
        }

        /// <summary>
        /// Converts grid space to render space.
        /// This is needed because chess notation uses ascending y-axis order (origin bottom left),
        /// whereas WinForms notation uses descending y-axis order (origin top left).
        /// </summary>
        /// <param name="grid">The coordinates in grid space.</param>
        /// <returns>The transformed coordinates in render space.</returns>
        private Vector2 GridSpaceToRenderSpace(Vector2 grid)
        {
            int x = (grid.X - 1) * CellRenderWidth;
            int y = ((CellRenderHeight * Board.BoardHeight) - CellRenderHeight) - ((grid.Y - 1) * CellRenderHeight);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Converts render space to grid space.
        /// This is needed because chess notation uses ascending y-axis order (origin bottom left),
        /// whereas WinForms notation uses descending y-axis order (origin top left).
        /// </summary>
        /// <param name="render">The coordinates in render space.</param>
        /// <returns>The transformed coordinates in grid space.</returns>
        private Vector2 RenderSpaceToGridSpace(Vector2 render)
        {
            int cellX = render.X / CellRenderWidth;
            cellX = cellX < 0 ? 0 : cellX;
            cellX = cellX >= Board.BoardWidth ? Board.BoardWidth - 1 : cellX;

            int cellY = ((CellRenderHeight * Board.BoardHeight) - render.Y) / CellRenderHeight;
            cellY = cellY < 0 ? 0 : cellY;
            cellY = cellY >= Board.BoardHeight ? Board.BoardHeight - 1 : cellY;

            return new Vector2(cellX, cellY);
        }
    }
}