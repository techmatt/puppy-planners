using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    //
    // Class members factored out for serialization
    //
    public class GameStateData
    {
        public bool paused = true;
        public List<Resource> resources;
    }

    public class GameState
    {
        public Map map = new Map();
        public GameStateData data = new GameStateData();
    }
}
