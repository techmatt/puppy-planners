using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace grandLARP
{
    static class Constants
    {
        public const int imgWidth = 640;
        public const int imgHeight = 480;

        public const string baseDir = @"C:\Code\puppy-planners\grandLARP\";
        public const string dataDir = baseDir + @"data\";
        public const string landscapeDir = baseDir + @"art\landscapes\";
        public const string symbolDir = baseDir + @"art\symbols\";
        public const string outDir = baseDir + @"out\";
    }
    class Vertex
    {
        public int x;
        public int y;
        public string trueID;
        public string sampledID;
        public string publicName, internalName, desc, symbol, image;
        public bool isBorder = true;

        public void resampleID()
        {

        }

        public Bitmap loadImage()
        {
            Bitmap bmpIn = new Bitmap(Image.FromFile(Constants.landscapeDir + image));
            var bmpOut = new Bitmap((int)Constants.imgWidth, (int)Constants.imgHeight);
            var graph = Graphics.FromImage(bmpOut);
            var brush = new SolidBrush(Color.Black);

            graph.InterpolationMode = InterpolationMode.High;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            float scale = Math.Min(Constants.imgWidth / bmpIn.Width, Constants.imgHeight / bmpIn.Height);
            var scaleWidth = (int)(bmpIn.Width * scale);
            var scaleHeight = (int)(bmpIn.Height * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, Constants.imgWidth, Constants.imgHeight));
            graph.DrawImage(bmpOut, new Rectangle(((int)Constants.imgWidth - scaleWidth) / 2, ((int)Constants.imgHeight - scaleHeight) / 2, scaleWidth, scaleHeight));

            return bmpOut;
        }
    }

    class Maze
    {
        public void loadID()
        {
            var lines = File.ReadAllLines(Constants.dataDir + "trueIDMap.txt");
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Vertex v = new Vertex();
                    v.trueID = "Z";
                    v.x = i;
                    v.y = j;
                    vertices[i, j] = v;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    vertices[i + 1, j + 1].trueID = lines[i][j].ToString();
                    vertices[i + 1, j + 1].isBorder = false;
                }
            }
        }

        public void loadInfo()
        {
            var lines = File.ReadAllLines(Constants.dataDir + "placeInfo.txt");
            Vertex activeVertex = null;
            foreach (string line in lines)
            {
                
                if(line.Contains("=") && !line.StartsWith("#"))
                {
                    //id=A
                    //internalName=almanac
                    //name=A peaceful cave
                    //desc=Description!There is stuff here.
                    //image=almnac.jpg
                    //symbol=a.jpg
                    var parts = line.Split('=');
                    var p = parts[0];
                    var v = parts[1];
                    if (p == "id")
                    {
                        foreach (var vertex in vertices)
                            if (vertex.trueID == v)
                                activeVertex = vertex;
                    }
                    if (p == "internalName")
                        activeVertex.internalName = v;
                    if (p == "name")
                        activeVertex.publicName = v;
                    if (p == "desc")
                        activeVertex.desc = v;
                    if (p == "image")
                        activeVertex.image = v;
                    if (p == "symbol")
                        activeVertex.symbol = v;
                }
            }
        }

        public void makeGMImage()
        {
            const int buffer = 10;
            const int wSize = (int)Constants.imgWidth + buffer;
            const int hSize = (int)Constants.imgHeight + buffer;
            var imgOut = new Bitmap(wSize * 5, hSize * 5);
            var graph = Graphics.FromImage(imgOut);

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Vertex v = vertices[i + 1, j + 1];
                    Bitmap vImg = v.loadImage();

                    graph.DrawImage(vImg, new Point(i * wSize, j * hSize));
                }
            }

            imgOut.Save(Constants.outDir + "GMImage.png");
        }

        public Maze()
        {
            vowels.Add('A');
            vowels.Add('E');
            vowels.Add('I');
            vowels.Add('O');
            vowels.Add('U');
            vowels.Add('Y');

            consonants.Add('B');
            consonants.Add('C');
            consonants.Add('D');
            consonants.Add('F');
            consonants.Add('G');
            consonants.Add('H');
            consonants.Add('J');
            consonants.Add('K');
            consonants.Add('L');
            consonants.Add('M');
            consonants.Add('N');
            consonants.Add('P');
            consonants.Add('Q');
            consonants.Add('R');
            consonants.Add('S');
            consonants.Add('T');
            consonants.Add('V');
            consonants.Add('W');
            consonants.Add('X');
            consonants.Add('Y');
            
            loadID();
            loadInfo();
            makeGMImage();

            // ******* 3333333 ******* 6543456 3333333
            // *ABCDE* 3012113 *A+T+N* 5432345 3222223
            // *FGHIJ* 3001013 *BC+U+* 4321234 3211123
            // *KLMNO* 3200113 *C+S+P* 3210123 3210123
            // *PQRST* 3110013 *D+C++* 4321234 3211123
            // *UVWXY* 3201123 *W+++L* 5432345 3222223
            // ******* 3333333 ******* 6543456 3333333

            // S = shuttle landing site
            // L = Hannah's lab
            // N = Niesseling's portal
            // B = button to irritate the Niesseling
            // U = Uzuki Umiak
            // T = Tabitha's picnic table with her corpse
            // C = warp crystal chips
            // W = warp crystal mines
            // A = Almanac
            // P = pool of water
            // D = one of Hannah's devices

            // a locations  is *I*L*
            // R is the radius, L is the label, I is the 'interesting' flag (vowel is good, consnant is boring)
            // presence of a Z anywhere is really bad
        }
        public List<char> vowels = new List<char>();
        public List<char> consonants = new List<char>();
        public Vertex[,] vertices = new Vertex[7, 7];
    }
}
