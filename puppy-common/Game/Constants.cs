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
        public static string dataDir = "../../../data/";
        public static string imageDir = dataDir + "images/";
        public static string imageOutDir = dataDir + "imagesOut/";
        public static string fileHostDir = "../../../client-javascript/";

        //
        // Map
        //
        public static int mapSize = 14;
        public static int mapCellSize = 48;

        //
        // Game speed
        //
        public static double ticksPerSecond = 3.0;

        //
        // Base storage values
        //
        public static double baseFoodStorage = 20.0;
        public static double baseWoodStorage = 10.0;

        //
        // Debug
        //
        public static bool dumpImages = false;
        public static bool autoJoinSession = true;
    }
}
