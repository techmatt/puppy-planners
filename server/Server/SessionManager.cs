using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
                    return "error: session already exists";
                }

            GameSession newSession = new GameSession(app, sessionName);

            if(sessions.ContainsKey(newSession.id))
            {
                //
                // This should never happen; if it does, increase the length of the session ID.
                //
                app.error("session ID already exists");
                return "error: session ID already exists";
            }

            sessions.Add(newSession.id, newSession);

            return newSession.id;
        }

        string sessionList(Dictionary<string, string> parameters)
        {
            var sessionsSerialize = new List<GameSessionSerialize>();
            foreach(var sIn in sessions.Values)
            {
                var sOut = new GameSessionSerialize();
                sOut.sessionName = sIn.name;
                sOut.sessionID = sIn.id;
                sOut.playerNames = sIn.players;
                sessionsSerialize.Add(sOut);
            }
            
            return app.serializer.Serialize(sessionsSerialize);
        }

        public string dispatchCommand(string command, Dictionary<string, string> parameters)
        {
            if (command == "newSession") return newSession(parameters);
            if (command == "sessionList") return sessionList(parameters);
            
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
