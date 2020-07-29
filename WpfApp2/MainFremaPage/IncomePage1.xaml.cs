using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
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
using WpfApp2.Helpers;

namespace WpfApp2.MainFrame
{
    /// <summary>
    /// Interaction logic for IncomePage1.xaml
    /// </summary>
    /// 
    public class Rate
    {
        [JsonProperty("ccy")]
        public string Ccy { get; set; }

        [JsonProperty("base_ccy")]
        public string BaseCcy { get; set; }

        [JsonProperty("buy")]
        public double Buy { get; set; }

        [JsonProperty("sale")]
        public string Sale { get; set; }
    }
    public partial class IncomePage1 : Page
    {
        ProductsHelper helper = new ProductsHelper();
        List<Rate> rates;
        double calculatedIncome;
        public IncomePage1()
        {
            InitializeComponent();
        }

        async Task GetIncomeAsync()
        {
            calculatedIncome = await helper.GetPricesSumAsync();
            incomeLabel.Content = calculatedIncome;
            await Task.Run(() => { rates = GetCurrencyRates(); });
            try
            {
                DollarLabel.Content  =  (calculatedIncome / rates[0].Buy).ToString("F2");
                EuroLabel.Content    =  (calculatedIncome / rates[1].Buy).ToString("F2");
                RubleLabel.Content   =  (calculatedIncome / rates[2].Buy).ToString("F2");
                BitcoinLabel.Content =  (decimal)(calculatedIncome / rates[0].Buy / rates[3].Buy);
            }
            catch
            {
                DollarLabel.Content  = 0;
                EuroLabel.Content    = 0;
                RubleLabel.Content   = 0;
                BitcoinLabel.Content = 0;

                MessageBox.Show("Не вдалося конвертувати валюти!");
            }

        }

        List<Rate> GetCurrencyRates()
        {
            string json;
            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5");
            }

            return JsonConvert.DeserializeObject<List<Rate>>(json);
        }

        async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await GetIncomeAsync();
        }
    }
}
