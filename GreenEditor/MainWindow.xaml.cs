using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Win32;

namespace GreenEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.AddCommand(WallCommand);
            this.AddCommand(PlayerCommand);
            this.AddCommand(ExitCommand);
            this.AddCommand(SpriteCommand);
        }

        void AddCommand(IEditorCommand command)
        {
            Action<object> execute = (_) =>
            {
                commandListBox.IsEnabled = false;
                command.Execute(worldDisplay);
                commandListBox.IsEnabled = true;
            };

            Button item = new Button();
            item.Content = new TextBlock { Text = command.GetDescription() };
            item.Command = new RelayCommand(execute, (_) => !worldDisplay.IsBusy );

            commandListBox.Items.Add(item);
        }

        void SaveData(object sender, RoutedEventArgs e)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("map",
                new XAttribute("gravity", "0 30"),
                new XAttribute("next-level", "map"));

            foreach (var obj in worldDisplay.Objects)
            {
                root.Add(obj.Data);
            }

            doc.Add(root);
            doc.Save("map.xml");
            MessageBox.Show("Saved");
        }

        void LoadData(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            bool? result = ofd.ShowDialog();
            if (result == null || !result.Value) { return; }

            worldDisplay.Objects.Clear();
            XDocument doc = XDocument.Load(ofd.FileName);
            XElement root = doc.Element("map");

            foreach (var elem in root.Elements())
            {
                WorldObject obj = new WorldObject(elem.Name.LocalName);
                obj.Data = elem;
                obj.SetShapeFromData();
                
                obj.DisplayBrush = Brushes.DarkGray;
                if (elem.Name == "sprite")
                { obj.DisplayBrush = new ImageBrush(new BitmapImage(new Uri(elem.Attribute("texture").Value))); }
                worldDisplay.AddObject(obj);
            }

            MessageBox.Show("Barely Loaded");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (worldDisplay.IsBusy) { return; }
            if (e.Key == Key.W)
            {
                WallCommand.Execute(worldDisplay); // winner of `international ugly code competition`.            
            }
            else if (e.Key == Key.E)
            {
                ExitCommand.Execute(worldDisplay);
            }
            else if (e.Key == Key.P)
            {
                PlayerCommand.Execute(worldDisplay);
            }

            base.OnKeyDown(e);
        }

        readonly WallCommand WallCommand = new WallCommand();
        readonly ExitCommand ExitCommand = new ExitCommand();
        readonly PlayerCommand PlayerCommand = new PlayerCommand();
        readonly SpriteCommand SpriteCommand = new SpriteCommand();
    }
}
