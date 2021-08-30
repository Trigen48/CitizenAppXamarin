using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitizenApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactMainMaster : ContentPage
    {
        public ListView ListView;

        public ContactMainMaster()
        {
            InitializeComponent();

            BindingContext = new ContactMainMasterViewModel();
            ListView = MenuItemsListView;
        }

        class ContactMainMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ContactMainMasterMenuItem> MenuItems { get; set; }

            public ContactMainMasterViewModel()
            {
                MenuItems = new ObservableCollection<ContactMainMasterMenuItem>(new[]
                { new ContactMainMasterMenuItem { Id = 0, Title = "Profile" ,  TargetType= typeof(ProfilePage)},
                    new ContactMainMasterMenuItem { Id = 1, Title = "New Contact",  TargetType= typeof(NewContactPage) },
                    new ContactMainMasterMenuItem { Id = 2, Title = "Generate QR" ,  TargetType= typeof(GenerateQRPage)},
                   
                    new ContactMainMasterMenuItem { Id = 3, Title = "Sign Out"  },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}