using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AxisPlayer.Controls
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        public Player()
        {
            InitializeComponent();
        }

        [Category("General")]
        public byte[] Frame
        {
            get { return (byte[])GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Frame.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(byte[]), typeof(Player), new PropertyMetadata(null));



        public string Location
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(Player), new PropertyMetadata(null));


    }
}
