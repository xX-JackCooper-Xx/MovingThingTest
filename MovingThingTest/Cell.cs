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
    public class Cell
    {
        public int row;
        public int col;
        public Vector2 screenPos;
        public float permeable;
        public Cell? pathParent;
        public double pathCostG = 999999;
        public double pathCostH = 999999;
        public double pathCostF = 999999;
        public Color color;
        public Vector2 gridCoord;
        public float cellSize;
        public int ID;
        public bool clear;
        public Pen pen = new Pen(Color.Black);
        public SolidBrush cellBrush = new SolidBrush(Color.Black);

        public Vector2[] neighbours = {
            new Vector2(-1,0),
            new Vector2(0,1),
            new Vector2(0,-1),
            new Vector2(1,0)
        };
        public Cell()
        {

        }

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



        public void resetPathFind()
        {
            pathParent = null;
            pathCostG = 999999;
            pathCostH = 999999;
            pathCostF = 999999;

        }

        public virtual void drawCell(PaintEventArgs e, Vector2 topleft, float cellSize, int i, int j)
        {
            this.cellSize = cellSize;
            
            //RectangleF rect = new RectangleF((screenPos.X - topleft.X*cellSize), (screenPos.Y-topleft.Y*cellSize), cellSize, cellSize);
            RectangleF rect = new RectangleF((i - topleft.X) * cellSize, (j - topleft.Y) * cellSize, cellSize, cellSize);
            e.Graphics.FillRectangle(cellBrush, rect);
            e.Graphics.DrawRectangles(pen, new[] { rect });
        }

        public Cell getNeighbour(Grid grid, int i)
        {
            Vector2 cellCoord = new Vector2(col, row);
            Vector2 neighbour = cellCoord + neighbours[i];

            //if(i == 3)
            //{
            //    Array.Reverse(neighbours);
            //}

            return grid.cellArr[(int)neighbour.X, (int)neighbour.Y];
        }

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

    public class Wall : Cell
    {
        public Wall(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            clear = false;
            permeable = 0;
            color = Color.Gray;
            cellBrush.Color = color;
            ID = 0;
        }
        
    }

    public class Grass : Cell
    {
        public Grass(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 1;
            color = Color.Green;
            cellBrush.Color = color;
            ID = 1;
        }
    }

    public class Dirt : Cell
    {
        public Dirt(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 0.25f;
            color = Color.Brown;
            cellBrush.Color = color;
            ID = 2;
        }
    }

    public class Border : Cell
    {
        public Border(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            clear = false;
            permeable = 0;
            color = Color.Gray;
            cellBrush.Color = color;
            ID = 3;
        }
    }
    public class Glass : Cell
    {
        public Glass(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 0;
            color = Color.Green;
            cellBrush.Color = color;
            ID = 4;
        }

        public override void drawCell(PaintEventArgs e, Vector2 topLeft, float cellSize, int i, int j)
        {
            base.drawCell(e ,topLeft, cellSize, i, j);

            Pen pen = new Pen(Color.White)
            {
                Width = 4
            };



            RectangleF rect = new RectangleF(screenPos.X+3, screenPos.Y+3, cellSize-4, cellSize-4);
            e.Graphics.DrawRectangles(pen, new[] { rect });
        }
    }
    public class Spawn : Cell {
        public Spawn(int col, int row, Vector2 screenPos, float cellSize) : base(col, row, screenPos, cellSize)
        {
            permeable = 1;
            color = Color.Green;
            cellBrush.Color = color;
            ID = 101;
        }

        public override void drawCell(PaintEventArgs e, Vector2 topleft, float cellSize, int i, int j)
        {
            base.drawCell(e, topleft, cellSize, i, j);

            cellBrush.Color = Color.OrangeRed;
            RectangleF rect = new RectangleF((i - topleft.X) * cellSize, (j - topleft.Y) * cellSize, cellSize, cellSize);
            PointF[] tri = new PointF[] { new PointF((i - topleft.X) * cellSize, (j - topleft.Y) * cellSize), new PointF((i - topleft.X) * cellSize, (j + 1 - topleft.Y) * cellSize), new PointF((i - topleft.X + 1) * cellSize, (j + 0.5f - topleft.Y) * cellSize) };
            e.Graphics.FillPolygon(cellBrush, tri);
            cellBrush.Color = color;
        }
    }

}
