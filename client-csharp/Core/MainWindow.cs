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
        HttpClient client = new HttpClient();
        const string serverUrl = "http://localhost:8080/puppies/";
        const string serverQueryUrl = "http://localhost:8080/puppies/p&";


        public string request(string query)
        {
            var responseString = client.GetStringAsync(serverQueryUrl + query);
            
            bool success = responseString.Wait(1000);
            if(!success)
            {
                Console.WriteLine("request timeout: ", query);
                return "error";
            }

            return responseString.Result;
        }

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
            string sessionListJSON = request("sessionList");
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
            request("newSession&sessionName=" + textBoxSessionName.Text);
            updateSessionList();
        }

        private void buttonJoinSession_Click(object sender, EventArgs e)
        {
            string sessionID = listBoxSessions.SelectedItem.ToString().SplitOnString(" ID=").Last();
            request("joinSession&session=" + sessionID + "&playerName=" + textBoxPlayerName.Text + "&role=" + comboBoxRole.SelectedItem.ToString());
            updateSessionList();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            comboBoxRole.SelectedIndex = 0;
        }
    }
}
