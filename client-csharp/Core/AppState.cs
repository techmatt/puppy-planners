using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace client_csharp
{
    class AppState
    {
        public List< Dictionary<string, Object> > JSONToDictionaryList(string JSON)
        {
            object[] o = serializer.Deserialize<object[]>(JSON);

            var result = new List<Dictionary<string, Object>>();
            if(o[0] is Dictionary<string, Object>)
            {
                result.Add(o[0] as Dictionary<string, Object>);
            }
            else
            {
                Console.Write("invalid parse: " + JSON);
                return result;
            }
            return result;
        }

        public JavaScriptSerializer serializer = new JavaScriptSerializer();
    }
}
