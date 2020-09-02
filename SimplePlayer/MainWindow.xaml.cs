using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using MJPEG.Core;

namespace SimplePlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker _worker = new BackgroundWorker();

        private StreamDecoder _decoder = new StreamDecoder("http://83.128.74.78:8083/mjpg/video.mjpg");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            _decoder.StartDecodingAsync();

            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerAsync();
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                // TODO temporary
                Thread.Sleep(1000); 

                var frame = _decoder.GetLastFrame();

                if (frame == null) continue;

                Dispatcher.Invoke(() => player.Source = frame.ToImageSource());
            }
        }
    }
}
