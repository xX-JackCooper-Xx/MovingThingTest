using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace MovingThingTest
{
    public class Grid
    {
        public float rows;
        public float cols;
        public float cellSize;
        public float rowsOffset;
        public float colsOffset;
        public Cell[,] cellArr; 
        public Size screenSize;
        public Grid(PictureBox pictureBox)
        {
            this.screenSize.Width = Screen.PrimaryScreen.Bounds.Width;
            this.screenSize.Height = Screen.PrimaryScreen.Bounds.Height;
            this.rows = 9;
            this.cols = 16;

            float tempcellSize1 = screenSize.Height / rows;
            float tempcellSize2 = screenSize.Width / cols;

            if (tempcellSize1 < tempcellSize2)
            {
                this.cellSize = tempcellSize1;
            } else
            {
                this.cellSize = tempcellSize2;
            }

            cellArr = new Cell[(int)cols, (int)rows];

            this.rowsOffset = screenSize.Height / 2 - rows / 2 * cellSize;
            this.colsOffset = screenSize.Width / 2 - cols / 2 * cellSize;
        }   
        
        public void createGrid()
        {
            for(int i = 0; i < cols; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    Vector2 cellPos = new Vector2(colsOffset + (i*cellSize), rowsOffset + (j * cellSize));
                    cellArr[i,j] = new Cell(i,j,cellPos,cellSize);
                }
            }
        }

        public void drawGrid(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    g.DrawRectangle(pen, cellArr[i, j].cellPos.X, cellArr[i, j].cellPos.Y, cellSize, cellSize);
                }
            }
        }

        public Vector2 getCellFromPoint(Point clickPos)
        {
            Vector2 clickPosVec = new Vector2(clickPos.X, clickPos.Y);

            clickPosVec.X = (int)MathF.Floor(((clickPosVec.X - colsOffset) / cellSize));
            clickPosVec.Y = (int)MathF.Floor(((clickPosVec.Y - rowsOffset) / cellSize));
    

            return clickPosVec;
        }

         
        public Vector2 getCellFromCoord(Vector2 gridCoord)
        {

            Vector2 cellVec = new Vector2(gridCoord.X, gridCoord.Y);

            cellVec.X = (int)MathF.Floor(((cellVec.X - colsOffset) / cellSize));
            cellVec.Y = (int)MathF.Floor(((cellVec.Y - rowsOffset) / cellSize));


            return cellVec;
        }

        public Vector2 getCoordFromGrid(Vector2 gridCoord, Grid grid)
        {
            Vector2 targetVec = new Vector2();
            try
            {
                targetVec.X = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y].cellPos.X;
                targetVec.Y = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y].cellPos.Y;
            }
            catch
            {
                targetVec = new Vector2(-1, -1);
            }
            return targetVec;

        }

        public int PathFind(Grid grid, Cell currentCell, Cell targetCell, BinTree? tree)
        {
            if (tree == null)
            {
                tree = new BinTree(null, null);
            }

       


            List<Cell> openList = new List<Cell>();
            List<Cell> closedList = new List<Cell>();

            float lowestF = 999999999;
            int searchDepth = 1;

            openList.Add(currentCell);

            for (int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    try
                    {
                        Cell tempCell = grid.cellArr[currentCell.col + i,currentCell.row + j];  
                        if (tempCell.permeable == 1)
                        {
                            openList.Add(grid.cellArr[currentCell.col + i, currentCell.row + j]);
                            tempCell.pathParent = currentCell;
                            if (i == 0 || j == 0)
                            {
                                tempCell.pathCostG = 10;
                            }
                            else
                            {
                                tempCell.pathCostG = 14;
                            }
                            tempCell.pathCostH = (Math.Abs(tempCell.row - targetCell.row) + Math.Abs(tempCell.col - targetCell.col))*10;
                            tempCell.pathCostF = tempCell.pathCostG + tempCell.pathCostH;

                        }
                        grid.cellArr[currentCell.col + i, currentCell.row + j] = tempCell;
                        tree.Add(tempCell.pathCostF, grid.cellArr[currentCell.col + i, currentCell.row + j]);

                    }
                    catch {}
                }
            }
            openList.Remove(currentCell);
            closedList.Add(currentCell);

            foreach (Cell cell in openList)
            {
                Cell cell1 = tree.ReturnInOrder(searchDepth);
                PathFind(grid, cell1, targetCell, tree);
            }

            searchDepth++;
            return 5;
        }
    }

    public class Cell
    {
        public int row;
        public int col;
        public float cellSize;
        public Vector2 cellPos;
        public float permeable;
        public Cell pathParent;
        public int pathCostG;
        public int pathCostH;
        public int pathCostF;
        public Cell(int col, int row, Vector2 cellPos, float cellSize)
        {
            this.row = row;
            this.col = col;
            this.cellPos = cellPos;
            this.cellSize = cellSize;
            this.permeable = 1;
        }
    }
}
