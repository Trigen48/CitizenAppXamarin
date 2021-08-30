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
    public partial class ContactMain : MasterDetailPage
    {
        public ContactMain()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ContactMainMasterMenuItem;
            if (item == null)
                return;


            if (item.Id == 0)
            {
                ((ContactMain)App.Current.MainPage).Detail = new NavigationPage(new ProfilePage(Common.Server.getAccount(), false));
                IsPresented = false;
            }
            else if (item.Id == 3)
            {
                Common.Server.ClearAll();
                App.Current.MainPage = new SignInPage();
                return;
            }
            else if (item.TargetType != null)
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
                IsPresented = false;
            }


            MasterPage.ListView.SelectedItem = null;
        }
    }
}