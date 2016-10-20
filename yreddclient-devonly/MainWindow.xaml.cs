using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace yreddclient_devonly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            App tt = (yreddclient_devonly.App) App.Current;
            //Title = "aabb";
            //gTitle.Text = "aabbcc";
            InitializeComponent();
        }

        public override void EndInit()
        {
            base.EndInit();
            Title = "yredd dev client";
        }
        private void log(string toAppend)
        {
            logs.Text += "\n" + toAppend;
            logs.ScrollToEnd();
        }
        private async void checkConnection(object sender, RoutedEventArgs e)
        {
            log("Checking connection...");
            var client = new HttpClient();
            try
            {
                var uri = new Uri("http://localhost/");
                var resp = await client.GetAsync(uri);
                if (resp.IsSuccessStatusCode)
                {
                    log("Connection working!");
                }
                else
                {
                    log("Connection working? sC: " + resp.StatusCode);
                }
            }catch(HttpRequestException ERR)
            {
                log("Exception: " + ERR.Message);
       
            }
            
        }

        private async Task<JObject> GetJson(string uri)
        {
            var URI = new Uri(uri);
            var client = new HttpClient();
            try
            {
                var req = await client.GetAsync(URI);
                if (req.IsSuccessStatusCode)
                {
                    var rawString = await req.Content.ReadAsStringAsync();
                    return (JObject) JsonConvert.DeserializeObject(rawString);
                }
                
            }catch(HttpRequestException e) { }
            return null;
        }
        private async void updateMods(object sender, RoutedEventArgs e)
        {
            log("Updating mod listings...");
            var game1 = gSelect.SelectedItem;
            if(game1 == null)
            {
                log("Error!! No game selected??");
            }else
            {
                var game = ((ComboBoxItem)game1).Name;
                var mods = await GetJson("http://localhost/mods/" + game + "/list");
                if (mods != null) {
                    //log(mods.ToString());
                    var mud = (JObject) mods.GetValue("mods");
                    foreach (var mod in mud)
                    {
                        log(mod.Key + ": " + mod.Value);
                    }
                }else
                {
                    log("Failed to fetch " + game + " mods.");
                }
            }
        }
    }
}
