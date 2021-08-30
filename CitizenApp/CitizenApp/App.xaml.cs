using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitizenApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            if(Common.Server.loadAccount() && Common.Server.getAccount()!=null)
            {


                if(Common.Server.getAccount().AccountSession.LoginExpire>DateTime.Now)
                {
                    Common.Server.loadContacts();
                    MainPage = new ContactMain();
                    return;
                }
             
            }

            MainPage = new SignInPage();
            //MainPage = new MainPage();
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
