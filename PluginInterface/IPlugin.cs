using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PluginInterface
{
    public interface IPlugin
    {
        void doOperation(ref TransformedBitmap image);
        Button getPluginButton();
    }
}
