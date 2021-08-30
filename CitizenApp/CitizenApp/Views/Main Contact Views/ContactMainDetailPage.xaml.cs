using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CitizenApp.Common;
using CitizenApp.Common.Net.Request;
namespace CitizenApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactMainDetail : ContentPage
    {
        System.Threading.Thread th;
        private ObservableCollection<Contact> contactList;

        public ObservableCollection<Contact> ContactList
        {
            get { return contactList; }
            set { contactList = value; }
        }

        public ContactMainDetail()
        {
            InitializeComponent();
            ContactList = new ObservableCollection<Contact>();


            refreshContact();

            ContactListView.ItemsSource = ContactList;

            ContactListView.ItemSelected += ContactListView_ItemSelected;

        }

        private void ContactListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
         
                var contact = (Contact)e.SelectedItem;

                ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new ProfilePage(contact));


            }
            catch
            {

            }
        }

        public void refreshContact()
        {

            //ContactListView.ItemsSource= null;
            ContactList.Clear();
 
            if (Server.hasContacts())
            {
                var contacts = Server.GetContacts();

                foreach (Contact contact in contacts)
                {
                    ContactList.Add(contact);
                }
               
            }
        }
       
        public void cntMenu_Delete(object sender, EventArgs e)
        {

            var mi = sender as MenuItem;
            var contact = (Contact)mi.CommandParameter;
            


            showLoader();

            th = new System.Threading.Thread(() =>
            {

                Common.Net.Response.ContactResponse response = null;
                try
                {
                    response = Server.SendRequest<Common.Net.Response.ContactResponse>("account/removecontact", new RemoveContactRequest() { AccountKey = contact.AccountKey });
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
                                    ContactList.Remove(contact);
                                    Server.removeContact(contact.AccountKey);
                                    DisplayAlert("Success", response.message, "ok");
                                    refreshContact();
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
        protected override void OnDisappearing()
        {
            contactList.Clear();
            base.OnDisappearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            showLoader();
     
            th = new System.Threading.Thread(() =>
            {

                Common.Net.Response.ContactResponse response = null;
                try
                {
                    response = Server.SendRequest<Common.Net.Response.ContactResponse>("account/contacts", null);
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
                                    if (Server.SaveContacts(response.Contacts))
                                    {     
                                        refreshContact();
                                    }
                                    else
                                    {
                                        DisplayAlert("Error", "Invalid contacts data", "ok");
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

        private void hideLoader()
        {
            ContactListView.IsVisible = true;
            layoutLoading.IsVisible = false;
        }

        private void showLoader()
        {

            ContactListView.IsVisible = false;
            layoutLoading.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {

            if (th != null && th.IsAlive && th.ThreadState == System.Threading.ThreadState.Running)
            {
                th.Abort();
                hideLoader();
                return false;
            }
            return base.OnBackButtonPressed();
        }

    }
}