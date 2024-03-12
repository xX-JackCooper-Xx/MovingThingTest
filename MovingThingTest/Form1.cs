using System.Numerics;
using System.Diagnostics;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Data;
using System.DirectoryServices.ActiveDirectory;

namespace MovingThingTest
{
    public partial class Form1 : Form
    {

        public Box box;
        public Grid grid;
        public bool cameraLock = false;

        public bool drag = false;
        public Vector2 mouseDownGridCoord;

        public int mode = 0;
        public int tyle = 0;

        public Form1()
        {
            InitializeComponent();
            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;

            grid = new Grid(pictureBox1);
            grid.createGrid();

            this.box = new Box(grid, grid.cellArr[1, 1].screenPos, grid.cellSize);


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            grid.updateScreenSize(pictureBox1);
            box.UpdatePos(grid, grid.pathStack);
            pictureBox1.Invalidate();


        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
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

        private void tyleButton_Click(object sender, EventArgs e)
        {
            Color[] colours = new Color[] { Color.Gray, Color.Red, Color.Blue };
            tyle = (tyle + 1) % 3;

            tyleButton.BackColor = colours[tyle];
        }

        private void modeButton_Click(object sender, EventArgs e)
        {
            string[] names = new string[] { "Move", "Place", "Pan" };
            mode = (mode + 1) % 3;

            modeButton.Text = names[mode];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            grid.cameraSize.Y += 2;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
            box.boxSize = grid.cellSize;
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            grid.cameraSize.Y -= 2;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
            box.boxSize = grid.cellSize;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mode == 2)
            {
                drag = true;
                Point p = pictureBox1.PointToScreen(e.Location);
                mouseDownGridCoord = new Vector2(p.X, p.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mode == 2)
            {
                drag = false;
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            grid.cameraSize.Y = grid.cameraSize.Y * 1 - e.Delta / 200f;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
            box.boxSize = grid.cellSize;
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
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Width = Width;
            pictureBox1.Height = Height;
        }
    }


}