using System.Windows;
using NBMSystem.ViewModels;
using NBMSystem.Input;
using System.Collections.Generic;


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

        List<MessageInput> messages = new List<MessageInput>();
        List<string> quarantine_url = new List<string>();
        List<string> mentions = new List<string>();
        Dictionary<string, int> hashtags = new Dictionary<string, int>();
        Dictionary<string, string> incident_reports = new Dictionary<string, string>();

        private void Submit(object sender, RoutedEventArgs e)
        {
            string header_text = HeaderTextBox.Text.ToUpper();
            string body_text = BodyTextBox.Text;

            
        }

    }
}
