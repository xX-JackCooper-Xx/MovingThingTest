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
using System.Dynamic;

namespace MovingThingTest
{
    public class Grid
    {
        // Class fields
        public int rows; // Number of rows in the grid
        public int cols; // Number of columns in the grid
        public float cellSize; // Size of each grid cell
        public float rowsOffset; // Offset for rows
        public float colsOffset; // Offset for columns
        public Cell[,] cellArr; // Array to store grid cells
        public Size screenSize; // Size of the screen
        List<Cell> openList = new List<Cell>(); // List of cells in the open list for pathfinding
        List<Cell> closedList = new List<Cell>(); // List of cells in the closed list for pathfinding

        public Vector2 cameraPosition; // Position of the camera
        public Vector2 cameraSize; // Size of the camera
        public float cameraRatio; // Aspect ratio of the camera

        public Stack<Cell> pathStack = new Stack<Cell>(); // Stack to store the path found by pathfinding algorithm

        public List<Vector2[]> wallSides = new List<Vector2[]>(); // List to store sides of walls

        // Constructor for Grid class
        public Grid(int width, int height)
        {
            // Initialize screen size
            this.screenSize.Width = width;
            this.screenSize.Height = height;
            this.rows = 18; // Default number of rows
            this.cols = 32; // Default number of columns

            // Calculate camera size and position
            float screenSizeRatio = (float)screenSize.Width / (float)screenSize.Height;
            cameraSize.Y = 7;
            cameraSize.X = screenSizeRatio * cameraSize.Y;

            cameraPosition = new Vector2(cameraSize.X / 2, cameraSize.Y / 2);
            cameraRatio = cameraSize.X / cameraSize.Y;

            // Initialize cell array
            cellArr = new Cell[cols, rows];

            // Calculate cell size
            cellSize = calculateCellSize();

            this.rowsOffset = 0;
            this.colsOffset = 0;
        }

        // Overloaded constructor for Grid class
        public Grid(int width, int height, int rows, int cols)
        {
            // Initialize screen size, rows, and columns
            this.screenSize.Width = width;
            this.screenSize.Height = height;
            this.rows = rows;
            this.cols = cols;

            // Calculate camera size and position
            float screenSizeRatio = (float)screenSize.Width / (float)screenSize.Height;
            cameraSize.Y = 7;
            cameraSize.X = screenSizeRatio * cameraSize.Y;

            cameraPosition = new Vector2(cameraSize.X / 2, cameraSize.Y / 2);
            cameraRatio = cameraSize.X / cameraSize.Y;

            // Initialize cell array
            cellArr = new Cell[cols, rows];

            // Calculate cell size
            cellSize = calculateCellSize();

            this.rowsOffset = 0;
            this.colsOffset = 0;
        }

        // Method to update screen size
        public void updateScreenSize(int width, int height)
        {
            // Calculate changes in width and height
            float changeWidth = (width - screenSize.Width) / cellSize;
            float changeHeight = (height - screenSize.Height) / cellSize;

            // Update camera size and position
            cameraSize.Y += changeHeight;
            cameraSize.X += changeWidth;

            cameraPosition.Y += changeHeight / 2f;
            cameraPosition.X += changeWidth / 2f;
            cameraRatio = cameraSize.X / cameraSize.Y;

            // Update screen size
            screenSize.Width = width;
            screenSize.Height = height;
        }

        // Method to calculate cell size
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

        // Method to create the grid
        public void createGrid()
        {
            // Loop through each cell in the grid
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    // Calculate screen position of cell
                    Vector2 screenPos = new Vector2(colsOffset + (i * cellSize), rowsOffset + (j * cellSize));
                    // Initialize cell as Grass
                    cellArr[i, j] = new Grass(i, j, screenPos, cellSize);
                }
                // Set cells in the first and last rows as border cells
                cellArr[i, 0] = cellArr[i, 0].toBorder();
                cellArr[i, rows - 1] = cellArr[i, rows - 1].toBorder();
            }

