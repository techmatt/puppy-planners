using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    struct PlayerData
    {
        public string name;
        public Role role;
    }

    class GameSessionData
    {
        public string sessionName, sessionID;
        public List<PlayerData> players = new List<PlayerData>();
    }

    public enum Role
    {
        Builder,
        Culture,
        Intrigue,
        Military
    }

    class Misc
    {
        
    }
}
