using Chess.Game;
using Chess.Representation;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Chess.GUI
{
    /// <summary>
    /// The main form for the chess board GUI.
    /// </summary>
    public partial class BoardForm : Form
    {
        /// <summary>
        /// Constructs a BoardForm and registers all the components.
        /// </summary>
        public BoardForm()
        {
            InitializeComponent();

            SetTimerLabel(new TimeSpan(0, 0, 0));

            _engine.OnCheck += OnCheck;
            _engine.OnCheckCleared += OnCheckCleared;
            _engine.OnCheckmate += OnCheckmate;
            _engine.OnTurnMade += OnTurnMade;

            _gameWindow.CellList = _engine.Board.Cells;
            _gameWindow.OnCellDown += _engine.OnCellDown;
            _gameWindow.OnCellUp += _engine.OnCellUp;
            _gameWindow.ValidMoveHandler = _engine.GetValidMoves;

            _timer.Interval = 1000;
            _timer.Tick += OnTimerElapsed;
            _timer.Start();

            _stopWatch.Start();
        }

        /// <summary>
        /// The chess engine which handles all gameplay.
        /// </summary>
        private Engine _engine = new Engine();

        /// <summary>
        /// A timer used to update the user interface.
        /// </summary>
        private Timer _timer = new Timer();

        /// <summary>
        /// A stopwatch used to trigger an update of the timer every second.
        /// </summary>
        private Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// Handles updating the user interface at the end of each turn.
        /// </summary>
        /// <param name="team">The team that made this turn.</param>
        /// <param name="move">The move made this turn.</param>
        public void OnTurnMade(Team team, Move move)
        {
            Debug.Assert(team != Team.Invalid);

            if (team == Team.White)
            {
                SetActiveTeamLabel(Team.Black);
            }
            else
            {
                SetActiveTeamLabel(Team.White);
            }
        }

        /// <summary>
        /// Called when a unit is in check. Updates the user interface.
        /// </summary>
        /// <param name="king">The king in check.</param>
        private void OnCheck(IReadOnlyCell king)
        {
            SetActiveTeamCheckStatus(king.Occupant.Team, true);
        }

        /// <summary>
        /// Called when a unit is in check. Updates the user interface.
        /// </summary>
        /// <param name="king">The king in check.</param>
        private void OnCheckCleared(IReadOnlyCell king)
        {
            SetActiveTeamCheckStatus(king.Occupant.Team, false);
        }

        /// <summary>
        /// Called when a unit is in checkmate. Prompts the user to play again or exit.
        /// </summary>
        /// <param name="king">The king in checkmate.</param>
        private void OnCheckmate(IReadOnlyCell king)
        {
            Team winner = king.Occupant.Team == Team.Black ? Team.White : Team.Black;

            if (MessageBox.Show(this, string.Format("Game over! {0} wins! Play again?", winner), 
                                "Game Over", MessageBoxButtons.YesNo)  == DialogResult.Yes)
            {
                ResetGame();
            }
            else
            {
                QuitGame();
            }
        }

        /// <summary>
        /// Restarts the game in response to the user's input.
        /// </summary>
        private void OnClickRestartDropDownButton(object sender, EventArgs e)
        {
            ResetGame();
        }

        /// <summary>
        /// Quits teh game in response to the user's input.
        /// </summary>
        private void OnClickQuitDropDownButton(object sender, EventArgs e)
        {
            QuitGame();
        }

        /// <summary>
        /// Triggers an update of the timer label when called.
        /// </summary>
        private void OnTimerElapsed(object sender, EventArgs e)
        {
            SetTimerLabel(_stopWatch.Elapsed);
        }

        /// <summary>
        /// Resets the game to its default state, including user interface elements.
        /// </summary>
        private void ResetGame()
        {
            SetTimerLabel(new TimeSpan(0, 0, 0));
            SetActiveTeamLabel(Team.Black);
            SetActiveTeamCheckStatus(Team.Black, false);
            SetActiveTeamCheckStatus(Team.White, false);
            _engine.ResetGame();
            _stopWatch.Restart();
            _gameWindow.Invalidate();
        }
        
        /// <summary>
        /// Quits the game entirely.
        /// </summary>
        private void QuitGame()
        {
            Application.Exit();
        }

        /// <summary>
        /// Updates the timer label using the timespan provided.
        /// </summary>
        private void SetTimerLabel(TimeSpan time)
        {
            _labelTimer.Text = string.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
        }

        /// <summary>
        /// Emboldens the label matching the provided team to provide
        /// a hint to the player as to the active team.
        /// </summary>
        /// <param name="team">The active team.</param>
        private void SetActiveTeamLabel(Team team)
        {
            if (team == Team.Black)
            {
                _labelBlack.Font = new Font(Font, FontStyle.Bold);
                _labelWhite.Font = new Font(Font, FontStyle.Regular);
            }
            else
            {
                _labelBlack.Font = new Font(Font, FontStyle.Regular);
                _labelWhite.Font = new Font(Font, FontStyle.Bold);
            }
        }

        /// <summary>
        /// Sets the font colour of the label matching the provided team to provide
        /// a hint to the player that their king is in check.
        /// </summary>
        /// <param name="team">The active team.</param>
        /// <param name="check">Whether that team is in check or not.</param>
        private void SetActiveTeamCheckStatus(Team team, bool check)
        {
            if (team == Team.Black)
            {
                if (check)
                {
                    _labelBlack.ForeColor = Color.Red;
                }
                else
                {
                    _labelBlack.ForeColor = Color.Black;
                }
            }
            else
            {
                if (check)
                {
                    _labelWhite.ForeColor = Color.Red;
                }
                else
                {
                    _labelWhite.ForeColor = Color.Black;
                }
            }
        }
    }
}