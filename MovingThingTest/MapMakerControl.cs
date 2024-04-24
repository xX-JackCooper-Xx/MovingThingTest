using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;

namespace MovingThingTest
{
    public partial class MapMakerControl : UserControl
    {
        // Declaration of class variables

        // Represents the grid
        public Grid grid;
        // List of enemy paths
        public List<enemyPath> enemyPaths = new List<enemyPath>();
        // Number of the current path
        public int pathNumber = 0;
        // Type of item being placed on the grid
        public Type item = typeof(Grass);

        // Flags for mouse actions
        public bool drag = false;
        public bool place = false;
        // Stores the grid coordinates where the mouse was initially clicked
        public Vector2 mouseDownGridCoord;

        // Mode variables
        public int mode = 0; // 0 for placing items, 1 for panning

        // Placeholder for future functionality
        public int tyle = 0;

        // List of grid coordinates where items are being placed
        public List<Vector2> placingVecs = new List<Vector2>();

        // Constructor for the MapMakerControl
        public MapMakerControl(int gridWidth, int gridHeight)
        {
            InitializeComponent();

            // Initialize the grid with specified width and height
            grid = new Grid(this.Width, this.Height, gridHeight, gridWidth);
            // Create the grid
            grid.createGrid();
        }

        // Overloaded constructor for the MapMakerControl
        public MapMakerControl(string filePath, Grid grid, List<enemyPath> enemyPaths)
        {
            InitializeComponent();
            this.grid = grid;
            this.enemyPaths = enemyPaths;
        }

        // Event handler for the timer tick event
        private void timer_Tick(object sender, EventArgs e)
        {
            // Update the screen size of the grid
            grid.updateScreenSize(Width, Height);
            // Invalidate the picture box to trigger repainting
            pictureBox.Invalidate();
        }

        // Event handler for the mode button click event
        private void modeButton_Click(object sender, EventArgs e)
        {
            // Toggle between place mode and pan mode
            string[] names = new string[] { "Place", "Pan" };
            mode = (mode + 1) % 2;
            modeButton.Text = names[mode];
        }

        // Event handler for the picture box paint event
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            // Flag to determine if mouse input is valid
            bool valid = true;
            // Calculate the movement of the mouse
            Vector2 mouseMove = new Vector2((MousePosition.X - mouseDownGridCoord.X) / grid.cellSize, (MousePosition.Y - mouseDownGridCoord.Y) / grid.cellSize);

            // If dragging the grid
            if (drag)
            {
                // Adjust the camera position based on mouse movement
                grid.cameraPosition -= mouseMove;
                // Update the initial mouse down coordinates
                mouseDownGridCoord = new Vector2(MousePosition.X, MousePosition.Y);
            }

            // If placing an item
            if (place)
            {
                // Initialize a target cell
                Cell targetCell = new Cell();
                targetCell = targetCell.toBorder();
                // Calculate the grid coordinates of the mouse pointer
                Vector2 targetGridCoord = grid.getCellCoordFromPoint(PointToClient(new Point((int)MousePosition.X, (int)MousePosition.Y)));

                // Check if the target grid coordinates are within the grid bounds
                if (targetGridCoord.X > 0 && targetGridCoord.Y > 0 && targetGridCoord.X < grid.cols && targetGridCoord.Y < grid.rows)
                {
                    targetCell = grid.cellArr[(int)targetGridCoord.X, (int)targetGridCoord.Y];
                }

                // Check if the target grid coordinates are not already occupied and the cell is not a border cell
                if (!placingVecs.Contains(targetGridCoord) && (targetCell is not Border))
                {
                    // If the item being placed is an existing enemy path, add a point to the path
                    if (item.Name == "existingEnemyPath")
                    {
                        enemyPaths[pathNumber].addPoint(targetCell, grid);
                    }
                    else
                    {
                        // Placeholder for future functionality
                    }

                    // Place the item on the grid
                    grid.placeTyle(targetCell, item);
                    // Place the item on all enemy paths
                    foreach (enemyPath p in enemyPaths)
                    {
                        p.placeOver(targetCell, grid);
                    }

                    // Add the grid coordinates to the list of placing vectors
                    placingVecs.Add(targetGridCoord);
                }
            }

            // Draw the grid
            grid.drawGrid(e);
            // Calculate the top left corner of the visible grid
            Vector2 topLeft = new Vector2((grid.cameraPosition.X - grid.cameraSize.X / 2), (grid.cameraPosition.Y - grid.cameraSize.Y / 2));
            // Draw all existing enemy paths
            foreach (existingEnemyPath P in enemyPaths)
            {
                P.drawPath(e, topLeft, grid.cellSize, Font);
            }
        }

        // Event handler for the picture box mouse down event
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Determine the action based on the current mode
            switch (mode)
            {
                // If in place mode
                case 0:
                    // Set the place flag to true
                    place = true;
                    break;
                // If in pan mode
                case 1:
                    // Set the drag flag to true and record the initial mouse down coordinates
                    drag = true;
                    Point p = PointToScreen(e.Location);
                    mouseDownGridCoord = new Vector2(p.X, p.Y);
                    break;
            }
        }

        // Event handler for the picture box mouse up event
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            // If in place mode, clear the list of placing vectors and set the place flag to false
            if (mode == 0)
            {
                placingVecs.Clear();
                place = false;
            }
            // If in pan mode, set the drag flag to false
            if (mode == 1)
            {
                drag = false;
            }
        }

        // Event handler for the picture box mouse wheel event
        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // Adjust the camera size and cell size based on the mouse wheel movement
            grid.cameraSize.Y = grid.cameraSize.Y * 1 - e.Delta / 200f;
            grid.cameraSize.X = grid.cameraSize.Y * grid.cameraRatio;
            grid.cellSize = grid.calculateCellSize();
        }
    }
}
