using System.Windows;
using NBMSystem.ViewModels;
using NBMSystem.Input;
using NBMSystem.MessageTypes;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;

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
        Dictionary<string, string> sir_reports = new Dictionary<string, string>();

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
            message.Body = body;

            if (m_header[0].Equals('S')) { SmsSplit(message); }
            else if (m_header[0].Equals('E')) { EmailSplit(message); }
        }

        // Spilts message into vars,
        // checks for abbreviation,
        // creates SMS object
        private void SmsSplit(MessageInput message)
        {
            //Assigning variables
            string Header = message.Header;
            string Body = message.Body;
            string Number = message.Body.Split(' ')[0];
            string Sender = message.Body.Split(' ')[1];
            string Text = message.Body.Replace(Sender, null).Replace(Number, null);

            //checking abbreviations
            List<string> abbreviations = new List<string>();
            List<string> abb_extended = new List<string>();

            using (var reader = new StreamReader(@"../../../Documents/textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var words = line.Split(',');
                    abbreviations.Add(words[0]);
                    abb_extended.Add(words[1]);
                }
            }
            foreach (string word in Text.Split(' '))
            {
                foreach (string abr in abbreviations)
                {
                    if (word == abr)
                    {
                        //finding abbreviations and there meaning
                        int index = abbreviations.IndexOf(abr);
                        string all = abb_extended[index];

                        //extending abbreviations
                        string words = word + " <" + all + "> ";

                        int index_2 = Text.IndexOf(word);

                        char wordFinal;
                        string wordFinal2;

                        try
                        {
                            wordFinal = Text[index_2 + 1 + word.Length];

                            wordFinal2 = wordFinal + "";
                            if (wordFinal2.Contains("<"))
                            {
                                break;
                            }
                            else
                            {
                                string newtext = Text.Replace(word, words);
                                Text = newtext;
                            }
                        }
                        catch
                        {
                            string newtext = Text.Replace(word, words);
                            Text = newtext;
                        }
                    }
                }
            }
            // Creating Object
            SmsMessageType SMS = new SmsMessageType()
            {
                Header = Header,
                Body = Body,
                SmsSender = Sender,
                SmsNumber = Number,
                SmsText = Text
            };
            //Addition to list for JSON
            messages.Add(SMS);

            //Addition to list box
            MessagesBox.Items.Add(SMS.Header);

            // Outputs to UI
            SenderOutput.Text = SMS.SmsSender;
            SubNumOutput.Text = SMS.SmsNumber;
            TextOutput.Text = SMS.SmsText;
        }

        private void EmailSplit(MessageInput message)
        {
            // Assigning variables
            string Header = message.Header;
            string Body = message.Body;
            string Sender = message.Body.Split(',')[0];
            string Subject = message.Body.Split(',')[1];
            string Text = Body.Split(',')[2];

            // Incident List
            List<string> incidents = new List<string>();
            incidents.Add("Theft");
            incidents.Add("Staff_Attack");
            incidents.Add("ATM_Theft");
            incidents.Add("Raid");
            incidents.Add("Customer_Attack");
            incidents.Add("Staff_Abuse");
            incidents.Add("Bomb_Threat");
            incidents.Add("Terrorism");
            incidents.Add("Suspicious_Incident");
            incidents.Add("Intelligence");
            incidents.Add("Cash Loss");

            // Checking for SIR in Subject
            if(Subject.Contains("SIR"))
            {
                Text = Body.Split(',')[2] + ", " + Body.Split(',')[3] + ", " + Body.Split(',')[4];
                Boolean sir_logged = false;

                foreach (string incident in incidents)
                {
                    string email_sir = ((Text.Split(',')[1]).ToLower());
                    email_sir = Regex.Replace(email_sir, @"\s+", "");
                    if (email_sir == incident)
                    {
                        sir_reports.Add(Text.Split(',')[0], Text.Split(',')[1]);
                        SIRListBox.Items.Add(email_sir + ", " + Sender);
                        sir_logged = true;
                    }

                }
                if (!sir_logged)
                {
                    throw new ArgumentException("S.I.R cannot be found");
                }

            }

        }
    }
}
