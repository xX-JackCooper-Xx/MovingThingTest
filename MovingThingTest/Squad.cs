
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MovingThingTest
{
    public class Squad
    {
        List<Soldier> units = new List<Soldier>();
        public Cell currentCell;
        public Vector2 gridCoord;
        Vector2 movingVec = new Vector2(0, 0);
        public Vector2 centerCoord;
        List<Cell> targetCells = new List<Cell>();
        List<int> formationDirections = new List<int>();
        int direction = 0;
        List<Stack<Cell>> cellStacks = new List<Stack<Cell>>();
        public string formation = "herringbone";
        string[] formations = { "herringbone", "wedge" };

        public Squad(Cell currentCell, Grid grid, int size)
        {
            this.currentCell = currentCell;
            gridCoord = new Vector2(currentCell.col, currentCell.row);
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);

            units = new List<Soldier>();
            cellStacks = new List<Stack<Cell>>();
            for (int i = 0; i < size; i++)
            {
                units.Add(new Soldier(currentCell));
                cellStacks.Add(new Stack<Cell>());
            }
            loadFormation(formation, grid);

        }

        public void updatePos(Stack<Cell> cellStack, Grid grid, List<Enemy> enemies)
        {
            if(currentCell.gridCoord != gridCoord)
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
                updateDiretion();
                updateFormation(formation, grid);
            }
            centerCoord = new Vector2(gridCoord.X + 0.5f, gridCoord.Y + 0.5f);


            updateUnits(grid, enemies);
        }

        public void changeFormation(int i, Grid grid)
        {
            formation = formations[i];
            loadFormation(formation, grid);
        }

        public void drawSquad(PaintEventArgs e, Grid grid, float size)
        {
            //SolidBrush brush = new SolidBrush(Color.Yellow);
            //Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            //e.Graphics.FillRectangle(brush, (gridCoord.X - topLeft.X) * size, (gridCoord.Y - topLeft.Y) * size, size, size);
            //Ray ray = Ray.castRay(grid, centerCoord, direction * Math.PI / 2 - Math.PI / 2);
            //ray.drawRay(e, topLeft, size);
            drawUnits(e, grid, size);
        }

        public void updateDiretion()
        {
            if(movingVec.X == 0.1f)
            {
                direction = 1;
            }
            else if(movingVec.X == -0.1f)
            {
                direction = 3;
            }
            else if(movingVec.Y == 0.1f)
            {
                direction = 2;
            }
            else if(movingVec.Y == -0.1f)
            {
                direction = 0;
            }
        }

        public Vector2 getVecfromDirection(int direction)
        {
            Vector2 returnVec = Vector2.Zero;
            switch (direction) {
                case 0:
                    returnVec.Y = -1;
                    break;
                case 1:
                    returnVec.X = 1;
                    break;
                case 2:
                    returnVec.Y = 1;
                    break;
                case 3:
                    returnVec.X = -1;
                    break;
            }
            return returnVec;

        }

        public void updateFormation(string formation, Grid grid)
        {
            List<Cell> targetCellsTemp = new List<Cell>();
            List<int> formationDirectionsTemp = new List<int>();
            switch (formation)
            {
                case "herringbone":
                    targetCellsTemp.Add(currentCell);
                    formationDirectionsTemp.Add(direction);
                    formationDirectionsTemp.Add((direction + 1) % 4);

                    for (int i = 0; i < targetCells.Count() - 2; i++)
                    {
                        targetCellsTemp.Add(targetCells[i]);
                    }

                    for (int i = 1; i < formationDirections.Count() - 2; i++)
                    {
                        formationDirectionsTemp.Add((formationDirectionsTemp[i] + 2) % 4);
                    }

                    if(targetCells.Count > 1)
                    {
                        targetCellsTemp.Add(targetCells[targetCells.Count - 2]);
                        formationDirectionsTemp.Add((direction + 2) % 4);
                    }
                    if(targetCells.Count == 2)
                    {
                        formationDirectionsTemp.RemoveRange(1, 2);
                        formationDirectionsTemp.Add(((direction + 2) % 4));
                    }
                    //targetCells.Add(grid.cellArr[currentCell.col - (int)directionVec.X * (units.Count-1), currentCell.row - (int)directionVec.Y * (units.Count-1)]);
                    //formationDirections.Add((direction + 2) % 4);
                    targetCells = targetCellsTemp;
                    formationDirections = formationDirectionsTemp;
                    break;
                case "wedge":
                    targetCells = new List<Cell>();
                    formationDirections = new List<int>();
                    wedge(grid);

                    break;
            }
        }

        public void loadFormation(string formation, Grid grid)
        {
            targetCells = new List<Cell>();
            formationDirections = new List<int>();
            int tempDirection = direction;
            Vector2 directionVec;
            switch(formation){
                case "herringbone":
                    targetCells.Add(currentCell);
                    formationDirections.Add(direction);
                    for(int i = 1; i < units.Count; i++)
                    {
                        directionVec = getVecfromDirection(tempDirection);

                        if (grid.cellArr[targetCells[i-1].col - (int)directionVec.X, targetCells[i-1].row - (int)directionVec.Y].permeable != 0)
                        {
                            targetCells.Add(grid.cellArr[targetCells[i - 1].col - (int)directionVec.X, targetCells[i - 1].row - (int)directionVec.Y]);
                        }

                        else
                        {
                            tempDirection = (tempDirection + 1) % 4;
                            for (int j = 0; j < 4; j++)
                            {
                                Vector2 neighbourDir = grid.cellArr[targetCells[i-1].col, targetCells[i - 1].row].neighbours[tempDirection];
                                Cell cell = grid.cellArr[targetCells[i - 1].col + (int)neighbourDir.X, targetCells[i - 1].row + (int)neighbourDir.Y];
                                if (cell.permeable != 0)
                                {
                                    targetCells.Add(cell);
                                    break;
                                }
                                tempDirection = (tempDirection + 1) % 4;
                            }

                        }
                        formationDirections.Add((direction + 2 * i - 1) % 4);
                    }
                    formationDirections[formationDirections.Count - 1] = (direction + 2) % 4;


                    break;
                case "wedge":
                    wedge(grid);

                    break;
            }
        }

        public void wedge(Grid grid)
        {
            targetCells.Add(currentCell);
            foreach (Soldier s in units)
            {
                formationDirections.Add(direction);
                formationDirections.Add(direction);
                formationDirections.Add(direction);
                formationDirections.Add(direction);
                formationDirections.Add(direction);
            }
            Vector2 behind = getVecfromDirection((direction + 2) % 4);
            Vector2 left = getVecfromDirection((direction + 3) % 4);
            Vector2 right = getVecfromDirection((direction + 1) % 4);
            Vector2 infront = getVecfromDirection(direction);

            Cell nextCell = grid.cellArr[currentCell.col + (int)(behind.X + left.X), currentCell.row + (int)(behind.Y + left.Y)];
            Cell infrontNextCell = grid.cellArr[nextCell.col + (int)infront.X, nextCell.row + (int)infront.Y];
            Cell insideNextCell = grid.cellArr[nextCell.col + (int)right.X, nextCell.row + (int)right.Y];

            if(units.Count == 1)
            {
                return;
            }
            if (nextCell.permeable != 0 && !(infrontNextCell.permeable == 0 && insideNextCell.permeable == 0))
            {
                targetCells.Add(nextCell);
            }
            else if (nextCell.permeable == 0 && infrontNextCell.permeable == 0 && insideNextCell.permeable == 0)
            {
                targetCells.Add(currentCell);
            }
            else if (infrontNextCell.permeable == 0)
            {
                targetCells.Add(insideNextCell);
            }
            else
            {
                targetCells.Add(infrontNextCell);
            }

            if (units.Count == 2) { return; }

            nextCell = grid.cellArr[currentCell.col + (int)(behind.X + right.X), currentCell.row + (int)(behind.Y + right.Y)];
            infrontNextCell = grid.cellArr[nextCell.col + (int)infront.X, nextCell.row + (int)infront.Y];
            insideNextCell = grid.cellArr[nextCell.col + (int)left.X, nextCell.row + (int)left.Y];

            if (nextCell.permeable != 0 && !(infrontNextCell.permeable == 0 && insideNextCell.permeable == 0))
            {
                targetCells.Add(nextCell);
            }
            else if (nextCell.permeable == 0 && infrontNextCell.permeable == 0 && insideNextCell.permeable == 0)
            {
                targetCells.Add(currentCell);
            }
            else if (infrontNextCell.permeable == 0)
            {
                targetCells.Add(insideNextCell);
            }
            else
            {
                targetCells.Add(infrontNextCell);
            }
            int max = units.Count - 2;
            for (int i = 1; i < max; i++)
            {
                int insideMult = ((2 * i) % 4) - 1;
                nextCell = grid.cellArr[targetCells[i].col + (int)(behind.X - right.X * insideMult), targetCells[i].row + (int)(behind.Y - right.Y * insideMult)];
                infrontNextCell = grid.cellArr[nextCell.col + (int)infront.X, nextCell.row + (int)infront.Y];
                insideNextCell = grid.cellArr[nextCell.col + (int)(right.X * insideMult), nextCell.row + (int)(right.Y * insideMult)];
                if (nextCell.permeable != 0 && !(infrontNextCell.permeable == 0 && insideNextCell.permeable == 0))
                {
                    targetCells.Add(nextCell);
                }
                else if (nextCell.permeable == 0 && infrontNextCell.permeable == 0 && insideNextCell.permeable == 0)
                {
                    targetCells.Add(currentCell);
                }
                else if (infrontNextCell.permeable == 0)
                {
                    targetCells.Add(insideNextCell);
                }
                else
                {
                    targetCells.Add(infrontNextCell);
                }
            }
        }

        public void updateUnits(Grid grid, List<Enemy> enemies)
        {
            int i = 0;
            foreach (Soldier s in units)
            {
                if (s.currentCell != targetCells[i])
                {
                    cellStacks[i] = grid.PathFind(grid, s.currentCell, targetCells[i]);
                    grid.resetPathFind();
                }
                s.updatePos(cellStacks[i], 0.1f, 1);
                s.direction = formationDirections[i];
                i++;
                s.checkForEnemy(enemies, grid);
            }
            
        }

        public void drawUnits(PaintEventArgs e, Grid grid, float size)
        {
            foreach(Soldier s in units)
            {
                s.drawSoldier(e, grid, size);
            }
        }
    }
}
