using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MovingThingTest
{
    public class Enemy : Soldier
    {
        enemyPath path = new enemyPath();

        int pathSection = 0;
        Stack<Cell> pathStack = new Stack<Cell>(); 
        public bool shooting = false;
        public Enemy(enemyPath enemyPath) : base(enemyPath.pathAnchors[0])
        {
            Stack<Cell> tempStack = new Stack<Cell>();
            color = Color.Black;
            this.path = enemyPath;
            foreach (Cell cell in enemyPath.pathCellsLists[0])
            {
                tempStack.Push(cell);
            }
            foreach(Cell cell in tempStack)
            {
                pathStack.Push(cell);
            }
        }

        public void updatePos()
        {
            if (shooting)
            {
                shooting = false;
                return;
            }
            if (pathStack.Count > 0)
            {
                base.updatePos(pathStack, 0.05f, 2);
                return;
            }

            pathSection++;
            if(pathSection == path.pathAnchors.Count - 1)
            {
                pathSection = 0;
                if (!path.loop)
                {
                    path.pathCellsLists.Reverse();
                    for (int i = 0; i < path.pathCellsLists.Count; i++)
                    {
                        path.pathCellsLists[i].Reverse();
                    }
                }
            }
            

            Stack<Cell> tempStack = new Stack<Cell>();
            foreach (Cell cell in path.pathCellsLists[pathSection])
            {
                tempStack.Push(cell);
            }
            foreach(Cell cell in tempStack)
            {
                pathStack.Push(cell);
            }
        }
    }
}
