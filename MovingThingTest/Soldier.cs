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
        protected Vector2 movingVec = new Vector2(0, 0);
        protected Vector2 centerCoord;
        public int direction = 0;
        protected Color color = Color.Red;
        Ray ray = new Ray();
        Ray viewRay = new Ray();
        bool shooting = false;
        double rayAngle = 0;
        Vector2 shootingPoint;
        public Soldier(Cell currentCell)
        {
            this.currentCell = currentCell;
            gridCoord = new Vector2(currentCell.col, currentCell.row);
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
        }

        public void updatePos(Stack<Cell> cellStack, float speed, int i)
        {
            if (currentCell.gridCoord != gridCoord)
            {
                gridCoord += movingVec;
                gridCoord = new Vector2(MathF.Round(gridCoord.X, i), MathF.Round(gridCoord.Y, i));
            }
            else if (cellStack.Count > 0)
            {
                currentCell = cellStack.Pop();
                if (currentCell.gridCoord == gridCoord)
                {
                    currentCell = cellStack.Pop();
                }
                movingVec = Vector2.Normalize(currentCell.gridCoord - gridCoord) * speed;
                gridCoord += movingVec;
            }
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
        }

        public virtual void drawSoldier(PaintEventArgs e, Grid grid, float size)
        { 
            SolidBrush brush = new SolidBrush(color);
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            e.Graphics.FillEllipse(brush, (gridCoord.X - topLeft.X) * size, (gridCoord.Y - topLeft.Y) * size, size, size);
            if (shooting)
            {
                viewRay = Ray.castRay(grid, centerCoord, rayAngle);
                viewRay.endPos = shootingPoint;
            }
            else
            {
                viewRay = Ray.castRay(grid, centerCoord, direction * Math.PI / 2 - Math.PI / 2);
            }
            viewRay.drawRay(e, topLeft, size);
        }

        public void checkForEnemy(List<Enemy> enemies, Grid grid)
        {
            shooting = false;
            foreach(Enemy enemy in enemies)
            {
                float distanceX = Math.Abs(enemy.centerCoord.X - centerCoord.X);
                float distanceY = Math.Abs(enemy.centerCoord.Y - centerCoord.Y);
                float distance = MathF.Sqrt(distanceY * distanceY + distanceX * distanceX);

                Vector2 normal = Vector2.Normalize(enemy.centerCoord - centerCoord);
                Vector2 right = new Vector2(1, 0);
                double dot = Vector2.Dot(normal, right);
                double det = normal.X * right.Y - normal.Y * right.X;
                double angle = -Math.Atan2(det, dot);

                if (distance <= 5 && distance > 0)
                {
                    ray = Ray.castRay(grid, centerCoord, angle);
                    Vector2 rayVector = ray.endPos - ray.startPos;
                    float rayVectorLength = MathF.Sqrt(rayVector.X * rayVector.X + rayVector.Y * rayVector.Y);
                    if (rayVectorLength >= distance && (ray.angle > direction* Math.PI / 2 - 3*Math.PI / 4 & ray.angle < direction * Math.PI / 2 - Math.PI / 4))
                    {
                        rayAngle = angle;
                        shootingPoint = enemy.centerCoord;
                        shooting = true;
                        enemy.shooting = true;
                    }
                }
            }
        }
    }
}
