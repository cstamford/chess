namespace Chess.GUI
{
    partial class BoardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardForm));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._fileDropDrownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this._restartDropDownButton = new System.Windows.Forms.ToolStripMenuItem();
            this._quitDropDownButton = new System.Windows.Forms.ToolStripMenuItem();
            this._bottomTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._labelWhite = new System.Windows.Forms.Label();
            this._labelBlack = new System.Windows.Forms.Label();
            this._labelTimer = new System.Windows.Forms.Label();
            this._mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._gameWindow = new Chess.GUI.GameWindow();
            this._toolStrip.SuspendLayout();
            this._bottomTableLayoutPanel.SuspendLayout();
            this._mainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileDropDrownButton});
            this._toolStrip.Location = new System.Drawing.Point(3, 3);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(428, 25);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _fileDropDrownButton
            // 
            this._fileDropDrownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._fileDropDrownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._restartDropDownButton,
            this._quitDropDownButton});
            this._fileDropDrownButton.Image = ((System.Drawing.Image)(resources.GetObject("_fileDropDrownButton.Image")));
            this._fileDropDrownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._fileDropDrownButton.Name = "_fileDropDrownButton";
            this._fileDropDrownButton.Size = new System.Drawing.Size(38, 22);
            this._fileDropDrownButton.Text = "File";
            // 
            // _restartDropDownButton
            // 
            this._restartDropDownButton.Name = "_restartDropDownButton";
            this._restartDropDownButton.Size = new System.Drawing.Size(110, 22);
            this._restartDropDownButton.Text = "Restart";
            this._restartDropDownButton.Click += new System.EventHandler(this.OnClickRestartDropDownButton);
            // 
            // _quitDropDownButton
            // 
            this._quitDropDownButton.Name = "_quitDropDownButton";
            this._quitDropDownButton.Size = new System.Drawing.Size(110, 22);
            this._quitDropDownButton.Text = "Quit";
            this._quitDropDownButton.Click += new System.EventHandler(this.OnClickQuitDropDownButton);
            // 
            // _bottomTableLayoutPanel
            // 
            this._bottomTableLayoutPanel.ColumnCount = 3;
            this._bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this._bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this._bottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this._bottomTableLayoutPanel.Controls.Add(this._labelWhite, 2, 0);
            this._bottomTableLayoutPanel.Controls.Add(this._labelBlack, 0, 0);
            this._bottomTableLayoutPanel.Controls.Add(this._labelTimer, 1, 0);
            this._bottomTableLayoutPanel.Location = new System.Drawing.Point(0, 512);
            this._bottomTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this._bottomTableLayoutPanel.Name = "_bottomTableLayoutPanel";
            this._bottomTableLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._bottomTableLayoutPanel.RowCount = 1;
            this._bottomTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._bottomTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._bottomTableLayoutPanel.Size = new System.Drawing.Size(512, 30);
            this._bottomTableLayoutPanel.TabIndex = 2;
            // 
            // _labelWhite
            // 
            this._labelWhite.Dock = System.Windows.Forms.DockStyle.Fill;
            this._labelWhite.Location = new System.Drawing.Point(357, 5);
            this._labelWhite.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this._labelWhite.Name = "_labelWhite";
            this._labelWhite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._labelWhite.Size = new System.Drawing.Size(150, 20);
            this._labelWhite.TabIndex = 3;
            this._labelWhite.Text = "White";
            this._labelWhite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _labelBlack
            // 
            this._labelBlack.Dock = System.Windows.Forms.DockStyle.Top;
            this._labelBlack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelBlack.Location = new System.Drawing.Point(5, 5);
            this._labelBlack.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this._labelBlack.Name = "_labelBlack";
            this._labelBlack.Size = new System.Drawing.Size(148, 20);
            this._labelBlack.TabIndex = 0;
            this._labelBlack.Text = "Black";
            this._labelBlack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _labelTimer
            // 
            this._labelTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._labelTimer.Location = new System.Drawing.Point(153, 5);
            this._labelTimer.Margin = new System.Windows.Forms.Padding(0);
            this._labelTimer.Name = "_labelTimer";
            this._labelTimer.Size = new System.Drawing.Size(204, 20);
            this._labelTimer.TabIndex = 4;
            this._labelTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _mainTableLayoutPanel
            // 
            this._mainTableLayoutPanel.AutoSize = true;
            this._mainTableLayoutPanel.ColumnCount = 1;
            this._mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._mainTableLayoutPanel.Controls.Add(this._bottomTableLayoutPanel, 0, 1);
            this._mainTableLayoutPanel.Controls.Add(this._gameWindow, 0, 0);
            this._mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainTableLayoutPanel.Location = new System.Drawing.Point(3, 28);
            this._mainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this._mainTableLayoutPanel.MaximumSize = new System.Drawing.Size(512, 0);
            this._mainTableLayoutPanel.MinimumSize = new System.Drawing.Size(512, 0);
            this._mainTableLayoutPanel.Name = "_mainTableLayoutPanel";
            this._mainTableLayoutPanel.RowCount = 2;
            this._mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._mainTableLayoutPanel.Size = new System.Drawing.Size(512, 398);
            this._mainTableLayoutPanel.TabIndex = 3;
            // 
            // _gameWindow
            // 
            this._gameWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._gameWindow.CellList = null;
            this._gameWindow.Location = new System.Drawing.Point(0, 0);
            this._gameWindow.Margin = new System.Windows.Forms.Padding(0);
            this._gameWindow.Name = "_gameWindow";
            this._gameWindow.Size = new System.Drawing.Size(512, 512);
            this._gameWindow.TabIndex = 1;
            // 
            // BoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(434, 429);
            this.Controls.Add(this._mainTableLayoutPanel);
            this.Controls.Add(this._toolStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BoardForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Game of Chess";
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._bottomTableLayoutPanel.ResumeLayout(false);
            this._mainTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private GUI.GameWindow _gameWindow;
        private System.Windows.Forms.ToolStripDropDownButton _fileDropDrownButton;
        private System.Windows.Forms.ToolStripMenuItem _restartDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem _quitDropDownButton;
        private System.Windows.Forms.TableLayoutPanel _bottomTableLayoutPanel;
        private System.Windows.Forms.Label _labelWhite;
        private System.Windows.Forms.TableLayoutPanel _mainTableLayoutPanel;
        private System.Windows.Forms.Label _labelBlack;
        private System.Windows.Forms.Label _labelTimer;
    }
}

