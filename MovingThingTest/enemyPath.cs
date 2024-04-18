using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using System.Numerics;
using Windows.ApplicationModel.Email.DataProvider;

namespace MovingThingTest
{
    public class enemyPath
    {
        protected List<Cell> pathAnchors = new List<Cell>();
        protected List<dynamic> pathCellsLists = new List<dynamic>();
        Stack<Cell> path = new Stack<Cell>();
        protected bool loop;
        public SolidBrush brush = new SolidBrush(Color.Red);
        public Color color = Color.Red;

        public enemyPath() { 
        
        }
        public enemyPath(bool loop)
        {
            this.loop = loop;
        }



        public string saveString()
        {
            string str = "";
            foreach (Cell cell in pathAnchors) 
            {
                str += cell.row.ToString() + "," + cell.col.ToString() + ",";
            }
            foreach (List<Cell> list in pathCellsLists)
            {
                str += "\n";
                foreach (Cell c in list)
                {
                    str += c.row.ToString() + "," + c.col.ToString() + ",";
                }

            }
            str += "\n";
            str += loop.ToString();
            str += "\n";
            str += color.ToArgb().ToString();
            return str;
        }
        public void addPoint(Cell cell, Grid grid)
        {
            if (!loop)
            {
                if (pathAnchors.Contains(cell))
                {
                    if (pathAnchors[pathAnchors.Count - 1] == cell)
                    {
                        pathAnchors.RemoveAt(pathAnchors.Count - 1);
                        if (pathAnchors.Count > 0)
                        {
                            pathCellsLists.RemoveAt(pathCellsLists.Count - 1);
                        }
                        return;
                    }
                    if (pathAnchors[0] == cell)
                    {
                        loop = true;
                    }
                }

                pathAnchors.Add(cell);

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

        public bool placeOver(Cell cell, Grid grid)
        {
            foreach(Cell anch in pathAnchors)
            {
                if (anch == cell)
                {
                    return false;
                }
            }
            int i = 0;
            List<int> toRemove = new List<int> ();
            foreach(List<Cell> pathPart in pathCellsLists)
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
            foreach(int j in toRemove)
            {
                pathCellsLists.RemoveAt(j);
                pathCellsLists.Insert(j, new List<Cell>());
                grid.resetPathFind();
                Stack<Cell> stack = grid.PathFind(grid, pathAnchors[j], pathAnchors[j+1]);
                foreach (Cell pathCell in stack)
                {
                    pathCellsLists[j].Add(pathCell);
                }
            }
            return true;
        }

    }

    public class existingEnemyPath : enemyPath
    {
        public int num;
        public existingEnemyPath(bool loop) : base(loop){ 
            
        }

        public existingEnemyPath(int num, Color color) {
            this.num = num;
            this.color = color;
        }

        public existingEnemyPath(bool loop, List<Cell> pathAnchors, List<dynamic> pathCellsLists, Color color)
        {
            this.loop = loop;
            this.pathAnchors = pathAnchors;
            this.pathCellsLists = pathCellsLists;
            this.color = color;
        }

        public void drawPath(PaintEventArgs e, Vector2 topleft, float cellSize, Font font)
        {
            font = new Font(font.Name, cellSize*0.8f, FontStyle.Regular);
            Pen p = new Pen(brush, cellSize * 0.4f);
            RectangleF rect;
            foreach (List<Cell> pathPart in pathCellsLists)
            {
                for (int i = 1; i < pathPart.Count; i++)
                {
                    e.Graphics.DrawLine(p, new Point((int)((pathPart[i - 1].col - topleft.X + 0.5f) * cellSize), (int)((pathPart[i - 1].row - topleft.Y + 0.5f) * cellSize)), new Point((int)((pathPart[i].col - topleft.X + 0.5f) * cellSize), (int)((pathPart[i].row - topleft.Y + 0.5f) * cellSize)));
                }

            }
            int j = 1;
            string str;
            foreach (Cell anchor in pathAnchors)
            {
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
