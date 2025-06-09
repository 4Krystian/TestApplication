using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BusinessLogic
{
    public class Tools
    {
        public static string GetRandomString(int Length)
        {
            StringBuilder _randomString = new StringBuilder();

            do
            {
                _randomString.Append(Path.GetRandomFileName().Replace(".", ""));

            } while (_randomString.Length < Length);

            return _randomString.ToString().Substring(0, Length);
        }

        public static T DeserializeJson<T>(string Value) where T : new()
        {
            if (!String.IsNullOrEmpty(Value))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Value);
            }
            else
            {
                return new T();
            }
        }
    }
}
