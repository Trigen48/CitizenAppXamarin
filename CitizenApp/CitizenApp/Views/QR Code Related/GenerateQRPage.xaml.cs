using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitizenApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenerateQRPage : ContentPage
    {
        System.Timers.Timer t;


        public GenerateQRPage()
        {
            InitializeComponent();
            t = new System.Timers.Timer();




            t.Interval = 10000; // elapse every 10 sec
            t.Elapsed += T_Elapsed;

            barcodeView.BarcodeOptions.Width = (int)Common.Server.w;
            barcodeView.BarcodeOptions.Height = (int)Common.Server.w;


            genQR();

            t.Start();


        }

        protected override void OnAppearing()
        {

            base.OnAppearing();

        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            genQR();
        }

        private void genQR()
        {
            barcodeView.BarcodeValue = Common.Server.getQRJoin();// Convert.ToBase64String(Encoding.UTF8.GetBytes("newqr/?profile=12121&req=" + DateTime.Now.Ticks.ToString()));
        }

        protected override bool OnBackButtonPressed()
        {
            ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new ContactMainDetail());
            return true;
            // return base.OnBackButtonPressed();
        }

        protected override void OnDisappearing()
        {
            t.Stop();
            base.OnDisappearing();
        }


    }
}