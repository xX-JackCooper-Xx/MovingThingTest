﻿using System;
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
        Thread th;
        public Main()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(openMapMaker);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void openMapMaker()
        {
            Application.Run(new MapMaker());
        }

        private void openPlayerForm()
        {
            Application.Run(new PlayerForm());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(openPlayerForm);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
