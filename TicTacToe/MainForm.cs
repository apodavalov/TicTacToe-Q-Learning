using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using TicTacToeLib;
using System.Collections.Generic;

namespace TicTacToe
{
    public partial class MainForm : Form
    {
        const double Alpha = 0.5, Gamma = 0.5;
        const int BoardWidth = 3, BoardHeight = 3, CountInRow = 3, Depth = 9, GameCount = 100000;
        private Game _Game;
        private IDictionary<PlayerMark, ITicTacToeAi> _Map;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0)
            {
                return;
            }
            Bitmap bitmap = new Bitmap(ClientSize.Width, ClientSize.Height, PixelFormat.Format32bppArgb);
            Draw(bitmap);    
            e.Graphics.DrawImageUnscaled(bitmap, e.ClipRectangle);
        }

        private void Draw(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                int cellWidth = bitmap.Width / _Game.BoardWidth;
                int cellHeight = bitmap.Height / _Game.BoardHeight;

                for (int i = 1; i < _Game.BoardWidth; i++)
                {
                    int x = cellWidth * i;
                    g.DrawLine(Pens.Gray, x, 0, x, bitmap.Height);
                }
                
                for (int j = 1; j < _Game.BoardHeight; j++)
                {
                    int y = cellHeight * j;
                    g.DrawLine(Pens.Gray, 0, y, bitmap.Width, y);
                }
                
                for (int i = 0; i < _Game.BoardWidth; i++)
                {
                    for (int j = 0; j < _Game.BoardHeight; j++)
                    {
                        Cell cell = _Game[i, j];

                        if (cell.Empty)
                        {
                            continue;
                        }

                        PlayerMark playerMark = cell.PlayerMark;

                        DrawCell(g, playerMark, new Rectangle(cellWidth * i, cellHeight * j, cellWidth, cellHeight));
                    }
                }
            }
        }

        private void DrawCell(Graphics g, PlayerMark playerMark, Rectangle rectangle)
        {
            switch (playerMark)
            {
                case PlayerMark.Cross:
                    g.DrawLine(Pens.Red, new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom));
                    g.DrawLine(Pens.Red, new Point(rectangle.Right, rectangle.Top), new Point(rectangle.Left, rectangle.Bottom));
                    break;
                case PlayerMark.Nought:
                    g.DrawEllipse(Pens.Blue, rectangle);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Point p = e.Location;
            int x = BoardWidth * p.X / ClientSize.Width;
            int y = BoardHeight * p.Y / ClientSize.Height;

            if (!_Game.DoTurn(x, y))
            {
                return;
            }

            Invalidate();

            if (!_Game.Completed)
            {
                Point2D point = _Map[_Game.CurrentPlayer].GetTurn(_Game.Board, _Game.CurrentPlayer);
                _Game.DoTurn(point);
                Invalidate();

                if (!_Game.Completed)
                {
                    return;
                }
            }

            if (_Game.Winner.HasValue)
            {
                PlayerMark playerMark = _Game.Winner.Value;
                MessageBox.Show(string.Format("Player {0} win", playerMark), "Win/Lose", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Tie", "Tie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            NewGame();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _Map = Teacher.Teach(CountInRow, BoardWidth, BoardHeight, GameCount, Alpha, Gamma);
            NewGame();
        }

        private void NewGame()
        {
            _Game = new Game(CountInRow, BoardWidth, BoardHeight, default(PlayerMark));
            Invalidate();
        }
    }
}
