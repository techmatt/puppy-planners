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

        //
        // Game speed
        //
        public const double ticksPerSecond = 3.0;

        //
        // Base storage values
        //
        public const double baseFoodStorage = 20.0;
        public const double baseWoodStorage = 10.0;

        //
        // Debug
        //
        public const bool dumpImages = false;
        public const bool autoJoinSession = true;
        public const bool createDebugSession = true;
        public const bool echoServerRequests = false;
    }
}
