using System.Collections.Generic;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    public class QuestionSummaryResponse : RestResponse
    {
        public List<Question> Questions { get; set; }
    }
}
