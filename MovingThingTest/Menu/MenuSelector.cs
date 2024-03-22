using MovingThingTest.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovingThingTest
{
    public partial class MenuSelector : UserControl
    {
        int tabSize = 40;
        int imageSize = 60;
        public List<menuStrip> menuTabs = new List<menuStrip>();
        public List<dynamic> tabLists = new List<dynamic>() {
            new List<Cell>() {new Cell().toWall(), new Cell().toGrass(), new Cell().toDirt()},
            new List<Cell>() {new Cell().toSpawn()}
        };
        public Type selectedItem;
        
        public MenuSelector()
        {
            InitializeComponent();
            menuStrip blocks = new menuStrip("blocks");
            foreach (var val in tabLists[0])
            {
                blocks.addItem(val.GetType().Name, val.color);
            }
            menuStrip meta = new menuStrip("meta");
            foreach(var val in tabLists[1])
            {
                meta.addItem(val.GetType().Name, val.color);
            }
            menuTabs.Add(blocks);
            menuTabs.Add(meta);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int size = 0;
            for(int i = 0; i  < menuTabs.Count; i++) 
            {
                menuTabs[i].drawMenu(e, Width, size,imageSize, Font);
                size += tabSize + menuTabs[i].calculateSize(Width, imageSize);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPos = me.Location;
            int yPos = clickPos.Y;
            int size = 0;
            for (int i = 0; i < menuTabs.Count; i++)
            {

                size += tabSize;

                if (yPos < size)
                {
                    menuTabs[i].changeOpen();
                    break;
                }
                else
                {
                    size += menuTabs[i].calculateSize(Width, imageSize);
                    if(yPos < size)
                    {
                        size -= menuTabs[i].calculateSize(Width, imageSize);
                        int s = menuTabs[i].selectItem(clickPos, size, Width, imageSize);

                        if(s > -1)
                        {
                            selectedItem = tabLists[i][s].GetType();
                        }

                        break;
                    }
                }
            }
            pictureBox1.Invalidate();
        }
    }
}
