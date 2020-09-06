using AxisPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using ReactiveUI;
using System.Reactive.Disposables;
using Splat;
using AxisPlayer.Converters;
using MJPEG;

namespace AxisPlayer
{
    using Models;

    /// <summary>
    ///  MainWindow class derives off ReactiveWindow which implements the IViewFor<TViewModel>
    /// interface using a WPF DependencyProperty. We need this to use WhenActivated extension
    /// method that helps us handling View and ViewModel activation and deactivation.
    /// </summary>
    public partial class MainView : ReactiveWindow<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            ViewModel = new MainViewModel(
                Locator.Current.GetService<IInsecamSelector>()
                //Locator.Current.GetService<IStreamDecoder>()
                );

            // We create our bindings here. These are the code behind bindings which allow 
            // type safety. The bindings will only become active when the Window is being shown.
            // We register our subscription in our disposableRegistration, this will cause 
            // the binding subscription to become inactive when the Window is closed.
            // The disposableRegistration is a CompositeDisposable which is a container of 
            // other Disposables. We use the DisposeWith() extension method which simply adds 
            // the subscription disposable to the CompositeDisposable.
            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Page,
                    view => view.Page.Text)
                    .DisposeWith(disposableRegistration);

                this.BindCommand(ViewModel,
                    viewModel => viewModel.Prev,
                    view => view.ButtonPrev)
                    .DisposeWith(disposableRegistration);

                this.BindCommand(ViewModel,
                    viewModel => viewModel.Next,
                    view => view.ButtonNext)
                    .DisposeWith(disposableRegistration);


                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Sources,
                    view => view.List.ItemsSource)
                    .DisposeWith(disposableRegistration);
            });
        }
    }
}
