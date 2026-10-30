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
using TechStore.Wpf.Connection;
using TechStore.Wpf.ViewModels;

namespace TechStore.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
           
        }
        private async void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            // Verificăm dacă DataContext-ul curent sau ViewModel-ul de comenzi este accesibil
            if (DataContext is MainViewModel mainVm)
            {
                await mainVm.ComandaVM.LoadProduseAsync();
            }
        }
    }
}