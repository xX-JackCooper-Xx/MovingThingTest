using System.Numerics;
using System.Drawing;

namespace MovingThingTest
{
    public partial class Form1 : Form
    {

        public Box box;
        public Grid grid;
        public Form1()
        {
      
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;


            grid = new Grid(pictureBox1);
            grid.createGrid();

            this.box = new Box(grid.cellArr[0,0].cellPos, grid.cellSize);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPos = me.Location;
            Vector2 target;

            target = grid.getCellFromPoint(clickPos);
            Vector2 boxCell = grid.getCellFromCoord(box.pos);


            grid.PathFind(grid, grid.cellArr[(int)boxCell.X, (int)boxCell.Y], grid.cellArr[(int)target.X, (int)target.Y], null);

            box.Move(target, grid);
            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            grid.drawGrid(e);
            box.drawBox(e);
        }
    }

    public class Box
    {
        public Vector2 pos = new Vector2();
        Vector2 target = new Vector2();
        float boxSize;

        public Box(Vector2 pos, float boxSize)
        {
            this.pos = pos;
            this.target = pos;
            this.boxSize = boxSize;
        }

        public Vector2 Move(Vector2 target, Grid grid)
        {

            Vector2 targetVec = new Vector2();

            targetVec = grid.getCoordFromGrid(target, grid);
            if (targetVec ==  new Vector2(-1,-1))
            {

            }
            else
            {
                pos = targetVec;
            }
            return target;
        }

        public void drawBox(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Yellow);
            e.Graphics.DrawRectangle(pen, pos.X, pos.Y, boxSize, boxSize);
        }
    }
}