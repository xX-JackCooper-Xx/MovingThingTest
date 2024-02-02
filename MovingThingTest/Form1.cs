using System.Numerics;
using System.Diagnostics;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Data; 
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

namespace MovingThingTest
{
    public partial class Form1 : Form
    {

        public Box box;
        public Grid grid;
        public ListBox list;

        public int mode = 0;
        public int tyle = 0;

        public float cellSize;
        public float colsOffset;
        public float rowsOffset;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            grid = new Grid();
            grid.createGrid();

            this.cellSize = grid.calculateCellSize(grid.cameraSizeX, grid.cameraSizeY);
            this.colsOffset = grid.calculateColsOffset(grid.cameraSizeX, cellSize);
            this.rowsOffset = grid.calculateRowsOffset(grid.cameraSizeY, cellSize);

            this.box = new Box(grid, new Vector2(1, 1), grid.cellSize);

            list = new ListBox
            {
                Location = new Point(800, 40),
                Size = new Size(100, 800)
            };

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.cellSize = grid.calculateCellSize(grid.cameraSizeX, grid.cameraSizeY);
            this.colsOffset = grid.calculateColsOffset(grid.cameraSizeX, cellSize);
            this.rowsOffset = grid.calculateRowsOffset(grid.cameraSizeY, cellSize);

