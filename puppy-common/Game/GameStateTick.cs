using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// only for debugging purposes
using System.Diagnostics;


namespace game
{

	public partial class GameState
	{
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
				// locations now need employees to produce stuff.
				foreach (Puppy p in c.employees.Select(i=>puppies[i]))
				{
					BuildingInfo info = Database.Get.buildings [c.building.name];
					foreach (BuildingResourceProduction production in info.production) {
						Resource r = data.resources [production.resourceName];
						r.productionPerSecond += production.productionPerSecond;
					}
					foreach (BuildingResourceStorage storage in info.storage) {
						Resource r = data.resources [storage.resourceName];
						r.storage += storage.storage;
					}
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
				foreach(Puppy p in m.puppies.Select(x => puppies[x]).Where(p=>!p.currentlyMoving&&p.task=="scout"))
				{
					double scoutValue = p.skillEffectiveness("Explorer");
					map.scoutCell(m.coord, 1, scoutValue);
				}
			}
		}

		void processConstruction()
		{
			foreach (MapCell c in map.mapAsList.Where(c => c.building!=null && !c.building.constructed))
			{
				foreach (Puppy p in c.puppies.Select(x => puppies[x]).Where(p=>!p.currentlyMoving&&p.task=="worker"&&!p.isEmployee))
				{
					c.building.constructBuilding (p.skillEffectiveness ("Builder"));
				}
			}
		}


		void movePuppies()
		{
			foreach(Puppy p in puppies.Values)
			{
				p.destination = p.workLocation.toDouble ();
				//p.updateDestination();
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

		public void assignEmployees ()
		{
			foreach (Puppy p in puppies.Values)
				p.isEmployee = false;
			foreach (MapCell m in map.mapAsList.Where(c => c.building != null && c.building.constructed))
			{
				List<Puppy> eligiblePuppies = m.puppies.Select (i => puppies[i]).Where (p => p.task == m.building.info.employeeTask && !p.isEmployee).ToList();
				List<Puppy> employeePuppies = eligiblePuppies.Take (Math.Max (eligiblePuppies.Count, m.building.info.employeeCapacity)).ToList();
				m.employees = employeePuppies.Select(p=>p.name).ToList();

				foreach(Puppy p in employeePuppies) {
					p.isEmployee = true;
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
			assignEmployees ();		// req: movePuppies
			updateResourceRates();	// req: assignEmployees
			processConstruction();	//req: assignEmployees
			processProduction();	//req: assignEmployeees

			decrementCooldowns ();

			updatePuppyBuildingBindings();
			processScouting();

			foreach (Puppy p in puppies.Values)
				p.updateHappiness();

			data.tickCount++;
			//stopwatch.Stop();
			//var elapsed = stopwatch.ElapsedMilliseconds;
			//Console.WriteLine ("time for "+Convert.ToString(data.tickCount)+":  " + Convert.ToString (elapsed)+" ms");
		}
	}
}
