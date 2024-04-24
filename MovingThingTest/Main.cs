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
    public partial class Main : Form
    {
        Thread th; // Thread for opening new forms
        int width; // Width of the map
        int height; // Height of the map
        int squadSize; // Size of the player squad

        // Constructor for the Main form
        public Main()
        {
            Focus(); // Set focus to the form
            InitializeComponent(); // Initialize components of the form
        }

        // Event handler for the "New Map" button click
        private void newMapBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse width and height from text boxes
                width = Convert.ToInt16(widthBox.Text);
                height = Convert.ToInt16(heightBox.Text);

                // Check if width and height are greater than 2
                if (width <= 2 || height <= 2)
                {
                    MessageBox.Show("PLEASE ENTER HEIGHT AND WIDTHS GREATER THAN 2");
                }
                else
                {
                    // Close the current form and open a new MapMaker form in a new thread
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

        // Method to open the MapMaker form
        private void openMapMaker()
        {
            Application.Run(new MapMaker(width, height)); // Run the MapMaker form
        }

        // Method to open the PlayerForm form
        private void openPlayerForm()
        {
            Application.Run(new PlayerForm(squadSize)); // Run the PlayerForm form
        }

        // Event handler for the "Play" button click
        private void playBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse squad size from text box
                squadSize = Convert.ToInt16(squadBox.Text);

                // Check if squad size is greater than 0
                if (squadSize < 1)
                {
                    MessageBox.Show("PLEASE ENTER SQUAD SIZE GREATER THAN 0");
                }
                else
                {
                    // Close the current form and open a new PlayerForm form in a new thread
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
