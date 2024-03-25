using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using System.Numerics;
namespace MovingThingTest
{
    public class enemyPath
    {
        List<Cell> pathAnchors = new List<Cell>();
        List<dynamic> pathCellsLists = new List<dynamic>();
        List<Cell> path = new List<Cell>();
        bool loop;
        Grid grid;
        public SolidBrush brush = new SolidBrush(Color.Red);
        public Color color = Color.Red;

        public enemyPath() { 
        
        }
        public enemyPath(bool loop, Grid grid)
        {
            this.loop = loop;
            this.grid = grid;
        }

        public void addPoint(Cell cell)
        {
            pathAnchors.Add(cell);

            if(pathAnchors.Count > 1 ) {
                pathCellsLists.Add(new List<Cell>());
                Stack<Cell> stack = grid.PathFind(grid, pathAnchors[pathAnchors.Count - 2], pathAnchors[pathAnchors.Count - 1]);
                foreach(Cell pathCell in stack)
                {
                    pathCellsLists[pathCellsLists.Count-1].Add(pathCell);
                }
            }
        }

        public void drawPath(PaintEventArgs e, Vector2 topleft, float cellSize)
        {
            foreach (Cell anchor in pathAnchors)
            {
                RectangleF rect = new RectangleF((anchor.col - topleft.X) * cellSize, (anchor.row - topleft.Y) * cellSize, cellSize, cellSize);
                e.Graphics.FillEllipse(brush, rect);
            }
        }


        public void removePoint(Cell cell)
        {
        }
        

    }
}
