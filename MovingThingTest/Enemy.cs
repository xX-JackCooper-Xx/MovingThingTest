using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MovingThingTest
{
    public class Enemy : Soldier
    {
        enemyPath path = new enemyPath();

        int pathSection = 0;
        Stack<Cell> pathStack = new Stack<Cell>(); 
        public bool shooting = false;
        public int health = 100;
        public Enemy(enemyPath enemyPath) : base(enemyPath.pathAnchors[0])
        {
            Stack<Cell> tempStack = new Stack<Cell>();
            color = Color.Black;
            this.path = enemyPath;
            foreach (Cell cell in enemyPath.pathCellsLists[0])
            {
                tempStack.Push(cell);
            }
            foreach(Cell cell in tempStack)
            {
                pathStack.Push(cell);
            }
        }

        public override void drawSoldier(PaintEventArgs e, Grid grid, float size, int a, int b)
        {
            SolidBrush brush = new SolidBrush(color);
            Pen p = new Pen(Color.Black, 4);
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            e.Graphics.FillEllipse(brush, (gridCoord.X - topLeft.X) * size, (gridCoord.Y - topLeft.Y) * size, size, size);

            brush.Color = Color.Red;
            if (shooting)
            {
                RectangleF rect = new RectangleF((gridCoord.X - topLeft.X - 0.5f) * size, (gridCoord.Y - topLeft.Y - 0.5f) * size, 2 * size, size / 4);
                e.Graphics.FillRectangle(brush, rect);
                brush.Color = Color.Lime;
                RectangleF rect2 = rect;
                rect2.Width = rect.Width * health / 100f;
                e.Graphics.FillRectangle(brush, rect2);
                e.Graphics.DrawRectangles(p, new[] { rect });
            }
        }


        public void updatePos()
        {
            if (shooting)
            {
                health--;
                shooting = false;
                return;
            }
            if (pathStack.Count > 0)
            {
                if (currentCell.gridCoord != gridCoord)
                {
                    gridCoord += movingVec;
                    gridCoord = new Vector2(MathF.Round(gridCoord.X, 2), MathF.Round(gridCoord.Y, 2));
                }
                else if (pathStack.Count > 0)
                {
                    currentCell = pathStack.Pop();
                    if (currentCell.gridCoord == gridCoord)
                    {
                        currentCell = pathStack.Pop();
                    }
                    movingVec = Vector2.Normalize(currentCell.gridCoord - gridCoord) * 0.05f;
                    gridCoord += movingVec;
                }
                centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
                return;
            }

            pathSection++;
            if(pathSection == path.pathAnchors.Count - 1)
            {
                pathSection = 0;
                if (!path.loop)
                {
                    path.pathCellsLists.Reverse();
                    for (int i = 0; i < path.pathCellsLists.Count; i++)
                    {
                        path.pathCellsLists[i].Reverse();
                    }
                }
            }
            

            Stack<Cell> tempStack = new Stack<Cell>();
            foreach (Cell cell in path.pathCellsLists[pathSection])
            {
                tempStack.Push(cell);
            }
            foreach(Cell cell in tempStack)
            {
                pathStack.Push(cell);
            }
        }
    }
}
