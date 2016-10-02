﻿using System;
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
        
        public string randomLetter(Maze m)
        {
            char c = 'A';
            c += (char)m.random.Next(0, 26);
            return c.ToString();
        }
        public void resampleID(Maze m)
        {
            bool valid = true;
            sampledID = randomLetter(m) + randomLetter(m) + randomLetter(m) + randomLetter(m) + randomLetter(m);

            int warningChars = 0;
            foreach (char c in sampledID)
                if (c == 'Q' || c == 'X' || c == 'Z')
                    warningChars++;

            if (isBorder)
            {
                if (warningChars != 1) valid = false;
            }

            if(!isBorder)
            {
                if (warningChars != 0 && trueID != "Q" && trueID != "X") valid = false;
                if (sampledID[3] != trueID[0]) valid = false;

                if (internalName == "none" && m.vowels.Contains(sampledID[1])) valid = false;
                if (internalName != "none" && m.consonants.Contains(sampledID[1])) valid = false;
            }

            if (!valid) resampleID(m);
        }

        public Bitmap loadImage(Random r)
        {
            string imageFilename = Constants.landscapeDir + image;
            if(isBorder)
            {
                var allImages = Directory.EnumerateFiles(Constants.landscapeDir).ToList();
                imageFilename = allImages[r.Next(allImages.Count)];
            }

            Bitmap bmpIn = new Bitmap(Image.FromFile(imageFilename));
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
            //graph.DrawImage(bmpIn, new Rectangle(((int)Constants.imgWidth - scaleWidth) / 2, ((int)Constants.imgHeight - scaleHeight) / 2, scaleWidth, scaleHeight));
            graph.DrawImage(bmpIn, 0, 0, Constants.imgWidth, Constants.imgHeight);

            //bmpOut.Save(Constants.outDir + "test.jpg");

            return bmpOut;
        }
    }

    class Maze
    {
        public Random random = new Random();
        public List<char> vowels = new List<char>();
        public List<char> consonants = new List<char>();
        public Vertex[,] vertices = new Vertex[7, 7];
        
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
                    vertices[i + 1, j + 1].trueID = lines[j][i].ToString();
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
            var imgOut = new Bitmap(wSize * 7, hSize * 7);
            var graph = Graphics.FromImage(imgOut);

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Vertex v = vertices[i, j];
                    Bitmap vImg = v.loadImage(random);

                    graph.DrawImage(vImg, new Point(i * wSize, j * hSize));

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    graph.DrawString(v.sampledID, new System.Drawing.Font("Calibri", 48, FontStyle.Bold), Brushes.WhiteSmoke, (i + 0.5f) * wSize, (j + 0.5f) * hSize, sf);
                    graph.DrawString(v.internalName, new System.Drawing.Font("Calibri", 48, FontStyle.Bold), Brushes.WhiteSmoke, (i + 0.5f) * wSize, (j + 0.7f) * hSize, sf);
                }
            }

            imgOut.Save(Constants.outDir + "GMImage.jpg");
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

            foreach (Vertex v in vertices)
                v.resampleID(this);

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

    }
}
