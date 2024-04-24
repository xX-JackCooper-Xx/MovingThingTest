using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MovingThingTest
{
    // Class representing an enemy, inheriting from the Soldier class
    public class Enemy : Soldier
    {
        // Instance variable to store the enemy's path
        enemyPath path = new enemyPath();

        // Integer to keep track of the current section in the path
        int pathSection = 0;
        // Stack to hold the path cells
        Stack<Cell> pathStack = new Stack<Cell>();

        // Boolean flag indicating if the enemy is shooting
        public bool shooting = false;
        // Integer to represent the health of the enemy
        public int health = 100;

        // Constructor for creating an enemy instance with a specified path
        public Enemy(enemyPath enemyPath) : base(enemyPath.pathAnchors[0])
        {
            // Temporary stack to store path cells
            Stack<Cell> tempStack = new Stack<Cell>();
            // Set enemy color to black
            color = Color.Black;
            // Assign the provided path to the enemy
            this.path = enemyPath;
            // Push path cells into temporary stack and then into path stack
            foreach (Cell cell in enemyPath.pathCellsLists[0])
            {
                tempStack.Push(cell);
            }
            foreach (Cell cell in tempStack)
            {
                pathStack.Push(cell);
            }
        }

        // Override method to draw the enemy
        public override void drawSoldier(PaintEventArgs e, Grid grid, float size)
        {
            // Create brush and pen for drawing
            SolidBrush brush = new SolidBrush(color);
            Pen p = new Pen(Color.Black, 4);
            // Calculate top-left corner of the grid
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            // Draw the enemy
            e.Graphics.FillEllipse(brush, (gridCoord.X - topLeft.X) * size, (gridCoord.Y - topLeft.Y) * size, size, size);

            // Draw health bar if the enemy is shooting
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

        // Method to update the position of the enemy
        public void updatePos()
        {
            // Decrease health if the enemy is shooting
            if (shooting)
            {
                health--;
                shooting = false;
                return;
            }
            // Update position based on the current path
            if (pathStack.Count > 0)
            {
                //follows the updatePos method in the soldier class
                base.updatePos(pathStack, 0.05f, 2);
                return;
            }

            // Move to the next section of the path
            pathSection++;
            // Check if the end of the path is reached
            if (pathSection == path.pathAnchors.Count - 1)
            {
                pathSection = 0;
                // Reverse the path if it's not a loop
                if (!path.loop)
                {
                    path.pathCellsLists.Reverse();
                    for (int i = 0; i < path.pathCellsLists.Count; i++)
                    {
                        path.pathCellsLists[i].Reverse();
                    }
                }
            }

            // Push the cells of the new section into the path stack
            Stack<Cell> tempStack = new Stack<Cell>();
            foreach (Cell cell in path.pathCellsLists[pathSection])
            {
                tempStack.Push(cell);
            }
            foreach (Cell cell in tempStack)
            {
                pathStack.Push(cell);
            }
        }
    }
}
