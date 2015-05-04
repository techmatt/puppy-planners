using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
	public class Technology
	{
		public TechnologyInfo info;
		public bool researched = false;
		public string name;
		public string role;

		public Technology(TechnologyInfo t)
		{
			info = t;
			name = t.name;
			researched = false;
		}

			
	}
}