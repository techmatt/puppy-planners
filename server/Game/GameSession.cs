using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace server
{
    class GameSession
    {
        public AppState app;
        public GameState state = new GameState();
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
        }

        public string dispatchCommand(string command, Dictionary<string, string> parameters)
        {
            if (command == "getMap") return state.map.toJSON();

            app.error("unrecognized command: " + command + ", " + parameters.ToString());
            return "unknown command";
        }
    }
}
