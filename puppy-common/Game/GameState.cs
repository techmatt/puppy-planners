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
        public List<Resource> resources = new List<Resource>();
    }

    public class GameStateSerializer
    {
        public GameStateSerializer(GameState state)
        {
            map = state.map;
            data = state.data;
            puppies = state.puppies;
        }
        public Map map;
        public Dictionary<string, Puppy> puppies;
        public GameStateData data;
    }

    public class GameState
    {
        public Random random = new Random();
        public Map map = new Map();
        public Database database = new Database();
        public GameStateData data = new GameStateData();

        // puppies are indexed by initials
        public Dictionary<string, Puppy> puppies = new Dictionary<string, Puppy>();
        
        public GameState()
        {
            for (int i = 0; i < 4; i++)
                addNewPuppy();
        }

        void addNewPuppy()
        {
            Puppy p = new Puppy(this);
            puppies[p.initials] = p;
        }

        void updatePuppyHomes()
        {
            var homelessPuppies = new List<Puppy>();

            foreach(MapCell c in map.mapAsList.Where(c => c.building != null))
            {
                var info = database.buildings[c.building.name];
                if(info.population > 0)
                {
                    foreach(Puppy p in puppies.Values.Where(p => p.homeLocation.Compare(c.coord)))
                    {

                    }
                }
            }

            //
            // check for homeless puppies
            //
            foreach(Puppy p in puppies.Values)
            {
                Building home = map.getCell(p.homeLocation).building;
                /*if(home == null || )
                    p.homeLocation.x = -1;
                if (!p.homeLocation.isValid())
                    homeless.Add(p);*/
            }


        }

        void updateResourceRates()
        {
            foreach (Resource r in data.resources)
            {
                r.productionPerSecond = 0.0;
                r.storage = 0.0;
            }

            foreach(MapCell c in map.mapAsList.Where(c => c.building != null))
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

        void processResourceDeficit(Resource r, double deficit)
        {

        }

        void processResourceSurplus(Resource r, double surplus)
        {

        }

        void processProduction()
        {
            foreach (Resource r in data.resources)
            {
                r.value += r.productionPerSecond / Constants.ticksPerSecond;
                if (r.value < 0.0) processResourceDeficit(r, -r.value);
                if (r.value > r.storage) processResourceSurplus(r, r.value - r.storage);
                r.value = Util.bound(r.value, 0.0, r.storage);
            }
        }

        public void tick()
        {
            if (data.paused)
                return;

            updateResourceRates();
            processProduction();
        }
    }
}
