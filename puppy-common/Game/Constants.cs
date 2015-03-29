using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Constants
    {
        public static string dataDir = "../../../data/";
        public static string imageDir = dataDir + "images/";
        public static string imageOutDir = dataDir + "imagesOut/";
        public static int mapSize = 14;
        public static int mapCellSize = 48;
        public static double ticksPerSecond = 3.0;
    }
}
