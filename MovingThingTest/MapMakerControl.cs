using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovingThingTest
{
    public partial class MapMakerControl : UserControl
    {
        public Grid grid;
        public Type item = typeof(Grass);

        public bool drag = false;
        public bool place = false;
        public Vector2 mouseDownGridCoord;

        public int mode = 0;
        public int tyle = 0;

        public List<Vector2> placingVecs = new List<Vector2>();
        public MapMakerControl()
        {
            InitializeComponent();

            grid = new Grid(this.Width, this.Height);
            grid.createGrid();
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
            saveFile();
        }

        private void saveFile()
        {
            Save.BackColor = Color.Blue;
            using (StreamWriter sw = new StreamWriter("P:\\6th Form Computing\\17ParkinsonM\\NEA-File_Save\\saveFile.txt"))
            {
                for (int i = 0; i < grid.cols; i++)
                {
                    for (int j = 0; j < grid.rows; j++)
                    {
                        sw.Write(grid.cellArr[i, j].ID.ToString().PadLeft(3, '0'));
                    }
                    sw.Write('\n');
                }
                Save.BackColor = Color.Green;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
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
                    grid.placeTyle(targetCell, item);
                    placingVecs.Add(targetGridCoord);
                }
            }
            grid.drawGrid(e);
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
                    place = true;
                    break;
                case 1:
                    drag = true;
                    Point p = PointToScreen(e.Location);
                    mouseDownGridCoord = new Vector2(p.X, p.Y);
                    break;
            }
            if (mode == 1)
            {
                drag = true;
                Point p = PointToScreen(e.Location);
                mouseDownGridCoord = new Vector2(p.X, p.Y);
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
