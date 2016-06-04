using PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SepiaPlugin
{
    public class Greyscale: IPlugin
    {
        public TransformedBitmap doOperation(BitmapSource image)
        {
            TransformedBitmap tmp = new TransformedBitmap();
            tmp.BeginInit();
            tmp.Source = new FormatConvertedBitmap(image, PixelFormats.Gray8, BitmapPalettes.Gray256, 0.0);
            tmp.EndInit();
            return tmp;
        }

        public Button getPluginButton()
        {
            Button result = new Button();
            Label label = new Label();
            label.Content = "Grayscale";
            result.Content = label;

            return result;
        }
    }
}
