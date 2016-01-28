using Platform.Domain;

namespace WhoWhat.Domain.Question.Events
{
    public class QuestionVotedDown : Event
    {
        public string VoterId { get; set; }
        public int RatingShift { get; set; }
    }
}