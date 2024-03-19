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
            MapMakerControl uc = new MapMakerControl();
            uc.AutoScroll = true;
            uc.Dock = DockStyle.Fill;
            mapPanel.Controls.Add(uc);
            uc.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
