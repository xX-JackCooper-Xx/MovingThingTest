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
using System.Runtime.CompilerServices;

namespace MovingThingTest
{
    // Base class representing a single cell in the grid
    public class Cell
    {
        // Properties
        public int row;              // Row index of the cell
        public int col;              // Column index of the cell
        public Vector2 screenPos;    // Screen position of the cell
        public float permeable;      // Permeability of the cell (0: impassable, 1: passable)
        public Cell? pathParent;     // Parent cell in the pathfinding algorithm
        public double pathCostG = 999999;  // Cost of moving from the start cell to this cell
        public double pathCostH = 999999;  // Heuristic cost from this cell to the goal cell
        public double pathCostF = 999999;  // Total cost (G + H) of the cell in the pathfinding algorithm
        public Color color;          // Color of the cell
        public Vector2 gridCoord;    // Grid coordinates of the cell
        public float cellSize;       // Size of the cell
        public int ID;               // Unique identifier of the cell
        public bool clear;           // Flag indicating if the cell is clear
        public Pen pen = new Pen(Color.Black);                 // Pen for drawing cell borders
        public SolidBrush cellBrush = new SolidBrush(Color.Black);  // Brush for filling cell color

        // Array of relative coordinates of neighboring cells
        public Vector2[] neighbours = {
            new Vector2(0,1),
            new Vector2(-1,0),
            new Vector2(0,-1),
            new Vector2(1,0)
        };

        // Default constructor
        public Cell()
        {

        }

        // Constructor with parameters
        public Cell(int col, int row, Vector2 screenPos, float cellSize)
        {
            this.row = row;
            this.col = col;
            this.screenPos = screenPos;
            this.cellSize = cellSize;
            permeable = 1;
            color = Color.Green;
            gridCoord = new Vector2(col, row);
            ID = 999;
            clear = true;
            cellBrush.Color = color;
        }

        // Method to reset pathfinding attributes of the cell
        public void resetPathFind()
        {
            pathParent = null;
            pathCostG = 999999;
            pathCostH = 999999;
            pathCostF = 999999;
        }

        // Method to draw the cell on the screen
        public virtual void drawCell(PaintEventArgs e, Vector2 topleft, float cellSize, int i, int j)
        {
            this.cellSize = cellSize;

            RectangleF rect = new RectangleF((i - topleft.X) * cellSize, (j - topleft.Y) * cellSize, cellSize, cellSize);
            e.Graphics.FillRectangle(cellBrush, rect);
            e.Graphics.DrawRectangles(pen, new[] { rect });
        }

        // Method to get a neighboring cell
        public Cell getNeighbour(Grid grid, int i)
        {
            Vector2 cellCoord = new Vector2(col, row);
            Vector2 neighbour = cellCoord + neighbours[i];

            return grid.cellArr[(int)neighbour.X, (int)neighbour.Y];
        }

        // Methods to convert the cell to different types
        public Cell toWall()
        {
            Wall cell = new Wall(col, row, screenPos, cellSize);
            return cell;
        }

        public Cell toGrass()
        {
            Grass cell = new Grass(col, row, screenPos, cellSize);
            return cell;
        }

        public Cell toDirt()
        {
            Dirt cell = new Dirt(col, row, screenPos, cellSize);
            return cell;
        }

        public Cell toBorder()
        {
            Border cell = new Border(col, row, screenPos, cellSize);
            return cell;
        }

        public Cell toGlass()
        {
            Glass cell = new Glass(col, row, screenPos, cellSize);
            return cell;
        }

        public Cell toSpawn()
        {
            Spawn cell = new Spawn(col, row, screenPos, cellSize);
            return cell;
        }
    }

    // Derived class representing a Wall cell
    public class Wall : Cell
    {
        // Constructor
        public Wall(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            clear = false;
            permeable = 0;
            color = Color.Gray;
            cellBrush.Color = color;
            ID = 0;
        }
    }

    // Derived class representing a Grass cell
    public class Grass : Cell
    {
        // Constructor
        public Grass(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 1;
            color = Color.Green;
            cellBrush.Color = color;
            ID = 1;
        }
    }

    // Derived class representing a Dirt cell
    public class Dirt : Cell
    {
        // Constructor
        public Dirt(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 0.25f;
            color = Color.Brown;
            cellBrush.Color = color;
            ID = 2;
        }
    }

    // Derived class representing a Border cell
    public class Border : Cell
    {
        // Constructor
        public Border(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            clear = false;
            permeable = 0;
            color = Color.Gray;
            cellBrush.Color = color;
            ID = 3;
        }
    }

    // Derived class representing a Glass cell
    public class Glass : Cell
    {
        // Constructor
        public Glass(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 0;
            color = Color.Green;
            cellBrush.Color = color;
            ID = 4;
        }

        // Method to draw the Glass cell
        public override void drawCell(PaintEventArgs e, Vector2 topLeft, float cellSize, int i, int j)
        {
            base.drawCell(e, topLeft, cellSize, i, j);

            // Draw additional features for Glass cell
            Pen pen = new Pen(Color.White)
            {
                Width = 4
            };

            RectangleF rect = new RectangleF(screenPos.X + 3, screenPos.Y + 3, cellSize - 4, cellSize - 4);
            e.Graphics.DrawRectangles(pen, new[] { rect });
        }
    }

    // Derived class representing a Spawn cell
    public class Spawn : Cell
    {
        // Constructor
        public Spawn(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 1;
            color = Color.Green;
            cellBrush.Color = color;
            ID = 101;
        }

        // Method to draw the Spawn cell
        public override void drawCell(PaintEventArgs e, Vector2 topleft, float cellSize, int i, int j)
        {
            base.drawCell(e, topleft, cellSize, i, j);

            // Draw additional features for Spawn cell
            cellBrush.Color = Color.OrangeRed;
            RectangleF rect = new RectangleF((i - topleft.X) * cellSize, (j - topleft.Y) * cellSize, cellSize, cellSize);
            PointF[] tri = new PointF[] { new PointF((i - topleft.X) * cellSize, (j - topleft.Y) * cellSize), new PointF((i - topleft.X) * cellSize, (j + 1 - topleft.Y) * cellSize), new PointF((i - topleft.X + 1) * cellSize, (j + 0.5f - topleft.Y) * cellSize) };
            e.Graphics.FillPolygon(cellBrush, tri);
            cellBrush.Color = color;
        }
    }
}