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

			foreach (TechnologyInfo t in Database.Get.techs.Values)
				techs [t.name] = new Technology (t);
		}

        public bool paused = false;
        public int tickCount = 0;
		public Dictionary<string, Resource> resources = new Dictionary<string, Resource>(); //Now a dictionary
		public Dictionary<string, Technology> techs = new Dictionary<string, Technology>();
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

	// I've split this class across multiple files, GameState.cs and GameStateTick.cs for readability
	// GameState.cs should have all the commands that modify the game state, and
	// GameStateTick.cs should have all the functions that occur every tick
    public partial class GameState
    {
        public Random random = new Random();
        public Map map;
        public GameStateData data = new GameStateData();
		public GameLog log = new GameLog ();

        // puppies are indexed by initials
        public Dictionary<string, Puppy> puppies = new Dictionary<string, Puppy>();
		public List<Puppy> deadPuppies = new List<Puppy>();


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

		public void movePuppy(Puppy p, MapCell c)
		{
			if (!c.explored)
				throw log.error(data.tickCount, "You can move puppies to explored tiles");
			if (p.workLocation.isValid())
				map.cellAtCoord (p.workLocation).removePuppy (p);

			p.moveToCell (c);

			if (c.coord.isValid())
				c.addPuppy (p);
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

        public void buildBuilding(string buildingName, MapCell c)
        {
            if(c.building != null)
                throw log.error(data.tickCount, "You must destroy the existing building first");
            
			BuildingInfo info = Database.Get.buildings[buildingName];
            
            //verify we have the needed technology
			if (info.tech!="none" && !data.techs[info.tech].researched)
				throw log.error(data.tickCount, "You do not have the required technology: " + data.techs[info.tech].info.displayName);

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

			c.building = new Building(info);
        }

		public void research(string researchName)
		{
			Technology tech = data.techs [researchName];
			TechnologyInfo info = tech.info;
			if (tech.researched)
				throw log.error(data.tickCount, info.displayName + " has already been researched.");

			// verify sufficient resources
			foreach(var r in info.cost)
				if (data.resources[r.resourceName].value < r.cost)
					throw log.error(data.tickCount, "You do not have enough " + r.resourceName + " to research " + info.displayName);
			
			// subtract resources
			foreach(var r in info.cost)
				data.resources[r.resourceName].value -= r.cost;

			// mark as researched
			tech.researched = true;
		}


    }
}
