using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MovingThingTest.Menu
{
    public class menuStrip
    {
        public string name;
        private List<string> items = new List<string>();
        private List<Color> colours = new List<Color>();
        public bool open = false;
        private int padding = 5;

        public Pen pen = new Pen(Color.Black, 3);
        public SolidBrush brush = new SolidBrush(Color.Black);

        public menuStrip(string name)
        {
            this.name = name;
        }

        public void addItem(string item, Color color)
        {
            items.Add(item);
            colours.Add(color);
        }

        public void drawMenu(PaintEventArgs e, int width, int topLeft, int imageSize, Font font)
        {
            brush.Color = Color.Black;
            font = new Font(font.Name, 20, FontStyle.Bold);
            if(open)
            {
                PointF[] tri = {
                    new PointF(10, topLeft+10),
                    new PointF(30, topLeft + 10),
                    new Point(20, topLeft + 24),
                };
                e.Graphics.DrawPolygon(pen, tri);
                e.Graphics.DrawString(name, font, brush, 40, topLeft);

                int cols = width / (imageSize+padding);

                for(int i = 0; i < items.Count; i++)
                {
                    int col = i / cols;
                    int row = i % cols;
                    brush.Color = colours[i];
                    Rectangle rect = new Rectangle(row * (imageSize + padding) + 20, col * (imageSize + padding) + 40, imageSize, imageSize);
                    e.Graphics.DrawRectangle(pen, rect);
                    e.Graphics.FillRectangle(brush, rect);
                }
                
            }

            if (!open)
            {
                PointF[] tri = {
                    new PointF(10, topLeft+10),
                    new PointF(10, topLeft + 30),
                    new Point(24, topLeft + 20),
                };

                e.Graphics.DrawPolygon(pen, tri);
                e.Graphics.DrawString(name, font, brush, 40, topLeft);
                pen.Width = 1;
                e.Graphics.DrawLine(pen, new Point(5, topLeft + 35), new Point(width-5, topLeft + 35));
            }
        }

        public int calculateSize(int width, int imageSize)
        {
            imageSize = imageSize + padding;
            int cols = width / imageSize;
            float height = (float)items.Count / cols;
            return (int)Math.Ceiling(height*imageSize);
        }

        public void changeOpen()
        {
            open = !open;
        }
    }
}
