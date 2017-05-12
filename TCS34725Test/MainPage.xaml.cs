using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace TCS34725Test
{
    public sealed partial class MainPage : Page
    {
        TCS34725 _tcsDevice;

        public MainPage()
        {
            this.InitializeComponent();
            _tcsDevice = new TCS34725();
            _tcsDevice.Init();

            new Timer(this.TimerCallback, null, 0, 1);
        }

        private void TimerCallback(object state)
        {
            if (_tcsDevice._i2c != null)
            {
                _tcsDevice.WriteRegister(0x00, 0x03);
                _tcsDevice.WriteRegister(0x0F, 0x02);
                byte b = _tcsDevice.ReadByte(0x12);
                Debug.WriteLine(b);
                this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    bg.Background = new SolidColorBrush(_tcsDevice.ReadColor());
                });
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
