using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public bool UndoEnable
        {
            get
            { 
                return isUndoEnable;
            }
            set
            {
                isUndoEnable = value;
                NotifyPropertyChanged("UndoEnable");
            }
        }

        public bool RedoEnable
        {
            get 
            {
                return isRedoEnable;
            }
            set
            {
                isRedoEnable = value;
                NotifyPropertyChanged("RedoEnable");
            }
        }

        private BitmapImage baseImage;
        private BitmapSource currentImage;
        private Dictionary<Button, IPlugin> loadedPlugins;
        private Stack<IPlugin> undoStack;
        private Stack<IPlugin> redoStack;
        private bool isUndoEnable;
        private bool isRedoEnable;

        public MainWindow()
        {
            InitializeComponent();

            loadedPlugins = new Dictionary<Button,IPlugin>();
            loadPlugins();

            undoStack = new Stack<IPlugin>();
            redoStack = new Stack<IPlugin>();

            undoButton.DataContext = this;
            redoButton.DataContext = this;
        }

        private void loadPlugins()
        {
            foreach (string dll in Directory.GetFiles("./plugins", "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                foreach (Type type in assembly.GetTypes())
                {
                    toolBar.Items.Add(new Separator());
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

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                baseImage = new BitmapImage(new Uri(dlg.FileName));
                imageView.Source = baseImage;
                currentImage = baseImage;

                undoStack.Clear();
                UndoEnable = false;

                redoStack.Clear();
                RedoEnable = false;
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

            UndoEnable = true;
            RedoEnable = false;
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            currentImage = baseImage;

            IPlugin undoneOperation = undoStack.Pop();
            redoStack.Push(undoneOperation);

            foreach (IPlugin operation in undoStack)
            {
                currentImage = operation.doOperation(currentImage);
            }

            imageView.Source = currentImage;

            updateIsUndoEnable();
            RedoEnable = true;
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            IPlugin redoneOperation = redoStack.Pop();
            undoStack.Push(redoneOperation);
            currentImage = redoneOperation.doOperation(currentImage);
            imageView.Source = currentImage;

            updateIsUndoEnable();
            updateIsRedoEnable();
        }

        private void updateIsUndoEnable()
        {
            UndoEnable = undoStack.Count != 0;
        }

        private void updateIsRedoEnable()
        {
            RedoEnable = redoStack.Count != 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }    
    }
}
