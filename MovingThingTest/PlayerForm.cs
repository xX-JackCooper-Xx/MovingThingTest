using MovingThingTest.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MovingThingTest
{
    public partial class PlayerForm : Form
    {
        string filePath;
        public string item;
        public int pathNum;
        PlayerControl pc;
        int squadSize = 5;
        Thread th;
        bool turning;
        Keys turnKey;
        bool moving;
        Keys moveKey;
        public PlayerForm(int squadSize)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            this.squadSize = squadSize;
            pc = new PlayerControl(squadSize);
        }


        private void MapMaker_Load(object sender, EventArgs e)
        {

            pc.AutoScroll = true;
            pc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(pc);
            pc.Show();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (turning)
            {
                switch (turnKey)
                {
                    case Keys.L:
                        //mapPanel.Controls.Remove(pc);
                        pc.squad.units[0].direction = (pc.squad.units[0].direction + 2) % 360;
                        //mapPanel.Controls.Add(pc);
                        break;
                    case Keys.K:
                        pc.squad.units[0].direction = (pc.squad.units[0].direction - 2) % 360;
                        break;
                }
            }
            if (moving)
            {
                int forward = 0;
                switch (moveKey)
                {
                    case Keys.W:
                        forward = 1;
                        break;
                    case Keys.S:
                        forward = -1;
                        break;
                }

                double angleRad = pc.squad.units[0].direction / 360d * Math.PI * 2;
                Vector2 dirVec = new Vector2(0.1f * (float)Math.Sin(angleRad), 0.1f * -(float)(Math.Cos(angleRad))) * forward;
                Vector2 nextCoord = pc.squad.units[0].gridCoord + dirVec;
                Ray ray = Ray.castRay(pc.grid, pc.squad.units[0].centerCoord, angleRad - Math.PI * forward / 2);
                if (ray.magnitude >= 0.1)
                {
                    pc.squad.units[0].gridCoord += dirVec;
                }
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

        private void loadFile()
        {
            mapPanel.Controls.Remove(pc);
            int row;
            int col;
            Grid grid;
            Cell spawnCell = new Cell();
            List<enemyPath> enemyPaths = new List<enemyPath>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                col = Convert.ToInt16(line.Split(',')[0]);
                row = Convert.ToInt16(line.Split(',')[1]);

                grid = new Grid(mapPanel.Width, mapPanel.Height, row, col);
                grid.createGrid();
                spawnCell = grid.cellArr[1, 1];
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
                                spawnCell = grid.cellArr[i, j] = grid.cellArr[i, j];
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


            pc = new PlayerControl(filePath, grid, enemyPaths, spawnCell, squadSize);
            pc.AutoScroll = true;
            pc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(pc);
            pc.Show();
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

        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PlayerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K || e.KeyCode == Keys.L)
            {
                turning = true;
                turnKey = e.KeyCode;
            }
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
            {
                moving = true;
                moveKey = e.KeyCode;
            }
        }

        private void PlayerForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void PlayerForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void PlayerForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K || e.KeyCode == Keys.L)
            {
                turning = false;
            }
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
            {
                moving = false;
            }
        }
    }
}
