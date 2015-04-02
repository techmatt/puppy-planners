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
                app.sessionRequest("setPaused", "paused=false");
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
            app.suppressRequests = true;

            labelPaused.Text = app.gameData.paused ? "Game paused" : "Game running";

            app.updateMapBmp();
            pictureBoxMap.Image = app.mapBmp;

            StringBuilder resourceDesc = new StringBuilder();
            foreach(Resource r in app.gameData.resources)
            {
                resourceDesc.AppendLine(r.ToString());
            }

            textBoxResources.Text = resourceDesc.ToString();

            bool puppyListDirty = (listBoxPuppies.Items.Count != app.puppies.Count);
            if(!puppyListDirty)
            {
                int puppyIndex = 0;
                foreach(Puppy p in app.puppies.Values)
                {
                    string listItem = listBoxPuppies.Items[puppyIndex++].ToString();
                    string desc = p.initials + " (" + p.assignedPlayer + "): " + p.task;
                    if (listItem != desc)
                        puppyListDirty = true;
                }
            }

            if(puppyListDirty)
            {
                listBoxPuppies.Items.Clear();
                foreach(Puppy p in app.puppies.Values)
                {
                    string desc = p.initials + " (" + p.assignedPlayer + "): " + p.task;
                    listBoxPuppies.Items.Add(desc);
                }
            }

            app.suppressRequests = false;
        }

        private void timerGameUpdate_Tick(object sender, EventArgs e)
        {
            if (app.sessionID == null)
                return;

            string gameDataJSON = app.sessionRequest("getData");
            string gameMapJSON = app.sessionRequest("getMap");
            string puppiesJSON = app.sessionRequest("getPuppies");
            string allStateJSON = app.sessionRequest("getAllState");
            if (gameDataJSON == "" || gameMapJSON == "")
            {
                Console.Write("no response received");
                return;
            }

            app.gameData = app.serializer.Deserialize<GameStateData>(gameDataJSON);
            app.gameMap = app.serializer.Deserialize< List<MapCell> >(gameMapJSON);
            app.puppies = app.serializer.Deserialize<Dictionary<string, Puppy>>(puppiesJSON);

            updateGameUI();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (app.gameData == null) return;

            app.sessionRequest("setPaused", "paused=" + (!app.gameData.paused).ToString());
        }

        private string getSelectedPuppyInitials()
        {
            if (listBoxPuppies.SelectedItem == null)
                return "";
            
            string item = listBoxPuppies.SelectedItem.ToString();
            if (item.Length >= 2)
            {
                string initials = item[0].ToString() + item[1].ToString();
                if (app.puppies.ContainsKey(initials))
                    return initials;
            }
            return "";
        }

        private void listBoxPuppies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (getSelectedPuppyInitials().Length > 0)
            {
                Puppy p = app.puppies[getSelectedPuppyInitials()];
                textBoxPuppyData.Text = p.describe();

                app.suppressRequests = true;
                for (int i = 0; i < comboBoxPuppyAssignment.Items.Count; i++ )
                    if (comboBoxPuppyAssignment.Items[i].ToString() == p.assignedPlayer)
                        comboBoxPuppyAssignment.SelectedIndex = i;
                app.suppressRequests = false;
            }
        }

        private void comboBoxPuppyAssignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(getSelectedPuppyInitials().Length > 0)
                app.sessionRequest("assignPuppyToRole", "puppy=" + getSelectedPuppyInitials() + "&role=" + comboBoxPuppyAssignment.SelectedItem.ToString());
        }

        private void pictureBoxMap_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                foreach (var v in app.puppyMapLocations)
                {
                    if (v.Value.Contains(e.X, e.Y))
                    {
                        int selectedIndex = 0;
                        foreach (var i in listBoxPuppies.Items)
                        {
                            if (i.ToString().StartsWith(v.Key.initials))
                            {
                                listBoxPuppies.SelectedIndex = selectedIndex;
                                break;
                            }
                            selectedIndex++;
                        }
                    }
                }
            }
            if(e.Button == MouseButtons.Right)
            {
                foreach(var v in app.cellMapLocations)
                {
                    if(v.Value.Contains(e.X, e.Y))
                    {
                        app.sessionRequest("assignPuppyToRole", "puppy=" + getSelectedPuppyInitials() + "&role=" + comboBoxPuppyAssignment.SelectedItem.ToString());
                    }
                }
            }
            
        }

        private void pictureBoxMap_Click(object sender, EventArgs e)
        {

        }
    }
}
