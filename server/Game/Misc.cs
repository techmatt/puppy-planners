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

    struct GameSessionSerialize
    {
        public string sessionName, sessionID;
        public List<PlayerData> playerNames;
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
