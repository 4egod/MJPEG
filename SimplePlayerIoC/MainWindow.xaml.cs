using SimplePlayer;
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

namespace SimplePlayerIoC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISimpleService _simpleService;

        private BackgroundWorker _worker = new BackgroundWorker();

        public MainWindow(ISimpleService simpleService)
        {
            InitializeComponent();

            _simpleService = simpleService;

            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerAsync();
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                // TODO temporary
                Thread.Sleep(1000);

                var frame = _simpleService.GetLastFrame();

                if (frame == null) continue;

                Dispatcher.Invoke(() => player.Source = frame.ToImageSource());
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            _simpleService.StartDecoding();

            startButton.Visibility = Visibility.Hidden;
        }
    }
}
