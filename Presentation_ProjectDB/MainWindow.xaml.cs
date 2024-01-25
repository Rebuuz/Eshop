using Infrastructure.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation_ProjectDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserService _userService;
        public MainWindow(UserService userService)
        {
            InitializeComponent();
            _userService = userService;

            CreateDemoUser();
        }

        public void CreateDemoUser()
        {
            var result = _userService.CreateUser(new Infrastructure.Dtos.User
            {
                RoleName = "Admin",
                FirstName = "Ulrik",
                LastName = "Lager",
                PhoneNumber = "04454545",
                Email = "ulrik@domain.se",
                StreetName = "Sdffdf",
                City = "Ddfdfd",
                PostalCode =  "dfdf",
                UserName = "Ulriken",
                Password = "Password"  

            });

            if (result)
                MessageBox.Show("Lyckades");
            else
                MessageBox.Show("Något gick fel.");
        }
    }
}