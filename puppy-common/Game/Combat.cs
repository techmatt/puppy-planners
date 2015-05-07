using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace game
{
	public class Kitten
	{
		public CombatStats combat = new CombatStats();
		public string name;
		public string type;
		public string displayName;
		public DoubleCoord currentLocation;
		public DoubleCoord direction;

		public Kitten (string _type, double x, double y, double deltax, double deltay){
			combat = new CombatStats (); //TODO import stats from some file
			name = "";
			type = _type;

		}
	}

	public class CombatStats 
	{
		public double health = 1.0;
		public double armor = 0.0;
		public List<string> resistances = new List<string> ();
		public List<Attack> attacks = new List<Attack> (); 

		public CombatStats()
		{
		}
	}

	public class Attack
	{
		public double damage;
		public int cooldown;  // In ticks
		public int cooldownRemaining; // In ticks
		public int range;			// can attack anything strictly inside of range+1
		public string type;

		public Attack()
		{
		}

		public void decrementCooldown(int ticks)
		{
			cooldownRemaining = Math.Max (0, cooldownRemaining - ticks);
		}
	}

	public partial class GameState
	{
		// ADDITIONAL VARIABLES
		public List<Kitten> kittens = new List<Kitten>();


		// ADDITIONAL FUNCTIONS

		//DESTRUCTION
		public void killPuppy(Puppy p)
		{
			map.cellAtCoord (p.workLocation).employees.Remove (p.initials);
			puppies.Remove (p.name);
			deadPuppies.Add (p);
		}
		public void killKitten(Kitten k)
		{
			kittens.Remove (k);
			//TODO: loot
		}
		public void killBuildingInCell(MapCell m)
		{
			m.building=null;
		}


		//DAMAGE
		public void damagePuppy(Attack a, Puppy p)
		{
			double damage = a.damage - p.combat.armor;
			p.combat.health -= damage;
			if (p.combat.health < 0)
				killPuppy (p);
		}
		public void damageKitten(Attack a, Kitten k)
		{
			double damage = a.damage - k.combat.armor;
			k.combat.health -= damage;
			if (k.combat.health < 0)
				killKitten (k);
		}
		public void damageBuildingAtCell(Puppy p, Attack a, MapCell m)
		{
			// this mechanic is temporarily different, but we should make it consistant with the others
			p.combat.health-=a.damage; //kittens take damage from buildings
			m.building.health -= a.damage;

			if (m.building.health<0)
				killBuildingInCell(m);
		}



		// COMBAT FUNCTION FOR TICK
		public void decrementCooldowns()
		{
			foreach (Puppy p in puppies.Values)
				foreach (Attack a in p.combat.attacks)
					a.decrementCooldown (1);
			foreach (Kitten k in kittens)
				foreach (Attack a in k.combat.attacks)
					a.decrementCooldown (1);
		}


		// ATTACKING
		public Kitten closestKitten(Puppy p)
		{
			double x = p.currentLocation.x;
			double y = p.currentLocation.y;

			List<double> distances = kittens.Select (k => Math.Sqrt ((k.currentLocation.x - x) * (k.currentLocation.x - x) + (k.currentLocation.y - y) * (k.currentLocation.y - y))).ToList ();
			Kitten kitten = kittens[distances.ArgMin()];
			return kitten;
		}
		public Puppy closestPuppy(Kitten k)
		{
			double x = k.currentLocation.x;
			double y = k.currentLocation.y;

			List<Puppy> plist = puppies.Values.ToList ();

			List<double> distances = plist.Select (t => Math.Sqrt ((t.currentLocation.x - x) * (t.currentLocation.x - x) + (t.currentLocation.y - y) * (t.currentLocation.y - y))).ToList ();
			Puppy p = plist[distances.ArgMin()];
			return p;
		}


	}
}
