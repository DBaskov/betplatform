using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceTests
{
    public class AppConfig
    {

        public Dictionary<string, string> Tokens { get; set; }
        public AppConfig()
        {
            Tokens = new Dictionary<string, string>();
            Tokens.Add("twitch-api", "key");
        }
    }
}
