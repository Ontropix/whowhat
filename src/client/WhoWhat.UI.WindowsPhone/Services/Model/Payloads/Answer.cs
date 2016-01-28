using System;
using System.Collections.Generic;

namespace WhoWhat.UI.WindowsPhone.Services.Model
{
    [PropertyChanged.ImplementPropertyChanged]
    public class Answer
    {
        public string AnswerId { get; set; }

        /// <summary>
        /// Id of the question the answer belongs to.
        /// </summary>
        public string QuestionId { get; set; }

        public string Body { get; set; }
        public bool IsAccepted { get; set; }
        public Author Author { get; set; }
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }


        public Dictionary<string, VoteDirection> Votes { get; set; }

        //View fields
        public bool IsEdit { get; set; }

        public bool IsCurrentUserAuthor { get; set; }

        public string EditedBody { get; set; }

        public bool IsVotedUp { get; set; }

        public bool IsVotedDown { get; set; }

        public bool CanBeAccepted { get; set; }

        public bool CanBeEdited { get; set; }

        public bool IsVotingBusy { get; set; }
    }

    public enum VoteDirection
    {
        Up = 'U',
        Down = 'D'
    }
}
