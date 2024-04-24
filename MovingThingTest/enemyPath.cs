using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MovingThingTest
{
    // Class representing a path for enemies to follow
    public class enemyPath
    {
        // List of anchor cells defining the path
        public List<Cell> pathAnchors = new List<Cell>();
        // List of lists containing cells representing different sections of the path
        public List<dynamic> pathCellsLists = new List<dynamic>();
        // Stack to hold path cells
        Stack<Cell> path = new Stack<Cell>();
        // Boolean indicating if the path loops
        public bool loop;
        // Brush and color for drawing the path
        public SolidBrush brush = new SolidBrush(Color.Red);
        public Color color = Color.Red;

        // Default constructor
        public enemyPath()
        {

        }

        // Constructor with loop parameter
        public enemyPath(bool loop)
        {
            this.loop = loop;
        }

        // Method to generate a string representation of the path for saving
        public string saveString()
        {
            string str = "";
            // Serialize anchor cells
            foreach (Cell cell in pathAnchors)
            {
                str += cell.row.ToString() + "," + cell.col.ToString() + ",";
            }
            // Serialize path cell lists
            foreach (List<Cell> list in pathCellsLists)
            {
                str += "\n";
                foreach (Cell c in list)
                {
                    str += c.row.ToString() + "," + c.col.ToString() + ",";
                }

            }
            // Serialize loop status and color
            str += "\n";
            str += loop.ToString();
            str += "\n";
            str += color.ToArgb().ToString();
            return str;
        }

        // Method to add a point to the path
        public void addPoint(Cell cell, Grid grid)
        {
            // Check if loop is not set
            if (!loop)
            {
                // Check if the cell is already an anchor
                if (pathAnchors.Contains(cell))
                {
                    // Remove the cell if it's the last anchor
                    if (pathAnchors[pathAnchors.Count - 1] == cell)
                    {
                        pathAnchors.RemoveAt(pathAnchors.Count - 1);
                        if (pathAnchors.Count > 0)
                        {
                            pathCellsLists.RemoveAt(pathCellsLists.Count - 1);
                        }
                        return;
                    }
                    // Set loop if the first anchor is repeated
                    if (pathAnchors[0] == cell)
                    {
                        loop = true;
                    }
                }

                // Add the cell as an anchor
                pathAnchors.Add(cell);

                // Calculate path cells between anchor points
                if (pathAnchors.Count > 1)
                {
                    pathCellsLists.Add(new List<Cell>());
                    grid.resetPathFind();
                    Stack<Cell> stack = grid.PathFind(grid, pathAnchors[pathAnchors.Count - 2], pathAnchors[pathAnchors.Count - 1]);
                    foreach (Cell pathCell in stack)
                    {
                        pathCellsLists[pathCellsLists.Count - 1].Add(pathCell);
                    }
                }
            }
            else
            {
                // Disable loop if the first anchor is repeated
                if (pathAnchors[0] == cell)
                {
                    loop = false;
                    pathAnchors.RemoveAt(pathAnchors.Count - 1);
                    if (pathAnchors.Count > 0)
                    {
                        pathCellsLists.RemoveAt(pathCellsLists.Count - 1);
                    }
                }
            }
        }

        // Method to place the path over a cell
        public bool placeOver(Cell cell, Grid grid)
        {
            // Check if the cell is already an anchor
            foreach (Cell anch in pathAnchors)
            {
                if (anch == cell)
                {
                    return false;
                }
            }

            // Check if the cell is part of a path section
            int i = 0;
            List<int> toRemove = new List<int>();
            foreach (List<Cell> pathPart in pathCellsLists)
            {
                foreach (Cell c in pathPart)
                {
                    if (c == cell)
                    {
                        toRemove.Add(i);
                        break;
                    }
                }
                i++;
            }

            // Replace the affected path sections
            foreach (int j in toRemove)
            {
                pathCellsLists.RemoveAt(j);
                pathCellsLists.Insert(j, new List<Cell>());
                grid.resetPathFind();
                Stack<Cell> stack = grid.PathFind(grid, pathAnchors[j], pathAnchors[j + 1]);
                foreach (Cell pathCell in stack)
                {
                    pathCellsLists[j].Add(pathCell);
                }
            }
            return true;
        }

    }

    // Class representing an existing enemy path
    public class existingEnemyPath : enemyPath
    {
        // Integer representing the path number
        public int num;

        // Constructor with loop parameter
        public existingEnemyPath(bool loop) : base(loop)
        {

        }

        // Constructor with number and color parameters
        public existingEnemyPath(int num, Color color)
        {
            this.num = num;
            this.color = color;
        }

        // Constructor with loop, path anchors, path cells, and color parameters
        public existingEnemyPath(bool loop, List<Cell> pathAnchors, List<dynamic> pathCellsLists, Color color)
        {
            this.loop = loop;
            this.pathAnchors = pathAnchors;
            this.pathCellsLists = pathCellsLists;
            this.color = color;
        }

        // Method to draw the path
        public void drawPath(PaintEventArgs e, Vector2 topleft, float cellSize, Font font)
        {
            font = new Font(font.Name, cellSize * 0.8f, FontStyle.Regular);
            Pen p = new Pen(brush, cellSize * 0.4f);
            RectangleF rect;
            foreach (List<Cell> pathPart in pathCellsLists)
            {
                for (int i = 1; i < pathPart.Count; i++)
                {
                    // Draw lines between path cells
                    e.Graphics.DrawLine(p, new Point((int)((pathPart[i - 1].col - topleft.X + 0.5f) * cellSize), (int)((pathPart[i - 1].row - topleft.Y + 0.5f) * cellSize)), new Point((int)((pathPart[i].col - topleft.X + 0.5f) * cellSize), (int)((pathPart[i].row - topleft.Y + 0.5f) * cellSize)));
                }

            }
            int j = 1;
            string str;
            foreach (Cell anchor in pathAnchors)
            {
                // Draw path anchors
                if (loop && j == pathAnchors.Count)
                {
                    str = "L";
                }
                else
                {
                    str = Convert.ToString(j);
                }
                rect = new RectangleF((anchor.col - topleft.X) * cellSize, (anchor.row - topleft.Y) * cellSize, cellSize, cellSize);
                e.Graphics.FillEllipse(brush, rect);
                brush.Color = Color.Black;
                e.Graphics.DrawString(str, font, brush, rect.Left, rect.Top - cellSize * 0.2f);
                brush.Color = color;
                j++;
            }

        }
    }
}
