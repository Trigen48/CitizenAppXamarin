using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using CitizenApp.Common;
namespace CitizenApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        System.Threading.Thread th;
        Common.Contact profileContact;
        public ProfilePage(Common.Contact contact, bool isNotUser = true)
        {
            InitializeComponent();

            profileContact = contact;
            txtName.IsReadOnly = isNotUser;
            txtLastname.IsReadOnly = isNotUser;
            txtContactNumber.IsReadOnly = isNotUser;
            txtContactNumberWork.IsReadOnly = isNotUser;


            txtEmailaddress.IsReadOnly = isNotUser;
            txtOrgName.IsReadOnly = isNotUser;

            txtLinkedIn.IsReadOnly = isNotUser;
            txtTwitter.IsReadOnly = isNotUser;
            txtInstagram.IsReadOnly = isNotUser;
            txtWhatsappNo.IsReadOnly = isNotUser;

            txtName.Text = profileContact.Name;
            txtLastname.Text = profileContact.Lastname;
            txtContactNumber.Text = profileContact.ContactNumberHome;
            txtContactNumberWork.Text = profileContact.ContactNumberWork;
            txtOrgName.Text = profileContact.Organization;


            txtEmailaddress.Text = profileContact.EmailAddress;


            if (isNotUser == false)
            {
                txtLinkedIn.Text = profileContact.LinkedInHandle;
                txtTwitter.Text = profileContact.TwitterHandle;
                txtInstagram.Text = profileContact.InstagramHandle;
                txtWhatsappNo.Text = profileContact.WhatsAppNo;
                Title = "My Profile";
                lblTitle.Text = "My Profile";
            }
            else
            {
                txtLinkedIn.Text = ConcatIf(profileContact.LinkedInHandle, "https://linkedin.com/");
                txtTwitter.Text = ConcatIf(profileContact.TwitterHandle, "https://twitter.com/");
                txtInstagram.Text = ConcatIf(profileContact.InstagramHandle, "https://instagram.com/");
                txtWhatsappNo.Text = profileContact.WhatsAppNo;

                Title = txtName.Text + " " + txtLastname.Text;
                lblTitle.Text = Title+" Contact";
            }

            btnUpdate.IsVisible = isNotUser != true;
            btnUpdate.Clicked += BtnUpdate_Clicked;
        }

        private string ConcatIf(string value, string add)
        {
            if(value!=null && value.Trim()!="")
            {
                return add + value;
            }

            return value;
        }
        private void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            if (profileContact.AccountKey != Common.Server.getAccount().AccountKey)
            {
                DisplayAlert("profile error", "Cannot update profile", "OK");

                return;
            }

            string name = txtName.Text != null ? txtName.Text.Trim() : "";
            string lastname = txtLastname.Text != null ? txtLastname.Text.Trim() : "";
            string contactnumber = txtContactNumber.Text != null ? txtContactNumber.Text.Trim() : "";


            string worknumber= txtContactNumberWork.Text  != null ? txtContactNumberWork.Text.Trim() : "";
            string organization = txtOrgName.Text != null ? txtOrgName.Text.Trim() : "";

            string email =txtEmailaddress.Text != null ? txtEmailaddress.Text.Trim() : "";

            string linkedin=txtLinkedIn.Text != null ? txtLinkedIn.Text.Trim() : "";
            string twitter =txtTwitter.Text != null ? txtTwitter.Text.Trim() : "";
            string instagram=txtInstagram.Text != null ? txtInstagram.Text.Trim() : "";
            string whatsapp=txtWhatsappNo.Text != null ? txtWhatsappNo.Text.Trim() : "";


            if (Validator.IsNameOrLastname(name) == false)
            {
                DisplayAlert("Update Profile error", "Please enter a valid name 2 - 200 characters", "ok");
            }
            else if (Validator.IsNameOrLastname(lastname) == false)
            {
                DisplayAlert("Update Profile error", "Please enter a valid lastname 5 - 200 characters", "ok");
            }
            else if (Validator.IsCellNumber(contactnumber) == false)
            {
                DisplayAlert("Update Profile error", "Please enter a valid contact number 10 - 13 digits", "ok");
            }
            else if (worknumber.Length > 0 && Validator.IsCellNumber(worknumber) == false)
            {
                DisplayAlert("Update Profile error", "Please enter a valid work number 10 - 13 digits", "ok");
            }
            else if (email.Length > 0 && Validator.IsEmail(email) == false)
            {
                DisplayAlert("Update Profile error", "Please enter a valid email address", "ok");
            }
            else if (organization.Length > 0 && (organization.Length < 3 || organization.Length > 200))
            {
                DisplayAlert("Update Profile error", "Please enter a valid organization name", "ok");
            }
            else if (linkedin.Length > 0 && (linkedin.Length < 3 || linkedin.Length > 200))
            {
                DisplayAlert("Update Profile error", "Please enter a valid linkedin handle", "ok");
            }

            else if (twitter.Length > 0 && (twitter.Length < 3 || twitter.Length > 200))
            {
                DisplayAlert("Update Profile error", "Please enter a valid twitter handle", "ok");
            }

            else if (instagram.Length > 0 && (instagram.Length < 3 || instagram.Length > 200))
            {
                DisplayAlert("Update Profile error", "Please enter a valid instagram handle", "ok");
            }

            else if (whatsapp.Length > 0 && Validator.IsCellNumber(whatsapp) == false)
            {
                DisplayAlert("Update Profile error", "Please enter a valid whatsapp number 10 - 13 digits", "ok");
            }


            else
            {
                showLoader();
                var profileData = new Common.Net.Request.EditProfileRequest();


                
                profileData.Name = name;
                profileData.Lastname = lastname;
                profileData.ContactNumberHome = contactnumber;
                profileData.ContactNumberWork = worknumber;
                profileData.Organization = organization;

                profileData.EmailAddress = email;
                profileData.LinkedInHandle = linkedin;
                profileData.WhatsAppNo = whatsapp;
                profileData.TwitterHandle = twitter;
           
                profileData.InstagramHandle = instagram;
            
           

                th = new Thread(() =>
                {

                    Common.Net.Response.EditProfileResponse response = null;
                    try
                    {
                        response = Server.SendRequest<Common.Net.Response.EditProfileResponse>("account/editprofile/", profileData);
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
                                        if (response.AccountData.AccountKey == Server.getAccount().AccountKey)
                                        {
                                            Server.saveAccount(response);
                                        }
                                        DisplayAlert("Success", response.message, "ok");
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
            layoutProfile.IsVisible = true;
            layoutLoading.IsVisible = false;

        }

        private void showLoader()
        {

            layoutProfile.IsVisible = false;
            layoutLoading.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {

            if (th != null && th.IsAlive && th.ThreadState == System.Threading.ThreadState.Running)
            {
                th.Abort();
                hideLoader();
                return true;
            }

            ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new ContactMainDetail());

            return true;

        }
    }
}