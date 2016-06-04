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
        TransformedBitmap doOperation(TransformedBitmap image);
        Button getPluginButton();
    }
}
