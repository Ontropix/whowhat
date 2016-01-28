using System.Threading.Tasks;
using System.Windows.Input;
using WhoWhat.UI.WindowsPhone.Infrastructure;

namespace WhoWhat.UI.WindowsPhone.Views.Question
{
    public partial class AskQuestionView : View
    {
        public AskQuestionView()
        {
            InitializeComponent();
        }

        private async void QuestionKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Hide the keyboard
                this.Focus();

                //Wait a little to move focus to the Tags line
                await Task.Delay(200);

                //Open tags line
                TagsLine.Focus();

                e.Handled = true;
            }
        }
    }
}