using Android.SE.Omapi;
using Java.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static Android.Icu.Text.CaseMap;
using static System.IO.StreamReader;
using Console = System.Console;

namespace carGasPriceTrackerMain.PriceScannerOCR
{
    public class DBScanner
    {
        //public string name { get; set; }
        public List<Title> title { get; set; }
        //public List<Price> price { get; set; }

        public IList<Title> GetMainData()
        {
            try
            {
                var assembly = this.GetType().GetTypeInfo().Assembly;
                Stream stream =
                       assembly.GetManifestResourceStream(@"C:\Users\Deimante\source\repos\carGasPriceTrackerMain\carGasPriceTrackerMain\carGasPriceTrackerMain\PriceScannerOCR\prices.json");
                var reader = new StreamReader(stream);
                var kJson = reader.ReadToEnd();
                title = JsonConvert.DeserializeObject<List<Title>>(kJson);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return title;
        }

    }
}
