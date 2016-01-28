namespace WhoWhat.UI.WindowsPhone.Views.Question
{
    public partial class AnswerQuestionView
    {
        public AnswerQuestionView()
        {
            InitializeComponent();
            this.Loaded += (s, e) => TextBoxBody.Focus();
        }
    }
}
