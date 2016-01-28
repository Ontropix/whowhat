using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class AnswerVotedDown : Event
    {
        public string AnswerId { get; set; }
        public string VoterId { get; set; }
        public int RatingShift { get; set; }
    }
}