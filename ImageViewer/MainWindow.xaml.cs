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
        private BitmapImage baseImage;
        private TransformedBitmap currentImage;
        private Dictionary<Button, IPlugin> loadedPlugins;
        private Stack<IPlugin> undoStack;
        private Stack<IPlugin> redoStack;

        public MainWindow()
        {
            InitializeComponent();

            baseImage = new BitmapImage(new Uri(@"C:\Users\Kamil\Desktop\Studia\Magisterskie\Semestr I\Technologie programistyczne - aplikacje lokalne\ImageViewer\images\image.jpg"));
            imageView.Source = baseImage;

            currentImage = new TransformedBitmap();
            currentImage.BeginInit();
            currentImage.Source = baseImage;
            currentImage.EndInit();

            loadedPlugins = new Dictionary<Button,IPlugin>();
            loadPlugins();

            undoStack = new Stack<IPlugin>();
            redoStack = new Stack<IPlugin>();
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
                        pluginButton.Click += pluginButton_Click;

                        loadedPlugins.Add(pluginButton, plugin);
                        toolBar.Items.Add(pluginButton);
                    }
                }
            }
        }

        private void pluginButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            IPlugin plugin;
            loadedPlugins.TryGetValue(clickedButton, out plugin);
            currentImage = plugin.doOperation(currentImage);
            imageView.Source = currentImage;
            redoStack.Clear();
            undoStack.Push(plugin);
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            TransformedBitmap tb = new TransformedBitmap();
            tb.BeginInit();
            tb.Source = baseImage;
            tb.EndInit();
            currentImage = tb;

            IPlugin undoneOperation = undoStack.Pop();
            redoStack.Push(undoneOperation);

            foreach (IPlugin operation in undoStack)
            {
                currentImage = operation.doOperation(currentImage);
            }

            imageView.Source = currentImage;
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            IPlugin redoneOperation = redoStack.Pop();
            undoStack.Push(redoneOperation);
            currentImage = redoneOperation.doOperation(currentImage);
            imageView.Source = currentImage;
        }
    }
}
