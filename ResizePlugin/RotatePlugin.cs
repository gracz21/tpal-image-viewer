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
        public BitmapSource doOperation(BitmapSource image)
        {
            return new TransformedBitmap(image, new RotateTransform(90));
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
