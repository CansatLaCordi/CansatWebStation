using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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

namespace Cansat.Station
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Controls.Ribbon.RibbonWindow
    {
        VM.MainWindowViewModel vm;
        Image btn;
        BitmapImage bmp;
        public VM.MainWindowViewModel ViewModel
        {
            get { return vm; }
            set { vm = value; DataContext = value; }
        }
        public MainWindow()
        {
            ViewModel = new VM.MainWindowViewModel();
            InitializeComponent();
            InitMaps();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            bmp =new BitmapImage(new Uri("Icons/marker.png", UriKind.Relative));

        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Route")
            {
                AddlastMarker();
            }
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.StopListening();
        }

        void InitMaps()
        {
            MainMap.Manager.Mode = GMap.NET.AccessMode.ServerAndCache;
            //MessageBox.Show("Cache only");
            MainMap.MapProvider = GMapProviders.BingMap;
            MainMap.MinZoom = 3;
            MainMap.MaxZoom = 18;
            MainMap.Zoom = 12;

            MainMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            MainMap.ShowTileGridLines = false;
            //MainMap.Position = new PointLatLng(20.5713651, -103.6362335);
            MainMap.CanDragMap = true;
            // add your custom map db provider
            GMap.NET.CacheProviders.MsSQLPureImageCache ch = new GMap.NET.CacheProviders.MsSQLPureImageCache();
            ch.ConnectionString = ConfigurationManager.ConnectionStrings["CansatMapCache"].ConnectionString;



        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }

        void AddlastMarker()
        {
            Dispatcher.Invoke(() =>
            {
                //if (btn == null)

                var off = new Point(-12.5, -25);
                if (vm.Route.Count > 0)
                {
                    btn = new Image();
                    btn.Width = 25;
                    btn.Height = 25;
                    btn.Source =  bmp;
                    var m = vm.Route.Last();
                    //m.Position = new PointLatLng(20.5713651, -103.6362335);
                    m.Offset = off;
                    m.ZIndex = int.MaxValue;
                    m.Shape = btn;
                    MainMap.Position = m.Position;
                    MainMap.Markers.Add(m);
                    //MainMap.Zoom = 15;
                }

            });



        }
        private double GetRandomNumber(double v1, double v2)
        {
            var r = new Random();
            return Math.Min(v1, v2) + Math.Abs(v1 - v2) * r.NextDouble();
        }

        private void RibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        private void RibbonGallery_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainMap.MapProvider = (GMapProvider)e.NewValue;
        }


    }
}
