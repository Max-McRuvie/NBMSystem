using NBMSystem.ViewModels;

namespace NBM.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Top row text blocks of template
        public string URLTextBlock { get; private set; }
        public string MentionListTextBlock { get; private set; }
        public string SIRTextBlock { get; private set; }
        public string TrendingTextBlock { get; private set; }
        #endregion

        #region Middle row of text blocks
        public string InputTextBlock { get; private set; }
        public string MessageProccessedTextBlock { get; private set; }
        #endregion

        #region Header and Body text block
        public string HeaderTextBlock { get; private set; }
        public string BodyTextBlock { get; private set; }
        #endregion

        #region Buttons
        public string SubmitButtonText { get; private set; }
        public string SaveButtonText { get; private set; }
        #endregion

        public MainWindowViewModel()
        {
            #region Top row text blocks of template
            URLTextBlock = "URL Quarintine";
            MentionListTextBlock = "Mention List";
            SIRTextBlock = "SIR List";
            TrendingTextBlock = "Trending List";
            #endregion

            #region Middle Row of text blocks
            InputTextBlock = "Message Inputs";
            MessageProccessedTextBlock = "Proccessed Message";
            #endregion

            #region Header and body text block
            HeaderTextBlock = "Message Header";
            BodyTextBlock = "Message Body";
            #endregion

            #region Buttons
            SubmitButtonText = "Submit";
            SaveButtonText = "Save";
            #endregion

        }

    }
}
