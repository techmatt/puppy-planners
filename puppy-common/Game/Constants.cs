using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Constants
    {
        //
        // Directories
        //
        public const string dataDir = "../../../data/";
        public const string imageDir = dataDir + "images/";
        public const string imageOutDir = dataDir + "imagesOut/";
        public const string fileHostDir = "../../../client-javascript/";

        //
        // Map
        //
        public const int mapSize = 14;
        public const int mapCellSize = 52;
        public static Coord invalidCoord = new Coord(-1, -1);
		public static DoubleCoord invalidDoubleCoord = new DoubleCoord(-1.0, -1.0);

        //
        // Game speed
        //
        public const double ticksPerSecond = 3.0;

        //
        // Base storage values
        //
        //public const double baseFoodStorage = 20.0;
        //public const double baseWoodStorage = 10.0;

        //
        // Debug
        //
        public const bool dumpImages = false;
        public const bool autoJoinSession = true;
        public const bool createDebugSession = true;
        public const bool echoServerRequests = false;




		// I'm going to hardcode in all the role data.  Presumably, this should end up in a csv at some point, but I'm lazy...

		public static Dictionary<string,Role> playerRoles = new Dictionary<string,Role>
		{
			{"unassigned",new Role("unassigned", "Unassigned",new List<string>())},
			{"military",new Role("military", "Military",new List<string> (new string[] {"soldier"}))},
			{"culture",new Role("culture", "Culture",new List<string> (new string[] {"preacher"}))},
			{"intrigue",new Role("intrigue", "Intrigue",new List<string> (new string[] {"spy"}))},
			{"builder",new Role("builder", "Builder",new List<string> (new string[] {"scout","builder"}))},
		};
    }

	public class Role // these will be defined for each player.  
	{
		public string name;
		public string displayName;	// the name that the UI uses
		public List<string> tasks;	// the tasks that the corresponding player can assign to them

		public Role (string _name, string _displayName, List<string> _tasks)
		{
			name = _name;
			displayName=_displayName;
			tasks = _tasks;
		}
	}
}
