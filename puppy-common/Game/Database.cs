using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class SkillInfo
    {
        public string name;
        public string description;
        public double frequency;
        public double rangeMin;
        public double rangeMax;

        public double sampleLearningRate(Random random)
        {
            if (random.NextDouble() > frequency)
                return 0.0;
            return random.NextDouble(rangeMin, rangeMax);
        }
    }

    public class PuppyName
    {
        public PuppyName(string line)
        {
            var parts = line.Split(' ');
            firstName = parts[0];
            lastName = parts[1];
            fullName = firstName + " " + lastName;
            initials = firstName[0].ToString() + lastName[0].ToString();
        }
        public string firstName;
        public string lastName;
        public string fullName;
        public string initials;
    }

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
        public string type;
        public int workCap;
        public int residentCap;
        public int cultureCap;
        public int religionCap;
        public List<BuildingResourceCost> cost = new List<BuildingResourceCost>();
        public List<BuildingResourceProduction> production = new List<BuildingResourceProduction>();
        public List<BuildingResourceStorage> storage = new List<BuildingResourceStorage>();
    }

    public class Database
    {
        public Dictionary<string, BuildingInfo> buildings = new Dictionary<string, BuildingInfo>();
        public List<PuppyName> puppyNames = new List<PuppyName>();
        public Dictionary<string, SkillInfo> puppySkills = new Dictionary<string, SkillInfo>();
        
        public Database()
        {
            loadBuildings();
            loadNames();
            loadSkills();
        }

        public PuppyName randomPuppyName(GameState state)
        {
            while(true)
            {
                PuppyName randomName = puppyNames[state.random.Next(puppyNames.Count)];
                if(!state.puppies.ContainsKey(randomName.initials))
                {
                    return randomName;
                }
            }
        }

        void loadNames()
        {
            foreach (string s in File.ReadAllLines(Constants.dataDir + "puppyNames.txt"))
            {
                if (s.Length >= 5)
                {
                    puppyNames.Add(new PuppyName(s));
                }
            }
        }

        void loadSkills()
        {
            foreach (var line in parseCSVFile(Constants.dataDir + "skills.csv"))
            {
                SkillInfo skill = new SkillInfo();
                skill.name = line["name"];
                skill.frequency = Convert.ToDouble(line["frequency"]);
                skill.rangeMin = Convert.ToDouble(line["range min"]);
                skill.rangeMax = Convert.ToDouble(line["range max"]);
                skill.description = line["description"];

                if (skill.name != "none") puppySkills[skill.name] = skill;
            }
        }

        void loadBuildings()
        {
            foreach(var line in parseCSVFile(Constants.dataDir + "buildings.csv"))
            {
                BuildingInfo info = new BuildingInfo();
                info.name = line["name"];
                info.type = line["type"];
                info.workCap = Convert.ToInt32(line["work cap"]);
                info.residentCap = Convert.ToInt32(line["resident cap"]);
                info.cultureCap = Convert.ToInt32(line["culture cap"]);
                info.religionCap = Convert.ToInt32(line["religion cap"]);

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
