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

        public Pen pen = new Pen(Color.Black);
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

        public void drawMenu(PaintEventArgs e, float width, int topLeft, Font font)
        {
            if(open)
            {

            }

            if (!open)
            {
                PointF[] tri = {
                    new PointF(topLeft + 10, topLeft+10),
                    new PointF(topLeft + 10, topLeft + 30),
                    new Point(topLeft + 24, topLeft + 30),
                };

                e.Graphics.DrawPolygon(pen, tri);
                e.Graphics.DrawString(name, font, brush, topLeft + 50, topLeft + 10);
            }
        }
    }
}
