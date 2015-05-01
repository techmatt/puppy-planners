using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// only for debugging purposes
using System.Diagnostics;


namespace game
{
    //
    // Class members factored out for serialization
    //
    public class GameStateData
    {
		public GameStateData()
		{
			foreach (ResourceInfo r in Database.Get.resources.Values)
				resources [r.name] = new Resource (r);
				//resources.Add (new Resource (r));
		}

        public bool paused = false;
        public int tickCount = 0;
		public Dictionary<string, Resource> resources = new Dictionary<string, Resource>(); //Now a dictionary
    }

    public class GameStateSerializer
    {
        public GameStateSerializer(GameState state)
        {
            map = state.map;
            data = state.data;
            puppies = state.puppies;
			log = state.log;
        }
        public Map map;
        public Dictionary<string, Puppy> puppies;
        public GameStateData data;
		public GameLog log;
		public Database database = Database.Get;
    }

    public class GameState
    {
        public Random random = new Random();
        public Map map;
        public GameStateData data = new GameStateData();
		public GameLog log = new GameLog ();

        // puppies are indexed by initials
        public Dictionary<string, Puppy> puppies = new Dictionary<string, Puppy>();
        
        public GameState()
        {
            for (int i = 0; i < 4; i++)
                addNewPuppy();

			map = new Map ();
        }

        void addNewPuppy()
        {
            Puppy p = new Puppy(this);
            puppies[p.initials] = p;
        }

        public void assignPuppyTask(Puppy p, MapCell c, string task)
        {
            //
            // task = production, church, culture, home, scout, military, construction
            //
            if (task == "production") assignPuppyProduction(p, c);
            else if (task == "church") assignPuppyChurch(p, c);
            else if (task == "culture") assignPuppyCulture(p, c);
            else if (task == "home") assignPuppyHome(p, c);
            else if (task == "scout") assignPuppyScout(p, c);
            else if (task == "military") assignPuppyMilitary(p, c);
            else if (task == "construction") assignPuppyConstruction(p, c);
        }

        void assignPuppyProduction(Puppy p, MapCell c)
        {
            map.removePuppyFromAllWorkLists(p);
            p.workLocation = Constants.invalidCoord;

            if (c.building == null)
                throw log.error(data.tickCount, "assign production with no building");
			else if (c.productionPuppies.Count >= Database.Get.buildings[c.building.name].workCap)
                throw log.error(data.tickCount, "assign production to over-capacity building: " + c.building.name);
            
			p.assignedPlayer = "builder";
			p.tasks["builder"] = "builder";
            p.workLocation = c.coord;
            c.productionPuppies.Add(p.initials);
        }

        void assignPuppyScout(Puppy p, MapCell c)
        {
            map.removePuppyFromAllWorkLists(p);
            p.workLocation = Constants.invalidCoord;

            if (c.scoutPuppies.Count >= 1)
                throw log.error(data.tickCount, "You cannot assign multiple scouts to the same tile");
            else if (!c.explored)
                throw log.error(data.tickCount, "You can only assign scouts to explored tiles");

			p.assignedPlayer = "builder";
			p.tasks["builder"] = "scout";
            p.workLocation = c.coord;
            c.scoutPuppies.Add(p.initials);
        }

        void assignPuppyMilitary(Puppy p, MapCell c)
        {
            map.removePuppyFromAllWorkLists(p);
            p.workLocation = Constants.invalidCoord;

            if (c.militaryPuppies.Count >= 1)
                throw log.error(data.tickCount, "assign to occupied military location");

			p.assignedPlayer = "military";
            p.tasks["military"] = "soldier";
            p.workLocation = c.coord;
            c.militaryPuppies.Add(p.initials);
        }

        void assignPuppyConstruction(Puppy p, MapCell c)
        {
            map.removePuppyFromAllWorkLists(p);
            p.workLocation = Constants.invalidCoord;

            if (c.building == null || c.constructionPuppies.Count >= 1)
                throw log.error(data.tickCount, "assign to invalid building");

			p.assignedPlayer = "builder";
            p.tasks["builder"] = "builder";
            p.workLocation = c.coord;
            c.constructionPuppies.Add(p.initials);
        }

        void assignPuppyHome(Puppy p, MapCell c)
        {
            map.removePuppyFromList(p, x => x.homePuppies);
            p.homeLocation = Constants.invalidCoord;
            
			if (c.building == null || c.homePuppies.Count >= Database.Get.buildings[c.building.name].residentCap)
                throw log.error(data.tickCount, "assign to invalid building");

            p.homeLocation = c.coord;
            c.homePuppies.Add(p.initials);
        }

        void assignPuppyChurch(Puppy p, MapCell c)
        {
            map.removePuppyFromList(p, x => x.churchPuppies);
            p.churchLocation = Constants.invalidCoord;

			if (c.building == null || c.churchPuppies.Count >= Database.Get.buildings[c.building.name].religionCap)
                throw log.error(data.tickCount, "assign to invalid building");

            p.churchLocation = c.coord;
            c.churchPuppies.Add(p.initials);
        }

