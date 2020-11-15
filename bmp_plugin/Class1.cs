using InterfacePlugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bmp_plugin
{
    [PluginInfoAttribute(PluginName = "Bit map picture file viewer", FileFormat = ".bmp")]
    public class CSharpModule : IAppFunctionality
    {
        public void IncludeIt() {
            MessageBox.Show("You have just included plugin!");
        }

        public void OpenFile(string path) {
            Form1 form = new Form1();
            form.Show();
            Bitmap bitmap = new Bitmap(path);
            form.pictureBox1.Image = bitmap;
        }
    }
}
