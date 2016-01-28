using System;
using System.Collections.Generic;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    [PropertyChanged.ImplementPropertyChanged]
    public class Question
    {
        public string QuestionId { get; set; }
        public string Body { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUri { get; set; }
        public Author Author { get; set; }
        public List<string> Tags { get; set; }
        public int Rating { get; set; }

        //Statistics
        public int VotesCount { get; set; }
        public int AnswersCount { get; set; }

        //For view
        public bool IsVotedUp { get; set; }
        public bool IsVotedDown { get; set; }
        public bool IsVotingBusy { get; set; }
    }
}