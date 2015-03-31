using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Building
    {
        public Building()
        {

        }
        public Building(string _name)
        {
            name = _name;
            health = 1.0;
        }
        //
        // Buildings shouldn't store their position; generally you should pass around the MapCell object which encodes all terrain information.
        //

        public string name;
        public double health;

        // all puppies are indexed by initials
        public List<string> workPuppies = new List<string>();
        public List<string> residentPuppies = new List<string>();
        public List<string> culturePuppies = new List<string>();
        public List<string> religionPuppies = new List<string>();
    }
}
