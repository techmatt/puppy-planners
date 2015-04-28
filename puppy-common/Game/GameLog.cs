using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class GameException : Exception
    {
        public GameException()
            : base() { }
        public GameException(string message)
            : base(message) { }
    }

    public class GameLogEntry
    {
        public GameLogEntry(int _gameTick, string _message)
        {
            gameTick = _gameTick;
            message = _message;
        }
        public int gameTick;
        public string message;
    }

    public class GameLog
    {
        public void log(int gameTick, string message)
        {
            Console.WriteLine("log: " + message);
            entries.Add(new GameLogEntry(gameTick, message));
        }
        public GameException error(int gameTick, string message)
        {
            Console.WriteLine("error: " + message);
            entries.Add(new GameLogEntry(gameTick, message));
            return new GameException(message);
        }

        List<GameLogEntry> entries = new List<GameLogEntry>();
    }
}
