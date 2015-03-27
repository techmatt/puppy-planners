using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace game
{
    public struct MapCell
    {
        public Coord coord;
        public string type;
        public HashSet<string> resources;
        public Building building;
    }

    public class Map
    {
        public Map()
        {
            for(int x = 0; x < data.GetUpperBound(0); x++)
                for(int y = 0; y < data.GetUpperBound(1); y++)
                {
                    MapCell c = data[x, y];
                    mapAsList.Add(c);
                    c.coord = new Coord(x, y);
                    c.type = "water";
                }
        }

        //
        // in theory, JSON for large state objects could be cached, but I'll only do that if it matters.
        //
        public string toJSON(JavaScriptSerializer serializer)
        {
            return serializer.Serialize(mapAsList);
        }

        public MapCell[,] data = new MapCell[Constants.mapSize, Constants.mapSize];

        //
        // This is just a linear array of all elements in data; it is easier to serialize this.
        //
        public List<MapCell> mapAsList = new List<MapCell>();
    }
}