            box.UpdatePos(grid, grid.pathStack, cellSize, colsOffset, rowsOffset);
            pictureBox1.Invalidate();


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPos = me.Location;
            Cell targetCell;
            Vector2 targetGridCoord = grid.getGridCoordFromPoint(clickPos);


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
                        placeTyle(targetCell);
                        break;
                }
            }
        }

        private void placeTyle(Cell targetCell)
        {
            switch (tyle)
            {
                case 0:
                    if (targetCell is not Wall)
                    {
                        removeFromList(targetCell);
                        grid.cellArr[targetCell.col, targetCell.row] = targetCell.toWall();
                        grid.walls.Add(grid.cellArr[targetCell.col, targetCell.row]);
                    }
                    else
                    {
                        grid.walls.Remove(targetCell);
                        grid.cellArr[targetCell.col, targetCell.row] = targetCell.toGrass();
                        grid.grass.Add(grid.cellArr[targetCell.col, targetCell.row]);
                    }
                    break;
                case 1:
                    if (targetCell is not Dirt)
                    {
                        removeFromList(targetCell);
                        grid.cellArr[targetCell.col, targetCell.row] = targetCell.toDirt();
                        grid.dirt.Add(grid.cellArr[targetCell.col, targetCell.row]);
                    }
                    else
                    {
                        grid.dirt.Remove(targetCell);
                        grid.cellArr[targetCell.col, targetCell.row] = targetCell.toGrass();
                        grid.grass.Add(grid.cellArr[targetCell.col, targetCell.row]);
                    }
                    break;
                case 2:
                    if (targetCell is not Glass)
                    {
                        removeFromList(targetCell);
                        grid.cellArr[targetCell.col, targetCell.row] = targetCell.toGlass();
                        grid.glass.Add(grid.cellArr[targetCell.col, targetCell.row]);
                    }
                    else
                    {
                        grid.glass.Remove(targetCell);
                        grid.cellArr[targetCell.col, targetCell.row] = targetCell.toGrass();
                        grid.grass.Add(grid.cellArr[targetCell.col, targetCell.row]);
                    }
                    break;
            }
        }

        private void removeFromList(Cell c)
        {
            switch (c)
            {
                case Wall:
                    grid.walls.Remove(c);
                    break;
                case Grass:
                    grid.grass.Remove(c);
                    break;
                case Dirt:
                    grid.dirt.Remove(c);
                    break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            grid.drawGrid(e, cellSize, colsOffset, rowsOffset);
            box.drawBox(grid, e, cellSize, colsOffset, rowsOffset);
            box.drawVisionCone(grid, e, cellSize, colsOffset, rowsOffset);
            grid.drawWalls(e);
        }

        private void tyleButton_Click(object sender, EventArgs e)
        {
            Color[] colours = new Color[] { Color.Gray, Color.Red, Color.Blue };
            tyle = (tyle + 1) % 3;

            tyleButton.BackColor = colours[tyle];
        }

        private void modeButton_Click(object sender, EventArgs e)
        {
            string[] names = new string[] { "Move", "Place" };
            mode = (mode + 1) % 2;

            modeButton.Text = names[mode];
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            grid.cameraSizeX = grid.cameraSizeX / 2;
            grid.cameraSizeY = grid.cameraSizeY / 2;
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            grid.cameraSizeX = grid.cameraSizeX * 2;
            grid.cameraSizeY = grid.cameraSizeY * 2;
        }
    }

    public class Box
    {
        public Vector2 gridCoord = new Vector2();
        public Cell currentCell;
        public Cell targetCell;


        float boxSize;
        Vector2 movingVec = new Vector2(0, 0);


        public Box(Grid grid, Vector2 gridCoord, float boxSize)
        {
            this.gridCoord = gridCoord;

            this.boxSize = boxSize;

            currentCell = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y];
            targetCell = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y];
        }

        public void UpdatePos(Grid grid, Stack<Cell> cellStack, float cellSize, float colsOffset, float rowsOffset)
        {
            //if (currentCell.screenPos != screenPos)
            //{
            //    screenPos += movingVec;
            //}
            //else if (cellStack.Count > 0)
            //{
            //    currentCell = cellStack.Pop();
            //    if (currentCell.screenPos == screenPos)
            //    {
            //        currentCell = cellStack.Pop();
            //    }
            //    movingVec = Vector2.Normalize(currentCell.screenPos - screenPos) * (int)(grid.cellSize * 0.5);
            //    screenPos += movingVec;
            //}

            if (currentCell.col != gridCoord.X && currentCell.row != gridCoord.Y)
            {
                gridCoord += movingVec;
            }
            else if (cellStack.Count > 0)
            {
                currentCell = cellStack.Pop();
                if (currentCell.col == gridCoord.X && currentCell.row == gridCoord.Y)
                {
                    currentCell = cellStack.Pop();
                }
                movingVec = Vector2.Normalize(new Vector2(currentCell.col - gridCoord.X, currentCell.row - gridCoord.Y));
                gridCoord += movingVec;
            }

            //if(cellStack.Count > 0 && moveCooldown <= 0)
            //{
            //    currentCell = cellStack.Pop();
            //    moveCooldown = 1;
            //}
            //else
            //{
            //    moveCooldown--;
            //}
        }

        public void drawBox(Grid grid, PaintEventArgs e, float cellSize, float colsOffset, float rowsOffset)
        {
            Vector2 screenPos = grid.calculateScreenPos(gridCoord.X, gridCoord.Y, colsOffset, rowsOffset, cellSize);
            SolidBrush brush = new SolidBrush(Color.Yellow);
            e.Graphics.FillRectangle(brush, screenPos.X, screenPos.Y, cellSize, cellSize);
        }

        public List<Point> getFurthestWallPoints(Wall wall, Grid grid, Vector2 screenPos)
        {
            Point centerScreenPos = new Point((int)(screenPos.X + boxSize / 2), (int)(screenPos.Y + boxSize / 2));
            Point wallScreenPos = new Point((int)screenPos.X, (int)screenPos.Y);

            List<Point> closestPoints = new List<Point>();

            float relativeX = screenPos.X - centerScreenPos.X;
            float relativeY = screenPos.Y - centerScreenPos.Y;
            if (relativeX > 0 && relativeY >= -1 * grid.cellSize)
            {
                closestPoints.Add(new Point(wallScreenPos.X, wallScreenPos.Y + (int)grid.cellSize));
            }
            if (relativeX > 0 && relativeY <= 0)
            {
                closestPoints.Add(wallScreenPos);
            }
            if (relativeX >= -1 * grid.cellSize && relativeY > 0)
            {
                closestPoints.Add(new Point(wallScreenPos.X + (int)grid.cellSize, wallScreenPos.Y));
            }
            if (relativeX >= -1 * grid.cellSize && relativeY < -1 * grid.cellSize)
            {
                closestPoints.Add(new Point(wallScreenPos.X + (int)grid.cellSize, wallScreenPos.Y + (int)grid.cellSize));
            }
            if (relativeX <= 0 && relativeY > 0)
            {
                closestPoints.Add(wallScreenPos);
            }
            if (relativeX <= 0 && relativeY < -1 * grid.cellSize)
            {
                closestPoints.Add(new Point(wallScreenPos.X, wallScreenPos.Y + (int)grid.cellSize));
            }
            if (relativeX < -1 * grid.cellSize && relativeY >= -1 * grid.cellSize)
            {
                closestPoints.Add(new Point(wallScreenPos.X + (int)grid.cellSize, wallScreenPos.Y + (int)grid.cellSize));
            }
            if (relativeX < -1 * grid.cellSize && relativeY <= 0)
            {
                closestPoints.Add(new Point(wallScreenPos.X + (int)grid.cellSize, wallScreenPos.Y));
            }

            return closestPoints;
        }

        public List<Point> createMask(List<Point> points, Grid grid, Vector2 screenPos)
        {
            List<Point> farPoints = new List<Point>();
            Point centerScreenPos = new Point((int)(screenPos.X + boxSize / 2), (int)(screenPos.Y + boxSize / 2));
            Vector2 centerScreenVec = new Vector2(centerScreenPos.X, centerScreenPos.Y);
            List<Vector2> cornerVecs = new List<Vector2>()
            {
                new Vector2(grid.colsOffset + grid.cellSize, grid.cellSize + grid.rowsOffset),
                new Vector2(grid.cellSize * (grid.cols-1) + grid.colsOffset, grid.cellSize + grid.rowsOffset),
                new Vector2(grid.colsOffset + grid.cellSize, grid.cellSize*(grid.rows-1)+ grid.rowsOffset),
                new Vector2(grid.cellSize * (grid.cols-1) + grid.colsOffset,grid.cellSize *(grid.rows - 1) + grid.rowsOffset)
            };

            cornerVecs.Reverse();

            List<Vector2> vecsToCorners = new List<Vector2>();
            foreach (Vector2 vec in cornerVecs)
            {
                vecsToCorners.Add(Vector2.Subtract(vec, centerScreenVec));
            }

            float a;
            float b;

            foreach (Point point in points)
            {
                Vector2 direction = Vector2.Normalize(new Vector2(point.X - centerScreenPos.X, point.Y - centerScreenPos.Y));

                if (direction.Y < 0)
                {
                    a = (grid.rowsOffset + grid.cellSize - point.Y) / direction.Y;
                }
                else
                {
                    a = (grid.rowsOffset + grid.cellSize * (grid.rows - 1) - point.Y) / direction.Y;
                }
                if (direction.X < 0)
                {
                    b = (grid.colsOffset + grid.cellSize - point.X) / direction.X;
                }
                else
                {
                    b = (grid.cellSize * (grid.cols - 1) + grid.colsOffset - point.X) / direction.X;
                }


                float max = Math.Min(a, b);

                Point farPoint = new Point((int)(point.X + direction.X * max), (int)(point.Y + direction.Y * max));
                farPoints.Add(farPoint);
            }

            List<Vector2> vecsToPoints = new List<Vector2>();

            foreach (Point point in farPoints)
            {
                vecsToPoints.Add(Vector2.Subtract(new Vector2(point.X, point.Y), centerScreenVec));
            }

            for (int i = 0; i < vecsToCorners.Count; i++)
            {
                bool u = (vecsToPoints[0].Y * vecsToCorners[i].X - vecsToPoints[0].X * vecsToCorners[i].Y) * (vecsToPoints[0].Y * vecsToPoints[1].X - vecsToPoints[0].X * vecsToPoints[1].Y) >= 0;
                bool v = (vecsToPoints[1].Y * vecsToCorners[i].X - vecsToPoints[1].X * vecsToCorners[i].Y) * (vecsToPoints[1].Y * vecsToPoints[0].X - vecsToPoints[1].X * vecsToPoints[0].Y) >= 0;
                if (u && v)
                {
                    farPoints.Add(new Point((int)cornerVecs[i].X, (int)cornerVecs[i].Y));
                }
            }
            return farPoints;
        }

        public void drawVisionCone(Grid grid, PaintEventArgs e, float cellSize, float colsOffset, float rowsOffset)
        {

            Vector2 screenPos = grid.calculateScreenPos((int)gridCoord.X, (int)gridCoord.Y, colsOffset, rowsOffset, cellSize);
            Color color;
            color = Color.FromArgb(100, 0, 0, 0);
            SolidBrush brush;
            brush = new SolidBrush(color);

            Region region = new Region();
            Region regionInverse = new Region();
            foreach (Wall wall in grid.walls)
            {
                List<Point> points = getFurthestWallPoints(wall, grid, screenPos);

                List<Point> farPoints = createMask(points, grid, screenPos);
                List<Point> allPoints = new List<Point>() { points[0], farPoints[0] };
                for (int i = 2; i < farPoints.Count; i++)
                {
                    allPoints.Add(farPoints[i]);
                }
                allPoints.Add(farPoints[1]);
                allPoints.Add(points[1]);

                GraphicsPath mask = new GraphicsPath();
                mask.AddPolygon(allPoints.ToArray());

                region.Exclude(mask);
            }
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            regionInverse.Exclude(region);
            e.Graphics.FillRegion(brush, regionInverse);
        }
    }
}