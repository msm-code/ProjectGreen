using System.Collections.Generic;
using System;

namespace ProjectGreen
{
    class Settings
    {
        private static Settings userData = Content.Load<Settings>("user");
        public static Settings UserData { get { return userData; } }

        Dictionary<string, string> data;

        public Settings(Dictionary<string, string> data)
        {
            this.data = data;
        }

        public bool Contains(string id)
        {
            return data.ContainsKey(id);
        }

        public string Get(params string[] idName)
        {
            string realId = string.Join("::", idName);

            string value;
            if (data.TryGetValue(realId, out value))
            { return value; }
            return null;
        }

        public bool GetBool(params string[] id)
        {
            string result = Get(id);
            if (result == "true") { return true; }
            if (result == "false") { return false; }
            throw new InvalidOperationException("Invalid boolean value");
        }

        public double GetDouble(params string[] id)
        {
            string result = Get(id);
            return double.Parse(result, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
