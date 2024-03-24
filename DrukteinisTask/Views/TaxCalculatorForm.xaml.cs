using DrukteinisTask.ViewModel;
using System.Windows;

namespace DrukteinisTask.Views
{
    public partial class TaxCalculatorForm : Window
    {
        public TaxCalculatorForm()
        {
            InitializeComponent();
            DataContext = new TaxCalculatorViewModel();
        }
    }
}
