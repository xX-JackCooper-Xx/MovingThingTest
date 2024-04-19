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
        bool savedAs = false;
        bool saved = false;
        string filePath;
        public Type item;
        public int pathNum;
        MenuSelector ms = new MenuSelector("map");
        MapMakerControl uc = new MapMakerControl(32, 18);
        Thread th;
        public MapMaker(int width, int height)
        {
            uc = new MapMakerControl(width, height);
            InitializeComponent();
        }


        private void MapMaker_Load(object sender, EventArgs e)
        {

            uc.AutoScroll = true;
            uc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(uc);
            uc.Show();


            ms.AutoScroll = true;
            ms.Dock = DockStyle.Fill;
            controlsPanel.Controls.Add(ms);
            ms.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void controlsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            item = ms.selectedItem;
            if (item.Name == "enemyPath")
            {
                Random rnd = new Random();
                Color col = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
                ms.menuTabs[2].addItem("EP " + (ms.tabLists[2].Count), col);
                ms.tabLists[2].Add(new existingEnemyPath(ms.tabLists[2].Count - 1, col));
                ms.selectedItem = typeof(existingEnemyPath);
                item = ms.selectedItem;
                ms.pictureBox1.Invalidate();
                uc.pathNumber = ms.tabLists[2].Count - 2;
                ms.pathNumber = uc.pathNumber;
                uc.enemyPaths.Add(new existingEnemyPath(ms.pathNumber + 1, col));
            }
            if (item.Name == "existingEnemyPath")
            {
                uc.pathNumber = ms.pathNumber;
            }
            uc.item = item;
        }
        private void MapMaker_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void MapMaker_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void MapMaker_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!savedAs)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;

                    saveFile();
                    savedAs = true;
                    saved = true;
                }
            }
            else if (!saved)
            {
                saveFile();
                saved = true;
            }
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                loadFile();
            }
        }

        private void saveFile()
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(uc.grid.cols.ToString());
                sw.Write(",");
                sw.Write(uc.grid.rows.ToString());
                sw.Write('\n');
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
                sw.Write(uc.enemyPaths.Count.ToString());
                sw.Write('\n');
                foreach (enemyPath ep in uc.enemyPaths)
                {
                    sw.Write(ep.saveString());
                    sw.Write("\n");
                }
            }
        }

        private void loadFile()
        {
            mapPanel.Controls.Remove(uc);
            controlsPanel.Controls.Remove(ms);
            int row;
            int col;
            Grid grid;
            List<enemyPath> enemyPaths = new List<enemyPath>();
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

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(openMainForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void openMainForm()
        {
            Application.Run(new Main());
        }
    }
}
