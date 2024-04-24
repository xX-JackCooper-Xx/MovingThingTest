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
    public partial class MapMaker : Form
    {
        bool savedAs = false; // Flag to track if the file has been saved as
        bool saved = false; // Flag to track if the file has been saved
        string filePath; // Path of the file
        public Type item; // Currently selected item type
        public int pathNum; // Number of paths
        MenuSelector ms = new MenuSelector("map"); // Menu selector instance
        MapMakerControl uc = new MapMakerControl(32, 18); // Map maker control instance
        Thread th; // Thread for opening new forms

        // Constructor for the MapMaker form
        public MapMaker(int width, int height)
        {
            uc = new MapMakerControl(width, height); // Initialize map maker control with specified width and height
            InitializeComponent(); // Initialize components of the form
        }

        // Event handler for the MapMaker form load event
        private void MapMaker_Load(object sender, EventArgs e)
        {
            // Add map maker control to the mapPanel
            uc.AutoScroll = true;
            uc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(uc);
            uc.Show();

            // Add menu selector to the controlsPanel
            ms.AutoScroll = true;
            ms.Dock = DockStyle.Fill;
            controlsPanel.Controls.Add(ms);
            ms.Show();
        }

        // Event handler for the timer tick event
        private void timer_Tick(object sender, EventArgs e)
        {
            item = ms.selectedItem; // Update currently selected item

            // If the selected item is enemyPath, add a new enemy path
            if (item.Name == "enemyPath")
            {
                // Generate a random color for the new enemy path
                Random rnd = new Random();
                Color col = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));

                // Add the new enemy path to the menu selector and map maker control
                ms.menuTabs[2].addItem("EP " + (ms.tabLists[2].Count), col);
                ms.tabLists[2].Add(new existingEnemyPath(ms.tabLists[2].Count - 1, col));
                ms.selectedItem = typeof(existingEnemyPath);
                item = ms.selectedItem;
                ms.pictureBox1.Invalidate();
                uc.pathNumber = ms.tabLists[2].Count - 2;
                ms.pathNumber = uc.pathNumber;
                uc.enemyPaths.Add(new existingEnemyPath(ms.pathNumber + 1, col));
            }

            // If the selected item is existingEnemyPath, update path number in the map maker control
            if (item.Name == "existingEnemyPath")
            {
                uc.pathNumber = ms.pathNumber;
            }
            uc.item = item;
        }

        // Event handler for the Save button click event
        private void Save_Click(object sender, EventArgs e)
        {
            // If the file hasn't been saved as yet, prompt for file save location
            if (!savedAs)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                    saveFile(); // Save the file
                    savedAs = true;
                    saved = true;
                }
            }
            // If the file has been saved as, save the file
            else if (!saved)
            {
                saveFile(); // Save the file
                saved = true;
            }
        }

        // Event handler for the Load button click event
        private void LoadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                loadFile(); // Load the file
            }
        }

        // Method to save the file
        private void saveFile()
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                // Write grid dimensions to the file
                sw.Write(uc.grid.cols.ToString());
                sw.Write(",");
                sw.Write(uc.grid.rows.ToString());
                sw.Write('\n');

                // Write cell IDs to the file
                for (int i = 0; i < uc.grid.cols; i++)
                {
                    for (int j = 0; j < uc.grid.rows; j++)
                    {
                        sw.Write(uc.grid.cellArr[i, j].ID.ToString().PadLeft(3, '0'));
                        sw.Write(",");
                    }
                    sw.Write('\n');
                }
                sw.Write('\n');

                // Write number of enemy paths and their details to the file
                sw.Write(uc.enemyPaths.Count.ToString());
                sw.Write('\n');
                foreach (enemyPath ep in uc.enemyPaths)
                {
                    sw.Write(ep.saveString());
                    sw.Write("\n");
                }
            }
        }

        // Method to load the file
        private void loadFile()
        {
            // Remove existing map maker control and menu selector
            mapPanel.Controls.Remove(uc);
            controlsPanel.Controls.Remove(ms);
            int row;
            int col;
            Grid grid;
            List<enemyPath> enemyPaths = new List<enemyPath>();

            // Read file contents and create grid and enemy paths accordingly
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                col = Convert.ToInt16(line.Split(',')[0]);
                row = Convert.ToInt16(line.Split(',')[1]);

                grid = new Grid(mapPanel.Width, mapPanel.Height, row, col);
                grid.createGrid();

                for (int i = 0; i < col; i++)
                {
                    line = sr.ReadLine();
                    string[] numArr = line.Split(',');
                    int j = 0;
                    foreach (string str in numArr)
                    {
                        switch (str)
                        {
                            case "000":
                                grid.cellArr[i, j] = grid.cellArr[i, j].toWall();
                                break;
                            case "001":
                                grid.cellArr[i, j] = grid.cellArr[i, j].toGrass();
                                break;
                            case "002":
                                grid.cellArr[i, j] = grid.cellArr[i, j].toDirt();
                                break;
                            case "003":
                                grid.cellArr[i, j] = grid.cellArr[i, j].toBorder();
                                break;
                            case "101":
                                grid.cellArr[i, j] = grid.cellArr[i, j].toSpawn();
                                break;
                        }
                        j++;
                    }
                }
                sr.ReadLine();
                line = sr.ReadLine();
                int pathNumber = Convert.ToInt16(line);

                for (int i = 0; i < pathNumber; i++)
                {
                    List<Cell> pathAnchors = new List<Cell>();
                    List<dynamic> pathCellsLists = new List<dynamic>();
                    bool loop = false;
                    Color color = Color.White;

                    line = sr.ReadLine();
                    string[] strArr = line.Split(",");

                    for (int j = 0; j < strArr.Length - 1; j += 2)
                    {
                        pathAnchors.Add(grid.cellArr[Convert.ToInt16(strArr[j + 1]), Convert.ToInt16(strArr[j])]);
                    }

                    for (int j = 0; j < strArr.Length / 2 - 1; j++)
                    {
                        List<Cell> tempList = new List<Cell>();
                        line = sr.ReadLine();
                        string[] strArr2 = line.Split(",");
                        for (int k = 0; k < strArr2.Length - 1; k += 2)
                        {
                            tempList.Add(grid.cellArr[Convert.ToInt16(strArr2[k + 1]), Convert.ToInt16(strArr2[k])]);
                        }
                        pathCellsLists.Add(tempList);
                    }
                    line = sr.ReadLine();
                    loop = Convert.ToBoolean(line);
                    line = sr.ReadLine();
                    color = Color.FromArgb(Convert.ToInt32(line));
                    enemyPaths.Add(new existingEnemyPath(loop, pathAnchors, pathCellsLists, color));

                }

            }

            // Create new map maker control and menu selector with loaded data
            uc = new MapMakerControl(filePath, grid, enemyPaths);
            uc.AutoScroll = true;
            uc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(uc);
            uc.Show();

            ms = new MenuSelector(enemyPaths, "map");
            ms.AutoScroll = true;
            ms.Dock = DockStyle.Fill;
            controlsPanel.Controls.Add(ms);
            ms.Show();
        }

        // Event handler for the Exit button click event
        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(openMainForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        // Method to open the Main form
        private void openMainForm()
        {
            Application.Run(new Main());
        }
    }
}
