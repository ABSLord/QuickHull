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

        //private Graphics graphics;

        List<Point> points;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
            points = new List<Point>();
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
                if(points.Count >= 2)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
