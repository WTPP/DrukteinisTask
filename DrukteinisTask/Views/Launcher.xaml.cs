using DrukteinisTask.ViewModel;
using DrukteinisTask.Views;
using System.Windows;

namespace DrukteinisTask
{
    public partial class Launcher : Window
    {
        public Launcher()
        {
            InitializeComponent();
            DataContext = new MirrorViewModel();
        }

        private void OpenTicketBuyForm(object sender, RoutedEventArgs e)
        {
            var newForm = new TicketBuyForm(); 
            newForm.Show();
        }

        private void OpenTaxCalculatorForm(object sender, RoutedEventArgs e)
        {
            var newForm = new TaxCalculatorForm();
            newForm.Show();
        }
    }
}
