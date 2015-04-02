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
        public Dictionary<string, Brush> brushes = new Dictionary<string, Brush>();
        public Dictionary<string, Pen> pens = new Dictionary<string, Pen>();
        public Dictionary<string, Font> fonts = new Dictionary<string, Font>();
        public AssetManager assets = new AssetManager();
        public bool suppressRequests = false;
        public Dictionary<Puppy, Rectangle> puppyMapLocations = new Dictionary<Puppy, Rectangle>();
        public Dictionary<MapCell, Rectangle> cellMapLocations = new Dictionary<MapCell, Rectangle>();
        
        public AppState()
        {
            //
            // Terrain brushes
            //
            brushes.Add("water", new SolidBrush(Color.Blue));
            brushes.Add("grass", new SolidBrush(Color.LawnGreen));
            brushes.Add("dirt", new SolidBrush(Color.SandyBrown));

            //
            // Puppy brushes
            //
            brushes.Add("work", new SolidBrush(Color.FromArgb(160, Color.LightSkyBlue)));
            brushes.Add("home", new SolidBrush(Color.FromArgb(160, Color.LightPink)));

            //
            // Default pens and brushes
            //
            brushes.Add("black", new SolidBrush(Color.Black));
            pens.Add("black", new Pen(Color.Black, 1.0f));

            //
            // Fonts
            //
            fonts.Add("puppy", new Font("Times New Roman", 8.0f));
        }

        public string request(string query)
        {
            if (suppressRequests)
                return "";

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
            puppyMapLocations = new Dictionary<Puppy, Rectangle>();
            cellMapLocations = new Dictionary<MapCell, Rectangle>();

            if(g == null) g = Graphics.FromImage(mapBmp);
            g.Clear(Color.Black);
            foreach(MapCell c in gameMap)
            {
                Point cellPoint = new Point(c.coord.x * Constants.mapCellSize, c.coord.y * Constants.mapCellSize);
                Rectangle mapRectangle = new Rectangle(cellPoint.X, cellPoint.Y, cellPoint.X + Constants.mapCellSize, cellPoint.Y + Constants.mapCellSize);
                cellMapLocations[c] = mapRectangle;
                g.FillRectangle(brushes[c.type], mapRectangle);

                if(c.building != null)
                {
                    var image = assets.images[c.building.name];
                    g.DrawImage(image.bmp, cellPoint);

                    var puppyList = new List<Tuple<Puppy, Brush>>();

                    foreach(string initials in c.homePuppies)
                        puppyList.Add( Tuple.Create(puppies[initials], brushes["home"]));

                    foreach(string initials in c.workPuppies)
                        puppyList.Add( Tuple.Create(puppies[initials], brushes["work"]));

                    const int puppyAStart = 3;
                    const int puppySize = 20;
                    const int puppyBStart = Constants.mapCellSize - puppyAStart - puppySize;
                    const int puppyMStart = Constants.mapCellSize / 2 - puppySize / 2;
                    Size puppyFontOffset = new Size(0, 3);

                    var puppyOffsets = new List<Size>();
                    if(puppyList.Count == 4)
                    {
                        puppyOffsets.Add(new Size(puppyAStart, puppyAStart));
                        puppyOffsets.Add(new Size(puppyAStart, puppyBStart));
                        puppyOffsets.Add(new Size(puppyBStart, puppyAStart));
                        puppyOffsets.Add(new Size(puppyBStart, puppyBStart));
                    }
                    if (puppyList.Count == 3)
                    {
                        puppyOffsets.Add(new Size(puppyMStart, puppyAStart));
                        puppyOffsets.Add(new Size(puppyBStart, puppyAStart));
                        puppyOffsets.Add(new Size(puppyBStart, puppyBStart));
                    }
                    if (puppyList.Count == 2)
                    {
                        puppyOffsets.Add(new Size(puppyAStart, puppyMStart));
                        puppyOffsets.Add(new Size(puppyBStart, puppyMStart));
                    }
                    if (puppyList.Count == 1)
                    {
                        puppyOffsets.Add(new Size(puppyMStart, puppyMStart));
                    }

                    for (int i = 0; i < puppyOffsets.Count; i++)
                    {
                        Puppy p = puppyList[i].Item1;
                        Point o = cellPoint + puppyOffsets[i];
                        Rectangle r = new Rectangle(o.X, o.Y, puppySize, puppySize);
                        g.FillRectangle(puppyList[i].Item2, r);
                        g.DrawRectangle(pens["black"], r);
                        g.DrawString(p.initials, fonts["puppy"], brushes["black"], o + puppyFontOffset);
                        puppyMapLocations[p] = r;
                    }
                }
            }
            
        }

        
    }
}
