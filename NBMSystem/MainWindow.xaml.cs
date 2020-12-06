using System.Windows;
using NBMSystem.ViewModels;
using NBMSystem.Input;
using NBMSystem.MessageTypes;
using NBMSystem.FileSave;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace NBMSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        List<MessageInput> messagesList = new List<MessageInput>(); // message list
        List<string> quarantineUrlList = new List<string>(); // URL quarintine list
        List<string> mentionsList = new List<string>(); // mentons list
        Dictionary<string, int> hashtagsDict = new Dictionary<string, int>(); // hashtag list
        Dictionary<string, string> sirReportsDict = new Dictionary<string, string>(); // SIR report list
        List<string> incidentList = new List<string>(); // incident list

        public MainWindow()
        {
            InitializeComponent();

            // Adding contents to incidentList
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
        
        // Spliting the MessageInput into there category
        private void MessageTypeSplit(object sender, RoutedEventArgs e)
        {
            MessageInput message = new MessageInput();

            message.Header = HeaderTextBox.Text.ToUpper();
            message.Body = BodyTextBox.Text;

            // Calling methods depending on starting char
            if (message.Header[0].Equals('S')) 
            { 
                SmsSplit(message); 
            }
            else if (message.Header[0].Equals('E')) 
            { 
                EmailSplit(message); 
            }
            else if (message.Header[0].Equals('T'))
            {
                TweetSplit(message);
            }
            else
            {
                throw new ArgumentException("Error: Header does not contain 'S', 'E', or 'T' chars");
            }
        }

        // Spilts message into vars,
        // checks for abbreviation,
        // creates SMS object
        private void SmsSplit(MessageInput message)
        {
            //Assigning variables
            string number = message.Body.Split(' ')[0];
            string sender = message.Body.Split(' ')[1];
            string text = message.Body.Replace(sender, null).Replace(number, null);

            //checking abbreviations
            List<string> abbreviations = new List<string>();
            List<string> abrExtended = new List<string>();

            using (var reader = new StreamReader(@"../../../Documents/textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var words = line.Split(',');
                    abbreviations.Add(words[0]);
                    abrExtended.Add(words[1]);
                }
            }
            foreach (string word in text.Split(' '))
            {
                foreach (string abr in abbreviations)
                {
                    if (word == abr)
                    {
                        //finding abbreviations and there meaning
                        int index = abbreviations.IndexOf(abr);
                        string all = abrExtended[index];

                        //extending abbreviations
                        string words = word + " <" + all + "> ";

                        int index2 = text.IndexOf(word);

                        try
                        {
                            char wordFinal = text[index2 + 1 + word.Length];

                            string wordFinal2 = wordFinal + "";
                            if (wordFinal2.Contains("<"))
                            {
                                break;
                            }
                            else
                            {
                                text = text.Replace(word, words);
                            }
                        }
                        catch
                        {
                            text = text.Replace(word, words);
                        }
                    }
                }
            }
            // Creating Object
            SmsMessageType sms = new SmsMessageType()
            {
                Header = message.Header,
                Body = message.Body,
                SmsSender = sender,
                SmsNumber = number,
                SmsText = text
            };
            //Addition to list for JSON
            messagesList.Add(sms);

            //Addition to list box
            MessagesBox.Items.Add(sms.Header);

            // Outputs to UI
            SenderOutput.Text = sms.SmsSender;
            SubNumOutput.Text = sms.SmsNumber;
            TextOutput.Text = sms.SmsText;
        }

        private void EmailSplit(MessageInput message)
        {
            // Assigning variables
            string sender = message.Body.Split(',')[0];
            string subject = message.Body.Split(',')[1];
            string text = message.Body.Split(',')[2];

            // Checking for SIR in Subject
            if(subject.Contains("SIR"))
            {
                text = message.Body.Split(',')[2] + ", " + message.Body.Split(',')[3] + ", " + message.Body.Split(',')[4];
                Boolean sirLogged = false;

                foreach (string incident in this.incidentList)
                {
                    string emailSir = ((text.Split(',')[1]).ToLower());
                    emailSir = Regex.Replace(emailSir, @"\s+", "");
                    if (emailSir == incident)
                    {
                        sirReportsDict.Add(text.Split(',')[0], text.Split(',')[1]);
                        SirListBox.Items.Add(emailSir + ", " + sender);
                        sirLogged = true;
                    }

                }
                if (!sirLogged)
                {
                    throw new ArgumentException("Error: SIR cannot be found");
                }
            }

            // Checking URLs
            foreach (string word in text.Split(' '))
            {
                if(word.StartsWith("http:") || word.StartsWith("https:") || word.StartsWith("www."))
                {
                    text = text.Replace(word, "<URL Quarintined>");
                    foreach (string word1 in (text).Split(' '))
                    {
                        if (!quarantineUrlList.Contains(word))
                        {
                            quarantineUrlList.Add(word);
                            UrlQuarintineListBox.Items.Add(word);
                        }
                    }
                }
            }

            // Creating object
            EmailMessageType email = new EmailMessageType()
            {
                Header = message.Header,
                Body = message.Body,
                EmailSender = sender,
                EmailSubject = subject,
                EmailText = text
            };

            //Addition to list for JSON
            messagesList.Add(email);

            //Addition to list box
            MessagesBox.Items.Add(email.Header);

            // Outputs to UI
            SenderOutput.Text = email.EmailSender;
            SubNumOutput.Text = email.EmailSubject;
            TextOutput.Text = email.EmailText;
        }

        // Tweet Split
        private void TweetSplit(MessageInput message)
        {
            // Declaring variables
            string sender = message.Body.Split(' ')[0];
            string text = message.Body.Replace(sender, null);

            // Search for # and @
            foreach(string word in text.Split(' '))
            {
                // @ Search
                if (word.StartsWith("@"))
                {
                    if (!mentionsList.Contains(word)){
                        mentionsList.Add(word);
                        MentionListBox.Items.Add(word);
                    }
                }
                // # Search
                if (word.StartsWith("#"))
                {
                    if (hashtagsDict.ContainsKey(word))
                    {
                        hashtagsDict[word] += 1;
                    }
                    else
                    {
                        hashtagsDict.Add(word, 1);
                    }
                }
            }
            //checking abbreviations
            List<string> abbreviations = new List<string>();
            List<string> abrExtended = new List<string>();

            using (var reader = new StreamReader(@"../../../Documents/textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var words = line.Split(',');
                    abbreviations.Add(words[0]);
                    abrExtended.Add(words[1]);
                }
            }
            foreach (string word in text.Split(' '))
            {
                foreach (string abr in abbreviations)
                {
                    if (word == abr)
                    {
                        //finding abbreviations and there meaning
                        int index = abbreviations.IndexOf(abr);
                        string all = abrExtended[index];

                        //extending abbreviations
                        string words = word + " <" + all + "> ";

                        int index2 = text.IndexOf(word);

                        try
                        {
                            char wordFinal = text[index2 + 1 + word.Length];

                            string wordFinal2 = wordFinal + "";
                            if (wordFinal2.Contains("<"))
                            {
                                break;
                            }
                            else
                            {
                                text = text.Replace(word, words);
                            }
                        }
                        catch
                        {
                            text = text.Replace(word, words);
                        }
                    }
                }
            }
            // Creating Object
            TweetMessageType tweet = new TweetMessageType()
            {
                Header = message.Header,
                Body = message.Body,
                TweetSender = sender,
                TweetText = text
            };
            // Add to message List
            messagesList.Add(tweet);
            // Adding to listbox
            MessagesBox.Items.Add(tweet.Header);
            // Outputs
            SenderOutput.Text = " ";
            SubNumOutput.Text = tweet.TweetSender;
            TextOutput.Text = tweet.TweetText;
            // Trending List
            TrendingListBox.Items.Clear();
            var tSort = hashtagsDict.OrderBy(x => x.Value);
            foreach(var item in tSort.OrderByDescending(key => key.Value))
            {
                TrendingListBox.Items.Add(item);
            }
        }

        private void JsonSave(object sender, RoutedEventArgs e)
        {
            JsonFileSave json = new JsonFileSave();
            json.saveToJson(messagesList);
        }
    }
}
