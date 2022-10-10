using carGasPriceTrackerMain.PriceScannerOCR;
using Java.IO;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using System;

namespace carGasPriceTrackerMain
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var readJson = new DBScanner();
            System.Console.WriteLine(readJson.GetMainData());
        }
    }
}
