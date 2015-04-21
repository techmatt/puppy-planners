using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Building
    {
        public Building(BuildingInfo b)
        {
			name = b.name;

            health = 1.0;
			constructionProgress = 0.0;
			constructed = false;

			info = b;

			maxBuilders = 1;
        }

		public void finish ()
		{
			this.constructed = true;
			this.maxBuilders = 0;

			//TODO: This should punt all the people currently working on the building.
		}


        //
        // Buildings shouldn't store their position; generally you should pass around the MapCell object which encodes all terrain information.
        //

        public string name;
        public double health;
		public double constructionProgress;
		public bool constructed;

		public int maxBuilders;
//		public int maxResidents;
//		public int maxWorkers;
//		public int maxCulture;
		public BuildingInfo info;


		public void construct(double additionalProgress)
		{
			constructionProgress+=additionalProgress;
			if (constructionProgress>=1.0)
				this.constructed=true;
		}




    }
}
