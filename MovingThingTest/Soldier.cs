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
        // Current cell of the soldier
        public Cell currentCell;
        // Coordinates of the soldier on the grid
        public Vector2 gridCoord;
        // Vector representing the movement of the soldier
        protected Vector2 movingVec = new Vector2(0, 0);
        // Center coordinates of the soldier
        protected Vector2 centerCoord;
        // Direction the soldier is facing
        public int direction = 0;
        // Color of the soldier
        protected Color color = Color.Red;
        // Ray for shooting
        Ray ray = new Ray();
        // Ray for viewing
        Ray viewRay = new Ray();
        // Flag indicating if the soldier is shooting
        bool shooting = false;
        // Angle for shooting
        double rayAngle = 0;
        // Point where the soldier is shooting
        Vector2 shootingPoint;

        // Constructor
        public Soldier(Cell currentCell)
        {
            // Initialize soldier properties
            this.currentCell = currentCell;
            gridCoord = new Vector2(currentCell.col, currentCell.row);
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);
        }

        // Method to update soldier position
        public void updatePos(Stack<Cell> cellStack, float speed, int i)
        {
            // Update position if not reached destination
            if (currentCell.gridCoord != gridCoord)
            {
                gridCoord += movingVec;
                gridCoord = new Vector2(MathF.Round(gridCoord.X, i), MathF.Round(gridCoord.Y, i));
            }
            // Move to the next cell if available
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

        // Method to draw the soldier
        public virtual void drawSoldier(PaintEventArgs e, Grid grid, float size)
        {
            // Draw soldier
            SolidBrush brush = new SolidBrush(color);
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            e.Graphics.FillEllipse(brush, (gridCoord.X - topLeft.X) * size, (gridCoord.Y - topLeft.Y) * size, size, size);
            // Draw ray for shooting or viewing
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

        // Method to check for nearby enemies and determine if in line of sight
        public void checkForEnemy(List<Enemy> enemies, Grid grid)
        {
            // Reset shooting flag
            shooting = false;
            // Loop through all enemies
            foreach (Enemy enemy in enemies)
            {
                // Calculate distances between soldier and enemy
                float distanceX = Math.Abs(enemy.centerCoord.X - centerCoord.X);
                float distanceY = Math.Abs(enemy.centerCoord.Y - centerCoord.Y);
                float distance = MathF.Sqrt(distanceY * distanceY + distanceX * distanceX);

                // Calculate angle between soldier and enemy
                Vector2 normal = Vector2.Normalize(enemy.centerCoord - centerCoord);
                Vector2 right = new Vector2(1, 0);
                double dot = Vector2.Dot(normal, right);
                double det = normal.X * right.Y - normal.Y * right.X;
                double angle = -Math.Atan2(det, dot);

                // Check if enemy is within shooting range and in the soldier's field of view
                if (distance <= 5 && distance > 0)
                {
                    // Cast ray from soldier to enemy
                    ray = Ray.castRay(grid, centerCoord, angle);
                    Vector2 rayVector = ray.endPos - ray.startPos;
                    float rayVectorLength = MathF.Sqrt(rayVector.X * rayVector.X + rayVector.Y * rayVector.Y);
                    // If ray reaches enemy and enemy is within soldier's field of view
                    if (rayVectorLength >= distance && (ray.angle > direction * Math.PI / 2 - 3 * Math.PI / 4 & ray.angle < direction * Math.PI / 2 - Math.PI / 4))
                    {
                        // Set shooting angle and point
                        rayAngle = angle;
                        shootingPoint = enemy.centerCoord;
                        // Set shooting flag to true
                        shooting = true;
                        // Set enemy shooting flag to true
                        enemy.shooting = true;
                    }
                }
            }
        }

    }
}
