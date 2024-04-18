using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MovingThingTest
{
    public class Soldier
    {
        public Cell currentCell;
        public Vector2 gridCoord;
        Vector2 movingVec = new Vector2(0, 0);
        Vector2 centerCoord;
        List<Cell> targetCells = new List<Cell>();
        public int direction = 0;

        public Soldier(Cell currentCell)
        {
            this.currentCell = currentCell;
            gridCoord = new Vector2(currentCell.col, currentCell.row);
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
        }

        public void updatePos(Stack<Cell> cellStack)
        {
            if (currentCell.gridCoord != gridCoord)
            {
                gridCoord += movingVec;
                gridCoord = new Vector2(MathF.Round(gridCoord.X, 1), MathF.Round(gridCoord.Y, 1));
            }
            else if (cellStack.Count > 0)
            {
                currentCell = cellStack.Pop();
                if (currentCell.gridCoord == gridCoord)
                {
                    currentCell = cellStack.Pop();
                }
                movingVec = Vector2.Normalize(currentCell.gridCoord - gridCoord) * 0.1f;
                gridCoord += movingVec;
            }
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
        }

        public void drawSoldier(PaintEventArgs e, Grid grid, float size)
        { 
            SolidBrush brush = new SolidBrush(Color.Red);
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            e.Graphics.FillEllipse(brush, (gridCoord.X - topLeft.X) * size, (gridCoord.Y - topLeft.Y) * size, size, size);
            Ray ray = Ray.castRay(grid, centerCoord, direction * Math.PI / 2 - Math.PI / 2);
            ray.drawRay(e, topLeft, size);
        }
    }
}
