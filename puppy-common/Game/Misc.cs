using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Resource
    {
        public Resource()
        {

        }
        public Resource(string resourceName)
        {
            name = resourceName;
        }
        public override string ToString()
        {
            string sign = productionPerSecond >= 0 ? "+" : "-";

            string valueString = value.ToString("#.##");
            if (valueString.Length == 0 || valueString[0] == '.') valueString = "0" + valueString;

            return name + ": " + valueString + " / " + ((int)storage).ToString() + " (" + sign + productionPerSecond.ToString("#.##") + "/sec)";
        }
        public string name;
        public double value;
        public double storage;
        public double productionPerSecond;
    }

    public struct PlayerData
    {
        public string name;
        public Role role;

        public override string ToString()
        {
            return name + "=" + role.ToString();
        }
    }

    public class GameSessionData
    {
        public string sessionName, sessionID;
        public List<PlayerData> players = new List<PlayerData>();

        public override string ToString()
        {
            string playerList = "";
            foreach(PlayerData p in players)
            {
                playerList += p.ToString() + ", ";
            }
            if(playerList.Length <= 2)
            {
                playerList = "no players";
            }
            else
            {
                playerList.Substring(0, playerList.Length - 2);
            }
            return sessionName + ": " + playerList + " ID=" + sessionID;
        }
    }

    public enum Role
    {
        Builder,
        Culture,
        Intrigue,
        Military
    }

    public class Misc
    {
        
    }
}
