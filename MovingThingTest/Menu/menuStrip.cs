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
        // Name of the menu strip
        public string name;
        // List of menu items
        private List<string> items = new List<string>();
        // List of colors for menu items
        private List<Color> colours = new List<Color>();
        // Flag indicating if the menu strip is open
        public bool open = false;
        // Padding between menu items
        private int padding = 20;

        // Pen and brush for drawing
        public Pen pen = new Pen(Color.Black, 3);
        public SolidBrush brush = new SolidBrush(Color.Black);

        // Constructor
        public menuStrip(string name)
        {
            this.name = name;
        }

        // Method to add a menu item with color
        public void addItem(string item, Color color)
        {
            items.Add(item);
            colours.Add(color);
        }

        // Method to draw the menu strip
        public void drawMenu(PaintEventArgs e, int width, int topLeft, int imageSize, Font font)
        {
            brush.Color = Color.Black;
            pen.Width = 3;
            font = new Font(font.Name, 20, FontStyle.Bold);
            // If menu strip is open
            if (open)
            {
                // Draw triangle indicator
                PointF[] tri = {
                    new PointF(10, topLeft+10),
                    new PointF(30, topLeft + 10),
                    new Point(20, topLeft + 24),
                };
                e.Graphics.DrawPolygon(pen, tri);
                e.Graphics.DrawString(name, font, brush, 40, topLeft);

                int cols = width / (imageSize + padding);

                int xPad = (width - (cols * imageSize + padding * (cols - 1))) / 2;

                // Draw menu items
                for (int i = 0; i < items.Count; i++)
                {
                    int col = i / cols;
                    int row = i % cols;
                    brush.Color = colours[i];
                    Rectangle rect = new Rectangle(row * (imageSize + padding) + xPad, col * (imageSize + padding) + topLeft + 40, imageSize, imageSize);
                    e.Graphics.DrawRectangle(pen, rect);
                    e.Graphics.FillRectangle(brush, rect);
                    brush.Color = Color.Black;
                    font = new Font(font.Name, 15, FontStyle.Regular);
                    e.Graphics.DrawString(items[i], font, brush, new PointF(row * (imageSize + padding) + xPad, col * (imageSize + padding) + topLeft + 40));
                }

            }

            // If menu strip is closed
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
                e.Graphics.DrawLine(pen, new Point(5, topLeft + 35), new Point(width - 5, topLeft + 35));
            }
        }

        // Method to calculate the size of the menu strip when open
        public int calculateSize(int width, int imageSize)
        {
            if (open)
            {
                imageSize = imageSize + padding;
                int cols = width / imageSize;
                float height = (float)items.Count / cols;
                return (int)Math.Ceiling(height) * imageSize;
            }
            else
            {
                return 0;
            }
        }

        // Method to toggle the open state of the menu strip
        public void changeOpen()
        {
            open = !open;
        }

        // Method to select a menu item based on click position
        public int selectItem(Point clickPos, int size, int width, int imageSize)
        {
            bool selectX = false;
            bool selectY = false;

            clickPos.Y -= size;

            int cols = width / (imageSize + padding);

            int rows = (int)Math.Ceiling((float)items.Count / cols);

            int xPad = (width - (cols * imageSize + padding * (cols - 1))) / 2;

            int xPos = xPad;
            int yPos = 0;

            int col = 0;
            int row = 0;

            // Find the column and row of the clicked position
            for (int i = 0; i < cols; i++)
            {
                if (xPad > clickPos.X)
                {
                    break;
                }
                xPos += imageSize;
                if (xPos > clickPos.X)
                {
                    col = i;
                    selectX = true;
                    break;
                }
                else
                {
                    xPos += padding;
                }
                if (xPos > clickPos.X)
                {
                    break;
                }
            }

            for (int j = 0; j < rows; j++)
            {
                yPos += imageSize;
                if (yPos > clickPos.Y)
                {
                    row = j;
                    selectY = true;
                    break;
                }
                else
                {
                    yPos += padding;
                }
                if (yPos > clickPos.Y)
                {
                    break;
                }
            }

            // Return the index of the selected item
            if (selectX && selectY)
            {
                return (row * cols + col);
            }
            else
            {
                return -1;
            }

        }
    }
}
