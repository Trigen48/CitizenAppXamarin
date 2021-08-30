using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CitizenApp.Common;

namespace CitizenApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        Thread th;

        public SignInPage()
        {
            InitializeComponent();

            btnLogin.Clicked += BtnLogin_Clicked;



            // btnRegister.Clicked += BtnRegister_Clicked;

  
            var registerGesture = new TapGestureRecognizer();
            registerGesture.Tapped += (s, e) =>
            {
                BtnRegister_Clicked(null, null);
            };
            btnRegister.GestureRecognizers.Add(registerGesture);

        }

        private void BtnRegister_Clicked(object sender, EventArgs e)
        {
            SetPrivateWidth();
            App.Current.MainPage = new SignUpPage();
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            SetPrivateWidth();

            string username = txtUsername.Text!=null ? txtUsername.Text.Trim():"";
            string password = txtPassword.Text != null ? txtPassword.Text.Trim() : "";

            if (username.Length < 5 || username.Length > 15 || Validator.IsUsername(username) == false)
            {
                DisplayAlert("Login error", "Please enter a valid username 5 - 15 characters", "ok");
            }
            else if (password.Length < 5 || password.Length > 20)
            {
                DisplayAlert("Login error", "Please enter a valid username 5 - 15 characters", "ok");
            }
            else
            {
                showLoader();
                var loginData = new Common.Net.Request.LoginRequest();

                loginData.Username = username;
                loginData.Password = password;

                th = new Thread(() =>
                {

                    Common.Net.Response.LoginResponse response = null;
                    try
                    {
                        response = Server.SendRequest<Common.Net.Response.LoginResponse>("login", loginData);
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
                                        if (Server.saveAccountByLogin(response))
                                        {
                                           
                                            App.Current.MainPage = new ContactMain();
                                            return;
                                        }
                                        else
                                        {
                                            DisplayAlert("Error", "Invalid profile data", "ok");
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
                        hideLoader();
                    });

                });

                th.Start();


            }

        }


        private void hideLoader()
        {
            layoutLogin.IsVisible = true;
            layoutLoading.IsVisible = false;

        }

        private void showLoader()
        {

            layoutLogin.IsVisible = false;
            layoutLoading.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {

            if (th != null && th.IsAlive && th.ThreadState == ThreadState.Running)
            {
                th.Abort();
                hideLoader();
                return false;
            }
            return base.OnBackButtonPressed();
        }

        private void SetPrivateWidth()
        {

            double w = 0;


            if (this.Width > this.Height)
            {
                w = this.Width;
            }
            else
            {
                w = this.Height;
            }
            Common.Server.w = w;
        }
    }
}