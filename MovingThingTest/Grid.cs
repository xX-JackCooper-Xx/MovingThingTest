﻿using System;
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
        public Cell[,] cellArr;
        public Size screenSize;
        List<Cell> openList = new List<Cell>();
        List<Cell> closedList = new List<Cell>();

        public Vector2 cameraPosition;
        public Vector2 cameraSize;
        public float cameraRatio;


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
            this.rows = 18;
            this.cols = 32;

            float screenSizeRatio = (float)screenSize.Width / (float)screenSize.Height;
            cameraSize.Y = 7;
            cameraSize.X = screenSizeRatio * cameraSize.Y;

            cameraPosition = new Vector2(cameraSize.X/2, cameraSize.Y/2);



            cameraRatio = cameraSize.X / cameraSize.Y;

            //int tempcellSize1 = (screenSize.Height / rows);
            //int tempcellSize2 = (screenSize.Width / cols);

            //if (tempcellSize1 < tempcellSize2)
            //{
            //    this.cellSize = tempcellSize1;
            //}
            //else
            //{
            //    this.cellSize = tempcellSize2;
            //}

            //cellArr = new Cell[cols, rows];

            //this.rowsOffset = screenSize.Height / 2 - (float)rows / 2 * cellSize;
            //this.colsOffset = screenSize.Width / 2 - (float)cols / 2 * cellSize;

            //float tempcellSize1 = screenSize.Height / cameraSize.Y;
            //float tempcellSize2 = screenSize.Width / cameraSize.X;

            //if (tempcellSize1 < tempcellSize2)
            //{
            //    this.cellSize = tempcellSize1;
            //}
            //else
            //{
            //    this.cellSize = tempcellSize2;
            //}

            cellArr = new Cell[cols, rows];

            cellSize = calculateCellSize();

            this.rowsOffset = 0;
            this.colsOffset = 0;
            

        }

        public float calculateCellSize()
        {
            float cellSize;
            float tempcellSize1 = screenSize.Height / cameraSize.Y;
            float tempcellSize2 = screenSize.Width / cameraSize.X;

            if (tempcellSize1 < tempcellSize2)
            {
                cellSize = tempcellSize1;
            }
            else
            {
                cellSize = tempcellSize2;
            }

            return cellSize;
        }

        public void createGrid()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Vector2 screenPos = new Vector2(colsOffset + (i * cellSize), rowsOffset + (j * cellSize));
                    cellArr[i, j] = new Grass(i, j, screenPos, cellSize);
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

        public void drawGrid(PaintEventArgs e)
        {
            Vector2 topLeftCell = new Vector2(MathF.Floor(cameraPosition.X - cameraSize.X / 2), MathF.Floor(cameraPosition.Y - cameraSize.Y / 2));
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2),(cameraPosition.Y - cameraSize.Y / 2));
            Vector2 bottomRightCell = new Vector2(MathF.Ceiling(cameraPosition.X + cameraSize.X / 2), MathF.Ceiling(cameraPosition.Y + cameraSize.Y / 2));

            for (int i = (int)topLeftCell.X; i < (int)bottomRightCell.X; i++)
            {
                for(int j = (int)topLeftCell.Y; j < (int)bottomRightCell.Y; j++)
                {
                    if (i >= 0 && j >= 0 && i < cols && j < rows)
                    {
                        cellArr[i, j].drawCell(e, topLeft, cellSize, i, j);
                    }
                }
            }

            //foreach(Cell cell in cellArr)
            //{
            //    cell.drawCell(e);
            //}
        }

        public void drawWalls(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);

            foreach(Wall wall in walls)
            {
                Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));
                //wall.drawCell(e, topLeft, cellSize);
                //cellBrush = new SolidBrush(wall.color);
                //Rectangle rect = new Rectangle((int)wall.screenPos.X, (int)wall.screenPos.Y, wall.cellSize, wall.cellSize);
                //e.Graphics.FillRectangle(cellBrush, rect);
                //e.Graphics.DrawRectangle(pen, rect);
            }
        }

        public Vector2 getGridCoordFromPoint(Point clickPos)
        {
            Vector2 clickPosVec = new Vector2(clickPos.X, clickPos.Y);
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));


            clickPosVec.X = (clickPosVec.X + topLeft.X*cellSize) / cellSize;
            clickPosVec.Y = (clickPosVec.Y + topLeft.Y*cellSize) / cellSize;

            return clickPosVec;
        }



        public Vector2 getCellCoordFromPoint(Point clickPos)
        {
            Vector2 clickPosVec = new Vector2(clickPos.X, clickPos.Y);
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));


            clickPosVec.X = (int)MathF.Floor(((clickPosVec.X + topLeft.X * cellSize) / cellSize));
            clickPosVec.Y = (int)MathF.Floor(((clickPosVec.Y + topLeft.Y * cellSize) / cellSize));

            return clickPosVec;
        }

        public PointF getPointFromGridPoint(PointF gridPoint)
        {
            PointF point = new PointF();
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));

            point.X = (cellSize * (gridPoint.X - topLeft.X)) ;
            point.Y = (cellSize * (gridPoint.Y - topLeft.Y));

            return point;
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

            cellVec.X = (int)MathF.Floor(((cellVec.X - colsOffset) / cellSize));
            cellVec.Y = (int)MathF.Floor(((cellVec.Y - rowsOffset) / cellSize));


            return cellVec;
        }

        

        public Vector2 getScreenCoordFromGridCoord(Vector2 gridCoord, Grid grid)
        {
            Vector2 targetVec = new Vector2();
            try
            {
                targetVec.X = cellArr[(int)gridCoord.X, (int)gridCoord.Y].screenPos.X;
                targetVec.Y = cellArr[(int)gridCoord.X, (int)gridCoord.Y].screenPos.Y;
            }
            catch
            {
                targetVec = new Vector2(-1, -1);
            }
            return targetVec;

        }

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

                            //neighbour.color = Color.Blue;
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

                        cellArr[neighbour.col, neighbour.row] = neighbour;
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
                pathStack.Push(cellArr[cell.col, cell.row]);
                cell = cell.pathParent;
            }
            pathStack.Push(cellArr[cell.col,cell.row]);
            return pathStack;
        }

        public void ColourPath()
        {
            foreach(Cell c in pathStack)
            {
                c.color = Color.Red;
            }
        }

        public void placeTyle(Cell targetCell, int tyle)
        {
            switch (tyle)
            {
                case 0:
                    if (targetCell is not Wall)
                    {
                        removeFromList(targetCell);
                        cellArr[targetCell.col, targetCell.row] = targetCell.toWall();
                        walls.Add(cellArr[targetCell.col, targetCell.row]);
                    }
                    else
                    {
                        walls.Remove(targetCell);
                        cellArr[targetCell.col, targetCell.row] = targetCell.toGrass();
                        grass.Add(cellArr[targetCell.col, targetCell.row]);
                    }
                    break;
                case 1:
                    if (targetCell is not Dirt)
                    {
                        removeFromList(targetCell);
                        cellArr[targetCell.col, targetCell.row] = targetCell.toDirt();
                        dirt.Add(cellArr[targetCell.col, targetCell.row]);
                    }
                    else
                    {
                        dirt.Remove(targetCell);
                        cellArr[targetCell.col, targetCell.row] = targetCell.toGrass();
                        grass.Add(cellArr[targetCell.col, targetCell.row]);
                    }
                    break;
                case 2:
                    if (targetCell is not Glass)
                    {
                        removeFromList(targetCell);
                        cellArr[targetCell.col, targetCell.row] = targetCell.toGlass();
                        glass.Add(cellArr[targetCell.col, targetCell.row]);
                    }
                    else
                    {
                        glass.Remove(targetCell);
                        cellArr[targetCell.col, targetCell.row] = targetCell.toGrass();
                        grass.Add(cellArr[targetCell.col, targetCell.row]);
                    }
                    break;
            }
        }
        private void removeFromList(Cell c)
        {
            switch (c)
            {
                case Wall:
                    walls.Remove(c);
                    break;
                case Grass:
                    grass.Remove(c);
                    break;
                case Dirt:
                    dirt.Remove(c);
                    break;
            }
        }
    }
}
