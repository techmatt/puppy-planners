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
        public Dictionary<string, GameSession> sessions = new Dictionary<string, GameSession>();

        string newSession(Dictionary<string, string> parameters)
        {
            string sessionName = parameters["sessionName"];
            app.log(EventType.Server, "creating new session: " + sessionName);

            foreach(var s in sessions)
                if(s.Value.name == sessionName)
                {
                    app.error("session already exists");
                    return "session already exists";
                }

            GameSession newSession = new GameSession(app, sessionName);

            if(sessions.ContainsKey(newSession.id))
            {
                //
                // This should never happen; if it does, increase the length of the session ID.
                //
                app.error("session ID already exists");
                return "session ID already exists";
            }

            sessions.Add(newSession.id, newSession);

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

            if(parameters.ContainsKey("session"))
            {
                string sessionID = parameters["session"];
                if(!sessions.ContainsKey(sessionID))
                {
                    app.error("session ID not found");
                    return "session ID not found";
                }

                GameSession session = sessions[sessionID];
                session.dispatchCommand(command, parameters);
            }

            app.error("unrecognized command: " + command + ", " + parameters.ToString());
            return "unknown command";
        }
    }
}
