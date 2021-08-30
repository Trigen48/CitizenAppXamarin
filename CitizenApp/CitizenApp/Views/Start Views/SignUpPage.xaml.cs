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
    public partial class SignUpPage : ContentPage
    {
        Thread th;
        public SignUpPage()
        {
            InitializeComponent();

           
            btnRegister.Clicked += BtnRegister_Clicked;

            //btnLogin.Clicked += BtnLogin_Clicked;

            var loginGesture = new TapGestureRecognizer();
            loginGesture.Tapped += (s, e) =>
            {
                BtnLogin_Clicked(null, null);
            };
            btnLogin.GestureRecognizers.Add(loginGesture);

        }


        private void BtnRegister_Clicked(object sender, EventArgs e)
        {
            SetPrivateWidth();


            string name = txtName.Text != null ? txtName.Text.Trim() : "";
            string lastname = txtLastname.Text != null ? txtLastname.Text.Trim() : "";
            string contactnumber = txtContactNumber.Text != null ? txtContactNumber.Text.Trim() : "";
            string username = txtUsername.Text != null ? txtUsername.Text.Trim() : "";
            string password = txtPassword.Text != null ? txtPassword.Text.Trim() : "";
            string organization = txtOrgName.Text != null ? txtOrgName.Text.Trim() : "";

            if (Validator.IsNameOrLastname(name) == false)
            {
                DisplayAlert("Sign up error", "Please enter a valid name 2 - 200 characters", "ok");
            }
            else if (Validator.IsNameOrLastname(lastname) == false)
            {
                DisplayAlert("Sign up error", "Please enter a valid lastname 2 - 200 characters", "ok");
            }
            else if (Validator.IsCellNumber(contactnumber) == false)
            {
                DisplayAlert("Sign up error", "Please enter a valid contact number 10 - 13 digits", "ok");
            }
            else if (organization.Length > 0 && (organization.Length < 2 || organization.Length > 200))
            {
                DisplayAlert("Update Profile error", "Please enter a valid organization name", "ok");
            }
            else if (username.Length < 5 || username.Length > 15 || Validator.IsUsername(username) == false)
            {
                DisplayAlert("Sign up error", "Please enter a valid username 5 - 15 characters", "ok");
            }
            else if (password.Length < 5 || password.Length > 20)
            {
                DisplayAlert("Sign Up", "Please enter a valid username 5 - 15 characters", "ok");
            }
            else
            {
                showLoader();
                var registerData = new Common.Net.Request.RegisterRequest();

                registerData.Username = username;
                registerData.Password = password;

                registerData.ContactNumberHome = contactnumber;
                registerData.Name = name;
                registerData.Lastname = lastname;
                registerData.Organization = organization;

                th = new Thread(() =>
                {

                    Common.Net.Response.RegisterResponse response = null;
                    try
                    {
                        response = Server.SendRequest<Common.Net.Response.RegisterResponse>("new", registerData);
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
                                        if (Server.saveAccountByRegister(response))
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

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {

            SetPrivateWidth();
            App.Current.MainPage = new SignInPage();
        }


        private void hideLoader()
        {
            layoutRegister.IsVisible = true;
            layoutLoading.IsVisible = false;

        }

        private void showLoader()
        {

            layoutRegister.IsVisible = false;
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