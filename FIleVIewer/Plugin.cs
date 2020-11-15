using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FIleVIewer
{
    class Plugin
    {
        public Assembly IncludedAssambly { get; set; }
        public string Extension { get; set; }
    }
}
