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
    public partial class PlayerForm : Form
    {
        // File path for loading maps
        string filePath;
        // Selected item
        public string item;
        // Selected path number
        public int pathNum;
        // Menu selector instance
        MenuSelector ms = new MenuSelector("play");
        // Player control instance
        PlayerControl pc;
        // Size of the squad
        int squadSize = 5;
        // Thread for opening main form
        Thread th;

        // Constructor with squad size parameter
        public PlayerForm(int squadSize)
        {
            InitializeComponent();
            this.squadSize = squadSize;
            pc = new PlayerControl(squadSize);
        }

        // Event handler for form load event
        private void MapMaker_Load(object sender, EventArgs e)
        {
            // Set properties and add player control to map panel
            pc.AutoScroll = true;
            pc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(pc);
            pc.Show();

            // Set properties and add menu selector to controls panel
            ms.AutoScroll = true;
            ms.Dock = DockStyle.Fill;
            controlsPanel.Controls.Add(ms);
            ms.Show();
        }

        // Event handler for timer tick event
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update squad formation if path number changes
            if (ms.pathNumber != pathNum)
            {
                pathNum = ms.pathNumber;
                pc.squad.changeFormation(pathNum, pc.grid);
            }
        }

        // Event handler for load button click event
        private void LoadBtn_Click(object sender, EventArgs e)
        {
            // Open file dialog to select map file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                // Load selected file
                loadFile();
            }
        }

        // Method to load map file
        private void loadFile()
        {
            // Remove existing player control and menu selector
            mapPanel.Controls.Remove(pc);
            controlsPanel.Controls.Remove(ms);
            int row;
            int col;
            Grid grid;
            Cell spawnCell = new Cell();
            List<enemyPath> enemyPaths = new List<enemyPath>();
            // Open the selected map file using StreamReader
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                // Extract the number of columns and rows from the first line of the file
                col = Convert.ToInt16(line.Split(',')[0]);
                row = Convert.ToInt16(line.Split(',')[1]);

                // Create a grid and populate its cells
                grid = new Grid(mapPanel.Width, mapPanel.Height, row, col);
                grid.createGrid();
                spawnCell = grid.cellArr[1, 1];
                for (int i = 0; i < col; i++)
                {
                    // Read each line representing a row of the grid from the file
                    line = sr.ReadLine();
                    string[] numArr = line.Split(',');
                    int j = 0;
                    // Process each value in the row
                    foreach (string str in numArr)
                    {
                        // Convert the value to a cell type based on its code
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
                                // Set spawn cell to the cell with the spawn point
                                spawnCell = grid.cellArr[i, j];
                                break;
                        }
                        j++;
                    }
                }
                // Skip the empty line
                sr.ReadLine();
                // Read the next line containing the number of enemy paths
                line = sr.ReadLine();
                int pathNumber = Convert.ToInt16(line);

                // Load enemy paths
                for (int i = 0; i < pathNumber; i++)
                {
                    List<Cell> pathAnchors = new List<Cell>();
                    List<dynamic> pathCellsLists = new List<dynamic>();
                    bool loop = false;
                    Color color = Color.White;

                    // Read the line containing the anchor points for the current path
                    line = sr.ReadLine();
                    string[] strArr = line.Split(",");

                    // Process each pair of coordinates in the anchor points
                    for (int j = 0; j < strArr.Length - 1; j += 2)
                    {
                        // Convert the coordinates to cell positions in the grid
                        pathAnchors.Add(grid.cellArr[Convert.ToInt16(strArr[j + 1]), Convert.ToInt16(strArr[j])]);
                    }

                    // Read the lines containing the list of cells for the current path
                    for (int j = 0; j < strArr.Length / 2 - 1; j++)
                    {
                        List<Cell> tempList = new List<Cell>();
                        line = sr.ReadLine();
                        string[] strArr2 = line.Split(",");
                        // Process each pair of coordinates in the list of cells
                        for (int k = 0; k < strArr2.Length - 1; k += 2)
                        {
                            // Convert the coordinates to cell positions in the grid
                            tempList.Add(grid.cellArr[Convert.ToInt16(strArr2[k + 1]), Convert.ToInt16(strArr2[k])]);
                        }
                        // Add the list of cells to the pathCellsLists
                        pathCellsLists.Add(tempList);
                    }
                    // Read the line indicating if the path is a loop
                    line = sr.ReadLine();
                    loop = Convert.ToBoolean(line);
                    // Read the line containing the color of the path
                    line = sr.ReadLine();
                    color = Color.FromArgb(Convert.ToInt32(line));
                    // Create an enemy path object with the loaded data and add it to the list
                    enemyPaths.Add(new existingEnemyPath(loop, pathAnchors, pathCellsLists, color));
                }
            }

            // Create a new player control and menu selector with the loaded data
            pc = new PlayerControl(filePath, grid, enemyPaths, spawnCell, squadSize);
            pc.AutoScroll = true;
            pc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(pc);
            pc.Show();

            ms = new MenuSelector(enemyPaths, "play");
            ms.AutoScroll = true;
            ms.Dock = DockStyle.Fill;
            controlsPanel.Controls.Add(ms);
            ms.Show();
        }


        // Event handler for exit button click event
        private void exitBtn_Click(object sender, EventArgs e)
        {
            // Close current form and open main form
            this.Close();
            th = new Thread(openMainForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        // Method to open main form
        private void openMainForm()
        {
            Application.Run(new Main());
        }
    }
}