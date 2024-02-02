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
using System.Security.Cryptography.X509Certificates;

namespace MovingThingTest
{
    public class Cell
    {
        public int row;
        public int col;
        public float permeable;
        public Cell? pathParent;
        public double pathCostG = 999999;
        public double pathCostH = 999999;
        public double pathCostF = 999999;
        public Color color;



        public Vector2[] neighbours = {
            new Vector2(-1,0),
            new Vector2(0,1),
            new Vector2(0,-1),
            new Vector2(1,0)
        };
        public Cell()
        {

        }

        public Cell(int col, int row)
        {
            this.row = row;
            this.col = col;
            permeable = 1;
            color = Color.Green;
        }



        public void resetPathFind()
        {
            pathParent = null;
            pathCostG = 999999;
            pathCostH = 999999;
            pathCostF = 999999;

        }

        public virtual void drawCell(PaintEventArgs e, Grid grid)
        {
            Pen pen = new Pen(Color.Black);
            SolidBrush cellBrush = new SolidBrush(color);

            Vector2 screenPos = grid.calculateScreenPos((int)(col-grid.cameraCoord.X), (int)(row-grid.cameraCoord.Y), grid.colsOffset, grid.rowsOffset, grid.cellSize);

            Rectangle rect = new Rectangle((int)screenPos.X, (int)screenPos.Y, (int)grid.cellSize, (int)grid.cellSize);
            e.Graphics.FillRectangle(cellBrush, rect);
            e.Graphics.DrawRectangle(pen, rect);
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
            Wall cell = new Wall(col, row);
            return cell;
        }

        public Cell toGrass()
        {
            Grass cell = new Grass(col, row);
            return cell;
        }

        public Cell toDirt()
        {
            Dirt cell = new Dirt(col, row);
            return cell;
        }

        public Cell toBorder()
        {
            Border cell = new Border(col, row);
            return cell;
        }

        public Cell toGlass()
        {
            Glass cell = new Glass(col, row);
            return cell;
        }
    }

    public class Wall : Cell
    {
        public Wall(int col, int row) : base(col, row)
        {
            permeable = 0;
            color = Color.Gray;
        }
    }

    public class Grass : Cell
    {
        public Grass(int col, int row) : base(col, row)
        {
            permeable = 1;
            color = Color.Green;
        }
    }

    public class Dirt : Cell
    {
        public Dirt(int col, int row) : base(col, row)
        {
            permeable = 0.25f;
            color = Color.Brown;
        }
    }

    public class Border : Cell
    {
        public Border(int col, int row) : base(col, row)
        {
            permeable = 0;
            color = Color.Gray;
        }
    }
    public class Glass : Cell
    {
        public Glass(int col, int row) : base(col, row)
        {
            permeable = 0;
            color = Color.Green;
        }

        public override void drawCell(PaintEventArgs e, Grid grid)
        {
            base.drawCell(e, grid);

            Pen pen = new Pen(Color.White)
            {
                Width = 4
            };

            Vector2 screenPos = grid.calculateScreenPos((int)(col - grid.cameraCoord.X), (int)(row - grid.cameraCoord.Y), grid.colsOffset, grid.rowsOffset, grid.cellSize);

            Rectangle rect = new Rectangle((int)screenPos.X+3, (int)screenPos.Y+3, (int)grid.cellSize -4, (int)grid.cellSize -4);
            e.Graphics.DrawRectangle(pen, rect);
        }
    }
}
