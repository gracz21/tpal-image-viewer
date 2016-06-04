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
    public class RotateLeftPlugin : IPlugin
    {
        public BitmapSource doOperation(BitmapSource image)
        {
            return new TransformedBitmap(image, new RotateTransform(-90));
        }

        public Button getPluginButton()
        {
            Button result = new Button();
            Label label = new Label();
            label.Content = "Rotate left";
            result.Content = label;

            return result;
        }
    }

    public class RotateRightPlugin: IPlugin
    {
        public BitmapSource doOperation(BitmapSource image)
        {
            return new TransformedBitmap(image, new RotateTransform(90));
        }

        public Button getPluginButton()
        {
            Button result = new Button();
            Label label = new Label();
            label.Content = "Rotate right";
            result.Content = label;

            return result;
        }
    }
}
