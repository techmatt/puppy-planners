using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace grandLARP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Maze m;
            //m = new Maze("maze00");
            //m = new Maze("maze01");
            //m = new Maze("maze02");
            //m = new Maze("maze03");
            //m = new Maze("maze04");
            m = new Maze("maze05");
            m = new Maze("maze06");
            m = new Maze("maze07");
            m = new Maze("maze08");
            m = new Maze("maze09");
            m = new Maze("maze10");
        }

        private void buttonBase_Click(object sender, EventArgs e)
        {
            string input = "welcome to the taralym network";
            var values = new List<string>();
            /*values.Add("1");
            values.Add("2");
            values.Add("3");
            values.Add("B0");
            values.Add("B1");
            values.Add("B2");
            values.Add("B3");
            values.Add("C0");
            values.Add("C1");
            values.Add("C2");
            values.Add("C3");
            values.Add("D0");
            values.Add("D1");
            values.Add("D2");
            values.Add("D3");
            values.Add("BA0");
            values.Add("BA1");
            values.Add("BA2");
            values.Add("BA3");
            values.Add("BB0");
            values.Add("BB1");
            values.Add("BB2");
            values.Add("BB3");
            values.Add("BC0");
            values.Add("BC1");
            values.Add("BC2");
            values.Add("BC3");*/

            values.Add("B");
            values.Add("C");
            values.Add("D");
            values.Add("FA");
            values.Add("FB");
            values.Add("FC");
            values.Add("FD");
            values.Add("GA");
            values.Add("GB");
            values.Add("GC");
            values.Add("GD");
            values.Add("HA");
            values.Add("HB");
            values.Add("HC");
            values.Add("HD");
            values.Add("FEA");
            values.Add("FEB");
            values.Add("FEC");
            values.Add("FED");
            values.Add("FFA");
            values.Add("FFB");
            values.Add("FFC");
            values.Add("FFD");
            values.Add("FGA");
            values.Add("FGB");
            values.Add("FGC");
            values.Add("FGD");

            var map = new Dictionary<char, string>();
            for(int i = 0; i < 26; i++)
                map[(char)('a' + i)] = values[i];
            map[' '] = values[26];

            string outputA = "";
            string outputB = "";
            foreach (char c in input)
            {
                outputA = outputA + map[c];
                foreach(char c2 in map[c])
                {
                    outputB = outputB + c2 + "\r\n";
                }
            }
            textBox1.Text = outputB;
        }
    }
}
