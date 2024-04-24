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
        // Declare variables
        public Squad squad; // Instance of Squad class
        public Grid grid; // Instance of Grid class
        public List<enemyPath> enemyPaths = new List<enemyPath>(); // List of enemy paths
        public Stack<Cell> cellStack = new Stack<Cell>(); // Stack of cells
        public List<Enemy> enemies = new List<Enemy>(); // List of enemies
        public bool drag = false; // Flag for mouse drag operation
        public Vector2 mouseDownGridCoord; // Coordinates of mouse down event

        public int mode = 0; // Mode for player control
        public int tyle = 0; // Not used in the current implementation

        // Constructor for initializing PlayerControl with squad size
        public PlayerControl(int squadSize)
        {
            InitializeComponent();

            // Initialize grid and squad
            grid = new Grid(this.Width, this.Height);
            grid.createGrid();
            squad = new Squad(grid.cellArr[1, 1], grid, squadSize);
        }

        // Constructor for initializing PlayerControl with file data
        public PlayerControl(string filePath, Grid grid, List<enemyPath> enemyPaths, Cell spawnCell, int squadSize)
        {
            InitializeComponent();
            this.grid = grid;
            this.enemyPaths = enemyPaths;
            loadEnemies(enemyPaths);
            squad = new Squad(spawnCell, grid, squadSize);
        }

        // Method to load enemies from enemy paths
        public void loadEnemies(List<enemyPath> enemyPaths)
        {
            foreach (enemyPath path in enemyPaths)
            {
                enemies.Add(new Enemy(path));
            }
        }

        // Event handler for timer tick event
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update grid size and positions of enemies and squad
            grid.updateScreenSize(Width, Height);
            List<Enemy> deadEnemies = new List<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.updatePos();
                if (enemy.health <= 0) { deadEnemies.Add(enemy); }
            }
            foreach (Enemy enemy in deadEnemies)
            {
                enemies.Remove(enemy);
            }
            squad.updatePos(cellStack, grid, enemies);
            pictureBox.Invalidate();
        }

        // Event handler for mode button click event
        private void modeButton_Click(object sender, EventArgs e)
        {
            // Toggle between Move and Pan modes
            string[] names = new string[] { "Move", "Pan" };
            mode = (mode + 1) % 2;
            modeButton.Text = names[mode];
        }

        // Event handler for picture box paint event
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            // Handle drag operation and draw grid, squad, and enemies
            if (drag)
            {
                Vector2 mouseMove = new Vector2((MousePosition.X - mouseDownGridCoord.X) / grid.cellSize, (MousePosition.Y - mouseDownGridCoord.Y) / grid.cellSize);
                grid.cameraPosition -= mouseMove;
                mouseDownGridCoord = new Vector2(MousePosition.X, MousePosition.Y);
            }
            grid.drawGrid(e);
            squad.drawSquad(e, grid, grid.cellSize);
            foreach (Enemy enemy in enemies)
            {
                enemy.drawSoldier(e, grid, grid.cellSize);
            }
        }

        // Event handler for picture box click event
        private void pictureBox_Click(object sender, EventArgs e)
        {
            // Handle click event based on selected mode
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
                            }
                            break;
                    }
                }
            }
        }

        // Event handler for mouse down event on picture box
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Start drag operation in Pan mode
            if (mode == 1)
            {
                drag = true;
                Point p = PointToScreen(e.Location);
                mouseDownGridCoord = new Vector2(p.X, p.Y);
            }
        }

        // Event handler for mouse up event on picture box
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            // End drag operation in Pan mode
            if (mode == 1)
            {
                drag = false;
            }
        }

        // Event handler for mouse wheel event on picture box
        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // Zoom in/out the grid using mouse wheel
            grid.cameraSize.Y = grid.cameraSize.Y * 1 - e.Delta / 200f;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
        }
    }
}
