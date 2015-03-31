using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace game
{
    public class MapCellResource
    {
        public string name;
    }

    public class MapCell
    {
        public Coord coord;
        public string type;
        public List<MapCellResource> resources;
        public Building building;
        public double scoutCost;
        public bool explored;
    }

    public class Map
    {
        public Map()
        {
            Random random = new Random();

            for(int x = 0; x < data.GetUpperBound(0); x++)
                for(int y = 0; y < data.GetUpperBound(1); y++)
                {
                    MapCell c = new MapCell();
                    data[x, y] = c;
                    mapAsList.Add(c);
                    c.coord = new Coord(x, y);

                    //
                    // TODO: do actual terrain somehow
                    //
                    int terrainType = random.Next(2);
                    if (terrainType == 0)
                        c.type = "grass";
                    else
                        c.type = "dirt";
                }
            int d = Constants.mapSize / 2 - 1;
            data[d + 0, d + 0].building = new Building("field");
            data[d + 0, d + 1].building = new Building("hovel");
            data[d + 1, d + 0].building = new Building("hovel");
            data[d + 1, d + 1].building = new Building("field");
            scoutCell(new Coord(d + 0, d + 0), 1);
            scoutCell(new Coord(d + 0, d + 1), 1);
            scoutCell(new Coord(d + 1, d + 0), 1);
            scoutCell(new Coord(d + 1, d + 1), 1);
        }

        public void scoutCell(Coord c, int scoutRadius)
        {
            for(int xOffset = -scoutRadius; xOffset <= scoutRadius; xOffset++)
                for(int yOffset = -scoutRadius; yOffset <= scoutRadius; yOffset++)
                {
                    MapCell cell = data[c.x + xOffset, c.y + yOffset];
                    cell.explored = true;
                }
        }

        //
        // in theory, JSON for large state objects could be cached, but I'll only do that if it matters.
        //
        public string toJSON(JavaScriptSerializer serializer)
        {
            return serializer.Serialize(mapAsList);
        }

        public IEnumerable<MapCell> cellsWithBuildings()
        {
            return mapAsList.Where(m => m.building != null);
        }

        public MapCell[,] data = new MapCell[Constants.mapSize, Constants.mapSize];

        //
        // This is just a linear array of all elements in data; it is easier to serialize this.
        //
        public List<MapCell> mapAsList = new List<MapCell>();

        public MapCell getCell(Coord coord)
        {
            return data[coord.x, coord.y];
        }
    }
}
