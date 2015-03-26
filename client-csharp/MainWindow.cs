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

namespace client_csharp
{
    public partial class MainWindow : Form
    {
        HttpClient client = new HttpClient();
        const string serverUrl = "http://localhost:8080/puppies/";
        const string serverQueryUrl = "http://localhost:8080/puppies/p?";


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
        }

        public void updateSessionButtonState()
        {
            buttonJoinSession.Enabled = (textBoxPlayerName.TextLength >= 3);
            buttonNewSession.Enabled = (textBoxSessionName.TextLength >= 3);
        }

        private void textBoxPlayerName_TextChanged(object sender, EventArgs e)
        {
            updateSessionButtonState();
        }

        private void textBoxSessionName_TextChanged(object sender, EventArgs e)
        {
            updateSessionButtonState();
        }

        private void buttonUpdateSessionList_Click(object sender, EventArgs e)
        {
            string list = request("sessionList");
            int a = 5;
        }
    }
}
