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
        public Vector2 centerCoord;
        public int direction = 0;
        protected Color color = Color.Red;
        Ray ray = new Ray();
        Ray viewRay = new Ray();
        bool shooting = false;
        double rayAngle = 0;
        Vector2 shootingPoint;
        List<Ray> rays = new List<Ray>();
        public Soldier(Cell currentCell)
        {
            this.currentCell = currentCell;
            gridCoord = new Vector2(currentCell.col, currentCell.row);
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
        }

        public void updatePos(Stack<Cell> cellStack, float speed, int i, Grid grid)
        {
            //if (currentCell.gridCoord != gridCoord)
            //{
            //    gridCoord += movingVec;
            //    gridCoord = new Vector2(MathF.Round(gridCoord.X, i), MathF.Round(gridCoord.Y, i));
            //}
            //else if (cellStack.Count > 0)
            //{
            //    currentCell = cellStack.Pop();
            //    if (currentCell.gridCoord == gridCoord)
            //    {
            //        currentCell = cellStack.Pop();
            //    }
            //    movingVec = Vector2.Normalize(currentCell.gridCoord - gridCoord) * speed;
            //    gridCoord += movingVec;
            //}
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
        }

        public virtual void drawSoldier(PaintEventArgs e, Grid grid, float size, int screenHeight, int screenWidth)
        {
            List<Point> points = new List<Point>();
            List<Point> seemPoints = new List<Point>();
            SolidBrush brush = new SolidBrush(Color.Gray);
            Pen p = new Pen(Color.Black, 5);
            double fov = 60d / 360d * Math.PI * 2;
            //double fov = Math.PI / 2;
            double d = (fov) / (screenWidth);
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            rays = new List<Ray>();
            e.Graphics.FillEllipse(brush, (gridCoord.X - topLeft.X) * size, (gridCoord.Y - topLeft.Y) * size, size, size);
            for (int i = 0; i < screenWidth; i = i + 1)
            {
                rays.Add(Ray.castRay(grid, centerCoord, direction/360f * 2*Math.PI - Math.PI/2 + fov/2 - d * i));
            }
            rays[0].drawRay(e, topLeft, size);
            rays[rays.Count - 1].drawRay(e, topLeft, size);
            int j = 0;
            foreach (Ray ray in rays)
            {
                //double angle = Math.Atan(1 / ray.magnitude);
                //if (angle > Math.PI / 6 )
                //{
                //    angle = Math.PI / 6;
                //}
                //double proportion = angle / (Math.PI / 6);
                //double rectSize = proportion * screenHeight;
                //points.Add(new Point(screenWidth - j, (int)(rectSize / 2 + screenHeight / 2)));
                //double angleDifference = direction / 180f * Math.PI - ray.angle - Math.PI/2;
                //if(angleDifference < 0) {
                //    angleDifference += 2 * Math.PI;
                //}
                //if(angleDifference > 2 * Math.PI)
                //{
                //    angleDifference -= 2 * Math.PI;
                //}
                double angleDifference = (direction/180d * Math.PI - ray.angle + Math.PI/2d);
                //double angleDifference = 0;
                double rectSize = 2 * screenHeight / (ray.magnitude*Math.Cos(angleDifference));
                if(rectSize > screenHeight)
                {
                    rectSize = screenHeight;
                }
                points.Add(new Point(screenWidth - j, (int)(rectSize / 2 + screenHeight / 2)));
                //Rectangle rect = new Rectangle(screenWidth - j, (int)(-rectSize / 2 + screenHeight / 2), 1, (int)rectSize);
                //e.Graphics.FillRectangle(brush, rect);
                if (j + 1 < rays.Count)
                {
                    if ((int)ray.endPos.X != (int)rays[j + 1].endPos.X || (int)ray.endPos.Y != (int)rays[j + 1].endPos.Y)
                    {
                        seemPoints.Add(new Point(screenWidth - j, (int)(rectSize / 2 + screenHeight / 2)));
                    }
                }
                j = j + 1;
            }
            for (int i = points.Count - 1; i >= 0; i--)
            {
                points.Add(new Point(points[i].X, screenHeight - points[i].Y));
            }
            e.Graphics.FillPolygon(brush, points.ToArray());
            e.Graphics.DrawPolygon(p, points.ToArray());
            List<Point> tempPoints = new List<Point>();
            foreach(Point point in seemPoints) {
                Point lowPoint = new Point(point.X, screenHeight - point.Y);
                e.Graphics.DrawLine(p, point, lowPoint);
            }
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
