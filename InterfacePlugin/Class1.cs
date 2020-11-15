using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfacePlugin
{
    public interface IAppFunctionality
    {
        void IncludeIt();
        void OpenFile(string path);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PluginInfoAttribute : System.Attribute
    {
        public string PluginName { get; set; }
        public string FileFormat { get; set; }
    }
}
