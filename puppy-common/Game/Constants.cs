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
        // Debug
        //
        public static bool dumpImages = false;
    }
}
