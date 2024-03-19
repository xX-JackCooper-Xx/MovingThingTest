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
    public partial class UserControl1 : UserControl
    {

        public Box box;
        public Grid grid;
        public bool cameraLock = false;

        public bool drag = false;
        public Vector2 mouseDownGridCoord;

        public int mode = 0;
        public int tyle = 0;
        public UserControl1()
        {
            InitializeComponent();

            grid = new Grid(this.Width, this.Height);
            grid.createGrid();

            this.box = new Box(grid, grid.cellArr[1, 1].screenPos, grid.cellSize);
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            grid.updateScreenSize(Height, Width);
            box.UpdatePos(grid, grid.pathStack);
            Invalidate();
        }

        private void UserControl1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPos = me.Location;
            Cell targetCell;
            Vector2 targetGridCoord = grid.getCellCoordFromPoint(clickPos);
            if (targetGridCoord.X >= 0 && targetGridCoord.Y >= 0 && targetGridCoord.X < grid.cols && targetGridCoord.Y < grid.rows)
            {
                targetCell = grid.cellArr[(int)targetGridCoord.X, (int)targetGridCoord.Y];
                if (targetCell is not Border && box.currentCell != targetCell)
                {
                    switch (mode)
                    {
                        case 0:
                            grid.resetPathFind();
                            if (targetCell.permeable != 0)
                            {
                                grid.PathFind(grid, box.currentCell, targetCell);
                                //grid.ColourPath();
                            }
                            break;
                        case 1:
                            grid.placeTyle(targetCell, tyle);
                            break;
                        case 2:
                            break;
                    }
                }
            }
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            if (drag)
            {
                cameraLock = false;
                Vector2 mouseMove = new Vector2((MousePosition.X - mouseDownGridCoord.X) / grid.cellSize, (MousePosition.Y - mouseDownGridCoord.Y) / grid.cellSize);
                //Vector2 mouseMoveVec = grid.getGridCoordFromPoint()
                grid.cameraPosition -= mouseMove;
                mouseDownGridCoord = new Vector2(MousePosition.X, MousePosition.Y);
            }
            if (cameraLock)
            {
                grid.cameraPosition = box.gridCoord + new Vector2(0.5f, 0.5f);
            }
            grid.drawGrid(e);
            box.drawBox(e, grid);
            //box.vision(grid, e);
            //box.drawVisionCone(grid, e);
            //grid.drawWalls(e);
        }

        private void modeButton_Click(object sender, EventArgs e)
        {
            string[] names = new string[] { "Move", "Place", "Pan" };
            mode = (mode + 1) % 3;

            modeButton.Text = names[mode];
        }

        private void tyleButton_Click(object sender, EventArgs e)
        {
            Color[] colours = new Color[] { Color.Gray, Color.Red, Color.Blue };
            tyle = (tyle + 1) % 3;

            tyleButton.BackColor = colours[tyle];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cameraLock)
            {
                cameraLock = false;
            }
            else
            {
                grid.cameraPosition = box.gridCoord;
                cameraLock = true;
            }
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

        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mode == 2)
            {
                drag = true;
                Point p = PointToScreen(e.Location);
                mouseDownGridCoord = new Vector2(p.X, p.Y);
            }
        }

        private void UserControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mode == 2)
            {
                drag = false;
            }
        }

        private void UserControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            grid.cameraSize.Y = grid.cameraSize.Y * 1 - e.Delta / 200f;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
            box.boxSize = grid.cellSize;
        }
    }
}
