using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace MovingThingTest
{
    public class Grid
    {
        public int rows;
        public int cols;
        public float cellSize;
        public float rowsOffset;
        public float colsOffset;

        public Vector2 cameraCoord;
        public int cameraSizeX;
        public int cameraSizeY;

        public Cell[,] cellArr;
        public Size screenSize;
        List<Cell> openList = new List<Cell>();
        List<Cell> closedList = new List<Cell>();

        public Stack<Cell> pathStack = new Stack<Cell>();

        public List<Cell> walls = new List<Cell>();
        public List<Vector2[]> wallSides = new List<Vector2[]>();
        public List<Cell> grass = new List<Cell>();
        public List<Cell> dirt = new List<Cell>();
        public List<Cell> glass = new List<Cell>();

        public Grid()
        {
            this.screenSize.Width = Screen.PrimaryScreen.Bounds.Width;
            this.screenSize.Height = Screen.PrimaryScreen.Bounds.Height;
            this.rows = 100;
            this.cols = 100;

            this.cameraCoord = new Vector2(0, 0);
            this.cameraSizeX = 32;
            this.cameraSizeY = 18;

            int tempcellSize1 = (screenSize.Height / rows);
            int tempcellSize2 = (screenSize.Width / cols);

            if (tempcellSize1 < tempcellSize2)
            {
                this.cellSize = tempcellSize1;
            }
            else
            {
                this.cellSize = tempcellSize2;
            }

            cellArr = new Cell[cols, rows];

            this.rowsOffset = screenSize.Height / 2 - (float)rows / 2 * cellSize;
            this.colsOffset = screenSize.Width / 2 - (float)cols / 2 * cellSize;
        }

        public Vector2 calculateScreenPos(float col, float row, float colsOffset, float rowsOffset, float cellSize)
        {
            return new Vector2(colsOffset + (col * cellSize), rowsOffset + (row * cellSize));
        }


        public float calculateCellSize(int width, int height)
        {
            float tempcellSize1 = ((float)screenSize.Height / (float)height);
            //float tempcellSize2 = ((float)screenSize.Width / (float)width);

            //if (tempcellSize1 < tempcellSize2)
            //{
            //    cellSize = tempcellSize1;
            //}
            //else
            //{
            //    cellSize = tempcellSize2;
            //}
            return tempcellSize1;
        }

        public float calculateRowsOffset(int height , float cellSize)
        {
            return rowsOffset = (float)screenSize.Height / 2 - (float)height / 2 * cellSize;
        }

        public float calculateColsOffset(int width, float cellSize)
        {
            return colsOffset = (float)screenSize.Width / 2 - (float)width / 2 * cellSize;
        }

        public void createGrid()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    cellArr[i, j] = new Grass(i, j);
                    grass.Add(cellArr[i, j]);
                }
                cellArr[i, 0] = cellArr[i, 0].toBorder();

                cellArr[i, rows - 1] = cellArr[i, rows - 1].toBorder();
            }

            for (int i = 0; i < rows; i++)
            {
                cellArr[0, i] = cellArr[0, i].toBorder();

                cellArr[cols - 1, i] = cellArr[cols - 1, i].toBorder();
            }

            
        }

        public void drawGrid(PaintEventArgs e, float cellSize, float colsOffset, float rowsOffset)
        {
            this.cellSize = cellSize;
            this.colsOffset = colsOffset;
            this.rowsOffset = rowsOffset;
            for (int i = (int)cameraCoord.Y; i < Math.Ceiling(cameraCoord.Y + cameraSizeY); i++)
            {
                for (int j = (int)cameraCoord.X; j < Math.Ceiling(cameraCoord.X + cameraSizeX); j++)
                {
                    if (j >= cols || i >= rows)
                    {
                        break;
                    }
                    cellArr[j, i].drawCell(e, this);
                }
            }

            //foreach (Cell cell in cellArr)
            //{
            //    cell.drawCell(e);
            //}
        }

        public void drawWalls(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);
            SolidBrush cellBrush;

            foreach(Wall wall in walls)
            {
                cellBrush = new SolidBrush(wall.color);
                Vector2 screenPos = calculateScreenPos(wall.col, wall.row, colsOffset, rowsOffset, cellSize);
                Rectangle rect = new Rectangle((int)screenPos.X, (int)screenPos.Y, (int)cellSize, (int)cellSize);
                e.Graphics.FillRectangle(cellBrush, rect);
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        public Vector2 getGridCoordFromPoint(Point clickPos)
        {
            Vector2 clickPosVec = new Vector2(clickPos.X, clickPos.Y);

            clickPosVec.X = (int)MathF.Floor(((clickPosVec.X - colsOffset) / cellSize));
            clickPosVec.Y = (int)MathF.Floor(((clickPosVec.Y - rowsOffset) / cellSize));

            return clickPosVec;
        }

        public Cell getLowestFCost(List<Cell> list)
        {
            double lowestF = 999999999;
            Cell lowestFCell = new Cell();
            foreach (Cell cell in list)
            {
                if (cell.pathCostF < lowestF)
                {
                    lowestFCell = cell;
                    lowestF = cell.pathCostF;
                }
            }
            return lowestFCell;
        }

        public Vector2 getGridCoordFromScreenCoord(Vector2 gridCoord)
        {

            Vector2 cellVec = new Vector2(gridCoord.X, gridCoord.Y);

            cellVec.X = (int)MathF.Floor((cellVec.X - colsOffset) / cellSize);
            cellVec.Y = (int)MathF.Floor((cellVec.Y - rowsOffset) / cellSize);


            return cellVec;
        }

        

        //public Vector2 getScreenCoordFromGridCoord(Vector2 gridCoord, Grid grid)
        //{
        //    Vector2 targetVec = new Vector2();
        //    try
        //    {
        //        targetVec.X = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y].screenPos.X;
        //        targetVec.Y = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y].screenPos.Y;
        //    }
        //    catch
        //    {
        //        targetVec = new Vector2(-1, -1);
        //    }
        //    return targetVec;

        //}

        public void resetPathFind()
        {
            foreach(Cell c in openList)
            {

                cellArr[c.col, c.row].resetPathFind();
            }
            foreach (Cell c in closedList)
            {
                cellArr[c.col, c.row].resetPathFind();
            }

            openList = new List<Cell>();
            closedList = new List<Cell>();

        }

        public double calculateHeuristic(Cell currenctCell, Cell targetCell)
        {
            int x = Math.Abs(targetCell.col - currenctCell.col);
            int y = Math.Abs(targetCell.row - currenctCell.row);
            double z = (x+y) * (1.02);

            return z;
        }

        public Stack<Cell> PathFind(Grid grid, Cell currentCell, Cell targetCell)
        {
            if (currentCell.pathCostG == 999999)
            {
                currentCell.pathCostG = 0;
            }

            pathStack = new Stack<Cell>();

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Cell neighbour = currentCell.getNeighbour(grid, i);
                    if (!(closedList.Contains(neighbour) || neighbour.permeable == 0))
                    {
                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);

                            //grid.cellArr[currentCell.col + i, currentCell.row + j].color = Color.Blue;
                            neighbour.pathParent = currentCell;
                            neighbour.pathCostG = currentCell.pathCostG + (1 * (1 / currentCell.permeable));

                        }

                        if (neighbour.pathCostG > (currentCell.pathCostG + 1))
                        {
                            neighbour.pathCostG = currentCell.pathCostG + (1 * (1 / currentCell.permeable));
                            neighbour.pathParent = currentCell;
                        }

                        neighbour.pathCostH = calculateHeuristic(neighbour, targetCell);
                        neighbour.pathCostF = neighbour.pathCostH + neighbour.pathCostG;

                        grid.cellArr[neighbour.col, neighbour.row] = neighbour;
                    }
                }
                catch { }
            }

            try
            {
                if (!(closedList.Contains(targetCell) || openList.Count == 0))
                {
                    PathFind(grid, getLowestFCost(openList), targetCell);
                }
                
            }
            catch { }


            if (pathStack.Count == 0)
            {
                getParents(grid, targetCell);
            }
            return pathStack;
        }

        public Stack<Cell> getParents(Grid grid, Cell targetCell)
        {
            Cell cell = targetCell;

            while(cell.pathParent!= null)
            {
                pathStack.Push(grid.cellArr[cell.col, cell.row]);
                cell = cell.pathParent;
            }
            pathStack.Push(grid.cellArr[cell.col,cell.row]);
            return pathStack;
        }

        public void ColourPath()
        {
            foreach(Cell c in pathStack)
            {
                c.color = Color.Red;
            }
        }
    }
}
