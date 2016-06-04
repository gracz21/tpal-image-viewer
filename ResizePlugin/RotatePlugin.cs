using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace RotatePlugin
{
    public class RotatePlugin: IPlugin
    {
        public TransformedBitmap doOperation(TransformedBitmap image)
        {
            TransformedBitmap tb = new TransformedBitmap();
            tb.BeginInit();
            tb.Source = image;
            tb.Transform = new RotateTransform(90);
            tb.EndInit();
            return tb;
        }

        public Button getPluginButton()
        {
            Button result = new Button();
            Label label = new Label();
            label.Content = "Rotate";
            result.Content = label;

            return result;
        }
    }
}
