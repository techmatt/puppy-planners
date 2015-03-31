using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace game
{
    public struct Coord
    {
        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public bool isValid()
        {
            return (x != -1 && y != -1);
        }
        public bool Compare(Coord c)
        {
            return (x == c.x) && (y == c.y);
        }

        public int x, y;
    }
    
    public static class Util
    {
        public static double bound(double value, double low, double high)
        {
            if (value < low) return low;
            if (value > high) return high;
            return value;
        }

        public static string[] SplitOnString(this string s, string separator)
        {
            return s.Split(new string[] { separator }, StringSplitOptions.None);
        }

        public static string GetPostData(this HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return null;
            }
            using (System.IO.Stream body = request.InputStream) // here we have data
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static List<T> Shuffle<T>(this List<T> list)  
        {
            return new List<T>(list.OrderBy(x => Guid.NewGuid()));
        }
    }
}
