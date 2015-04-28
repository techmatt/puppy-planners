using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using game;

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
                if(s.Value.data.sessionName == sessionName)
                {
                    app.error("session already exists");
                    return "error: session already exists";
                }

            GameSession newSession = new GameSession(app, sessionName);

            if(sessions.ContainsKey(newSession.data.sessionID))
            {
                //
                // This should never happen; if it does, increase the length of the session ID.
                //
                app.error("session ID already exists");
                return "error: session ID already exists";
            }

            sessions.Add(newSession.data.sessionID, newSession);

            return newSession.data.sessionID;
        }

        string sessionList(Dictionary<string, string> parameters)
        {
            var sessionData = new List<GameSessionData>();
            foreach(var s in sessions.Values)
            {
                sessionData.Add(s.data);
            }

            return app.serializer.Serialize(sessionData);
        }

        public string dispatchCommand(string commandLine)
        {
            var parts = commandLine.Split('&');
            string command = parts[0];
            Dictionary<string,string> parameters = new Dictionary<string,string>();
            for(int partIndex = 1; partIndex < parts.Length; partIndex++)
            {
                var v = parts[partIndex].Split('=');
                parameters[v[0]] = v[1];
            }
            return dispatchCommand(command, parameters);
        }

        public string dispatchCommand(string command, Dictionary<string, string> parameters)
        {
            try
            {
                if (command == "reset") { sessions.Clear(); return ""; }
                if (command == "newSession") return newSession(parameters);
                if (command == "sessionList") return sessionList(parameters);

                if (parameters.ContainsKey("session"))
                {
                    string sessionID = parameters["session"];
                    if (!sessions.ContainsKey(sessionID))
                    {
                        app.error("session ID not found");
                        return "session ID not found";
                    }

                    GameSession session = sessions[sessionID];
                    return session.dispatchCommand(command, parameters);
                }

                app.error("unrecognized command: " + command + ", " + parameters.ToString());
                return "unknown command";
            }
            catch (GameException ex)
            {
                Console.Write("error: " + ex.ToString());
                //Debugger.Break();
                return "error: " + ex.ToString();
            }
        }
    }
}
