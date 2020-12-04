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

            MessageSplit(header_text, body_text);
        }

        // Spliting the MessageInput into there category
        private void MessageSplit(string header, string body)
        {
            MessageInput message = new MessageInput();

            string m_header;

            m_header = header.ToUpper();
            message.Header = m_header;

            if (m_header[0].Equals('S')) { SmsSplit(message); }
        }

        private void SmsSplit(MessageInput message)
        {
            //Assigning variables
            string Header = message.Header;
            string Body = message.Body;
            string Number = message.Body.Split(' ')[0];
            string Sender = message.Body.Split(' ')[1];
            string Text = message.Body.Replace(Sender, null).Replace(Number, null);


        }
    }
}
