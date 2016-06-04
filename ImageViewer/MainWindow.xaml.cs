using PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageViewer
{
    public partial class MainWindow : Window
    {
        private BitmapImage image;
        private Type[] loadedTypes;

        public MainWindow()
        {
            InitializeComponent();
            image = new BitmapImage(new Uri(@"C:\Users\Kamil\Desktop\Studia\Magisterskie\Semestr I\Technologie programistyczne - aplikacje lokalne\ImageViewer\images\image.jpg"));
            imageView.Source = image;
            loadPlugins();
        }

        private void loadPlugins()
        {
            foreach (string dll in Directory.GetFiles("./plugins", "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass && type.IsPublic && typeof(IPlugin).IsAssignableFrom(type))
                    {
                        Object obj = Activator.CreateInstance(type);
                        IPlugin plugin = (IPlugin)obj;
                        Button pluginButton = plugin.getPluginButton();
                        toolBar.Items.Add(pluginButton);
                    }
                }
            }
        }
    }
}
