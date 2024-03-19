using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovingThingTest
{
    public partial class MapMaker : Form
    {
        public MapMaker()
        {
            InitializeComponent();
        }


        private void MapMaker_Load(object sender, EventArgs e)
        {
            //Form1 form1 = new Form1();
            //form1.TopLevel = false;
            //form1.AutoScroll = true;
            //form1.Dock = DockStyle.Fill;
            //panel1.Controls.Add(form1);
            //form1.Show();

            UserControl1 uc = new UserControl1();
            uc.AutoScroll= true;
            uc.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc);
            uc.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
