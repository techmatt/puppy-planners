using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class BuildingResourceCost
    {
        public BuildingResourceCost(string resourceDesc)
        {
            var parts = resourceDesc.Split('=');
            resourceName = parts[0];
            cost = Convert.ToInt32(parts[1]);
        }
        public string resourceName;
        public int cost;
    }

    public class BuildingResourceProduction
    {
        public BuildingResourceProduction(string resourceDesc)
        {
            var parts = resourceDesc.Split('=');
            resourceName = parts[0];
            productionPerSecond = Convert.ToDouble(parts[1]);
        }
        public string resourceName;
        public double productionPerSecond;
    }

    public class BuildingResourceStorage
    {
        public BuildingResourceStorage(string resourceDesc)
        {
            var parts = resourceDesc.Split('=');
            resourceName = parts[0];
            storage = Convert.ToDouble(parts[1]);
        }
        public string resourceName;
        public double storage;
    }

    public class BuildingInfo
    {
        public string name;
        public int population;
        public List<BuildingResourceCost> cost = new List<BuildingResourceCost>();
        public List<BuildingResourceProduction> production = new List<BuildingResourceProduction>();
        public List<BuildingResourceStorage> storage = new List<BuildingResourceStorage>();
    }

    public class Database
    {
        public Dictionary<string, BuildingInfo> buildings = new Dictionary<string, BuildingInfo>();
        
        public Database()
        {
            loadBuildings();
        }

        void loadBuildings()
        {
            foreach(var line in parseCSVFile(Constants.dataDir + "buildings.csv"))
            {
                BuildingInfo info = new BuildingInfo();
                info.name = line["name"];
                info.population = Convert.ToInt32(line["pop"]);

                var costHeaders = new string[] { "cost A", "cost B", "cost C" };
                foreach (string header in costHeaders)
                {
                    string resourceDesc = line[header];
                    if (resourceDesc != "none")
                        info.cost.Add(new BuildingResourceCost(resourceDesc));
                }

                var resourceHeaders = new string[] { "resource A", "resource B", "resource C" };
                foreach(string header in resourceHeaders)
                {
                    string resourceDesc = line[header];
                    if(resourceDesc != "none")
                        info.production.Add(new BuildingResourceProduction(resourceDesc));
                }

                var storageHeaders = new string[] { "storage A", "storage B", "storage C" };
                foreach (string header in storageHeaders)
                {
                    string resourceDesc = line[header];
                    if (resourceDesc != "none")
                        info.storage.Add(new BuildingResourceStorage(resourceDesc));
                }

                if(info.name != "none") buildings[info.name] = info;
            }
        }

        List< Dictionary<string, string> > parseCSVFile(string filename)
        {
            var result = new List<Dictionary<string, string>>();

            List<string> header = null;

            foreach(string line in File.ReadAllLines(filename))
            {
                var parts = new List<string>(line.Split(','));
                if(header == null)
                {
                    header = parts;
                    continue;
                }
                
                var dict = new Dictionary<string, string>();
                for(int i = 0; i < parts.Count; i++)
                {
                    dict.Add(header[i], parts[i]);
                }
                result.Add(dict);
            }

            return result;
        }
    }
}
