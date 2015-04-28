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

        //
        // This is a list of all the things puppies might be doing at this cell.
        // Many only make sense in the context of buildings, but for parallel
        // structure, it is cleanest if everything lives in MapCell.
        //
        public List<string> scoutPuppies = new List<string>();
        public List<string> militaryPuppies = new List<string>();
        public List<string> constructionPuppies = new List<string>();
        public List<string> productionPuppies = new List<string>();
        public List<string> homePuppies = new List<string>();
        public List<string> culturePuppies = new List<string>();
        public List<string> churchPuppies = new List<string>();

        public Building building;
        public double scoutCostRemaining;
        public bool explored = false;
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

                    c.scoutCostRemaining = GameFunctions.scoutCost(c.coord.x + 0.5 - Constants.mapSize / 2.0,
                                                                   c.coord.y + 0.5 - Constants.mapSize / 2.0);

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

            scoutCell(new Coord(d + 0, d + 0), 1, 1e10);
            scoutCell(new Coord(d + 0, d + 1), 1, 1e10);
            scoutCell(new Coord(d + 1, d + 0), 1, 1e10);
            scoutCell(new Coord(d + 1, d + 1), 1, 1e10);
        }

        public void removePuppyFromAllWorkLists(Puppy p)
        {
            removePuppyFromList(p, x => x.scoutPuppies);
            removePuppyFromList(p, x => x.constructionPuppies);
            removePuppyFromList(p, x => x.militaryPuppies);
            removePuppyFromList(p, x => x.productionPuppies);
        }

        public void removePuppyFromList(Puppy p, Func<MapCell, List<string>> listType)
        {
            foreach (MapCell m in mapAsList)
            {
                List<string> l = listType(m);
                if (l.Contains(p.initials))
                    l.Remove(p.initials);
            }
        }

        public void scoutCell(Coord c, int scoutRadius, double scoutValue)
        {
            int regionsToScout = 0;
            for (int xOffset = -scoutRadius; xOffset <= scoutRadius; xOffset++)
                for (int yOffset = -scoutRadius; yOffset <= scoutRadius; yOffset++)
                    if (!data[c.x + xOffset, c.y + yOffset].explored)
                        regionsToScout++;

            for(int xOffset = -scoutRadius; xOffset <= scoutRadius; xOffset++)
                for(int yOffset = -scoutRadius; yOffset <= scoutRadius; yOffset++)
                {
                    MapCell cell = data[c.x + xOffset, c.y + yOffset];
                    if(!cell.explored)
                    {
                        cell.scoutCostRemaining -= scoutValue / regionsToScout;
                        if(cell.scoutCostRemaining < 0.0)
                        {
                            cell.explored = true;
                            cell.scoutCostRemaining = 0.0;
                        }
                    }
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
