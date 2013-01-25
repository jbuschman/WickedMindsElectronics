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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.ComponentModel;

namespace WME.ARDrone
{
    /// <summary>
    /// Interaction logic for WMEDronePlayer.xaml
    /// </summary>
    [ToolboxBitmap(typeof(WMEDronePlayer))]
    public partial class WMEDronePlayer : UserControl
    {
        private DispatcherTimer tmrVideo;
        private WMEVideoAPI.IplImage cvImage;
        private bool playing = false;
        private const double DPI = 96.0;

        public WMEDronePlayer()
        {
            InitializeComponent();
        }

        public bool Playing { get { return playing; } }

        public int Start()
        {
            int result;
            result = WMEVideoAPI.DroneOpen();

            if (result == 0)
            {
                tmrVideo.Start();
                playing = true;
            }
            else
            {
                tmrVideo.Stop();
                playing = false;
            }
            return result;
        }

        public void Stop()
        {
            if (tmrVideo != null && tmrVideo.IsEnabled)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(@"/WMEDronePlayer;component/Images/ardrone2.jpg", UriKind.Relative);
                logo.EndInit();
                imageVideo.Source = logo;
                tmrVideo.Stop();
                txtInfo.Text = string.Empty;
                WMEVideoAPI.DroneClose();
            }
            this.Width = 640;
            this.Height = 360;
            imageVideo.Width = 640;
            imageVideo.Height = 360;
            playing = false;
        }

        private void tmrVideo_Tick(object sender, EventArgs e)
        {
            // Get the latest OpenCV image decoded by WMEDroneAPI.dll as a managed OpenCV image object
            cvImage = WMEVideoAPI.IplImage.FromDrone();
            this.Width = cvImage.width;
            this.Height = cvImage.height;
            imageVideo.Width = cvImage.width;
            imageVideo.Height = cvImage.height;

            // update the displayed video frame by creating a BitmapSource from the OpenCV image
            BitmapSource bSrc = BitmapSource.Create(cvImage.width, cvImage.height, DPI, DPI, PixelFormats.Bgr24, null, cvImage.imageData, cvImage.RawStride);
            imageVideo.Source = bSrc;
            txtInfo.Text = string.Format("{0} x {1}", cvImage.width, cvImage.height);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tmrVideo = new DispatcherTimer();
            tmrVideo.Interval = TimeSpan.FromMilliseconds(33);
            tmrVideo.Tick += tmrVideo_Tick;
        }
    }
}
