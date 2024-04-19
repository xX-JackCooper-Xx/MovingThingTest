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
        public List<dynamic> tabLists = new List<dynamic>() { };
        public Type selectedItem = typeof(Grass);
        public int pathNumber = 0;
        private Rectangle selectedRectangle = new Rectangle();

        string form;

        public MenuSelector(string form)
        {
            InitializeComponent();
            switch (form)
            {
                case "map":
                    createMap();
                    break;
                case "play":
                    play();
                    break;
            }

        }

        public void createMap()
        {
            tabLists.Add(new List<Cell>() { new Cell().toWall(), new Cell().toGrass(), new Cell().toDirt() });
            tabLists.Add(new List<Cell>() { new Cell().toSpawn() });
            tabLists.Add(new List<enemyPath>() { new enemyPath() });
            menuStrip blocks = new menuStrip("blocks");
            foreach (var val in tabLists[0])
            {
                blocks.addItem(val.GetType().Name, val.color);
            }
            menuStrip meta = new menuStrip("meta");
            foreach (var val in tabLists[1])
            {
                meta.addItem(val.GetType().Name, val.color);
            }
            menuStrip path = new menuStrip("path");
            foreach (var val in tabLists[2])
            {
                path.addItem(val.GetType().Name, val.color);

            }

            menuTabs.Add(blocks);
            menuTabs.Add(meta);
            menuTabs.Add(path);
        }

        public void loadMap(List<enemyPath> ep)
        {
            tabLists.Add(new List<Cell>() { new Cell().toWall(), new Cell().toGrass(), new Cell().toDirt() });
            tabLists.Add(new List<Cell>() { new Cell().toSpawn() });
            tabLists.Add(new List<enemyPath>() { new enemyPath() });
            menuStrip blocks = new menuStrip("blocks");
            foreach (var val in tabLists[0])
            {
                blocks.addItem(val.GetType().Name, val.color);
            }
            menuStrip meta = new menuStrip("meta");
            foreach (var val in tabLists[1])
            {
                meta.addItem(val.GetType().Name, val.color);
            }
            menuStrip path = new menuStrip("path");
            foreach (var val in tabLists[2])
            {
                path.addItem(val.GetType().Name, val.color);
            }
            int i = 1;
            foreach (var val in ep)
            {
                path.addItem("EP " + i.ToString(), val.color);
                tabLists[2].Add(new existingEnemyPath(tabLists[2].Count - 1, val.color));
                i++;
            }

            menuTabs.Add(blocks);
            menuTabs.Add(meta);
            menuTabs.Add(path);
        }

        public void play()
        {
            tabLists.Add(new List<string>() { "Herring\nbone", "Wedge" });
            menuStrip formations = new menuStrip("formations");
            foreach(string str in tabLists[0])
            {
                formations.addItem(str, Color.Blue);
            }
            menuTabs.Add(formations);
        }

        public MenuSelector(List<enemyPath> ep, string form)
        {
            InitializeComponent();
            switch (form)
            {
                case "map":
                    loadMap(ep);
                    break;
                case "play":
                    play();
                    break;

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int size = 0;
            for (int i = 0; i < menuTabs.Count; i++)
            {
                menuTabs[i].drawMenu(e, Width, size, imageSize, Font);
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
                    if (yPos < size)
                    {
                        size -= menuTabs[i].calculateSize(Width, imageSize);
                        int s = menuTabs[i].selectItem(clickPos, size, Width, imageSize);

                        if (s > -1)
                        {
                            selectedItem = tabLists[i][s].GetType();
                            if (selectedItem.Name == "existingEnemyPath")
                            {
                                pathNumber = s - 1;
                            }
                            if (selectedItem.Name == "String")
                            {
                                pathNumber = s;
                            }
                        }

                        break;
                    }
                }
            }
            pictureBox1.Invalidate();
        }
    }
}
