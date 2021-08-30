using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;
namespace CitizenApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewContactPage : ZXingScannerPage
    {
        public NewContactPage()
        {
            InitializeComponent();

            this.OnScanResult += Handle_OnScanResult;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            IsScanning = false;
        }

        void returnFromView()
        {
            ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new ContactAddPage());
        }
        protected override bool OnBackButtonPressed()
        {
            ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new ContactMainDetail());
            return true;
            // return base.OnBackButtonPressed();
        }

        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread( ()=>
            {

            

                if (result.Text.Length == 129)
                {
                    var contact = Common.Server.getContact(result.Text);
                    if (contact != null)
                    {
                        IsScanning = false;
                        DisplayAlert("Contact error", "Contact already exists", "OK");
                        IsScanning = true;
                    }
                    else
                    {
                        IsScanning = false;
                        Common.Server.contactRequest = result.Text;
                        returnFromView();
                    }
                }
            


            });
        }
    }
}