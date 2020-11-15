using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using InterfacePlugin;

namespace FIleVIewer
{
    public partial class Main : Form
    {
        private List<Plugin> pluginList = new List<Plugin>();
        private List<Setting> settings;

        public Main() {
            InitializeComponent();
            LoadAssamblies();
        }

        private void LoadAssamblies() {
            settings = Setting.readFromFile();
            if (settings == null) settings = new List<Setting>();
            else {
                foreach (var asm in settings) {
                    Assembly assem = Assembly.LoadFrom(asm.AssamblyPath);
                    Plugin plug = new Plugin {
                        IncludedAssambly = assem,
                        Extension = asm.Extension
                    };
                    pluginList.Add(plug);
                }
            }
        }
                    

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK) {
               foreach (Plugin format in pluginList) {
                    if (dlg.FileName.Contains(format.Extension)) {
                        MessageBox.Show("Can open this file.");
                        var theClassTypes = from t in format.IncludedAssambly.GetTypes()
                                            where
                                            t.IsClass && (t.GetInterface("IAppFunctionality") != null)
                                            select t;
                        foreach (Type t in theClassTypes) {
                            IAppFunctionality itfApp = (IAppFunctionality)(format.IncludedAssambly.CreateInstance(t.FullName, true));
                            itfApp.OpenFile(dlg.FileName);
                        }
                        return;
                    }
                }
                MessageBox.Show("Can't opent this file");
            }
        }

        private bool LoadExternalModule(string path) {
            bool foundPlugIn = false;
            Assembly thePlugInAsm = null;
            Setting setting = new Setting();
            Plugin plugin = new Plugin();
            try {
                // Dynamically load the selected assembly. 
                thePlugInAsm = Assembly.LoadFrom(path);
                setting.AssamblyName = thePlugInAsm.GetName().ToString();
                setting.AssamblyPath = path;
                plugin.IncludedAssambly = thePlugInAsm;

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return foundPlugIn;
            }

            // Get all IAppFunctionality compatible classes in assembly.
            var theClassTypes = from t in thePlugInAsm.GetTypes()
                                where t.IsClass && (t.GetInterface("IAppFunctionality") != null)
                                select t;

            // Now, create the object and call IncludeIt() method.
            foreach (Type t in theClassTypes) {
                foundPlugIn = true;
                // Use late binding to create the type. 
                var itfApp = (IAppFunctionality)thePlugInAsm.CreateInstance(t.FullName, true);
                itfApp.IncludeIt();

                // Show extension  info. 
                setting.Extension = DisplayFileFormat(t);
                plugin.Extension = setting.Extension;
                bool isAdded = false;
                foreach (Plugin pl in pluginList) {
                    if (pl.IncludedAssambly.Equals(plugin.IncludedAssambly))
                        isAdded = true;
                }
                if (!isAdded) {
                    pluginList.Add(plugin);
                    settings.Add(setting);
                    Setting.addToFile(settings);
                }
            }
            return foundPlugIn;
        }

        private string DisplayFileFormat(Type t) {
            // Get [File Format] data.
            var fileFormat = from ci in t.GetCustomAttributes(false)
                             where
                             (ci.GetType() == typeof(PluginInfoAttribute))
                             select ci;

            // Show data. 
            string format = "";
            foreach (PluginInfoAttribute c in fileFormat) {
                MessageBox.Show(string.Format("The are {0} file format viewer", c.FileFormat), c.PluginName);
                format = c.FileFormat.ToString();

            }
            return format;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void showPluginsToolStripMenuItem_Click(object sender, EventArgs e) {
            string s = "";
            foreach (var format in settings) s += format.Extension + ";\n";
            MessageBox.Show(s);
        }

        private void addPluginToolStripMenuItem_Click(object sender, EventArgs e) {
            // Allow user to select an assembly to load. 
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK) {
                if (!dlg.FileName.Contains("plugin.dll"))
                    MessageBox.Show("It is NOT Plug-In!");
                else if (!LoadExternalModule(dlg.FileName))
                    MessageBox.Show("Nothing implements InterfacePlugIn!");
            }
        }
    }
}
