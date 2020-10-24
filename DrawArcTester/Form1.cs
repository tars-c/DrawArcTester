using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawArcTester
{
    public partial class Form1 : Form
    {
        public int ArcX { get; set; }
        public int ArcY { get; set; }
        public int ArcWidth { get; set; }
        public int ArcHeight { get; set; }
        public float ArcStartAngle { get; set; }
        public float ArcSweepAngle { get; set; }

        private NumericUpDown[] list;
        private decimal[] property_list;

        public Color ArcStrokeColor { get; set; }
        public Color ArcRectColor { get; set; }

        public int ArcStrokeSize { get; set; }
        public int ArcRectSize { get; set; }

        private int prevScrollValue { get; set; }



        public Form1()
        {
            InitializeComponent();
            
            list = new NumericUpDown[]
            {
                numericUpDown1,
                numericUpDown2,
                numericUpDown3,
                numericUpDown4,
                numericUpDown5,
                numericUpDown6,
            };

            property_list = new decimal[]
            {
                Convert.ToDecimal(ArcX = 0),
                Convert.ToDecimal(ArcY = 0),
                Convert.ToDecimal(ArcWidth = 300),
                Convert.ToDecimal(ArcHeight = 300),
                Convert.ToDecimal(ArcStartAngle = 0.0f),
                Convert.ToDecimal(ArcSweepAngle = 360.0f),
            };
            ArcStrokeColor = Color.Black;
            button1.BackColor = ArcStrokeColor;

            for (uint i=0; i<list.Length; ++i)
            {
                list[i].Maximum = 3000m;
                list[i].Text = property_list[i].ToString();
            }

            //panel1.Width = 300;
            //panel1.Height = 300;
            //panel1.AutoScrollMinSize = new Size(1500, 1500);
            vScrollBar2.Value = 0;
            vScrollBar2.Minimum = 0;
            vScrollBar2.SmallChange = ((vScrollBar2.Maximum - vScrollBar2.Minimum) / 40);
            vScrollBar2.LargeChange = ((vScrollBar2.Maximum - vScrollBar2.Minimum) / 40);
            vScrollBar2.Maximum = 2000 + (vScrollBar2.LargeChange - 1);
            vScrollBar2.Scroll += (sender, e) => {

                if (prevScrollValue != vScrollBar2.Value)
                {
                    panel1.Invalidate();
                }
                prevScrollValue = vScrollBar2.Value;
            };
            ArcStrokeSize = 3;
            ArcRectSize = 3;
            panel1.Controls.Add(vScrollBar2);   

            panel1.MouseEnter += (s, e) => panel1.Focus();
            this.panel1.MouseWheel += (s, e) => {
                // Debug
                //Console.Out.WriteLine(e.Delta);
                //listBox1.Items.Insert(0, vScrollBar2.Value + "," + -(e.Delta / 120) * vScrollBar2.SmallChange);
                
                // 델타값 계산
                int v = ((((-e.Delta) / 120) * vScrollBar2.SmallChange) / 2); 
                if (v + vScrollBar2.Value < 0)
                {
                    vScrollBar2.Value = 0;
                } else if (v + vScrollBar2.Value > (vScrollBar2.Maximum - (vScrollBar2.LargeChange)) + 1)
                {
                    vScrollBar2.Value = (vScrollBar2.Maximum - (vScrollBar2.LargeChange))+1;
                } else
                {
                    vScrollBar2.Value += v;
                }

                if (prevScrollValue != vScrollBar2.Value)
                {
                    panel1.Invalidate();
                }
                prevScrollValue = vScrollBar2.Value;
            };
                
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            label7.Text = vScrollBar2.Value + ": Hori";
            label8.Text = vScrollBar2.Value + ": Verti";

            Pen p = new Pen(ArcStrokeColor, ArcStrokeSize);

                GraphicsPath path = new GraphicsPath();
                Rectangle rect = new Rectangle((ArcX+ ArcRectSize) - panel1.HorizontalScroll.Value,
                                (ArcY + ArcRectSize) - vScrollBar2.Value,
                                ArcWidth,
                                ArcHeight);

            e.Graphics.DrawArc(p, rect,
                      ArcStartAngle, ArcSweepAngle);


            path.AddRectangle(rect);
                e.Graphics.DrawPath(new Pen(ArcRectColor, ArcRectSize), path);

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ArcX = Decimal.ToInt32((sender as NumericUpDown).Value);
            panel1.Invalidate();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ArcY = Decimal.ToInt32((sender as NumericUpDown).Value);
            panel1.Invalidate();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            ArcWidth = Decimal.ToInt32((sender as NumericUpDown).Value);
            panel1.Invalidate();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            ArcHeight = Decimal.ToInt32((sender as NumericUpDown).Value);
            panel1.Invalidate();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            ArcStartAngle = Decimal.ToSingle((sender as NumericUpDown).Value);
            if (ArcStartAngle > 360.0)
            {
                (sender as NumericUpDown).Value = 0.0m;
            }
            panel1.Invalidate();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            ArcSweepAngle = Decimal.ToSingle((sender as NumericUpDown).Value);
            if (ArcSweepAngle > 360.0)
            {
                (sender as NumericUpDown).Value = 0.0m;
            }
            panel1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                ArcStrokeColor = colorDialog1.Color;
                button1.BackColor = ArcStrokeColor;
                panel1.Invalidate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                ArcRectColor = colorDialog1.Color;
                button2.BackColor = ArcRectColor;
                panel1.Invalidate();
            }
        }
        static int i = 0;

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            vScrollBar2.Focus();
        }
    }
}
