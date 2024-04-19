using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovingThingTest
{
    public partial class PlayerControl : UserControl
    {
        string filePath;

        public Box box;
        public Squad squad;
        public Grid grid;
        public bool cameraLock = false;
        public List<enemyPath> enemyPaths = new List<enemyPath>();
        public Stack<Cell> cellStack = new Stack<Cell>();
        public List<Enemy> enemies = new List<Enemy>();
        public bool drag = false;
        public Vector2 mouseDownGridCoord;

        public int mode = 0;
        public int tyle = 0;
        public PlayerControl()
        {
            InitializeComponent();

            grid = new Grid(this.Width, this.Height);
            grid.createGrid();

            //box = new Box(grid, grid.cellArr[1, 1].screenPos, grid.cellSize);
            squad = new Squad(grid.cellArr[1, 1], grid, 5);
        }

        public PlayerControl(string filePath, Grid grid, List<enemyPath> enemyPaths, Cell spawnCell)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.grid = grid;
            this.enemyPaths = enemyPaths;
            loadEnemies(enemyPaths);
            squad = new Squad(spawnCell, grid, 5);
        }

        public void loadEnemies(List<enemyPath> enemyPaths)
        {
            foreach (enemyPath path in enemyPaths)
            {
                enemies.Add(new Enemy(path));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            grid.updateScreenSize(Width, Height);
            //box.UpdatePos(grid, grid.pathStack);

            foreach(Enemy enemy in enemies)
            {
                enemy.updatePos();
            }
            squad.updatePos(cellStack, grid, enemies);
            pictureBox1.Invalidate();
        }

        private void modeButton_Click(object sender, EventArgs e)
        {
            string[] names = new string[] { "Move", "Pan" };
            mode = (mode + 1) % 2;

            modeButton.Text = names[mode];
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (drag)
            {
                cameraLock = false;
                Vector2 mouseMove = new Vector2((MousePosition.X - mouseDownGridCoord.X) / grid.cellSize, (MousePosition.Y - mouseDownGridCoord.Y) / grid.cellSize);
                //Vector2 mouseMoveVec = grid.getGridCoordFromPoint()
                grid.cameraPosition -= mouseMove;
                mouseDownGridCoord = new Vector2(MousePosition.X, MousePosition.Y);
            }
            if (cameraLock)
            {
                grid.cameraPosition = box.gridCoord + new Vector2(0.5f, 0.5f);
            }
            grid.drawGrid(e);
            //box.drawBox(e, grid);
            squad.drawSquad(e, grid, grid.cellSize);
            foreach(Enemy enemy in enemies)
            {
                enemy.drawSoldier(e, grid, grid.cellSize);
            }
            //box.vision(grid, e);
            //box.drawVisionCone(grid, e);
            //grid.drawWalls(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPos = me.Location;
            Cell targetCell;
            Vector2 targetGridCoord = grid.getCellCoordFromPoint(clickPos);
            if (targetGridCoord.X >= 0 && targetGridCoord.Y >= 0 && targetGridCoord.X < grid.cols && targetGridCoord.Y < grid.rows)
            {
                targetCell = grid.cellArr[(int)targetGridCoord.X, (int)targetGridCoord.Y];
                if (targetCell is not Border && squad.currentCell != targetCell)
                {
                    switch (mode)
                    {
                        case 0:
                            grid.resetPathFind();
                            if (targetCell.permeable != 0)
                            {
                                cellStack = grid.PathFind(grid, squad.currentCell, targetCell);
                                //grid.ColourPath();
                            }
                            break;
                        case 1:
                            //grid.placeTyle(targetCell, item);
                            break;
                        case 2:
                            break;
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mode == 1)
            {
                drag = true;
                Point p = PointToScreen(e.Location);
                mouseDownGridCoord = new Vector2(p.X, p.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mode == 1)
            {
                drag = false;
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            grid.cameraSize.Y = grid.cameraSize.Y * 1 - e.Delta / 200f;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
            //box.boxSize = grid.cellSize;
        }
    }
}
