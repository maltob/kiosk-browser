using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiosky.Settings
{
    public class Settings
    {
        public string Title { get; set; }
        public string StartURL { get; set; }
        public string[] CycleURLs { get; set; }
        public int CycleTime { get; set; }
        public string[] AllowedDomains { get; set; }
        public string[] BlockPrograms { get; set; }
        public string[] AllowedPrograms { get; set; }
    }
}
