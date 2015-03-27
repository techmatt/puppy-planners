using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

namespace server
{
    class GameSession
    {
        public AppState app;
        public GameState state = new GameState();
        public List<PlayerData> players = new List<PlayerData>();
        public string name;
        public string id;

        public GameSession(AppState _app, string sessionName)
        {
            app = _app;
            name = sessionName;

            var bytes = new byte[8];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            id = BitConverter.ToString(bytes);
            id = id.ToLowerInvariant().Replace("-","");
        }

        public string addPlayer(Dictionary<string, string> parameters)
        {
            string playerName = parameters["playerName"];
            string roleName = parameters["roleName"];
            PlayerData data = new PlayerData();
            data.name = name;
            if(Enum.TryParse(roleName, out data.role))
            {
                foreach(var p in players)
                {
                    if(p.name == data.name || p.role == data.role)
                    {
                        app.error("duplicate role or name");
                        return "name or role already exists in session";
                    }
                }
                players.Add(data);
                return "";
            }
            else
            {
                app.error("invalid role");
                return "invalid role";
            }
        }

        public string dispatchCommand(string command, Dictionary<string, string> parameters)
        {
            if (command == "joinSession") return addPlayer(parameters);
            if (command == "getMap") return state.map.toJSON();

            app.error("unrecognized command: " + command + ", " + parameters.ToString());
            return "unknown command";
        }
    }
}
