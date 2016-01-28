using WhoWhat.UI.WindowsPhone.Infrastructure;

namespace WhoWhat.UI.WindowsPhone.Views.Search
{
    public partial class SearchByKeywordView : View
    {
        public SearchByKeywordView()
        {
            InitializeComponent();

            this.Loaded += (sender, args) => SearchTextBox.Focus();
            SearchTextBox.GotFocus += (sender, args) => SearchTextBox.SelectAll();
        }
    }
}