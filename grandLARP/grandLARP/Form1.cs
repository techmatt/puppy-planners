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
            string input = "welcome to the second part";
            var values = new List<string>();
            /*values.Add("1");
            values.Add("2");
            values.Add("3");
            values.Add("10");
            values.Add("11");
            values.Add("12");
            values.Add("13");
            values.Add("20");
            values.Add("21");
            values.Add("22");
            values.Add("23");
            values.Add("30");
            values.Add("31");
            values.Add("32");
            values.Add("33");
            values.Add("X00");
            values.Add("X01");
            values.Add("X02");
            values.Add("X03");
            values.Add("X10");
            values.Add("X11");
            values.Add("X12");
            values.Add("X13");
            values.Add("X20");
            values.Add("X21");
            values.Add("X22");
            values.Add("X23");*/

            values.Add("1");
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
            values.Add("BC3");

            var map = new Dictionary<char, string>();
            for(int i = 0; i < 26; i++)
                map[(char)('a' + i)] = values[i];
            map[' '] = values[26];

            string output = "";
            foreach(char c in input)
            {
                output = output + map[c];
            }
            textBox1.Text = output;
        }
    }
}
