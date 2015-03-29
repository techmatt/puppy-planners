using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using game;

namespace server
{
    public partial class MainWindow : Form
    {
        AppState app = new AppState();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void launchServer()
        {
            timerGameTick.Interval = (int)(1000 / Constants.ticksPerSecond);

            app.server.launch();
            buttonLaunch.Enabled = false;

            if (Constants.createDebugSession)
            {
                string sessionID = app.sessionManager.dispatchCommand("newSession&sessionName=DebugSession");
                app.sessionManager.dispatchCommand("joinSession&session=" + sessionID + "&playerName=DebugPlayer&role=Builder");
            }
        }

        private void buttonLaunch_Click(object sender, EventArgs e)
        {
            launchServer();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //
            // this is for my dual-monitor setup so it always shows up on the second monitor.
            // it will probably make the window invisible on other setups.
            //
            // this.Left = 2700;

            //
            // automatically launch the server
            //
            launchServer();
        }

        private void timerGameTick_Tick(object sender, EventArgs e)
        {
            foreach(var s in app.sessionManager.sessions)
            {
                s.Value.state.tick();
            }
        }
    }
}
