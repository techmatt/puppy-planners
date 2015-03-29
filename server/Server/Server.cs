using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.Threading;
using game;

namespace server
{
    class Server
    {
        public AppState app;
        public HttpListener listener;
        public Thread serverThread;

        public void launch()
        {
            if (listener != null)
            {
                MessageBox.Show("Server already running: restarting...");

                serverThread.Abort();
                listener.Close();
            }

            listener = new HttpListener();

            //listener.Prefixes.Add(@"http://brethil.stanford.edu:8080/puppies/");
            listener.Prefixes.Add(@"http://localhost:8080/puppies/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            listener.Start();

            app.log(EventType.Server, "Listener started");

            serverThread = new Thread(processRequests);
            serverThread.Start();
        }

        void processRequests()
        {
            while (true)
            {
                //
                // GetContext blocks while waiting for a request
                //
                HttpListenerContext context = listener.GetContext();

                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                //
                // See protocol.txt for the specification of how server requests should be formatted
                // /puppies/p&command&param1=val1&param2=val2&...
                // 

                //
                // We may need to clean the URL to parse non-ASCII stuff
                //
                //var cleanedUrl = HttpUtility.UrlDecode(request.RawUrl);
                var cleanedUrl = request.RawUrl;
                var parts = cleanedUrl.Split('&');

                if (Constants.echoServerRequests)
                    app.log(EventType.Server, "Request: " + request.RawUrl);

                string responseString = "error";

                if(parts[0] == "/puppies/p")
                {
                    //
                    // game command
                    //
                    string command = parts[1];
                    Dictionary<string,string> parameters = new Dictionary<string,string>();
                    for(int partIndex = 2; partIndex < parts.Length; partIndex++)
                    {
                        var v = parts[partIndex].Split('=');
                        parameters[v[0]] = v[1];
                    }
                    responseString = app.sessionManager.dispatchCommand(command, parameters);
                }
                else
                {
                    //
                    // file request (not currently handled)
                    //
                    try
                    {
                        responseString = File.ReadAllText(Constants.fileHostDir + parts[0].Replace("/puppies/", ""));
                    }
                    catch (Exception ex)
                    {
                        Console.Write("Failed to read file: " + cleanedUrl + " -> " + ex.ToString());
                    }
                }

                response.StatusCode = 200;
                response.StatusDescription = "OK";

                System.IO.Stream output = response.OutputStream;
                if(responseString.Length > 0)
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                    //
                    // Get a response stream and write the response to it.
                    //
                    response.ContentLength64 = buffer.Length;
                    output.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    response.ContentLength64 = 0;
                }
                output.Close();
                
            }
            //listener.Stop();
        }
    }
}
