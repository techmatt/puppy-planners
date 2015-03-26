using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class SessionManager
    {
        public AppState app;
        public List<GameSession> sessions = new List<GameSession>();

        string newSession(Dictionary<string, string> parameters)
        {
            return "";
        }

        string sessionList(Dictionary<string, string> parameters)
        {
            return "";
        }

        string joinSession(Dictionary<string, string> parameters)
        {
            return "";
        }

        public string dispatchCommand(string command, Dictionary<string, string> parameters)
        {
            if (command == "newSession") return newSession(parameters);
            if (command == "sessionList") return sessionList(parameters);
            if (command == "newSession") return joinSession(parameters);
            return "unknown command";
        }
    }
}
