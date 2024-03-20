using MovingThingTest.Menu;
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

        public dynamic item;
        MenuSelector ms = new MenuSelector();
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

            
            ms.AutoScroll = true;
            ms.Dock = DockStyle.Fill;
            controlsPanel.Controls.Add(ms);
            ms.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void controlsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            item = ms.selectedItem;
        }
    }
}
