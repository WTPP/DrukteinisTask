using System.Windows;

namespace DrukteinisTask.Views
{
    public partial class LotteryWinForm : Window
    {
        public LotteryWinForm()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
