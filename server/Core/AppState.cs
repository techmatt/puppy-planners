using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace server
{
    class AppState
    {
        public AppState()
        {
            server.app = this;
            sessionManager.app = this;
        }

        //
        // Logging happens so often it is useful to have a quick shortcut
        //
        public void log(EventType type, string message)
        {
            eventLog.log(type, message);
        }

        public void log(string message)
        {
            eventLog.log(EventType.General, message);
        }

        public void error(string message)
        {
            eventLog.log(EventType.Error, message);
        }

        public EventLog eventLog = new EventLog();
        public Server server = new Server();
        public SessionManager sessionManager = new SessionManager();
        public JavaScriptSerializer serializer = new JavaScriptSerializer();
    }
}
