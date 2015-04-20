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

			// Buildings should actually try to look themselves up.

            health = 1.0;
			constructionProgress = 0.0;
			constructed = false;

			maxBuilders = 1;
			maxResidents = 0;
			maxWorkers = 0;
			maxCulture = 0;
        }
        //
        // Buildings shouldn't store their position; generally you should pass around the MapCell object which encodes all terrain information.
        //

        public string name;
        public double health;
		public double constructionProgress;
		public bool constructed;

		public int maxBuilders;
		public int maxResidents;
		public int maxWorkers;
		public int maxCulture;



		public void construct(double additionalProgress)
		{
			constructionProgress+=additionalProgress;
			if (constructionProgress>=1.0)
				this.constructed=true;
		}




    }
}
