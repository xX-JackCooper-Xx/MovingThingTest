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
        // Size constants for tab and image
        int tabSize = 40;
        int imageSize = 60;
        // List to store menu strips
        public List<menuStrip> menuTabs = new List<menuStrip>();
        // List to store dynamic tab items
        public List<dynamic> tabLists = new List<dynamic>() { };
        // Selected item type
        public Type selectedItem = typeof(Grass);
        // Path number
        public int pathNumber = 0;
        // Rectangle representing the selected menu item
        private Rectangle selectedRectangle = new Rectangle();

        // Form type
        string form;

        // Constructor with form parameter
        public MenuSelector(string form)
        {
            InitializeComponent();
            // Initialize based on form type
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

        // Overloaded constructor with enemy paths and form parameters
        public MenuSelector(List<enemyPath> ep, string form)
        {
            InitializeComponent();
            // Initialize based on form type
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

        // Method to create map menu
        public void createMap()
        {
            // Add different tab lists
            tabLists.Add(new List<Cell>() { new Cell().toWall(), new Cell().toGrass(), new Cell().toDirt() });
            tabLists.Add(new List<Cell>() { new Cell().toSpawn() });
            tabLists.Add(new List<enemyPath>() { new enemyPath() });

            // Create menu strips for each tab
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

            // Add menu strips to the list
            menuTabs.Add(blocks);
            menuTabs.Add(meta);
            menuTabs.Add(path);
        }

        // Method to load map menu
        public void loadMap(List<enemyPath> ep)
        {
            // Add different tab lists
            tabLists.Add(new List<Cell>() { new Cell().toWall(), new Cell().toGrass(), new Cell().toDirt() });
            tabLists.Add(new List<Cell>() { new Cell().toSpawn() });
            tabLists.Add(new List<enemyPath>() { new enemyPath() });

            // Create menu strips for each tab
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

            // Add existing enemy paths
            int i = 1;
            foreach (var val in ep)
            {
                path.addItem("EP " + i.ToString(), val.color);
                tabLists[2].Add(new existingEnemyPath(tabLists[2].Count - 1, val.color));
                i++;
            }

            // Add menu strips to the list
            menuTabs.Add(blocks);
            menuTabs.Add(meta);
            menuTabs.Add(path);
        }

        // Method to initialize play menu
        public void play()
        {
            // Add formation names to the tab list
            tabLists.Add(new List<string>() { "Herring\nbone", "Wedge" });
            // Create menu strip for formations
            menuStrip formations = new menuStrip("formations");
            foreach (string str in tabLists[0])
            {
                formations.addItem(str, Color.Blue);
            }
            // Add menu strip to the list
            menuTabs.Add(formations);
        }

        // Event handler for timer tick event
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Refresh the picture box
            //pictureBox1.Invalidate();
        }

        // Event handler for picture box paint event
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int size = 0;
            // Iterate through menu tabs
            for (int i = 0; i < menuTabs.Count; i++)
            {
                // Draw each menu strip
                menuTabs[i].drawMenu(e, Width, size, imageSize, Font);
                size += tabSize + menuTabs[i].calculateSize(Width, imageSize);
            }
        }

        // Event handler for picture box click event
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPos = me.Location;
            int yPos = clickPos.Y;
            int size = 0;
            // Iterate through menu tabs
            for (int i = 0; i < menuTabs.Count; i++)
            {

                size += tabSize;

                if (yPos < size)
                {
                    // Toggle open state of menu strip
                    menuTabs[i].changeOpen();
                    break;
                }
                else
                {
                    size += menuTabs[i].calculateSize(Width, imageSize);
                    if (yPos < size)
                    {
                        size -= menuTabs[i].calculateSize(Width, imageSize);
                        // Select item within the menu strip
                        int s = menuTabs[i].selectItem(clickPos, size, Width, imageSize);

                        if (s > -1 && tabLists[i].Count > s)
                        {
                            // Set the selected item and path number
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
            // Refresh the picture box
            pictureBox1.Invalidate();
        }
    }
}
