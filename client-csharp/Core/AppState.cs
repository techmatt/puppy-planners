using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Drawing;
using game;

namespace client_csharp
{
    class AppState
    {
        const string serverUrl = "http://localhost:8080/puppies/";
        const string serverQueryUrl = "http://localhost:8080/puppies/p&";
        HttpClient client = new HttpClient();

        public string sessionID;
        public JavaScriptSerializer serializer = new JavaScriptSerializer();
        public GameStateData gameData;
        public List<MapCell> gameMap;
        public Dictionary<string, Puppy> puppies;
        public Bitmap mapBmp = new Bitmap(Constants.mapCellSize * Constants.mapSize, Constants.mapCellSize * Constants.mapSize);
        public Graphics g;
        public Dictionary<string, Brush> terrainBrushes = new Dictionary<string, Brush>();
        public AssetManager assets = new AssetManager();
        
        public AppState()
        {
            terrainBrushes.Add("water", new System.Drawing.SolidBrush(System.Drawing.Color.Blue));
            terrainBrushes.Add("grass", new System.Drawing.SolidBrush(System.Drawing.Color.LawnGreen));
            terrainBrushes.Add("dirt", new System.Drawing.SolidBrush(System.Drawing.Color.SandyBrown));
        }

        public string request(string query)
        {
            var responseString = client.GetStringAsync(serverQueryUrl + query);

            bool success = responseString.Wait(100000);
            if (!success)
            {
                Console.WriteLine("request timeout: ", query);
                return "error: timeout";
            }

            return responseString.Result;
        }

        public string sessionRequest(string command)
        {
            return request(command + "&session=" + sessionID);
        }

        public string sessionRequest(string command, string parameters)
        {
            return request(command + "&session=" + sessionID + "&" + parameters);
        }

        public void updateMapBmp()
        {
            if(g == null) g = Graphics.FromImage(mapBmp);
            g.Clear(Color.Black);
            foreach(MapCell c in gameMap)
            {
                Point s = new Point(c.coord.x * Constants.mapCellSize, c.coord.y * Constants.mapCellSize);
                g.FillRectangle(terrainBrushes[c.type], new Rectangle(s.X, s.Y, s.X + Constants.mapCellSize, s.Y + Constants.mapCellSize));

                if(c.building != null)
                {
                    var image = assets.images[c.building.name];
                    g.DrawImage(image.bmp, s);
                }
            }
        }

        
    }
}
