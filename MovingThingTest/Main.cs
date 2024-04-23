using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Playback;

namespace MovingThingTest
{
    public partial class Main : Form
    {
        Thread th;
        int width;
        int height;
        int squadSize;
        public Main()
        {
            Focus();
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void newMapBtn_Click(object sender, EventArgs e)
        {
            try
            {
                width = Convert.ToInt16(widthBox.Text);
                height = Convert.ToInt16(heightBox.Text);

                if (width <= 2 || height <= 2)
                {
                    MessageBox.Show("PLEASE ENTER HEIGHT AND WIDTHS GREATER THAN 2");
                }
                else
                {
                    this.Close();
                    th = new Thread(openMapMaker);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                }
            }
            catch
            {
                MessageBox.Show("PLEASE ENTER INTEGER VALUE FOR WIDTH AND HEIGHT");
            }
        }

        private void openMapMaker()
        {

            Application.Run(new MapMaker(width, height));
        }

        private void openPlayerForm()
        {
            Application.Run(new PlayerForm(squadSize));
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            try
            {
                squadSize = Convert.ToInt16(squadBox.Text);

                if (squadSize < 1)
                {
                    MessageBox.Show("PLEASE ENTER SQUAD SIZE GREATER THAN 0");
                }
                else
                {
                    this.Close();
                    th = new Thread(openPlayerForm);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                }
            }
            catch
            {
                MessageBox.Show("PLEASE ENTER INTEGER VALUE FOR SQUAD SIZE");
            }


        }
    }
}
