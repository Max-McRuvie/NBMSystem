﻿using System.Windows;
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
        
        List<MessageInput> messages = new List<MessageInput>(); // message list
        List<string> quarantineUrl = new List<string>(); // URL quarintine list
        List<string> mentions = new List<string>(); // mentons list
        Dictionary<string, int> hashtags = new Dictionary<string, int>(); // hashtag list
        Dictionary<string, string> sirReports = new Dictionary<string, string>(); // SIR report list
        List<string> incidentList = new List<string>(); // incident list

        public MainWindow()
        {
            InitializeComponent();

            this.incidentList.Add("theft");
            this.incidentList.Add("staffattack");
            this.incidentList.Add("atmtheft");
            this.incidentList.Add("raid");
            this.incidentList.Add("customerattack");
            this.incidentList.Add("staffabuse");
            this.incidentList.Add("bombthreat");
            this.incidentList.Add("terrorism");
            this.incidentList.Add("suspiciousincident");
            this.incidentList.Add("intelligence");
            this.incidentList.Add("cashloss");

            this.DataContext = new MainWindowViewModel();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            string headerText = HeaderTextBox.Text.ToUpper();
            string bodyText = BodyTextBox.Text;

            MessageSplit(headerText, bodyText);
        }

        // Spliting the MessageInput into there category
        private void MessageSplit(string header, string body)
        {
            MessageInput message = new MessageInput();

            message.Header = header.ToUpper();
            message.Body = body;

            // Calling methods depending on starting char
            if (header[0].Equals('S')) 
            { 
                SmsSplit(message); 
            }
            else if (header[0].Equals('E')) 
            { 
                EmailSplit(message); 
            }
        }

        // Spilts message into vars,
        // checks for abbreviation,
        // creates SMS object
        private void SmsSplit(MessageInput message)
        {
            //Assigning variables
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
                                Text = Text.Replace(word, words);
                            }
                        }
                        catch
                        {
                            Text = Text.Replace(word, words);
                        }
                    }
                }
            }
            // Creating Object
            SmsMessageType SMS = new SmsMessageType()
            {
                Header = message.Header,
                Body = message.Body,
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
            string header = message.Header;
            string body = message.Body;
            string sender = message.Body.Split(',')[0];
            string subject = message.Body.Split(',')[1];
            string text = body.Split(',')[2];

            // Checking for SIR in Subject
            if(subject.Contains("SIR"))
            {
                text = body.Split(',')[2] + ", " + body.Split(',')[3] + ", " + body.Split(',')[4];
                Boolean sirLogged = false;

                foreach (string incident in this.incidentList)
                {
                    string emailSir = ((text.Split(',')[1]).ToLower());
                    emailSir = Regex.Replace(emailSir, @"\s+", "");
                    if (emailSir == incident)
                    {
                        sirReports.Add(text.Split(',')[0], text.Split(',')[1]);
                        SirListBox.Items.Add(emailSir + ", " + sender);
                        sirLogged = true;
                    }

                }
                if (!sirLogged)
                {
                    throw new ArgumentException("SIR cannot be found");
                }

            }

        }
    }
}
