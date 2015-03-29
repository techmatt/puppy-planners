using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Drawing;
using System.IO;

namespace game
{
    public class ImageAsset
    {
        public ImageAsset(string filename)
        {
            Bitmap fullBmp = new Bitmap(Image.FromFile(filename));
            Bitmap fullBmpAlpha = new Bitmap(fullBmp.Width, fullBmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Func<Color, Color, double> colorDist = (x, y) =>
            {
                int diffR = x.R - y.R;
                int diffG = x.G - y.G;
                int diffB = x.B - y.B;
                return (Math.Abs(diffR) + Math.Abs(diffG) + Math.Abs(diffB)) / 255.0;
            };

            //Color backgroundColor = Color.FromArgb(0, 255, 255, 255);
            Color backgroundColor = fullBmp.GetPixel(0, 0);
            const double whitenessThreshold = 0.2;

            for (int y = 0; y < fullBmp.Height; y++)
                for(int x = 0; x < fullBmp.Width; x++)
                {
                    // c_i = (1 - a) * b_i + a * f_i
                    // f_i = (C_i - (1 - a) * b_i) / a
                    Color c = fullBmp.GetPixel(x, y);
                    double whiteness = colorDist(c, backgroundColor);
                    fullBmpAlpha.SetPixel(x, y, Color.FromArgb(255, c.R, c.G, c.B));
                    if(whiteness < whitenessThreshold)
                    {
                        double alpha = (int)Math.Round(whiteness / whitenessThreshold);
                        if(alpha > 0.0)
                        {
                            int fr = (int)Math.Round((c.R - (1.0 - alpha) * backgroundColor.R) / alpha);
                            int fg = (int)Math.Round((c.R - (1.0 - alpha) * backgroundColor.R) / alpha);
                            int fb = (int)Math.Round((c.R - (1.0 - alpha) * backgroundColor.R) / alpha);
                            fullBmpAlpha.SetPixel(x, y, Color.FromArgb(0, fr, fg, fb));
                        }
                        else
                        {
                            fullBmpAlpha.SetPixel(x, y, Color.FromArgb(0, backgroundColor.R, backgroundColor.G, backgroundColor.B));
                        }
                    }
                }

            bmp = new Bitmap(Constants.mapCellSize, Constants.mapCellSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(fullBmpAlpha, new Rectangle(0, 0, bmp.Width, bmp.Height));

            Bitmap testBmpColor = new Bitmap(bmp);
            Bitmap testBmpAlpha = new Bitmap(bmp);
            Bitmap testComposite = new Bitmap(bmp);

            Color compositeColor = Color.FromArgb(255, 100, 200, 100);

            for (int y = 0; y < testBmpColor.Height; y++)
                for (int x = 0; x < testBmpColor.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);

                    double alpha = c.A / 255.0;
                    int nr = (int)(c.R * alpha + compositeColor.R * (1.0 - alpha));
                    int ng = (int)(c.G * alpha + compositeColor.G * (1.0 - alpha));
                    int nb = (int)(c.B * alpha + compositeColor.B * (1.0 - alpha));

                    testBmpColor.SetPixel(x, y, Color.FromArgb(255, c.R, c.G, c.B));
                    testBmpAlpha.SetPixel(x, y, Color.FromArgb(255, c.A, c.A, c.A));
                    testComposite.SetPixel(x, y, Color.FromArgb(255, nr, ng, nb));
                }

            testBmpColor.Save(Constants.imageOutDir + Path.GetFileNameWithoutExtension(filename) + "_color.png");
            testBmpAlpha.Save(Constants.imageOutDir + Path.GetFileNameWithoutExtension(filename) + "_alpha.png");
            testComposite.Save(Constants.imageOutDir + Path.GetFileNameWithoutExtension(filename) + "_composite.png");
        }

        public string imageName;
        public Bitmap bmp;
    }

    public class AssetManager
    {
        public AssetManager()
        {
            loadImages();
        }

        void loadImages()
        {
            foreach(string s in Directory.EnumerateFiles(Constants.imageDir))
            {
                images.Add(Path.GetFileNameWithoutExtension(s), new ImageAsset(s));
            }
        }

        Dictionary<string, ImageAsset> images = new Dictionary<string, ImageAsset>();
    }
}
