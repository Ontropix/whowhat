namespace WhoWhat.Domain.Question
{
    internal class Vote
    {
        public Vote(VoteDirection direction, int ratingShift)
        {
            this.Direction = direction;
            this.RatingShift = ratingShift;
        }

        public VoteDirection Direction { get; private set; }
        public int RatingShift { get; private set; }
    }
}