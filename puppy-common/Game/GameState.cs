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
        public Resource getResource(string name)
        {
            foreach (Resource r in resources)
                if (r.name == name) return r;
            resources.Add(new Resource(name));
            return resources.Last();
        }

        public bool paused = true;
        public List<Resource> resources;
    }

    public class GameState
    {
        public Map map = new Map();
        public Database database = new Database();
        public GameStateData data = new GameStateData();

        void updateResourceRates()
        {
            foreach (Resource r in data.resources)
            {
                r.productionPerSecond = 0.0;
                r.storage = 0.0;
            }


            foreach(MapCell c in map.mapAsList)
            {
                BuildingInfo info = database.buildings[c.building.name];
                foreach(BuildingResourceProduction production in info.production)
                {
                    Resource r = data.getResource(production.resourceName);
                    r.productionPerSecond += production.productionPerSecond;
                }
                foreach (BuildingResourceStorage storage in info.storage)
                {
                    Resource r = data.getResource(storage.resourceName);
                    r.storage += storage.storage;
                }
            }
        }

        void processProduction()
        {
            foreach (Resource r in data.resources)
            {
                r.value += r.productionPerSecond / Constants.ticksPerSecond;
                r.value = Util.bound(r.value, 0.0, r.storage);
            }
        }

        void tick()
        {
            updateResourceRates();
            processProduction();
        }
    }
}
