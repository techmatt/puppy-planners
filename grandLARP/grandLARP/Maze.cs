using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grandLARP
{
    struct Vertex
    {
        public int x;
        public int y;
        public int radius;
        public string trueID;
        public string sampledID;

        public void resampleID()
        {

        }
    }

    class Maze
    {
        Maze()
        {
            radiusLetters.Add('A'); // 0
            radiusLetters.Add('B'); // 1
            radiusLetters.Add('C'); // 2
            radiusLetters.Add('Z'); // 3

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
        public List<char> critialLetters = new List<char>();
        public List<char> randomLetters = new List<char>();
        public List<char> radiusLetters = new List<char>();
    }
}
