using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace PluginInterface
{
    public interface IPlugin
    {
        void doOperation();
        Button getPluginButton();
    }
}
