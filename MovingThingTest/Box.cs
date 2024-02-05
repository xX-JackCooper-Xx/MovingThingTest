using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFiDirect;

namespace MovingThingTest
{
    public class Box
    {
        public Vector2 screenPos = new Vector2();
        public Cell currentCell;
        public Cell targetCell;
        public Vector2 gridCoord;

        public float boxSize;
        Vector2 movingVec = new Vector2(0, 0);


        public Box(Grid grid, Vector2 screenPos, float boxSize)
        {
            this.screenPos = screenPos;

            gridCoord = grid.getGridCoordFromScreenCoord(screenPos);

            this.boxSize = boxSize;

            currentCell = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y];
            targetCell = grid.cellArr[(int)gridCoord.X, (int)gridCoord.Y];
        }

        public void UpdatePos(Grid grid, Stack<Cell> cellStack)
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

            //if(cellStack.Count > 0 && moveCooldown <= 0)
            //{
            //    currentCell = cellStack.Pop();
            //    moveCooldown = 1;
            //}
            //else
            //{
            //    moveCooldown--;
            //}
        }

        public void drawBox(PaintEventArgs e, Grid grid)
        {
            SolidBrush brush = new SolidBrush(Color.Yellow);
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            e.Graphics.FillRectangle(brush, (gridCoord.X - topLeft.X)*boxSize, (gridCoord.Y - topLeft.Y)*boxSize, boxSize, boxSize);
        }

        public List<Point> getCloseWallPoints(Wall wall)
        {
            PointF gridPoint = new PointF(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
            Point wallScreenPos = new Point(wall.col, wall.row);

            List<Point> closestPoints = new List<Point>();

            float relativeX = wall.col - gridPoint.X;
            float relativeY = wall.row - gridPoint.Y;
            if (relativeX > 0 && relativeY >= -1)
            {
                closestPoints.Add(new Point(wallScreenPos.X, wallScreenPos.Y + 1));
            }
            if (relativeX > 0 && relativeY <= 0)
            {
                closestPoints.Add(wallScreenPos);
            }
            if (relativeX >= -1 && relativeY > 0)
            {
                closestPoints.Add(new Point(wallScreenPos.X + 1, wallScreenPos.Y));
            }
            if (relativeX >= -1 && relativeY < -1 )
            {
                closestPoints.Add(new Point(wallScreenPos.X + 1, wallScreenPos.Y + 1));
            }
            if (relativeX <= 0 && relativeY > 0)
            {
                closestPoints.Add(wallScreenPos);
            }
            if (relativeX <= 0 && relativeY < -1 )
            {
                closestPoints.Add(new Point(wallScreenPos.X, wallScreenPos.Y + 1));
            }
            if (relativeX < -1 && relativeY >= -1)
            {
                closestPoints.Add(new Point(wallScreenPos.X + 1, wallScreenPos.Y + 1));
            }
            if (relativeX < -1 && relativeY <= 0)
            {
                closestPoints.Add(new Point(wallScreenPos.X + 1, wallScreenPos.Y));
            }

            return closestPoints;
        }

        public List<PointF> createMask(List<Point> points, Grid grid)
        {
            List<PointF> farPoints = new List<PointF>();
            PointF gridPoint = new PointF(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
            Vector2 gridVec = new Vector2(gridPoint.X, gridPoint.Y);

            //List<Vector2> cornerVecs = new List<Vector2>()
            //{
            //    new Vector2(grid.colsOffset + grid.cellSize, grid.cellSize + grid.rowsOffset),
            //    new Vector2(grid.cellSize * (grid.cols-1) + grid.colsOffset, grid.cellSize + grid.rowsOffset),
            //    new Vector2(grid.colsOffset + grid.cellSize, grid.cellSize*(grid.rows-1)+ grid.rowsOffset),
            //    new Vector2(grid.cellSize * (grid.cols-1) + grid.colsOffset,grid.cellSize *(grid.rows - 1) + grid.rowsOffset)
            //};

            List<Vector2> cornerVecs = new List<Vector2>()
            {
                new Vector2(grid.cameraPosition.X + grid.cameraSize.X/2, grid.cameraPosition.Y + grid.cameraSize.Y/2),
                new Vector2(grid.cameraPosition.X - grid.cameraSize.X/2, grid.cameraPosition.Y + grid.cameraSize.Y/2),
                new Vector2(grid.cameraPosition.X + grid.cameraSize.X/2, grid.cameraPosition.Y - grid.cameraSize.Y/2),
                new Vector2(grid.cameraPosition.X - grid.cameraSize.X/2, grid.cameraPosition.Y - grid.cameraSize.Y/2)
            };

            List<Vector2> vecsToCorners = new List<Vector2>();
            foreach (Vector2 vec in cornerVecs)
            {
                vecsToCorners.Add(Vector2.Subtract(vec, gridVec));
            }

            float a;
            float b;

            foreach (Point point in points)
            {
                Vector2 direction = Vector2.Normalize(new Vector2(point.X - gridVec.X, point.Y - gridVec.Y));

                if (direction.Y < 0)
                {
                    a = (grid.cameraPosition.Y - grid.cameraSize.Y / 2 - point.Y) / direction.Y;
                }
                else
                {
                    a = (grid.cameraPosition.Y + grid.cameraSize.Y / 2 - point.Y) / direction.Y;
                }
                if (direction.X < 0)
                {
                    b = (grid.cameraPosition.X - grid.cameraSize.X / 2 - point.X) / direction.X;
                }
                else
                {
                    b = (grid.cameraPosition.X + grid.cameraSize.X / 2 - point.X) / direction.X;
                }


                float max = Math.Min(a, b);

                PointF farPoint = new PointF(point.X + direction.X * max, point.Y + direction.Y * max);
                farPoints.Add(farPoint);
            }

            List<Vector2> vecsToPoints = new List<Vector2>();

            foreach (PointF point in farPoints)
            {
                vecsToPoints.Add(Vector2.Subtract(new Vector2(point.X, point.Y), gridVec));
            }

            for (int i = 0; i < vecsToCorners.Count; i++)
            {
                bool u = (vecsToPoints[0].Y * vecsToCorners[i].X - vecsToPoints[0].X * vecsToCorners[i].Y) * (vecsToPoints[0].Y * vecsToPoints[1].X - vecsToPoints[0].X * vecsToPoints[1].Y) >= 0;
                bool v = (vecsToPoints[1].Y * vecsToCorners[i].X - vecsToPoints[1].X * vecsToCorners[i].Y) * (vecsToPoints[1].Y * vecsToPoints[0].X - vecsToPoints[1].X * vecsToPoints[0].Y) >= 0;
                if (u && v)
                {
                    farPoints.Add(new PointF(cornerVecs[i].X, cornerVecs[i].Y));
                }
            }
            return farPoints;
        }

        public void drawVisionCone(Grid grid, PaintEventArgs e)
        {
            Color color;
            color = Color.FromArgb(100, 0, 0, 0);
            SolidBrush brush;
            brush = new SolidBrush(color);

            Region region = new Region();
            Region regionInverse = new Region();

            List<Cell> walls = new List<Cell>();
            Vector2 topLeftCell = new Vector2(MathF.Floor(grid.cameraPosition.X - grid.cameraSize.X / 2), MathF.Floor(grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            Vector2 bottomRightCell = new Vector2(MathF.Ceiling(grid.cameraPosition.X + grid.cameraSize.X / 2), MathF.Ceiling(grid.cameraPosition.Y + grid.cameraSize.Y / 2));

            for (int i = (int)topLeftCell.X; i < (int)bottomRightCell.X; i++)
            {
                for (int j = (int)topLeftCell.Y; j < (int)bottomRightCell.Y; j++)
                {
                    if (i >= 0 && j >= 0 && i < grid.cols && j < grid.rows)
                    {
                        if (grid.cellArr[i, j] is Wall)
                        {
                            walls.Add(grid.cellArr[i, j]);
                        }
                    }
                }
            }

            foreach (Wall wall in walls)
            {
                List<Point> points = getCloseWallPoints(wall);

                List<PointF> farPoints = createMask(points, grid);
                List<PointF> allPoints = new List<PointF>() { points[0], farPoints[0] };
                for (int i = 2; i < farPoints.Count; i++)
                {
                    allPoints.Add(farPoints[i]);
                }
                allPoints.Add(farPoints[1]);
                allPoints.Add(points[1]);

                List<PointF> allScreenPoints = new List<PointF>();

                foreach (PointF point in allPoints) {
                    allScreenPoints.Add(grid.getPointFromGridPoint(point));
                }

                GraphicsPath mask = new GraphicsPath();
                mask.AddPolygon(allScreenPoints.ToArray());

                region.Exclude(mask);
            }
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            regionInverse.Exclude(region);
            e.Graphics.FillRegion(brush, regionInverse);
        }

        public void vision(Grid grid, PaintEventArgs e)
        {
            Vector2 centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
            double rad = 0;
            //while (!wallHit)
            //{
            //    int deltaY = Math.Abs((int)y - (int)lastY);

            //    if(deltaY > 0)
            //    {
            //        for(int i = deltaY; i>0; i--)
            //        {
            //            if (!grid.cellArr[(int)x-1, (int)(y - i)+1].clear)
            //            {
            //                wallHit = true;
            //            }
            //        }
            //        lastY = y;
            //    }
            //    else
            //    {
            //        x++;
            //        if (!grid.cellArr[(int)x, (int)y].clear)
            //        {
            //            wallHit = true;
            //        }
            //        y = (x * gradient) + c;
            //    }

            //}
            for (rad = 0; rad <= Math.PI / 2; rad = rad + 0.01)
            {
                double gradient = Math.Tan(rad);

                int left = Math.Sign(Math.Cos(rad));
                int up = Math.Sign(Math.Sin(rad));

                int left2 = Math.Abs((left - 1) / 2);
                int up2 = Math.Abs((up - 1) / 2);

                bool wallHit = false;
                double lastY = centerCoord.Y;

                double c = centerCoord.Y - centerCoord.X * gradient;

                double x = centerCoord.X + (1 - centerCoord.X % 1) * left;
                double y = (x * gradient) + c;
                while (!wallHit)
                {
                    int deltaY = Math.Abs((int)y - (int)lastY);

                    if (deltaY > 0)
                    {
                        for (int i = (int)lastY + 1; i < y; i = i + 1 * up)
                        {
                            if (!grid.cellArr[(int)(x - 1), (int)(i)].clear)
                            {
                                wallHit = true;
                                y = i;
                                x = (y - c) / gradient;
                            }
                        }
                    }
                    if (!grid.cellArr[(int)x, (int)y].clear)
                    {
                        wallHit = true;
                    }
                    if (!wallHit)
                    {
                        x = x + 1 * left;
                        lastY = y;
                        y = x * gradient + c;
                    }

                }
                //y = gradient * x + centerCoord.Y;

                //double deltay = Math.Abs(y - lastY);

                //for (double i = 0; i < deltay; i++)
                //{
                //    y = lastY + (1 - lastY % 1) * Math.Sign(Math.Sin(rad));

                //    if (!grid.cellArr[(int)x, (int)y].clear)
                //    {
                //        wallHit = true;
                //        lastY = y;
                //    }
                //}


                //if (!grid.cellArr[(int)x, (int)y].clear)
                //{
                //    wallHit = true;
                //    x = x - 1 * Math.Sign(Math.Cos(rad));
                //}

                //x = x + 1* Math.Sign(Math.Cos(rad));


                Pen p = new Pen(Color.Black);
                p.Width = 3;
                Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
                e.Graphics.DrawLine(p, new Point((int)((centerCoord.X - topLeft.X) * boxSize), (int)((centerCoord.Y - topLeft.Y) * boxSize)), new Point((int)((x - topLeft.X) * boxSize), (int)((y - topLeft.Y) * boxSize)));
            }
        }
    }
}
