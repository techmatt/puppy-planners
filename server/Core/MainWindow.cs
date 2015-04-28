using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            //timerGameTick.Interval = (int)(1000 / Constants.ticksPerSecond);

            app.server.launch();
            buttonLaunch.Enabled = false;
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
            this.Left = 2700;

            //
            // automatically launch the server
            //
            launchServer();
        }

        private void serverTick()
        {
            if (app.sessionManager.sessions.Count == 0 && Constants.createDebugSession)
            {
                string sessionID = app.sessionManager.dispatchCommand("newSession&sessionName=DebugSession");
                app.sessionManager.dispatchCommand("joinSession&session=" + sessionID + "&playerName=DebugPlayer&role=Builder");
            }

            foreach (var s in app.sessionManager.sessions)
            {
                s.Value.state.tick();
            }
        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            Task t = Task.Run(() =>
            {
                serverTick();
                Thread.Sleep((int)(1000 / Constants.ticksPerSecond));
            });
        }
    }
}
