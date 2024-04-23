﻿using MovingThingTest.Menu;
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
        string filePath;
        public string item;
        public int pathNum;
        MenuSelector ms = new MenuSelector("play");
        PlayerControl pc;
        int squadSize = 5;
        Thread th;
        public PlayerForm(int squadSize)
        {
            InitializeComponent();
            this.squadSize = squadSize;
            pc = new PlayerControl(squadSize);
        }


        private void MapMaker_Load(object sender, EventArgs e)
        {

            pc.AutoScroll = true;
            pc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(pc);
            pc.Show();


            ms.AutoScroll = true;
            ms.Dock = DockStyle.Fill;
            controlsPanel.Controls.Add(ms);
            ms.Show();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (ms.pathNumber != pathNum)
            {
                pathNum = ms.pathNumber;
                pc.squad.changeFormation(pathNum, pc.grid);
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
            controlsPanel.Controls.Remove(ms);
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

            ms = new MenuSelector(enemyPaths, "play");
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