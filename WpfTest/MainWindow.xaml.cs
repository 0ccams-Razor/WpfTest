using System;
using System.Collections.Generic;
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
using WpfTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<int> results1;
        List<int> results2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            titletb.Text = name.Text;
        }




        public async Task<T> RunAsync<T>(Func<Task<T>> func)
        {
            return await func();
        }

        private async void parrallel_Click(object sender, RoutedEventArgs e)
        {
            string[] urls =
            {
                "www.stackoverflow.com",
                "www.google.net",
                "www.facebook.com",
                "www.twitter.com"
            };

            List<int> inputs1 = makeIntList();
            List<int> inputs2 = makeIntList();

            try {
                parrallel.IsEnabled = false;
                Mouse.OverrideCursor = Cursors.Wait;

                // save the context and return here after task is done
                await Task.Run(() =>
                {
                    System.Threading.Tasks.Parallel.Invoke(
                        () => PingUrl(urls[0]),
                        () => PingUrl(urls[1]),
                        () => PingUrl(urls[2]),
                        () => PingUrl(urls[3]),

                        () => results1 = DoWork(inputs1),
                        () => results2 = DoWork(inputs2)

                    );
                });
                // back on UI-context here
                UpdateUI();
            }
            finally
            {
                parrallel.IsEnabled = true;
                Mouse.OverrideCursor = null;
            }

        }

        public List<int> DoWork(List<int> input)
        {
            List<int> primeNumbers = new List<int>();
            foreach (int number in input)
            {
                if (TPL.IsPrime(number))
                {
                    primeNumbers.Add(number);
                }
            }
            return primeNumbers;
        }

        public void UpdateUI()
        {
            name.Text = "Done";
            parallelOutput.Text = "";
            foreach (int number in results1)
            {
                parallelOutput.Text += number.ToString();
            }
        }

        void PingUrl(string url)
        {
            var ping = new System.Net.NetworkInformation.Ping();

            var result = ping.Send(url);

            if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Console.WriteLine(string.Format("{0} is online", url));
            }
        }

        public List<int> makeIntList()
        {
            List<int> returnList = new List<int>();
            for(int i = 0; i < 100; i++)
            {
                returnList.Add(i);
            }
            return returnList;
        }
    }


}
