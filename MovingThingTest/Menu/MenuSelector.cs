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
    public partial class MenuSelector : UserControl
    {
        public int j = 3;
        public List<menuStrip> menuTabs = new List<menuStrip>();
        public MenuSelector()
        {
            InitializeComponent();
            menuTabs.Add(new menuStrip("hello"));
        }
    }
}
