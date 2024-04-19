using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;

namespace MovingThingTest
{
    public partial class MapMakerControl : UserControl
    {

        string filePath;
        bool saved;

        public Grid grid;
        public List<enemyPath> enemyPaths = new List<enemyPath>();
        public int pathNumber = 0;
        public Type item = typeof(Grass);

        public bool drag = false;
        public bool place = false;
        public Vector2 mouseDownGridCoord;

        public int mode = 0;
        public int tyle = 0;

        public List<Vector2> placingVecs = new List<Vector2>();
        public MapMakerControl(int gridWidth, int gridHeight)
        {
            InitializeComponent();

            grid = new Grid(this.Width, this.Height, gridHeight, gridWidth);
            grid.createGrid();
        }

        public MapMakerControl(string filePath, Grid grid, List<enemyPath> enemyPaths)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.grid = grid;
            this.enemyPaths = enemyPaths;
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            grid.updateScreenSize(Width, Height);
            pictureBox1.Invalidate();
        }

        private void modeButton_Click(object sender, EventArgs e)
        {
            string[] names = new string[] { "Place", "Pan" };
            mode = (mode + 1) % 2;

            modeButton.Text = names[mode];
        }

        private void Save_Click(object sender, EventArgs e)
        {
            //if (!savedAs)
            //{
            //    Save.BackColor = Color.Blue;
            //    SaveFileDialog saveFileDialog = new SaveFileDialog();
            //    saveFileDialog.DefaultExt = "txt";
            //    if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        filePath = saveFileDialog.FileName;

            //        saveFile();
            //        savedAs = true;
            //        saved = true;
            //    }
            //    Save.BackColor = Color.Green;
            //}
            //else if (!saved)
            //{
            //    saveFile();
            //    saved = true;
            //    Save.BackColor = Color.Green;
            //}
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
            using (StreamReader sr = new StreamReader(filePath))
            {

            }
        }

        private void saveFile()
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(grid.cols.ToString());
                sw.Write(",");
                sw.Write(grid.rows.ToString());
                sw.Write('\n');
                for (int i = 0; i < grid.cols; i++)
                {
                    for (int j = 0; j < grid.rows; j++)
                    {
                        sw.Write(grid.cellArr[i, j].ID.ToString().PadLeft(3, '0'));
                    }
                    sw.Write('\n');
                }
                sw.Write('\n');
                sw.Write('\n');
                foreach (enemyPath ep in enemyPaths)
                {
                    sw.Write(ep.saveString());
                    sw.Write("\n");
                }
            }
            //Save.BackColor = Color.Blue;
            //using (StreamWriter sw = new StreamWriter("P:\\6th Form Computing\\17ParkinsonM\\NEA-File_Save\\saveFile.txt"))
            //{
            //    for (int i = 0; i < grid.cols; i++)
            //    {
            //        for (int j = 0; j < grid.rows; j++)
            //        {
            //            sw.Write(grid.cellArr[i, j].ID.ToString().PadLeft(3, '0'));
            //        }
            //        sw.Write('\n');
            //    }
            //    Save.BackColor = Color.Green;
            //}
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            bool valid = true;
            Vector2 mouseMove = new Vector2((MousePosition.X - mouseDownGridCoord.X) / grid.cellSize, (MousePosition.Y - mouseDownGridCoord.Y) / grid.cellSize);
            if (drag)
            {
                grid.cameraPosition -= mouseMove;
                mouseDownGridCoord = new Vector2(MousePosition.X, MousePosition.Y);
            }
            if (place)
            {
                Cell targetCell = new Cell();
                targetCell.toBorder();
                Vector2 targetGridCoord = grid.getCellCoordFromPoint(PointToClient(new Point((int)MousePosition.X, (int)MousePosition.Y)));
                if (targetGridCoord.X >= 0 && targetGridCoord.Y >= 0 && targetGridCoord.X < grid.cols && targetGridCoord.Y < grid.rows)
                {
                    targetCell = grid.cellArr[(int)targetGridCoord.X, (int)targetGridCoord.Y];
                }
                if (!placingVecs.Contains(targetGridCoord) && (targetCell is not Border))
                {
                    if (item.Name == "existingEnemyPath")
                    {
                        enemyPaths[pathNumber].addPoint(targetCell, grid);
                    }
                    else
                    {
                    }

                    grid.placeTyle(targetCell, item);
                    foreach (enemyPath p in enemyPaths)
                    {
                        p.placeOver(targetCell, grid);
                    }

                    placingVecs.Add(targetGridCoord);
                }
            }

            grid.drawGrid(e);
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            foreach (existingEnemyPath P in enemyPaths)
            {
                P.drawPath(e, topLeft, grid.cellSize, Font);
            }
            //box.vision(grid, e);
            //box.drawVisionCone(grid, e);
            //grid.drawWalls(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            switch (mode)
            {
                case 0:
                    saved = false;
                    //Save.BackColor = Color.Blue;
                    place = true;
                    break;
                case 1:
                    drag = true;
                    Point p = PointToScreen(e.Location);
                    mouseDownGridCoord = new Vector2(p.X, p.Y);
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mode == 0)
            {
                placingVecs.Clear();
                place = false;
            }
            if (mode == 1)
            {
                drag = false;
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            grid.cameraSize.Y = grid.cameraSize.Y * 1 - e.Delta / 200f;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
        }

        private void MapMakerControl_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MapMakerControl_KeyUp(object sender, KeyEventArgs e)
        {

        }


    }
}
