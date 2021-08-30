using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CitizenApp.Common;
namespace CitizenApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactAddPage : ContentPage
    {
        System.Threading.Thread th;
        public ContactAddPage()
        {
            InitializeComponent();


            th = new System.Threading.Thread(() =>
            {
                Common.Net.Response.ContactAddResponse response = null;

                try
                {
                    response = Common.Server.SendRequest<Common.Net.Response.ContactAddResponse>("account/addcontact", new Common.Net.Request.ContactAddRequest() { AccountKey = Common.Server.contactRequest });
                }
                catch (Exception ex)
                {

                }


                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {

                    if (response != null)
                    {

                        switch (response.code)
                        {
                            case 6:
                                break;

                            case 1:



                                if (response.status == true)
                                {
                                    if (Server.saveContact(response.ContactData))
                                    {
                                        ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new ContactMainDetail());

                                        return;
                                    }
                                    else
                                    {
                                        DisplayAlert("Error", "Invalid contact data", "ok");
                                    }
                                }
                                else
                                {
                                    DisplayAlert("Error", response.message, "ok");
                                }
                                break;

                            default:
                                DisplayAlert("Error", response.message, "ok");
                                break;
                        }

                    }
                    else
                    {
                        DisplayAlert("Error", "Could not connect to the server", "ok");
                    }

                    ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new NewContactPage());

                });
            });

            th.Start();

        }

        protected override bool OnBackButtonPressed()
        {

            if (th != null && th.IsAlive && th.ThreadState == System.Threading.ThreadState.Running)
            {
                th.Abort();
            
            }

            ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new GenerateQRPage());

            return true;
          
        }

    }
}