        void assignPuppyCulture(Puppy p, MapCell c)
        {
            map.removePuppyFromList(p, x => x.culturePuppies);
            p.cultureLocation = Constants.invalidCoord;

			if (c.building == null || c.culturePuppies.Count >= Database.Get.buildings[c.building.name].cultureCap)
                throw log.error(data.tickCount, "assign to invalid building");

            p.cultureLocation = c.coord;
            c.culturePuppies.Add(p.initials);
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
				var info = Database.Get.buildings[c.building.name];
                
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
            }
        }

        void updateResourceRates()
        {
			foreach (Resource r in data.resources.Values)
            {
                r.productionPerSecond = 0.0;
				r.storage = Database.Get.resources[r.name].baseStorage;
            }

            foreach(MapCell c in map.mapAsList.Where(c => c.building != null))
            {
				BuildingInfo info = Database.Get.buildings[c.building.name];
                foreach(BuildingResourceProduction production in info.production)
                {
					Resource r = data.resources[production.resourceName];
                    r.productionPerSecond += production.productionPerSecond;
                }
                foreach (BuildingResourceStorage storage in info.storage)
                {
					Resource r = data.resources[storage.resourceName];
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
            foreach (Resource r in data.resources.Values)
            {
                r.value += r.productionPerSecond / Constants.ticksPerSecond;
                if (r.value < 0.0) processResourceDeficit(r, -r.value);
                if (r.value > r.storage) processResourceSurplus(r, r.value - r.storage);
                r.value = Util.bound(r.value, 0.0, r.storage);
            }
        }

        void processScouting()
        {
            foreach(MapCell m in map.mapAsList)
            {
				foreach(Puppy p in m.scoutPuppies.Select(x => puppies[x]).Where(p=>!p.currentlyMoving))
                {
                    double scoutValue = p.skillEffectiveness("Explorer");
                    map.scoutCell(m.coord, 1, scoutValue);
                }
            }
        }

        public void buildBuilding(string buildingName, MapCell c)
        {
            if(c.building != null)
                throw log.error(data.tickCount, "You must destroy the existing building first");
            
			BuildingInfo info = Database.Get.buildings[buildingName];
            
            //TODO: verify we have the needed technology

            //
            // verify we have enough resources to build the building
            //
            foreach(var r in info.cost)
				if (data.resources[r.resourceName].value < r.cost)
                    throw log.error(data.tickCount, "You do not have enough " + r.resourceName + " to build a " + buildingName);

            //
            // subtract the needed resources
            //
            foreach(var r in info.cost)
				data.resources[r.resourceName].value -= r.cost;

			c.building = new Building(Database.Get.buildings[buildingName]);
        }
			
		void processConstruction()
		{
			foreach (MapCell c in map.mapAsList.Where(c => !(c.building==null) && !c.building.constructed))
			{
				foreach (String p in c.constructionPuppies.Where(p=>!puppies[p].currentlyMoving))
				{
					c.building.constructBuilding (puppies [p].skillEffectiveness ("Builder"));
				}
			}
		}


		void movePuppies()
		{
			foreach(Puppy p in puppies.Values)
			{
				p.updateDestination();
				double distance = Math.Sqrt(
					(p.destination.x - p.currentLocation.x) * (p.destination.x - p.currentLocation.x)
					+ (p.destination.y - p.currentLocation.y) * (p.destination.y - p.currentLocation.y));
				if (distance < p.movementRate) {
					//Arrives at destination or is already there
					p.currentLocation = p.destination;
					p.currentlyMoving = false;
				} else {
					// move a little bit closer.
					p.currentLocation.x+=p.movementRate*(p.destination.x - p.currentLocation.x)/distance;
					p.currentLocation.y+=p.movementRate*(p.destination.y - p.currentLocation.y)/distance;
					p.currentlyMoving = true;
				}
			}
		}

        public void tick()
        {
			if (data.paused)
				return;
            
			Console.WriteLine ("Starting tick " + Convert.ToString (data.tickCount));
			//			var stopwatch = new Stopwatch();

			//			stopwatch.Start();
			movePuppies();
			updateResourceRates();
			processConstruction();
			processProduction();
			updatePuppyBuildingBindings();
            updateResourceRates();
            processProduction();
            processScouting();

            updatePuppyBuildingBindings();

            foreach (Puppy p in puppies.Values)
                p.updateHappiness();

			data.tickCount++;
			//stopwatch.Stop();
			//var elapsed = stopwatch.ElapsedMilliseconds;
			//Console.WriteLine ("time for "+Convert.ToString(data.tickCount)+":  " + Convert.ToString (elapsed)+" ms");
        }
    }
}
