using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using game;

namespace server
{
    class GameSession
    {
        public AppState app;
        public GameState state = new GameState();

        //
        // This contains data that is separated out for serialization to the client
        //
        public GameSessionData data = new GameSessionData();
        
        public GameSession(AppState _app, string sessionName)
        {
            app = _app;
            data.sessionName = sessionName;

            var bytes = new byte[8];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            data.sessionID = BitConverter.ToString(bytes);
            data.sessionID = data.sessionID.ToLowerInvariant().Replace("-", "");
        }

        public string addPlayer(Dictionary<string, string> parameters)
        {
            string playerName = parameters["playerName"];
            string roleName = parameters["role"];
            PlayerData playerData = new PlayerData();
            playerData.name = playerName;
            if (Enum.TryParse(roleName, out playerData.role))
            {
                foreach(var p in data.players)
                {
                    if (p.name == playerData.name || p.role == playerData.role)
                    {
                        app.error("duplicate role or name");
                        return "error: name or role already exists in session";
                    }
                }
                data.players.Add(playerData);
                return "";
            }
            else
            {
                app.error("invalid role");
                return "error: invalid role";
            }
        }

        public string dispatchCommand(string command, Dictionary<string, string> parameters)
        {
            if (command == "joinSession") return addPlayer(parameters);
            if (command == "getMap") return state.map.toJSON(app.serializer);

            app.error("unrecognized command: " + command + ", " + parameters.ToString());
            return "unknown command";
        }
    }
}
