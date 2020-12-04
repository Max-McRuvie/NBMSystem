using System.Windows;
using NBMSystem.ViewModels;


namespace NBMSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            string header_text = HeaderTextBox.Text.ToUpper();
            string body_text = BodyTextBox.Text;

            
        }

    }
}
