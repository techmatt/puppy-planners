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
			return "";
			// I'm going to cut the role checking: we want the server to be totally independent from the UI: it shouldn't care about who is issuing orders.

//            if (Enum.TryParse(roleName, out playerData.role))
//            {
//                foreach(var p in data.players)
//                {
//                    if (p.name == playerData.name || p.role == playerData.role)
//                    {
//                        app.error("duplicate role or name");
//                        return "error: name or role already exists in session";
//                    }
//                }
//                app.log(EventType.Server, "adding " + playerData.name + " as " + playerData.role.ToString());
//                data.players.Add(playerData);
//                return "";
//            }
//            else
//            {
//                app.error("invalid role");
//                return "error: invalid role";
//            }
        }

        public string dispatchCommand(string command, Dictionary<string, string> parameters)
        {
            if (command == "joinSession") return addPlayer(parameters);

            //
            // Query
            //
            if (command == "getData") return app.serializer.Serialize(state.data);
            if (command == "getMap") return state.map.toJSON(app.serializer);
            if (command == "getPuppies") return app.serializer.Serialize(state.puppies);
            if (command == "getAllState") return app.serializer.Serialize(new GameStateSerializer(state));

            //
            // Action
            //
			if (command == "setPaused")
				state.data.paused = Convert.ToBoolean (parameters ["paused"]);
			else if (command == "assignPuppyToRole")
				state.puppies [parameters ["puppy"]].assignRole (parameters ["role"]);
            else if (command == "assignPuppyTask")
                state.assignPuppyTask(state.puppies[parameters["puppy"]], state.map.data[Convert.ToInt32(parameters["x"]), Convert.ToInt32(parameters["y"])], parameters["task"]);
			else if (command == "assignPuppyToTask")
				state.puppies [parameters ["puppy"]].assignTask (parameters ["role"],parameters ["task"]);
            else if (command == "buildBuilding")
                state.buildBuilding(parameters["name"], state.map.data[Convert.ToInt32(parameters["x"]), Convert.ToInt32(parameters["y"])]);
            else
            {
                app.error("unrecognized command: " + command);
                return "error: unknown command";
            }


            return "";
        }
    }
}
