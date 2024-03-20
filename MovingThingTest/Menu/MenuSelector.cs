using MovingThingTest.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovingThingTest
{
    public partial class MenuSelector : UserControl
    {
        int tabSize = 40;
        int imageSize = 80;
        public List<menuStrip> menuTabs = new List<menuStrip>();
        public MenuSelector()
        {
            InitializeComponent();
            menuStrip blocks = new menuStrip("hello");
            blocks.addItem("wall", Color.Gray);
            blocks.addItem("wall", Color.Gray);
            blocks.addItem("wall", Color.Gray);
            blocks.addItem("wall", Color.Gray);
            blocks.addItem("wall", Color.Gray);
            blocks.addItem("wall", Color.Gray);
            menuTabs.Add(blocks);
            menuStrip blocks2 = new menuStrip("world");
            blocks2.addItem("wall", Color.Red);
            menuTabs.Add(blocks2);
            menuTabs.Add(new menuStrip("world"));
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int size = 0;
            for(int i = 0; i  < menuTabs.Count; i++) 
            {
                menuTabs[i].drawMenu(e, Width, i * tabSize + size,imageSize, Font);
                if (menuTabs[i].open)
                {
                    size += menuTabs[i].calculateSize(Width, imageSize);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPos = me.Location;
            int yPos = clickPos.Y;
            int openSize = 0;
            for (int i = 0; i < menuTabs.Count; i++)
            {
                int size;
                if (menuTabs[i].open)
                {
                    size = tabSize + menuTabs[i].calculateSize(Width, imageSize);
                }
                else
                {
                    size = tabSize;
                }
                yPos -= size;
                if (yPos <= 0)
                {
                    menuTabs[i].changeOpen();
                    break;
                }
            }
        }
    }
}
