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

        public bool paused = false;
        public int tickCount = 0;
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
        public GameLog log = new GameLog();

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

        public void assignPuppyTask(Puppy p, MapCell c, string task)
        {

        }

        void assignPuppyHome(Puppy p, MapCell c)
        {
            map.removePuppyFromList(p, x => x.homePuppies);
            
            if (c.building == null || c.homePuppies.Count > database.buildings[c.building.name].residentCap)
            {
                log.error(data.tickCount, "assign to invalid building");
                return;
            }

            p.homeLocation = c.coord;
            c.homePuppies.Add(p.initials);
        }

        void assignPuppyChurch(Puppy p, MapCell c)
        {
            map.removePuppyFromList(p, x => x.churchPuppies);

            if (c.building == null || c.churchPuppies.Count > database.buildings[c.building.name].religionCap)
            {
                log.error(data.tickCount, "assign to invalid building");
                return;
            }

            p.churchLocation = c.coord;
            c.churchPuppies.Add(p.initials);
        }

        void assignPuppyCulture(Puppy p, MapCell c)
        {
            map.removePuppyFromList(p, x => x.culturePuppies);

            if (c.building == null || c.culturePuppies.Count > database.buildings[c.building.name].cultureCap)
            {
                log.error(data.tickCount, "assign to invalid building");
                return;
            }

            p.cultureLocation = c.coord;
            c.culturePuppies.Add(p.initials);
        }

        void assignPuppyWork(Puppy p, MapCell c)
        {
            map.removePuppyFromList(p, x => x.workPuppies);

            if (c.building == null || c.workPuppies.Count > database.buildings[c.building.name].workCap)
            {
                log.error(data.tickCount, "assign to invalid building");
                return;
            }

            p.workLocation = c.coord;
            c.workPuppies.Add(p.initials);
        }

        void updatePuppyBuildingBindings()
        {
            List<MapCell> openHomes = new List<MapCell>();
            List<MapCell> openCultures = new List<MapCell>();
            List<MapCell> openReligions = new List<MapCell>();

            //
            // check for under-populated buildings
            //
            foreach (MapCell c in map.cellsWithBuildings())
            {
                var info = database.buildings[c.building.name];
                
                int residenceVacancies = info.residentCap - c.homePuppies.Count;
                for (int i = 0; i < residenceVacancies; i++)
                    openHomes.Add(c);

                int cultureVacancies = info.cultureCap - c.culturePuppies.Count;
                for (int i = 0; i < cultureVacancies; i++)
                    openCultures.Add(c);

                int religionVacancies = info.religionCap - c.churchPuppies.Count;
                for (int i = 0; i < religionVacancies; i++)
                    openReligions.Add(c);
            }

            //
            // assign unbound puppies to random (TODO: make random) sites
            //
            foreach(Puppy p in puppies.Values)
            {
                if(openHomes.Count > 0 && !p.homeLocation.isValid())
                {
                    openHomes = openHomes.Shuffle();
                    assignPuppyHome(p, openHomes[0]);
                    openHomes.RemoveRange(0, 1);
                }
                if (openCultures.Count > 0 && !p.cultureLocation.isValid())
                {
                    openCultures = openCultures.Shuffle();
                    assignPuppyCulture(p, openCultures[0]);
                    openCultures.RemoveRange(0, 1);
                }
                if (openReligions.Count > 0 && !p.churchLocation.isValid())
                {
                    openReligions = openReligions.Shuffle();
                    assignPuppyChurch(p, openReligions[0]);
                    openReligions.RemoveRange(0, 1);
                }
                p.updateHappiness();
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

            updatePuppyBuildingBindings();

            data.tickCount++;
        }
    }
}
