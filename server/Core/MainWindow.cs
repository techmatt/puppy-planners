using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace server
{
    public partial class MainWindow : Form
    {
        AppState app = new AppState();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLaunch_Click(object sender, EventArgs e)
        {
            app.server.launch();
            buttonLaunch.Enabled = false;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }
    }
}
