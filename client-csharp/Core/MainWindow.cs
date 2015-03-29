using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Web.Script.Serialization;
using game;

namespace client_csharp
{
    public partial class MainWindow : Form
    {
        AppState app = new AppState();
        
        public MainWindow()
        {
            InitializeComponent();
            updateSessionButtonState();
        }

        void updateSessionButtonState()
        {
            buttonJoinSession.Enabled = (textBoxPlayerName.TextLength >= 3 && listBoxSessions.SelectedItem != null);
            buttonNewSession.Enabled = (textBoxSessionName.TextLength >= 3);
        }

        void updateSessionList()
        {
            string sessionListJSON = app.request("sessionList");
            if (sessionListJSON == "error") return;
            var sessionList = app.serializer.Deserialize<List<GameSessionData>>(sessionListJSON);

            listBoxSessions.Items.Clear();
            
            foreach(var session in sessionList)
            {
                listBoxSessions.Items.Add(session.ToString());
            }
            updateSessionButtonState();
        }

        private void textBoxPlayerName_TextChanged(object sender, EventArgs e)
        {
            updateSessionButtonState();
        }

        private void textBoxSessionName_TextChanged(object sender, EventArgs e)
        {
            updateSessionButtonState();
        }

        private void listBoxSessions_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSessionButtonState();
        }

        private void buttonUpdateSessionList_Click(object sender, EventArgs e)
        {
            updateSessionList();
        }

        private void buttonNewSession_Click(object sender, EventArgs e)
        {
            if(Constants.autoJoinSession)
                app.request("reset");

            app.request("newSession&sessionName=" + textBoxSessionName.Text);

            updateSessionList();

            if (Constants.autoJoinSession)
            {
                listBoxSessions.SelectedIndex = 0;
                joinSelectedSession();
            }
        }

        private void buttonJoinSession_Click(object sender, EventArgs e)
        {
            joinSelectedSession();
            
        }

        private void joinSelectedSession()
        {
            string sessionID = listBoxSessions.SelectedItem.ToString().SplitOnString(" ID=").Last();
            string result = app.request("joinSession&session=" + sessionID + "&playerName=" + textBoxPlayerName.Text + "&role=" + comboBoxRole.SelectedItem.ToString());
            if (result == "")
            {
                app.sessionID = sessionID;
            }
            updateSessionList();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            comboBoxRole.SelectedIndex = 0;
        }

        private void updateGameUI()
        {
            labelPaused.Text = app.gameData.paused ? "Game paused" : "Game running";

            app.updateMapBmp();
            pictureBoxMap.Image = app.mapBmp;

            StringBuilder resourceDesc = new StringBuilder();
            foreach(Resource r in app.gameData.resources)
            {
                resourceDesc.AppendLine(r.ToString());
            }

            textBoxResources.Text = resourceDesc.ToString();
        }

        private void timerGameUpdate_Tick(object sender, EventArgs e)
        {
            if (app.sessionID == null)
                return;

            string gameDataJSON = app.sessionRequest("getData");
            string gameMapJSON = app.sessionRequest("getMap");
            if (gameDataJSON == "" || gameMapJSON == "")
            {
                Console.Write("no response received");
                return;
            }

            app.gameData = app.serializer.Deserialize<GameStateData>(gameDataJSON);
            app.gameMap = app.serializer.Deserialize< List<MapCell> >(gameMapJSON);

            updateGameUI();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (app.gameData == null) return;

            app.sessionRequest("setPaused", "paused=" + (!app.gameData.paused).ToString());
        }
    }
}