            // Set cells in the first and last columns as border cells
            for (int i = 0; i < rows; i++)
            {
                cellArr[0, i] = cellArr[0, i].toBorder();
                cellArr[cols - 1, i] = cellArr[cols - 1, i].toBorder();
            }
        }

        // Method to draw the grid
        public void drawGrid(PaintEventArgs e)
        {
            // Calculate top left and bottom right cells visible on the screen
            Vector2 topLeftCell = new Vector2(MathF.Floor(cameraPosition.X - cameraSize.X / 2), MathF.Floor(cameraPosition.Y - cameraSize.Y / 2));
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));
            Vector2 bottomRightCell = new Vector2(MathF.Ceiling(cameraPosition.X + cameraSize.X / 2), MathF.Ceiling(cameraPosition.Y + cameraSize.Y / 2));

            // Loop through visible cells and draw them
            for (int i = (int)topLeftCell.X; i < (int)bottomRightCell.X; i++)
            {
                for (int j = (int)topLeftCell.Y; j < (int)bottomRightCell.Y; j++)
                {
                    if (i >= 0 && j >= 0 && i < cols && j < rows)
                    {
                        cellArr[i, j].drawCell(e, topLeft, cellSize, i, j);
                    }
                }
            }
        }

        // Method to get grid coordinates from a screen point
        public Vector2 getGridCoordFromPoint(Point clickPos)
        {
            Vector2 clickPosVec = new Vector2(clickPos.X, clickPos.Y);
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));

            clickPosVec.X = (clickPosVec.X + topLeft.X * cellSize) / cellSize;
            clickPosVec.Y = (clickPosVec.Y + topLeft.Y * cellSize) / cellSize;

            return clickPosVec;
        }

        // Method to get cell coordinates from a screen point
        public Vector2 getCellCoordFromPoint(Point clickPos)
        {
            Vector2 clickPosVec = new Vector2(clickPos.X, clickPos.Y);
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));

            clickPosVec.X = (int)MathF.Floor(((clickPosVec.X + topLeft.X * cellSize) / cellSize));
            clickPosVec.Y = (int)MathF.Floor(((clickPosVec.Y + topLeft.Y * cellSize) / cellSize));

            return clickPosVec;
        }

        // Method to get a screen point from a grid point
        public PointF getPointFromGridPoint(PointF gridPoint)
        {
            PointF point = new PointF();
            Vector2 topLeft = new Vector2((cameraPosition.X - cameraSize.X / 2), (cameraPosition.Y - cameraSize.Y / 2));

            point.X = (cellSize * (gridPoint.X - topLeft.X));
            point.Y = (cellSize * (gridPoint.Y - topLeft.Y));

            return point;
        }

        // Method to get the cell with the lowest F cost from a list of cells
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

        // Method to convert screen coordinates to grid coordinates
        public Vector2 getGridCoordFromScreenCoord(Vector2 gridCoord)
        {
            Vector2 cellVec = new Vector2(gridCoord.X, gridCoord.Y);

            cellVec.X = (int)MathF.Floor(((cellVec.X - colsOffset) / cellSize));
            cellVec.Y = (int)MathF.Floor(((cellVec.Y - rowsOffset) / cellSize));

            return cellVec;
        }

        // Method to convert grid coordinates to screen coordinates
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

        // Method to reset pathfinding
        public void resetPathFind()
        {
            foreach (Cell c in openList)
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

        // Method to calculate heuristic distance between two cells
        public double calculateHeuristic(Cell currentCell, Cell targetCell)
        {
            int x = Math.Abs(targetCell.col - currentCell.col);
            int y = Math.Abs(targetCell.row - currentCell.row);
            double z = (x + y) * (1.02);

            return z;
        }

        // Method to find a path from current cell to target cell
        public Stack<Cell> PathFind(Grid grid, Cell currentCell, Cell targetCell)
        {
            // If the current cell's G cost is set to maximum, reset it to 0
            if (currentCell.pathCostG == 999999)
            {
                currentCell.pathCostG = 0;
            }

            // Initialize the stack to store the path
            pathStack = new Stack<Cell>();

            // Remove the current cell from the open list and add it to the closed list
            openList.Remove(currentCell);
            closedList.Add(currentCell);

            // Iterate through each neighbor of the current cell
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    // Get the neighbor cell
                    Cell neighbour = currentCell.getNeighbour(grid, i);

                    // Check if the neighbor cell is not in the closed list and is passable
                    if (!(closedList.Contains(neighbour) || neighbour.permeable == 0))
                    {
                        // If the neighbor cell is not in the open list, add it
                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);

                            // Set the parent cell and update the G cost
                            neighbour.pathParent = currentCell;
                            neighbour.pathCostG = currentCell.pathCostG + (1 * (1 / currentCell.permeable));
                        }

                        // If the neighbor's G cost is greater than the current cell's G cost plus one, update it
                        if (neighbour.pathCostG > (currentCell.pathCostG + 1))
                        {
                            neighbour.pathCostG = currentCell.pathCostG + (1 * (1 / currentCell.permeable));
                            neighbour.pathParent = currentCell;
                        }

                        // Calculate and update the H and F costs for the neighbor cell
                        neighbour.pathCostH = calculateHeuristic(neighbour, targetCell);
                        neighbour.pathCostF = neighbour.pathCostH + neighbour.pathCostG;

                        // Update the cell in the grid array
                        cellArr[neighbour.col, neighbour.row] = neighbour;
                    }
                }
                catch { }
            }

            try
            {
                // If the target cell is not in the closed list and the open list is not empty, recursively call PathFind
                if (!(closedList.Contains(targetCell) || openList.Count == 0))
                {
                    PathFind(grid, getLowestFCost(openList), targetCell);
                }
            }
            catch { }

            // If the open list is empty, return the path stack
            if (openList.Count == 0)
            {
                return pathStack;
            }

            // If the path stack is still empty, retrieve the parents of cells in the path
            if (pathStack.Count == 0)
            {
                getParents(grid, targetCell);
            }
            return pathStack;
        }



        // Method to retrieve parents of cells in the path
        public Stack<Cell> getParents(Grid grid, Cell targetCell)
        {
            Cell cell = targetCell;

            while (cell.pathParent != null)
            {
                pathStack.Push(cellArr[cell.col, cell.row]);
                cell = cell.pathParent;
            }
            pathStack.Push(cellArr[cell.col, cell.row]);
            return pathStack;
        }

        // Method to color the path cells
        public void ColourPath()
        {
            foreach (Cell c in pathStack)
            {
                c.color = Color.Red;
            }
        }

        // Method to place a tile at a target cell
        public void placeTyle(Cell targetCell, Type type)
        {
            switch (type.Name)
            {
                case "Wall":
                    cellArr[targetCell.col, targetCell.row] = targetCell.toWall();
                    break;
                case "Dirt":
                    cellArr[targetCell.col, targetCell.row] = targetCell.toDirt();
                    break;
                case "Glass":
                    cellArr[targetCell.col, targetCell.row] = targetCell.toGlass();
                    break;
                case "Grass":
                    cellArr[targetCell.col, targetCell.row] = targetCell.toGrass();
                    break;
                case "Spawn":
                    cellArr[targetCell.col, targetCell.row] = targetCell.toSpawn();
                    break;
                case "enemyPath":
                    break;
            }
        }
    }
}
