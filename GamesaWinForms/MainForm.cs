using System;
using System.Drawing;
using System.Windows.Forms;

namespace GamesaWinForms
{
    public partial class MainForm : Form
    {
        const int MaxPoints = 10;
        const int Delta4Point = 20;

        static readonly Random Random = new Random();

        Point[] _points = new Point[MaxPoints];
        int _steps = 0;
        bool _moving = false;
        int _pointIndex;

        public MainForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            BackColor = Color.White;
            Size = new Size(800, 800);

            Paint += (sender, e) => DrawGame(e.Graphics);
            MouseDown += (sender, e) => PointBeginMove(e);
            MouseUp += (sender, e) => PointEndMove(e);
            MouseMove += (sender, e) => PointMoving(e);

            for (int i = 0; i < MaxPoints; i++)
            {
                _points[i] = new Point(Random.Next(0, ClientSize.Width), Random.Next(0, ClientSize.Height));
            }

            UpdateGameStatus();
        }

        void UpdateGameStatus()
        {
            Text = _steps.ToString();
        }

        void DrawGame(Graphics g)
        {
            var i = 0;
            for (i = 0; i < MaxPoints - 1; i++)
            {
                g.DrawLine(new Pen(Color.Red), _points[i], _points[i + 1]);
                g.DrawArc(new Pen(Color.Blue), _points[i].X - (Delta4Point / 2), _points[i].Y - (Delta4Point / 2), Delta4Point, Delta4Point, 0, 360);
            }
            g.DrawLine(new Pen(Color.Red), _points[i], _points[0]);
            g.DrawArc(new Pen(Color.Blue), _points[i].X - (Delta4Point / 2), _points[i].Y - (Delta4Point / 2), Delta4Point, Delta4Point, 0, 360);
        }

        void PointBeginMove(MouseEventArgs e)
        {
            for (int i = 0; i < MaxPoints; i++)
            {
                if (Math.Abs(_points[i].X - e.X) < Delta4Point && Math.Abs(_points[i].Y - e.Y) < Delta4Point)
                {
                    _pointIndex = i;
                    _moving = true;
                    _steps++;
                    break;
                }
            }
            UpdateGameStatus();
        }

        void PointEndMove(MouseEventArgs e)
        {
            _moving = false;
        }

        void PointMoving(MouseEventArgs e)
        {
            if (_moving)
            {
                _points[_pointIndex].X = e.X;
                _points[_pointIndex].Y = e.Y;
                Invalidate();
            }
        }
    }
}