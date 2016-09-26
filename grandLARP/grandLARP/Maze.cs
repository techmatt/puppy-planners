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
        public string trueID;
    }

    class Maze
    {
        Maze()
        {
            critialLetters.Add('T');
            critialLetters.Add('S');
            critialLetters.Add('M');
            critialLetters.Add('N');
            critialLetters.Add('R');

            randomLetters.Add('A');
            randomLetters.Add('E');
            randomLetters.Add('I');

            radiusLetters.Add('B'); // 0
            radiusLetters.Add('C'); // 1
            radiusLetters.Add('D'); // 2
            radiusLetters.Add('F'); // 3

            // ZZZZZZZ 0120120 *******
            // ZABCDEZ 2012111 *A+T+N*
            // ZFGHIJZ 1001012 *B++C+*
            // ZKLMNOZ 0200110 *++S++*
            // ZPQRSTZ 2110011 *+U+C+*
            // ZUVWXYZ 1201122 *W+++L*
            // ZZZZZZZ 0210210 *******

            // S = shuttle landing site
            // L = Hannah's lab
            // N = Niesseling's portal
            // B = button to irritate the Niesseling
            // U = Uzuki Umiak
            // T = Tabitha's picnic table with her corpse
            // C = warp crystal chips
            // W = warp crystal mines
            // A = Almanac

        }
        public List<char> critialLetters = new List<char>();
        public List<char> randomLetters = new List<char>();
        public List<char> radiusLetters = new List<char>();
    }
}
