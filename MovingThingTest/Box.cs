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
        public Vector2 centerCoord;

        public float boxSize;
        Vector2 movingVec = new Vector2(0, 0);


        public Box(Grid grid, Vector2 screenPos, float boxSize)
        {
            this.screenPos = screenPos;

            gridCoord = grid.getGridCoordFromScreenCoord(screenPos);
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);

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
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);

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
            double angle = 0;


            while (angle < 2 * Math.PI)
            {
                int signSin = Math.Sign(Math.Sin(angle));
                int signCos = Math.Sign(Math.Cos(angle));

                int getCellX = (-1 * signSin - 1) / 2;
                int getCellY = (signCos - 1) / 2;

                Cell cell = Ray.getCellFromRaycast(grid, centerCoord, angle);

                Vector2 nextPos = new Vector2(cell.col + getCellX, cell.row + getCellY);

                int clear = Convert.ToInt16(grid.cellArr[(int)nextPos.X, (int)nextPos.Y].clear);
                int notClear = Convert.ToInt16(!grid.cellArr[(int)nextPos.X, (int)nextPos.Y].clear);

                if (Math.Sign(Math.Tan(angle)) == 1)
                {
                    nextPos = new Vector2(cell.col - 1 * signSin * notClear, cell.row + 1 * signCos * clear);
                }
                else
                {
                    nextPos = new Vector2(cell.col - 1 * signSin * clear, cell.row + 1 * signCos * notClear);
                }

                Vector2 vecToNextPos = nextPos - centerCoord;

                Vector2 normalVec = Vector2.Normalize(vecToNextPos);
                angle = Math.Acos(Vector2.Dot(normalVec, new Vector2(1, 0)));

                Ray ray = Ray.castRay(grid, centerCoord, angle);

                Ray roundedRay = new Ray(ray.startPos, new Vector2((float)Math.Round(ray.endPos.X, 3), (float)Math.Round(ray.endPos.Y, 3)), angle);


                if (roundedRay.endPos != nextPos)
                {
                    Cell hitCell = Ray.getCellFromRaycast(grid, centerCoord, angle);
                    nextPos = new Vector2(hitCell.col, hitCell.row);
                }
            }


            //for (double rad = 0; rad <= 2*Math.PI; rad = rad + 0.05)
            //{
            //    Ray ray = Ray.castRay(grid, centerCoord, rad);

            //    Pen p = new Pen(Color.Black);
            //    p.Width = 3;
            //    Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            //    e.Graphics.DrawLine(p, new Point((int)((centerCoord.X - topLeft.X) * boxSize), (int)((centerCoord.Y - topLeft.Y) * boxSize)), new Point((int)((ray.endPos.X - topLeft.X) * boxSize), (int)((ray.endPos.Y - topLeft.Y) * boxSize)));
            //}
        }
    }
}
