using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickHull11
{

    public partial class Form1 : Form
    {

        private List<Point> points;

        public List<Point> hull;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
            points = new List<Point>();
            hull = new List<Point>();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            Pen pen = new Pen(Color.Red);
            if (e.Button == MouseButtons.Left)
            {
                points.Add(new Point(e.X, e.Y));
                graphics.DrawRectangle(pen, e.X, e.Y, 1, 1);
                pictureBox1.Invalidate();
                if(points.Count >= 3)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
            }
        }


        int Side(Point p1, Point p2, Point p)
        {
            int val = (p.Y - p1.Y) * (p2.X - p1.X) -
                      (p2.Y - p1.Y) * (p.X - p1.X);

            if (val > 0)
                return 1;
            if (val < 0)
                return -1;
            return 0;
        }

        int Distance(Point p1, Point p2, Point p)
        {
            return Math.Abs((p.Y - p1.Y) * (p2.X - p1.X) - (p2.Y - p1.Y) * (p.X - p1.X));
        }


        public void QuickHull()
        {

            if (points.Count < 3)
            {
                foreach(var p in points)
                {
                    hull.Add(p);
                }
                return;
            }
                
            Point pmin = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x < p2.x ? p1 : p2).point;

            Point pmax = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x > p2.x ? p1 : p2).point;

            hull.Add(pmin);
            hull.Add(pmax);

            List<Point> left = new List<Point>();
            List<Point> right = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                if (Side(pmin, pmax, p) == 1)
                    left.Add(p);
                else
                if (Side(pmin, pmax, p) == -1)
                    right.Add(p);
            }
            CreateHull(pmin, pmax, left);
            CreateHull(pmax, pmin, right);
        }

        public void CreateHull(Point a, Point b, List<Point> points)
        {
            int pos = hull.IndexOf(b);
            if (points.Count == 0)
                return;
            if (points.Count == 1)
            {
                Point pp = points[0];
                hull.Insert(pos, pp);
                return;
            }

            int dist = int.MinValue;
            int point = 0;

            for (int i = 0; i < points.Count; i++)
            {
                Point pp = points[i];
                int distance = Distance(a, b, pp);
                if (distance > dist)
                {
                    dist = distance;
                    point = i;
                }
            }

            Point p = points[point];
            hull.Insert(pos, p);
            List<Point> ap = new List<Point>();
            List<Point> pb = new List<Point>();

            // слева от AP
            for (int i = 0; i < points.Count; i++)
            {
                Point pp = points[i];
                if (Side(a, p, pp) == 1)
                {
                    ap.Add(pp);
                }
            }
            // слева от PB
            for (int i = 0; i < points.Count; i++)
            {
                Point pp = points[i];
                if (Side(p, b, pp) == 1)
                {
                    pb.Add(pp);
                }
            }
            CreateHull(a, p, ap);
            CreateHull(p, b, pb);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hull.Clear();
            if(points.Count!=0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics graphics1 = Graphics.FromImage(pictureBox1.Image);
                graphics1.Clear(Color.White);
                Pen pen1 = new Pen(Color.Red);
                foreach (var p in points)
                {
                    graphics1.DrawRectangle(pen1, p.X, p.Y, 1, 1);
                    pictureBox1.Invalidate();
                }

            }
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            Pen pen = new Pen(Color.Red);
            QuickHull();
            graphics.DrawPolygon(pen, hull.ToArray());
            pictureBox1.Invalidate();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
            button1.Enabled = false;
            button2.Enabled = false;
            points.Clear();
            hull.Clear();
        }

        //old algorithm
        /*void QuickHull(List<Point> a, Point p1, Point p2, int side)
        {
            int ind = -1;
            int max_dist = 0;
            for (int i = 0; i < a.Count; i++)
            {
                int tmp = Distance(p1, p2, a[i]);
                if (Side(p1, p2, a[i]) == side && tmp > max_dist)
                {
                    ind = i;
                    max_dist = tmp;
                }
            }
            if (ind == -1)
            {
                hull.Add(p1);
                hull.Add(p2);
                return;
            }
            QuickHull(a, a[ind], p1, -Side(a[ind], p1, p2));
            QuickHull(a, a[ind], p2, -Side(a[ind], p2, p1));
        }

        void Construction(List<Point> a, int n)
        {
            Point left = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x < p2.x ? p1 : p2).point;

            Point right = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x > p2.x ? p1 : p2).point;

            QuickHull(a, left, right, 1);

            QuickHull(a, left, right, -1);
        }

        private Point GetCentroid(List<Point> lst)
        {
            int x = 0;
            int y = 0;
            foreach(Point p in lst)
            {
                x += p.X;
                y += p.Y;
            }
            x = x / lst.Count;
            y = y / lst.Count;
            return new Point(x, y);
        }

        public double Angle(Point A, Point center)
        {
            double angle = Math.Atan2((A.Y - center.Y), (A.X - center.X));
            return angle;
        }*/

    }
}
