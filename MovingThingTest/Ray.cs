using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MovingThingTest
{
    public class Ray
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public double angle;
        public double magnitude;

        public Ray()
        {
            startPos = new Vector2(0,0);
            endPos = new Vector2(0,0);
            angle = 0;
            this.magnitude = 0;
        }
        public Ray(Vector2 startPos, Vector2 endPos, double angle) { 
            this.startPos = startPos;
            this.endPos = endPos;
            this.angle = angle;
            this.magnitude = Math.Sqrt((endPos.X - startPos.X) * (endPos.X - startPos.X) + (endPos.Y - startPos.Y) * (endPos.Y - startPos.Y));
        }

        public static Ray castRay(Grid grid, Vector2 startCoord, double angle)
        {
            double gradient = Math.Tan(angle);

            int signSin = Math.Sign(Math.Sin(angle));
            int signCos = Math.Sign(Math.Cos(angle));

            int getYBlockCorrectorX = (-1 * signCos - 1) / 2;
            int getYBlockCorrectorY = (signSin - 1) / 2;

            int getXBlockCorrectorX = (signCos - 1) / 2;

            int floorOrCeilX = (signCos + 1) / 2;
            int floorOrCeilY = (signSin + 1) / 2;

            bool wallHit = false;
            double lastY = startCoord.Y;

            double c = startCoord.Y - startCoord.X * gradient;

            double x = startCoord.X + (floorOrCeilX - startCoord.X % 1);
            double y = (x * gradient) + c;

            while (!wallHit)
            {
                int deltaY = Math.Abs((int)y - (int)lastY);

                if (deltaY > 0)
                {
                    for (int i = (int)(lastY + floorOrCeilY - lastY % 1); i * signSin < y * signSin; i = i + 1 * signSin)
                    {
                        if (!grid.cellArr[(int)(x + getYBlockCorrectorX), (i + getYBlockCorrectorY)].clear)
                        {
                            wallHit = true;
                            y = i;
                            x = (y - c) / gradient;
                            break;
                        }
                    }
                }
                if (!grid.cellArr[(int)(x + getXBlockCorrectorX), (int)y].clear && !wallHit)
                {
                    wallHit = true;
                }
                if (!wallHit)
                {
                    x = x + signCos;
                    lastY = y;
                    y = x * gradient + c;
                }
            }

            Vector2 endCoord = new Vector2((float)x, (float)y);
            Ray ray = new Ray(startCoord, endCoord, angle);
            return ray;
        }



        public static Cell getCellFromRaycast(Grid grid, Vector2 startCoord, double angle)
        {
            double gradient = Math.Tan(angle);

            int signSin = Math.Sign(Math.Sin(angle));
            int signCos = Math.Sign(Math.Cos(angle));

            int getYBlockCorrectorX = (-1 * signCos - 1) / 2;
            int getYBlockCorrectorY = (signSin - 1) / 2;

            int getXBlockCorrectorX = (signCos - 1) / 2;

            int floorOrCeilX = (signCos + 1) / 2;
            int floorOrCeilY = (signSin + 1) / 2;

            bool wallHit = false;
            double lastY = startCoord.Y;

            double c = startCoord.Y - startCoord.X * gradient;

            double x = startCoord.X + (floorOrCeilX - startCoord.X % 1);
            double y = (x * gradient) + c;

            while (!wallHit)
            {
                int deltaY = Math.Abs((int)y - (int)lastY);

                if (deltaY > 0)
                {
                    for (int i = (int)(lastY + floorOrCeilY - lastY % 1); i * signSin < y * signSin; i = i + 1 * signSin)
                    {
                        if (!grid.cellArr[(int)(x + getYBlockCorrectorX), (i + getYBlockCorrectorY)].clear)
                        {
                            return grid.cellArr[(int)(x + getYBlockCorrectorX), (i + getYBlockCorrectorY)];
                        }
                    }
                }
                if (!grid.cellArr[(int)(x + getXBlockCorrectorX), (int)y].clear && !wallHit)
                {
                    return grid.cellArr[(int)(x + getXBlockCorrectorX), (int)y];
                }
                if (!wallHit)
                {
                    x = x + signCos;
                    lastY = y;
                    y = x * gradient + c;
                }
            }

            return new Cell();
        }

        public static Ray roundRay(Ray ray, int i)
        {
            Ray ray2 = new Ray(ray.startPos, new Vector2((int)Math.Round(ray.endPos.X,i), (int)Math.Round(ray.endPos.Y,i)), ray.angle);   
            return ray2;
        }
    }
}
