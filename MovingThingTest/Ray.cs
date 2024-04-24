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
        // Start position of the ray
        public Vector2 startPos;
        // End position of the ray
        public Vector2 endPos;
        // Angle of the ray
        public double angle;
        // Magnitude of the ray
        public double magnitude;

        // Default constructor
        public Ray()
        {
            startPos = new Vector2(0, 0);
            endPos = new Vector2(0, 0);
            angle = 0;
            this.magnitude = 0;
        }

        // Parameterized constructor
        public Ray(Vector2 startPos, Vector2 endPos, double angle)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.angle = angle;
            this.magnitude = Math.Sqrt((endPos.X - startPos.X) * (endPos.X - startPos.X) + (endPos.Y - startPos.Y) * (endPos.Y - startPos.Y));
        }

        // Method to cast a ray and determine its end position
        public static Ray castRay(Grid grid, Vector2 startCoord, double angle)
        {
            // Calculate gradient of the ray
            double gradient = Math.Tan(angle);

            // Determine signs of sin and cos for quadrant information
            int signSin = Math.Sign(Math.Sin(angle));
            int signCos = Math.Sign(Math.Cos(angle));

            // Determine correction factors for getting cell blocks in Y direction
            int getYBlockCorrectorX = (-1 * signCos - 1) / 2;
            int getYBlockCorrectorY = (signSin - 1) / 2;

            // Determine correction factors for getting cell blocks in X direction
            int getXBlockCorrectorX = (signCos - 1) / 2;

            // Determine whether to floor or ceil X and Y values
            int floorOrCeilX = (signCos + 1) / 2;
            int floorOrCeilY = (signSin + 1) / 2;

            // Flag indicating if the ray has hit a wall
            bool wallHit = false;
            // Store the last Y position
            double lastY = startCoord.Y;

            // Calculate the constant term 'c' for the line equation (y = mx + c)
            double c = startCoord.Y - startCoord.X * gradient;

            // Initialize ray end position with adjusted X value
            double x = startCoord.X + (floorOrCeilX - startCoord.X % 1);
            double y = (x * gradient) + c;

            // Loop until wall is hit
            while (!wallHit)
            {
                // Calculate the change in Y
                int deltaY = Math.Abs((int)y - (int)lastY);

                // Check for vertical wall hits
                if (deltaY > 0)
                {
                    // Iterate through the cell blocks vertically
                    for (int i = (int)(lastY + floorOrCeilY - lastY % 1); i * signSin < y * signSin; i = i + 1 * signSin)
                    {
                        // Check if the current cell block is not clear (hit a wall)
                        if (!grid.cellArr[(int)(x + getYBlockCorrectorX), (i + getYBlockCorrectorY)].clear)
                        {
                            // Set wall hit flag
                            wallHit = true;
                            // Adjust end position to the wall
                            y = i;
                            x = (y - c) / gradient;
                            // Exit the loop
                            break;
                        }
                    }
                }
                // Check for horizontal wall hits
                if (!grid.cellArr[(int)(x + getXBlockCorrectorX), (int)y].clear && !wallHit)
                {
                    // Set wall hit flag
                    wallHit = true;
                }
                // If no wall hit, move along the ray
                if (!wallHit)
                {
                    x = x + signCos;
                    lastY = y;
                    y = x * gradient + c;
                }
            }

            // Calculate the end coordinates of the ray
            Vector2 endCoord = new Vector2((float)x, (float)y);
            // Create a new ray with start and end coordinates
            Ray ray = new Ray(startCoord, endCoord, angle);
            return ray;
        }

        // Method to cast a ray and retrieve the cell hit by the ray
        public static Cell getCellFromRaycast(Grid grid, Vector2 startCoord, double angle)
        {
            // Calculate gradient of the ray
            double gradient = Math.Tan(angle);

            // Determine signs of sin and cos for quadrant information
            int signSin = Math.Sign(Math.Sin(angle));
            int signCos = Math.Sign(Math.Cos(angle));

            // Determine correction factors for getting cell blocks in Y direction
            int getYBlockCorrectorX = (-1 * signCos - 1) / 2;
            int getYBlockCorrectorY = (signSin - 1) / 2;

            // Determine correction factors for getting cell blocks in X direction
            int getXBlockCorrectorX = (signCos - 1) / 2;

            // Determine whether to floor or ceil X and Y values
            int floorOrCeilX = (signCos + 1) / 2;
            int floorOrCeilY = (signSin + 1) / 2;

            // Flag indicating if the ray has hit a wall
            bool wallHit = false;
            // Store the last Y position
            double lastY = startCoord.Y;

            // Calculate the constant term 'c' for the line equation (y = mx + c)
            double c = startCoord.Y - startCoord.X * gradient;

            // Initialize ray end position with adjusted X value
            double x = startCoord.X + (floorOrCeilX - startCoord.X % 1);
            double y = (x * gradient) + c;

            // Loop until wall is hit
            while (!wallHit)
            {
                // Calculate the change in Y
                int deltaY = Math.Abs((int)y - (int)lastY);

                // Check for vertical wall hits
                if (deltaY > 0)
                {
                    // Iterate through the cell blocks vertically
                    for (int i = (int)(lastY + floorOrCeilY - lastY % 1); i * signSin < y * signSin; i = i + 1 * signSin)
                    {
                        // Check if the current cell block is not clear (hit a wall)
                        if (!grid.cellArr[(int)(x + getYBlockCorrectorX), (i + getYBlockCorrectorY)].clear)
                        {
                            // Return the cell hit by the ray
                            return grid.cellArr[(int)(x + getYBlockCorrectorX), (i + getYBlockCorrectorY)];
                        }
                    }
                }
                // Check for horizontal wall hits
                if (!grid.cellArr[(int)(x + getXBlockCorrectorX), (int)y].clear && !wallHit)
                {
                    // Return the cell hit by the ray
                    return grid.cellArr[(int)(x + getXBlockCorrectorX), (int)y];
                }
                // If no wall hit, move along the ray
                if (!wallHit)
                {
                    x = x + signCos;
                    lastY = y;
                    y = x * gradient + c;
                }
            }

            // Return an empty cell if no hit is found
            return new Cell();
        }


        // Method to round the end position of a ray to specific decimal places
        public static Ray roundRay(Ray ray, int i)
        {
            // Round the end position to specified decimal places
            Ray ray2 = new Ray(ray.startPos, new Vector2((int)Math.Round(ray.endPos.X, i), (int)Math.Round(ray.endPos.Y, i)), ray.angle);
            return ray2;
        }

        // Method to draw the ray
        public void drawRay(PaintEventArgs e, Vector2 topLeft, float size)
        {
            // Draw the ray line
            Pen p = new Pen(Color.Black, 5);
            e.Graphics.DrawLine(p, new Point((int)((startPos.X - topLeft.X) * size), (int)((startPos.Y - topLeft.Y) * size)), new Point((int)((endPos.X - topLeft.X) * size), (int)((endPos.Y - topLeft.Y) * size)));
        }
    }
}
