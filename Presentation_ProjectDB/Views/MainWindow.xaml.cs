
using Presentation_ProjectDB.ViewModels;
using System.Windows;


namespace Presentation_ProjectDB.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       
        
        public MainWindow(MainViewModel viewModel) 
        {
            InitializeComponent();
            

            DataContext = viewModel;
        }


       
    }
}