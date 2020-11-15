using InterfacePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace txt_plugin
{
    [PluginInfoAttribute(PluginName = "Text file viewer", FileFormat = ".txt")]
    public class CSharpModule : IAppFunctionality   {
        public void IncludeIt() {
            MessageBox.Show("You have just included plugin!");
        }
        public void OpenFile(string path) {
            TextViewForm form = new TextViewForm();
            form.Show();
            FileInfo file = new FileInfo(path);
            using (StreamReader streader = file.OpenText()) {
                string input = null;
                while ((input = streader.ReadLine()) != null) {
                    form.textBox1.Text += input + "\r\n";
                }
            }
        }
    }
}
